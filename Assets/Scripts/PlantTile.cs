using Slothsoft.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class PlantTile : TileBase {
        [SerializeField]
        AsepriteFile source;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = source.firstSprite;
            tileData.flags = TileFlags.LockAll;
        }
    }
}
