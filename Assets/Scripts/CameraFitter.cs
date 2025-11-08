using UnityEngine;

namespace BloomingCommunity.Runtime {
    [ExecuteAlways]
    sealed class CameraFitter : MonoBehaviour {
        [SerializeField]
        Camera fittedCamera;

        [Space]
        [SerializeField]
        int horizontalTiles = 32;
        [SerializeField]
        int verticalTiles = 18;

        float cameraSize {
            get {
                float targetRatio = (float)horizontalTiles / verticalTiles;
                float screenRatio = (float)Screen.width / Screen.height;
                return targetRatio <= screenRatio || Mathf.Approximately(targetRatio, screenRatio)
                    ? verticalTiles * 0.5f
                    : verticalTiles * 0.5f * targetRatio / screenRatio;
            }
        }

        void LateUpdate() {
            fittedCamera.orthographicSize = cameraSize;
        }
    }
}
