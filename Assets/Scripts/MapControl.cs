using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BloomingCommunity.Runtime {
    sealed class MapControl {
        readonly Grid grid;
        readonly Tilemap ground;
        readonly Tilemap objects;
        readonly Tilemap special;

        readonly TileDatabase tiles;

        public MapControl(Grid grid, TileDatabase tiles) {
            this.grid = grid;
            this.tiles = tiles;

            var tilemaps = grid.GetComponentsInChildren<Tilemap>();
            ground = tilemaps[0];
            objects = tilemaps[1];
            special = tilemaps[2];

            special.GetComponent<TilemapRenderer>().enabled = false;
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

        public bool IsFreeToSpawn(Vector2Int position) {
            return characters.None(c => c.isActive && c.position2D == position);
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

        internal bool TryGetCharacter(string name, out CharacterControl character) {
            foreach (var c in characters) {
                if (c.name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) {
                    character = c;
                    return true;
                }
            }

            character = default;
            return false;
        }

        internal IEnumerable<Vector2Int> FindPositionsOfType(string type) {
            switch (type) {
                case "off":
                    return special
                        .GetUsedTiles()
                        .Where(t => t.Item2 == tiles.off)
                        .Select(t => t.Item1.SwizzleXY())
                        .Where(IsFreeToSpawn);
                case "field":
                    break;
            }

            return Enumerable.Empty<Vector2Int>();
        }
    }
}