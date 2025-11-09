using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.UIElements;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class CharacterControl {
        public string name => asset.tag;

        public override string ToString() => $"Character {name}";

        readonly GameObject gameObject;
        readonly Animator animator;
        readonly FMODEventPlayer audio;
        readonly CharacterAsset asset;
        readonly MapControl map;
        readonly UIDocument speech;

        string speechText = string.Empty;
        string todoText = string.Empty;

        public ECharacterState state = ECharacterState.Idle;

        public CharacterControl(CharacterAsset asset, MapControl map, UIDocument speechPrefab) {
            gameObject = UObject.Instantiate(asset.prefab);
            gameObject.tag = asset.tag;

            if (gameObject.TryGetComponent(out animator)) {
                animator.runtimeAnimatorController = asset.animator;
            }

            audio = gameObject.AddComponent<FMODEventPlayer>();

            this.asset = asset;
            this.map = map;

            speech = UObject.Instantiate(speechPrefab, gameObject.transform);
            speech.rootVisualElement.AddToClassList(asset.tag);

            TeleportTo(position3D);
        }

        public void TeleportTo(Vector3 position) {
            position2D = map.WorldToGrid(position);
            position3D = map.GridToWorld(position2D);
        }

        public void TeleportTo(Vector2Int position) {
            position2D = position;
            position3D = map.GridToWorld(position2D);
        }

        public bool isActive = false;

        public Vector3 position3D { get; private set; }
        public Vector2Int position2D { get; private set; }
        public Vector2Int selectedPosition2D => position2D + facing;

        public Vector2Int facing = Vector2Int.down;

        static readonly Dictionary<Vector2Int, Quaternion> rotations = new() {
            [Vector2Int.up] = Quaternion.identity,
            [Vector2Int.down] = Quaternion.Euler(0, 0, 180),
            [Vector2Int.left] = Quaternion.Euler(0, 0, 90),
            [Vector2Int.right] = Quaternion.Euler(0, 0, -90),
        };

        Quaternion rotation3D => rotations[facing];

        static readonly Dictionary<Vector2Int, string> anim_facing = new() {
            [Vector2Int.up] = "Up_",
            [Vector2Int.down] = "Down_",
            [Vector2Int.left] = "Left_",
            [Vector2Int.right] = "Right_",
        };

        static readonly Dictionary<ECharacterState, string> anim_state = new() {
            [ECharacterState.Idle] = "Idle",
            [ECharacterState.Facing] = "Idle",
            [ECharacterState.Moving] = "Walk",
            [ECharacterState.Blocked] = "Walk",
            [ECharacterState.Growing] = "Grow",
            [ECharacterState.Plant] = "Sow",
        };

        float speechTimer = 0;

        public void Update(float deltaTime) {
            gameObject.SetActive(isActive);
            gameObject.name = $"{asset.tag}: {state}";

            if (isActive) {
                if (animator) {
                    animator.Play(anim_facing[facing] + anim_state[state]);
                    gameObject.transform.SetPositionAndRotation(position3D, Quaternion.Euler(0, 0, 0));
                } else {
                    gameObject.transform.SetPositionAndRotation(position3D, rotation3D);
                }

                if (isSpeaking) {
                    if (speechText == todoText) {
                        speechTimer -= deltaTime;
                        if (speechTimer < 0) {
                            speechText = todoText = string.Empty;
                        }
                    } else {
                        speechTimer -= deltaTime;

                        if (speechTimer > 0) {
                            float t = Mathf.Lerp(todoText.Length, 0, speechTimer / (todoText.Length * asset.letterDuration));
                            speechText = todoText[..Mathf.RoundToInt(t)];
                        } else {
                            speechText = todoText;
                            speechTimer = asset.speechPause;
                        }
                    }
                }

                speech.rootVisualElement.Q<Label>().text = speechText;
                speech.rootVisualElement.style.display = isSpeaking
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            }
        }

        public Vector2Int intendedMove;

        float stateTimer = 0;
        Vector2Int statePosition2D;

        public void FixedUpdate(float deltaTime) {
            switch (state) {
                case ECharacterState.Idle:
                    if (intendedMove != Vector2Int.zero) {
                        if (facing == intendedMove) {
                            if (map.IsFreeToMove(position2D + intendedMove)) {
                                StartMoving();
                            } else {
                                Bonk();
                            }
                        } else {
                            Face();
                        }

                        intendedMove = Vector2Int.zero;

                        if (deltaTime > 0) {
                            FixedUpdate(deltaTime);
                        }

                        return;
                    }

                    break;
                case ECharacterState.Facing:
                    stateTimer -= deltaTime;

                    if (stateTimer <= 0) {
                        state = ECharacterState.Idle;
                        if (stateTimer < 0) {
                            FixedUpdate(Mathf.Abs(stateTimer));
                        }

                        return;
                    }

                    break;
                case ECharacterState.Moving:
                    stateTimer -= deltaTime;

                    var previousPosition = map.GridToWorld(statePosition2D);
                    var targetPosition = map.GridToWorld(position2D);
                    position3D = asset.moveDuration > 0
                        ? Vector3.Lerp(targetPosition, previousPosition, stateTimer / asset.moveDuration)
                        : targetPosition;

                    if (stateTimer <= 0) {
                        state = ECharacterState.Idle;
                        audio.StopPlaying();
                        if (stateTimer < 0) {
                            FixedUpdate(Mathf.Abs(stateTimer));
                        }

                        return;
                    }

                    break;
                case ECharacterState.Blocked:
                    stateTimer -= deltaTime;

                    position3D = map.GridToWorld(position2D) + (0.1f * Mathf.Sin(stateTimer * Mathf.PI) * (Vector3)facing.SwizzleXY());

                    if (stateTimer <= 0) {
                        position3D = map.GridToWorld(position2D);
                        state = ECharacterState.Idle;
                        if (stateTimer < 0) {
                            FixedUpdate(Mathf.Abs(stateTimer));
                        }

                        return;
                    }

                    break;
                case ECharacterState.Plant:
                    state = ECharacterState.Idle;
                    break;
            }
        }

        void Face() {
            state = ECharacterState.Facing;
            facing = intendedMove;
            stateTimer = asset.facingDuration;
        }

        void StartMoving() {
            state = ECharacterState.Moving;
            statePosition2D = position2D;
            position2D += intendedMove;
            stateTimer = asset.moveDuration;

            if (!asset.stepEvent.IsNull) {
                audio.PlayRepeatedly(asset.stepEvent, asset.stepInterval);
            }
        }

        void Bonk() {
            asset.bonkEvent.PlayOnce();
            state = ECharacterState.Blocked;
            stateTimer = asset.blockedDuration;
        }

        internal void Plant(MapControl map, string plant) {
            state = ECharacterState.Plant;
            map.Plant(selectedPosition2D, plant);
        }

        internal bool isSpeaking => !string.IsNullOrEmpty(todoText);

        internal void Say(string text) {
            speechText = string.Empty;
            todoText = string.IsNullOrEmpty(text)
                ? string.Empty
                : text;
            speechTimer = asset.letterDuration * todoText.Length;
        }
    }
}