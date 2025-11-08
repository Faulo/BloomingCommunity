using UnityEngine;

namespace BloomingCommunity.Runtime {
    class PlantCommand : ICommand {
        readonly CharacterControl character;
        readonly MapControl map;
        readonly string plant;

        public PlantCommand(CharacterControl character, MapControl map, string plant) {
            this.character = character;
            this.map = map;
            this.plant = plant;
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            if (map.IsFreeToPlant(character.selectedPosition2D)) {
                Debug.Log($"{character.name}: {plant}");
                return true;
            } else {
                return true;
            }
        }
    }
}