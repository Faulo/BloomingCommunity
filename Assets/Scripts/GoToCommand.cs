using System.Linq;
using Slothsoft.UnityExtensions;

namespace BloomingCommunity.Runtime {
    sealed class GoToCommand : ICommand {
        readonly CharacterControl character;
        readonly MapControl map;
        readonly string target;

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

            return false;
        }
    }
}