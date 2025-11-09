using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class PlantTile : TileBase {
        [SerializeField]
        internal string id;

        [SerializeField]
        Sprite sprite;

        [SerializeField]
        PlantTile nextStage;

        internal bool fullyGrown => !nextStage;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = sprite;
            tileData.flags = TileFlags.LockAll;
        }
    }
}
