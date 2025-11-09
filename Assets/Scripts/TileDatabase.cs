using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class TileDatabase : ScriptableObject {
        [SerializeField]
        TileBase off;
        public bool IsOff(TileBase tile) => tile == off;

        [SerializeField]
        TileBase field;
        public bool IsField(TileBase tile) => tile == field;

        [SerializeField]
        SerializableKeyValuePairs<string, TileBase> plants = new();

        internal TileBase GetPlant(string name) => plants[name];
        internal bool TryGetPlant(string name, out TileBase tile) => plants.TryGetValue(name, out tile);
    }
}