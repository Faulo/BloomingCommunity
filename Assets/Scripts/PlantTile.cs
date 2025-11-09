using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class PlantTile : TileBase {
        [SerializeField]
        internal string id;

        [SerializeField]
        Sprite sprite;

        [SerializeField]
        internal PlantTile nextStage;

        [SerializeField]
        internal bool isSeed;

        [SerializeField]
        internal bool isFullyGrown;

        [SerializeField]
        internal bool isDead;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = sprite;
            tileData.flags = TileFlags.LockAll;
        }
    }
}
