#nullable enable
using UnityEngine;

namespace BloomingCommunity.Runtime {
    interface IMapMoveCalculator {
        Vector2Int CalculateMoveIntention(Vector2Int startPosition, IMap map, Vector2Int targetPosition);
    }
}