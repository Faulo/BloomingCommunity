#nullable enable
using System.Linq;
using BloomingCommunity.Runtime;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace BloomingCommunity.Tests.EditMode {
    [TestFixture(typeof(AStarChatGPT))]
    [TestFixture(typeof(AStarFaulo))]
    [TestOf(typeof(IMapMoveCalculator))]
    sealed class IMapMoveCalculatorTests<T> where T : IMapMoveCalculator, new() {
        readonly T sut = new();

        sealed class Map : IMap {
            readonly char[,] tiles;

            public Map(char[,] tiles) => this.tiles = tiles;

            bool IsInBounds(Vector2Int position) => position is { x: >= 0, y: >= 0 }
                && position.x < tiles.GetLength(0)
                && position.y < tiles.GetLength(1);

            public bool IsFreeToMove(Vector2Int position) => IsInBounds(position) && tiles[position.x, position.y] == '.';
        }

        public IMap CreateMap(string map) {
            var rows = map
                .Split('\n')
                .Select(s => s.Trim())
                .Where(s => s != string.Empty)
                .ToList();
            char[,] tiles = new char[rows[0].Length, rows.Count];

            for (int x = 0; x < rows[0].Length; x++) {
                for (int y = 0; y < rows.Count; y++) {
                    tiles[x, y] = rows[y][x];
                }
            }

            return new Map(tiles);
        }

        const string MAP_SMALL = @"
.#..
....
.#..
";

        const string MAP_BIG = @"
.#..................
.#.#................
.#..................
.#.#................
.#..................
.#.#............###.
.#.....#........#...
.#.#..#.....#.....#.
.#...#..#.........#.
...#....#.........#.
";

        [Test]
        [TestCase(-1, 0, false)]
        [TestCase(0, -1, false)]
        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(1, 2, false)]
        [TestCase(3, 2, true)]
        [TestCase(4, 2, false)]
        [TestCase(3, 3, false)]
        public void T00_GivenSmallMap_WhenIsFreeToMove_ThenWork(int x, int y, bool expected) {
            var map = CreateMap(MAP_SMALL);
            var position = new Vector2Int(x, y);

            bool actual = map.IsFreeToMove(position);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        public void T10_GivenPositionEqualsTarget_ThenNoIntention(int x, int y) {
            var map = CreateMap(MAP_SMALL);
            var position = new Vector2Int(x, y);

            var actual = sut.CalculateMoveIntention(position, map, position);

            Assert.That(actual, Is.EqualTo(Vector2Int.zero));
        }

        [Test]
        [TestCase(1, 1, 0, 1, -1, 0)]
        [TestCase(1, 1, 2, 1, 1, 0)]
        [TestCase(1, 1, 1, 0, 0, -1)]
        [TestCase(1, 1, 1, 2, 0, 1)]
        public void T11_GivenPositionNextToTarget_ThenReturnDelta(int startX, int startY, int targetX, int targetY, int expectedX, int expectedY) {
            var map = CreateMap(MAP_SMALL);

            var actual = sut.CalculateMoveIntention(new(startX, startY), map, new(targetX, targetY));

            Assert.That(actual, Is.EqualTo(new Vector2Int(expectedX, expectedY)));
        }

        [Test]
        [TestCase(0, 0, 2, 0, 0, 1)]
        [TestCase(0, 1, 2, 1, 1, 0)]
        [TestCase(0, 2, 2, 2, 0, -1)]
        public void T12_GivenWallInTheWay_ThenMoveAround(int startX, int startY, int targetX, int targetY, int expectedX, int expectedY) {
            var map = CreateMap(MAP_SMALL);

            var actual = sut.CalculateMoveIntention(new(startX, startY), map, new(targetX, targetY));

            Assert.That(actual, Is.EqualTo(new Vector2Int(expectedX, expectedY)));
        }

        const int WARMUP_COUNT = 3;
        const int MEASUREMENT_COUNT = 20;
        const int ITERATIONS_COUNT = 100;

        [Performance, Test]
        [TestCase(false, 0, 0, 3, 2)]
        [TestCase(true, 0, 0, 19, 9)]
        public void B00_CalculateMoveIntention(bool useBigMap, int startX, int startY, int targetX, int targetY) {
            var map = CreateMap(useBigMap ? MAP_BIG : MAP_SMALL);
            var start = new Vector2Int(startX, startY);
            var target = new Vector2Int(targetX, targetY);

            Measure
                .Method(() => sut.CalculateMoveIntention(start, map, target))
                .WarmupCount(WARMUP_COUNT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .IterationsPerMeasurement(ITERATIONS_COUNT)
                .Run();
        }
    }
}