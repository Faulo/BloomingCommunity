namespace BloomingCommunity.Runtime {
    sealed class SayCommand : ICommand {
        readonly CharacterControl character;
        readonly string text;

        public SayCommand(CharacterControl character, string text) {
            this.character = character;
            this.text = text;
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            return false;
        }
    }
}