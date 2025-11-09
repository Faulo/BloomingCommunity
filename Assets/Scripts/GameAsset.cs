using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class GameAsset : ScriptableObject {
        [SerializeField]
        public CharacterAsset avatar;
        [SerializeField]
        public CharacterAsset[] humans = Array.Empty<CharacterAsset>();
        [SerializeField]
        public float timeScale = 1;

        [Space]
        [SerializeField]
        public CommunityAsset community;
        [SerializeField]
        public TileDatabase tiles;
        [SerializeField]
        public UIDocument speechPrefab;
        [SerializeField]
        public APICall[] winningCalls = Array.Empty<APICall>();
        [SerializeField]
        internal UIDocument debugMenu;

        [CreateProperty]
        public bool gamePaused => manager is null || manager.isPaused;
        [CreateProperty]
        public DisplayStyle displayMainMenu => gamePaused ? DisplayStyle.Flex : DisplayStyle.None;

        GameManager manager;

        public void Start() {
            if (manager is not null) {
                manager.Quit();
            }

            manager = new(this);
        }

        public void Win() {
            foreach (var call in winningCalls) {
                call.CallAPI();
            }
        }

        public void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}