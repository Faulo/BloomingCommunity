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

        [Space]
        [SerializeField]
        public CommunityAsset community;
        [SerializeField]
        public TileDatabase tiles;
        [SerializeField]
        public UIDocument speechPrefab;

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

        public void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}