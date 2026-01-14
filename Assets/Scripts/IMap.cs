using UnityEngine;

namespace BloomingCommunity.Runtime {
    interface IMap {
        bool IsFreeToMove(Vector2Int position);
    }
}