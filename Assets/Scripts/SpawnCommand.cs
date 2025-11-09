using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class SpawnCommand : ICommand {
        readonly CharacterControl character;
        readonly MapControl map;
        readonly string target;

        public SpawnCommand(CharacterControl character, MapControl map, string target = null) {
            this.character = character;
            this.map = map;
            this.target = target ?? "off";
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            if (character.isActive) {
                return true;
            }

            var positions = map.FindPositionsOfType(target).ToList();
            if (positions.Count == 0) {
                Debug.LogWarning($"Cannot spawn {character} at '{target}', no room!");
                return false;
            }

            character.TeleportTo(positions.RandomElement());
            character.isActive = true;

            return true;
        }
    }
}