using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class CharacterControl {

        public string name => asset.tag;

        readonly GameObject gameObject;
        readonly CharacterAsset asset;
        readonly MapControl map;

        ECharacterState state = ECharacterState.Idle;

        public CharacterControl(CharacterAsset asset, MapControl map) {
            gameObject = UObject.Instantiate(asset.prefab);
            gameObject.tag = asset.tag;

            this.asset = asset;
            this.map = map;

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

        readonly Dictionary<Vector2Int, Quaternion> rotations = new() {
            [Vector2Int.up] = Quaternion.identity,
            [Vector2Int.down] = Quaternion.Euler(0, 0, 180),
            [Vector2Int.left] = Quaternion.Euler(0, 0, 90),
            [Vector2Int.right] = Quaternion.Euler(0, 0, -90),
        };

        Quaternion rotation3D => rotations[facing];

        public void Update(float deltaTime) {
            gameObject.SetActive(isActive);
            gameObject.transform.SetPositionAndRotation(position3D, rotation3D);
            gameObject.name = $"{asset.tag}: {state}";
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
                                state = ECharacterState.Moving;
                                statePosition2D = position2D;
                                position2D += intendedMove;
                                stateTimer = asset.moveDuration;
                            } else {
                                state = ECharacterState.Blocked;
                                stateTimer = asset.blockedDuration;
                            }
                        } else {
                            state = ECharacterState.Facing;
                            facing = intendedMove;
                            stateTimer = asset.facingDuration;
                        }

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
            }
        }
    }
}