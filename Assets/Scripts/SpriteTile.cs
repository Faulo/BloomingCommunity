using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class SpriteTile : TileBase {
        [SerializeField]
        Sprite sprite;
        [SerializeField]
        Color tint = Color.white;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = sprite;
            tileData.color = tint;
            tileData.flags = TileFlags.LockColor;
        }
    }
}