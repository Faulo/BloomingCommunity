using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace BloomingCommunity.Runtime {
    class GoToCommand : ICommand {
        readonly CharacterControl character;
        readonly MapControl map;
        readonly string target;

        Vector2Int? targetPosition;

        public GoToCommand(CharacterControl character, MapControl map, string target) {
            this.character = character;
            this.map = map;
            this.target = target;
        }

        SpawnCommand spawn;

        public bool TryUpdateAndFinish(float deltaTime) {
            if (!character.isActive) {
                spawn ??= new(character, map);
                if (!spawn.TryUpdateAndFinish(deltaTime)) {
                    return false;
                }
            }

            if (map.IsPositionsOfType(character.position2D, target) || map.IsPositionsOfType(character.selectedPosition2D, target)) {
                character.intendedMove = Vector2Int.zero;
                return character.state is ECharacterState.Idle or ECharacterState.Blocked or ECharacterState.Facing;
            }

            if (!targetPosition.HasValue || !map.IsFreeToMove(targetPosition.Value)) {
                var positions = map.FindPositionsOfType(target).ToList();
                if (positions.Count == 0) {
                    return false;
                }

                targetPosition = positions.RandomElement();
            }

            SetMoveIntention(character, map, targetPosition.Value);

            return false;
        }

        static int Sign(int i) => i < 0 ? -1 : 1;

        internal static void SetMoveIntention(CharacterControl character, MapControl map, Vector2Int targetPosition) {
            var delta = targetPosition - character.position2D;

            var verticalMove = Vector2Int.up * Sign(delta.y);
            var horizontalMove = Vector2Int.right * Sign(delta.x);

            bool shouldMoveVertical = (character.position2D + verticalMove) == targetPosition;
            bool canMoveVertical = map.IsFreeToMove(character.position2D + verticalMove);
            bool shouldMoveHorizontal = (character.position2D + horizontalMove) == targetPosition;
            bool canMoveHorizontal = map.IsFreeToMove(character.position2D + horizontalMove);
            bool preferVertical = Mathf.Abs(delta.y) > Mathf.Abs(delta.x);

            character.intendedMove = (shouldMoveVertical, shouldMoveHorizontal, canMoveVertical, canMoveHorizontal, preferVertical) switch {
                (true, _, _, _, _) => verticalMove,
                (_, true, _, _, _) => horizontalMove,
                (_, _, true, _, true) => verticalMove,
                (_, _, _, true, _) => horizontalMove,
                _ => URandom.Range(0, 2) switch {
                    0 => -horizontalMove,
                    _ => -verticalMove,
                },
            };
        }
    }
}