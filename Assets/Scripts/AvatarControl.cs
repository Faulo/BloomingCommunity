using System.Linq;
using Ink.Runtime;
using Slothsoft.UnityExtensions;
using Strayfarer.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class AvatarControl : InputActions.IPlayerActions {
        readonly CharacterControl character;
        readonly UIDocument debugPrefab;
        readonly CommunityControl community;

        public AvatarControl(CharacterControl character, InputActions input, UIDocument debugPrefab, CommunityControl community) {
            this.character = character;
            this.debugPrefab = debugPrefab;
            this.community = community;
            input.Player.AddCallbacks(this);
        }

        public void Update(float deltaTime) {
            character.Update(deltaTime);
        }

        public void FixedUpdate(float deltaTime) {
            character.intendedMove = intendedMove.magnitude > MOVEMENT_DEADZONE
                ? intendedMove.SnapToCardinal()
                : Vector2Int.zero;
            character.FixedUpdate(deltaTime);
        }

        Vector2 intendedMove;

        const float MOVEMENT_DEADZONE = 0.5f;

        public void OnMove(InputAction.CallbackContext context) {
            intendedMove = context.ReadValue<Vector2>();
        }
        public void OnFire(InputAction.CallbackContext context) {

        }
        public void OnPause(InputAction.CallbackContext context) {

        }

        UIDocument debug;
        public void OnDebug(InputAction.CallbackContext context) {
            if (!context.performed) {
                return;
            }

            ToggleDebug();
        }

        void ToggleDebug() {
            if (debug) {
                UObject.Destroy(debug.gameObject);
            } else {
                debug = UObject.Instantiate(debugPrefab);
                var list = debug.rootVisualElement.Q<SimpleListView>();
                list.onInstantiateItem += root => {
                    var button = new Button();
                    button.clicked += () => {
                        if (button.userData is Story story) {
                            community.ForceStartCutscene(story);
                        }

                        ToggleDebug();
                    };
                    root.Add(button);
                };
                list.onBindItem += (root, item) => {
                    if (item is Story story) {
                        var button = root.Q<Button>();
                        button.text = community.storyNames[story];
                        button.userData = story;

                        if (!community.stories.Contains(story)) {
                            button.style.opacity = 0.5f;
                        }
                    }
                };
                list.itemsSource = community.storyNames.Keys.ToList();
            }
        }
    }
}