using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BloomingCommunity.Runtime {
    sealed class AvatarControl : InputActions.IPlayerActions {
        readonly GameObject gameObject;

        public AvatarControl(GameObject gameObject, InputActions input) {
            this.gameObject = gameObject;
            input.Player.AddCallbacks(this);
        }

        public void FixedUpdate(float deltaTime) {
            gameObject.transform.position += (intendedMove * deltaTime).SwizzleXY();
        }

        Vector2 intendedMove;

        public void OnMove(InputAction.CallbackContext context) {
            intendedMove = context.ReadValue<Vector2>();
        }
        public void OnFire(InputAction.CallbackContext context) { }
        public void OnPause(InputAction.CallbackContext context) { }
    }
}