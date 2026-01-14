#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace BloomingCommunity.Runtime {
    sealed class AStarFaulo : IMapMoveCalculator {
        internal static AStarFaulo instance = new();

        sealed class SortIntegersNoDuplicates : IComparer<int> {
            public int Compare(int x, int y) {
                return x == y ? -1 : x.CompareTo(y);
            }
        }

        sealed class SortPositionsWithHeuristic : IComparer<Vector2Int> {
            readonly Dictionary<Vector2Int, int> goalScores;
            readonly Vector2Int targetPosition;

            public SortPositionsWithHeuristic(Dictionary<Vector2Int, int> goalScores, Vector2Int targetPosition) {
                this.goalScores = goalScores;
                this.targetPosition = targetPosition;
            }

            public int Compare(Vector2Int a, Vector2Int b) {
                return (goalScores[a] + HeuristicScore(a, targetPosition)).CompareTo(goalScores[b] + HeuristicScore(b, targetPosition));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int HeuristicScore(Vector2Int a, Vector2Int b) {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        public Vector2Int CalculateMoveIntention(Vector2Int startPosition, IMap map, Vector2Int targetPosition) {
            if (startPosition == targetPosition) {
                return Vector2Int.zero;
            }

            int capacity = HeuristicScore(targetPosition, startPosition);

            Dictionary<Vector2Int, bool> isFreeCache = new(capacity) {
                [targetPosition] = false,
                [startPosition] = true,
            };

            Dictionary<Vector2Int, int> goalScores = new(capacity) {
                [targetPosition] = 0
            };

            Dictionary<Vector2Int, Vector2Int> cameFrom = new(capacity);

            SortedList<int, Vector2Int> queuedPositions = new(capacity, new SortIntegersNoDuplicates()) {
                { 0, targetPosition }
            };

            ReadOnlySpan<Vector2Int> directions = stackalloc Vector2Int[] {
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left
            };

            while (queuedPositions.Count > 0) {
                var currentPosition = queuedPositions.Values[0];
                queuedPositions.RemoveAt(0);

                if (currentPosition == startPosition) {
                    return cameFrom[startPosition] - startPosition;
                }

                int randomDirection = URandom.Range(0, 4);

                for (int i = 0; i < 4; i++) {
                    var direction = directions[(i + randomDirection) & 3];
                    var neighborPosition = currentPosition + direction;

                    if (!isFreeCache.TryGetValue(neighborPosition, out bool isFree)) {
                        isFreeCache[neighborPosition] = isFree = map.IsFreeToMove(neighborPosition);
                    }

                    if (isFree) {
                        int newScore = goalScores[currentPosition] + 1;
                        if (!goalScores.TryGetValue(neighborPosition, out int oldScore) || oldScore > newScore) {
                            goalScores[neighborPosition] = newScore;
                            cameFrom[neighborPosition] = currentPosition;
                            int heuristicScore = newScore + HeuristicScore(neighborPosition, targetPosition);
                            queuedPositions.Add(heuristicScore, neighborPosition);
                        }
                    }
                }
            }

            return MoveRandomnly.instance.CalculateMoveIntention(startPosition, map, targetPosition);
        }
    }
}