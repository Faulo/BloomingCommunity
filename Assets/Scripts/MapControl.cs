using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    sealed class MapControl {
        readonly Grid grid;
        readonly Tilemap ground;
        readonly Tilemap objects;

        public MapControl(Grid grid) {
            this.grid = grid;

            var tilemaps = grid.GetComponentsInChildren<Tilemap>();
            ground = tilemaps[0];
        }

        public readonly List<CharacterControl> characters = new();

        public CharacterControl CreateCharacter(CharacterAsset asset) {
            var character = new CharacterControl(asset, this);
            characters.Add(character);
            return character;
        }

        public Vector2Int WorldToGrid(Vector3 position) => grid.WorldToCell(position).SwizzleXY();
        public Vector3 GridToWorld(Vector2Int position) => grid.GetCellCenterWorld(position.SwizzleXY());

        public bool IsFreeToMove(Vector2Int position) {
            return ground.GetTile(position.SwizzleXY());
        }

        public void Update(float deltaTime) {
            foreach (var c in characters) {
                c.Update(deltaTime);
            }
        }

        public void FixedUpdate(float deltaTime) {
            foreach (var c in characters) {
                c.FixedUpdate(deltaTime);
            }
        }
    }
}