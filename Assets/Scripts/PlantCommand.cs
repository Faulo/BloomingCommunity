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
            if (map.IsFreeToPlant(character.selectedPosition2D) && character.state == ECharacterState.Idle) {
                character.Plant(plant);
            }

            return character.state == ECharacterState.Idle;
        }
    }
}