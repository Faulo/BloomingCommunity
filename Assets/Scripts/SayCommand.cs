using UnityEngine;

namespace BloomingCommunity.Runtime {
    class SayCommand : ICommand {
        readonly CharacterControl character;
        readonly string text;

        public SayCommand(CharacterControl character, string text) {
            this.character = character;
            this.text = text;
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            Debug.Log($"{character.name}: {text}");
            return true;
        }
    }
}