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

        public bool TryUpdateAndFinish(float deltaTime) {
            if (!character.isActive) {
                var positions = map.FindPositionsOfType("off").ToList();
                if (positions.Count == 0) {
                    return false;
                }

                character.TeleportTo(positions.RandomElement());
                character.isActive = true;
                return false;
            }

            if (!targetPosition.HasValue || !map.IsFreeToMove(targetPosition.Value)) {
                var positions = map.FindPositionsOfType(target).ToList();
                if (positions.Count == 0) {
                    return false;
                }

                targetPosition = positions.RandomElement();
            }

            if ((character.position2D + character.facing) == targetPosition) {
                character.intendedMove = Vector2Int.zero;
                return true;
            }

            SetMoveIntention(character, map, targetPosition.Value);

            return false;
        }

        static int Sign(int i) => i < 0 ? -1 : 1;

        internal static void SetMoveIntention(CharacterControl character, MapControl map, Vector2Int targetPosition) {
            var delta = targetPosition - character.position2D;

            var verticalMove = Vector2Int.up * Sign(delta.y);
            var horizontalMove = Vector2Int.right * Sign(delta.x);

            bool canMoveVertical = map.IsFreeToMove(character.position2D + verticalMove);
            bool canMoveHorizontal = map.IsFreeToMove(character.position2D + horizontalMove);
            bool preferVertical = Mathf.Abs(delta.y * URandom.value) > Mathf.Abs(delta.x * URandom.value);

            character.intendedMove = (canMoveVertical, canMoveHorizontal, preferVertical) switch {
                (true, _, true) => verticalMove,
                (_, true, _) => horizontalMove,
                (_, _, true) => -horizontalMove,
                _ => -verticalMove,
            };
        }
    }
}