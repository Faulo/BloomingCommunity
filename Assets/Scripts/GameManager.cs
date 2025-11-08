using Unity.Properties;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class GameManager {

        readonly GameObject gameObject;
        readonly WorldState state;
        readonly InputActions input;
        readonly AvatarControl avatar;

        [CreateProperty]
        internal bool isPaused;

        public GameManager() {
            gameObject = new(nameof(GameManager));
            var runner = gameObject.AddComponent<ObjectRunner>();
            runner.onUpdate += OnUpdate;
            runner.onFixedUpdate += OnFixedUpdate;

            state = ScriptableObject.CreateInstance<WorldState>();
            state.grid = UObject.FindAnyObjectByType<Grid>();

            input = new InputActions();
            input.Enable();
            input.Player.Pause.performed += _ => isPaused = !isPaused;

            avatar = new(GameObject.FindGameObjectWithTag("Player"), input);
        }

        void OnUpdate(float deltaTime) {
        }

        void OnFixedUpdate(float deltaTime) {
            avatar.FixedUpdate(deltaTime);
        }

        internal void Quit() {
            UObject.Destroy(gameObject);
        }
    }
}