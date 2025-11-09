using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class TileDatabase : ScriptableObject {
        [SerializeField]
        TileBase[] offTiles;
        public bool IsOff(TileBase tile) => offTiles.Contains(tile);

        [SerializeField]
        TileBase field;
        public bool IsField(TileBase tile) => tile == field;

        [SerializeField]
        SerializableKeyValuePairs<string, TileBase> special = new();
        internal bool IsSpecial(TileBase tile) => special.Values.Contains(tile);
        internal bool TryGetSpecial(string name, out TileBase tile) => special.TryGetValue(name, out tile);

        [SerializeField]
        internal SerializableKeyValuePairs<string, TileBase> plants = new();

        internal TileBase GetPlant(string name) => plants[name];
        internal bool TryGetPlant(string name, out TileBase tile) => plants.TryGetValue(name, out tile);
    }
}