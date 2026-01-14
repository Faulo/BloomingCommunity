using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

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

        internal static void SetMoveIntention(CharacterControl character, MapControl map, Vector2Int targetPosition) {
            character.intendedMove = AStarFaulo.instance.CalculateMoveIntention(character.position2D, map, targetPosition);
        }
    }
}