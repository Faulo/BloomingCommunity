using System;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class GoToCommand : ICommand {
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

            var delta = targetPosition.Value - character.position2D;

            character.intendedMove = Mathf.Abs(delta.y) > Mathf.Abs(delta.x)
                ? Vector2Int.up * Math.Sign(delta.y)
                : Vector2Int.right * Math.Sign(delta.x);

            return false;
        }
    }
}