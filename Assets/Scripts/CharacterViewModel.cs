using Unity.Properties;

namespace BloomingCommunity.Runtime {
    sealed class CharacterViewModel {
        readonly CharacterControl character;

        [CreateProperty(ReadOnly = true)]
        public string speech => character.speechText;

        public CharacterViewModel(CharacterControl character) {
            this.character = character;
        }
    }
}