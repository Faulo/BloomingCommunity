using UnityEngine;

namespace BloomingCommunity.Runtime {
    class HarvestCommand : ICommand {
        readonly CharacterControl character;
        readonly MapControl map;
        readonly string target;

        Vector2Int? targetPosition;

        public HarvestCommand(CharacterControl character, MapControl map, string target) {
            this.character = character;
            this.map = map;
            this.target = target;
        }

        SpawnCommand spawn;
        GoToCommand go;

        bool hasHarvested = false;

        public bool TryUpdateAndFinish(float deltaTime) {
            if (!character.isActive) {
                spawn ??= new(character, map);
                if (!spawn.TryUpdateAndFinish(deltaTime)) {
                    return false;
                }
            }

            if (hasHarvested) {
                return character.state is ECharacterState.Idle;
            }

            if (map.IsPositionsOfType(character.selectedPosition2D, target)) {
                character.intendedMove = Vector2Int.zero;
                if (character.state is ECharacterState.Idle or ECharacterState.Blocked or ECharacterState.Facing) {
                    hasHarvested = true;
                    character.Harvest();
                }

                return false;
            }

            go ??= new(character, map, target);

            go.TryUpdateAndFinish(deltaTime);

            return false;
        }
    }
}