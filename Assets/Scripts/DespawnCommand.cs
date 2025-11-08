using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class DespawnCommand : ICommand {
        readonly CharacterControl character;
        readonly MapControl map;

        Vector2Int? targetPosition;

        public DespawnCommand(CharacterControl character, MapControl map) {
            this.character = character;
            this.map = map;
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            if (!character.isActive) {
                return true;
            }

            if (!targetPosition.HasValue) {
                var positions = map.FindPositionsOfType("off").ToList();
                if (positions.Count == 0) {
                    return false;
                }

                targetPosition = positions.RandomElement();
            }

            if ((character.position2D + character.facing) == targetPosition) {
                if (character.state == ECharacterState.Idle) {
                    character.intendedMove = Vector2Int.zero;
                    character.isActive = false;
                    return true;
                }

                return false;
            }

            GoToCommand.SetMoveIntention(character, map, targetPosition.Value);

            return false;
        }
    }
}