#nullable enable
using UnityEngine;
using URandom = UnityEngine.Random;

namespace BloomingCommunity.Runtime {
    readonly struct MoveRandomnly : IMapMoveCalculator {
        internal static MoveRandomnly instance = new();

        public Vector2Int CalculateMoveIntention(Vector2Int startPosition, IMap map, Vector2Int targetPosition) {
            return URandom.Range(0, 5) switch {
                1 => Vector2Int.up,
                2 => Vector2Int.down,
                3 => Vector2Int.left,
                4 => Vector2Int.right,
                _ => Vector2Int.zero
            };
        }
    }
}