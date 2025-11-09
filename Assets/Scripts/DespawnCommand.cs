using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class DespawnCommand : ICommand {
        readonly CharacterControl character;
        readonly MapControl map;
        readonly string target;

        Vector2Int? targetPosition;

        public DespawnCommand(CharacterControl character, MapControl map, string target = null) {
            this.character = character;
            this.map = map;
            this.target = target ?? "off";
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            if (!character.isActive) {
                return true;
            }

            if (!targetPosition.HasValue) {
                if (map.IsPositionsOfType(character.position2D, target) || map.IsPositionsOfType(character.selectedPosition2D, target)) {
                    character.intendedMove = Vector2Int.zero;
                    character.isActive = false;
                    return true;
                }

                var positions = map.FindPositionsOfType(target).ToList();
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