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
        public bool IsSpecial(TileBase tile) => special.Values.Contains(tile);
        public bool TryGetSpecial(string name, out TileBase tile) => special.TryGetValue(name, out tile);

        [SerializeField]
        PlantTile[] plants = Array.Empty<PlantTile>();

        public IEnumerable<string> plantNames => plants.Select(t => t.id).Distinct();

        public TileBase GetPlant(string name) {
            if (name.Contains('_')) {
                string[] args = name.Split('_');
                return plants
                    .Where(t => t.id == args[0])
                    .FirstOrDefault(t => args[1] switch {
                        "grown" => t.isFullyGrown,
                        "dead" => t.isDead,
                        "seed" => t.isSeed,
                        _ => throw new NotImplementedException(args[1]),
                    });
            } else {
                return plants.FirstOrDefault(t => t.id == name);
            }
        }

        public bool TryGetPlant(string name, out TileBase tile) {
            return tile = GetPlant(name);
        }
    }
}