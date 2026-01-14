#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    readonly struct AStarChatGPT : IMapMoveCalculator {
        internal static AStarChatGPT instance = new();

        public Vector2Int CalculateMoveIntention(Vector2Int startPosition, IMap map, Vector2Int targetPosition) {
            if (startPosition == targetPosition) {
                return Vector2Int.zero;
            }

            // --- Helpers ------------------------------------------------------------
            static int sign(int v) => v < 0 ? -1 : (v > 0 ? 1 : 0);
            static int heuristic(in Vector2Int a, in Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan

            // Binary min-heap over (pos, f, g) implemented via parallel lists to avoid extra types.
            var heapPos = new List<Vector2Int>(128);
            var heapF = new List<int>(128);
            var heapG = new List<int>(128);

            bool less(int i, int j) {
                // primary: smaller f
                // tie-break: larger g (prefers longer-known paths; tends to reduce dithering)
                int fi = heapF[i], fj = heapF[j];
                if (fi != fj) {
                    return fi < fj;
                }

                return heapG[i] > heapG[j];
            }

            void swap(int i, int j) {
                (heapPos[i], heapPos[j]) = (heapPos[j], heapPos[i]);
                (heapF[i], heapF[j]) = (heapF[j], heapF[i]);
                (heapG[i], heapG[j]) = (heapG[j], heapG[i]);
            }

            void heapPush(Vector2Int pos, int f, int g) {
                int i = heapPos.Count;
                heapPos.Add(pos);
                heapF.Add(f);
                heapG.Add(g);

                while (i > 0) {
                    int parent = (i - 1) >> 1;
                    if (!less(i, parent)) {
                        break;
                    }

                    swap(i, parent);
                    i = parent;
                }
            }

            bool heapTryPop(out Vector2Int pos, out int g) {
                int count = heapPos.Count;
                if (count == 0) {
                    pos = default;
                    g = default;
                    return false;
                }

                pos = heapPos[0];
                g = heapG[0];

                int last = count - 1;
                heapPos[0] = heapPos[last];
                heapF[0] = heapF[last];
                heapG[0] = heapG[last];

                heapPos.RemoveAt(last);
                heapF.RemoveAt(last);
                heapG.RemoveAt(last);

                // sift down
                int i = 0;
                while (true) {
                    int left = (i << 1) + 1;
                    if (left >= heapPos.Count) {
                        break;
                    }

                    int right = left + 1;

                    int best = left;
                    if (right < heapPos.Count && less(right, left)) {
                        best = right;
                    }

                    if (!less(best, i)) {
                        break;
                    }

                    swap(i, best);
                    i = best;
                }

                return true;
            }

            // --- A* -----------------------------------------------------------------
            int manhattan = heuristic(startPosition, targetPosition);

            // Without map bounds, we need a budget so we can't explode on infinite grids.
            // Also a soft bounding box around start/goal to avoid wandering too far.
            const int maxExpansions = 4096;
            int padding = Mathf.Clamp((manhattan * 3) + 16, 16, 256);

            int minX = Mathf.Min(startPosition.x, targetPosition.x) - padding;
            int maxX = Mathf.Max(startPosition.x, targetPosition.x) + padding;
            int minY = Mathf.Min(startPosition.y, targetPosition.y) - padding;
            int maxY = Mathf.Max(startPosition.y, targetPosition.y) + padding;

            bool inBounds(in Vector2Int p) => p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY;

            // gScore + parent pointers
            var gScore = new Dictionary<Vector2Int, int>(256) { [startPosition] = 0 };
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>(256);

            heapPush(startPosition, f: manhattan, g: 0);

            // 4-neighborhood
            ReadOnlySpan<Vector2Int> dirs = stackalloc Vector2Int[4] {
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left
            };

            bool found = false;
            int expansions = 0;

            while (expansions++ < maxExpansions && heapTryPop(out var current, out int currentG)) {
                // stale heap entry?
                if (!gScore.TryGetValue(current, out int bestKnownG) || bestKnownG != currentG) {
                    continue;
                }

                if (current == targetPosition) {
                    found = true;
                    break;
                }

                foreach (var dir in dirs) {
                    var neighbor = current + dir;

                    if (!inBounds(neighbor)) {
                        continue;
                    }

                    // Keep original semantics: target is always allowed (might be "occupied" by an enemy etc.)
                    if (neighbor != targetPosition && !map.IsFreeToMove(neighbor)) {
                        continue;
                    }

                    int tentativeG = currentG + 1;

                    if (gScore.TryGetValue(neighbor, out int oldG) && tentativeG >= oldG) {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;

                    int f = tentativeG + heuristic(neighbor, targetPosition);
                    heapPush(neighbor, f, tentativeG);
                }
            }

            if (found) {
                // Reconstruct only the first step from 'position' towards 'targetPosition'
                var step = targetPosition;

                // Walk back until the node whose parent is the start
                while (cameFrom.TryGetValue(step, out var prev) && prev != startPosition) {
                    step = prev;
                }

                var move = step - startPosition;

                // Ensure it's exactly one grid step
                move.x = sign(move.x);
                move.y = sign(move.y);
                return move;
            }

            return MoveRandomnly.instance.CalculateMoveIntention(startPosition, map, targetPosition);
        }
    }
}