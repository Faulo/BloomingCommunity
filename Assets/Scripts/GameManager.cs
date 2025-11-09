using Unity.Properties;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BloomingCommunity.Runtime {
    sealed class GameManager {
        readonly GameAsset asset;
        readonly GameObject gameObject;
        readonly MapControl map;
        readonly InputActions input;
        readonly AvatarControl avatar;
        readonly CommunityControl community;

        [CreateProperty]
        internal bool isPaused;

        public GameManager(GameAsset game) {
            asset = game;

            gameObject = new(nameof(GameManager));
            var runner = gameObject.AddComponent<ObjectRunner>();
            runner.onUpdate += OnUpdate;
            runner.onFixedUpdate += OnFixedUpdate;

            map = new MapControl(UObject.FindAnyObjectByType<Grid>(), game.tiles, game.speechPrefab, gameObject.transform);

            input = new InputActions();
            input.Enable();
            input.Player.Pause.performed += _ => isPaused = !isPaused;

            var character = map.CreateCharacter(game.avatar, false);

            character.isActive = true;

            community = new(game.community, map);

            avatar = new(character, input, game.debugMenu, community, game);

            foreach (var asset in game.humans) {
                map.CreateCharacter(asset, true);
            }
        }

        void OnUpdate(float deltaTime) {
            if (isPaused) {
                return;
            }

            deltaTime *= asset.timeScale;

            avatar.Update(deltaTime);
            map.Update(deltaTime);
        }

        void OnFixedUpdate(float deltaTime) {
            if (isPaused) {
                return;
            }

            deltaTime *= asset.timeScale;

            avatar.FixedUpdate(deltaTime);
            map.FixedUpdate(deltaTime);
            community.FixedUpdate(deltaTime);
        }

        internal void Quit() {
            UObject.Destroy(gameObject);
        }
    }
}