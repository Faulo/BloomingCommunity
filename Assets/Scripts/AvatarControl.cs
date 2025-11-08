using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BloomingCommunity.Runtime {
    sealed class AvatarControl : InputActions.IPlayerActions {
        readonly CharacterControl character;

        public AvatarControl(CharacterControl character, InputActions input) {
            this.character = character;
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
    }
}