using Unity.Properties;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class AvatarControl {
        public AvatarControl(GameObject prefab) {

        }
    }

    sealed class GameManager {

        readonly GameObject gameObject;
        readonly WorldState state;
        readonly InputActions input;

        [CreateProperty]
        internal bool isPaused;

        public GameManager() {
            gameObject = new(nameof(GameManager));
            var runner = gameObject.AddComponent<ObjectRunner>();
            runner.onUpdate += OnUpdate;
            runner.onFixedUpdate += OnFixedUpdate;

            state = ScriptableObject.CreateInstance<WorldState>();

            input = new InputActions();
            input.Player.Pause.performed += _ => isPaused = !isPaused;
            input.Enable();
        }

        void OnUpdate(float deltaTime) {
        }

        void OnFixedUpdate(float deltaTime) {
        }

        internal void Quit() {
            UObject.Destroy(gameObject);
        }
    }
}