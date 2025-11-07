using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class GameAsset : ScriptableObject {
        [CreateProperty]
        public bool gamePaused => manager is null || manager.isPaused;
        [CreateProperty]
        public DisplayStyle displayMainMenu => gamePaused ? DisplayStyle.Flex : DisplayStyle.None;

        GameManager manager;

        public void Start() {
            if (manager is not null) {
                manager.Quit();
            }

            manager = new();
        }

        public void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}