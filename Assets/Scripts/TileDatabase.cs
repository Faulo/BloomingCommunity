using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class TileDatabase : ScriptableObject {
        [SerializeField]
        public TileBase off;
    }
}