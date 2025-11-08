namespace BloomingCommunity.Runtime {
    class SayCommand : ICommand {
        readonly CharacterControl character;
        readonly string text;

        int index = 0;

        public SayCommand(CharacterControl character, string text) {
            this.character = character;
            this.text = text;
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            character.Say(text[..index]);
            index++;
            return index < text.Length;
        }
    }
}