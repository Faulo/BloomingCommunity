namespace BloomingCommunity.Runtime {
    class SayCommand : ICommand {
        readonly CharacterControl character;
        readonly string text;

        public SayCommand(CharacterControl character, string text) {
            this.character = character;
            this.text = text;

            character.Say(text);
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            return !character.isSpeaking;
        }
    }
}