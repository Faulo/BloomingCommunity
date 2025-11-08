using Unity.Properties;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class GameManager {

        readonly GameObject gameObject;
        readonly MapControl map;
        readonly InputActions input;
        readonly AvatarControl avatar;

        [CreateProperty]
        internal bool isPaused;

        public GameManager(GameAsset game) {
            gameObject = new(nameof(GameManager));
            var runner = gameObject.AddComponent<ObjectRunner>();
            runner.onUpdate += OnUpdate;
            runner.onFixedUpdate += OnFixedUpdate;

            map = new MapControl(UObject.FindAnyObjectByType<Grid>());

            input = new InputActions();
            input.Enable();
            input.Player.Pause.performed += _ => isPaused = !isPaused;

            var character = map.CreateCharacter(GameObject.FindGameObjectWithTag("Player"), game.avatar);

            avatar = new(character, input);
        }

        void OnUpdate(float deltaTime) {
            if (isPaused) {
                return;
            }

            map.Update(deltaTime);
        }

        void OnFixedUpdate(float deltaTime) {
            if (isPaused) {
                return;
            }

            avatar.FixedUpdate(deltaTime);
            map.FixedUpdate(deltaTime);
        }

        internal void Quit() {
            UObject.Destroy(gameObject);
        }
    }
}