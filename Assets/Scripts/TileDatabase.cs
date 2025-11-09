using System;
using System.Collections.Generic;
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
        PlantTile[] plants = Array.Empty<PlantTile>();

        internal IEnumerable<string> plantNames => plants.Select(t => t.id).Distinct();

        internal TileBase GetPlant(string name) => plants.FirstOrDefault(t => t.id == name);

        internal bool TryGetPlant(string name, out TileBase tile) {
            return tile = GetPlant(name);
        }
    }
}