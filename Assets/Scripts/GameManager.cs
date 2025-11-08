using Unity.Properties;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class GameManager {

        readonly GameObject gameObject;
        readonly MapControl map;
        readonly InputActions input;
        readonly AvatarControl avatar;
        readonly CommunityControl community;

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

            var character = map.CreateCharacter(game.avatar);

            character.isActive = true;

            avatar = new(character, input);

            foreach (var asset in game.humans) {
                map.CreateCharacter(asset);
            }

            community = new(game.community, map);
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
            community.FixedUpdate(deltaTime);
        }

        internal void Quit() {
            UObject.Destroy(gameObject);
        }
    }
}