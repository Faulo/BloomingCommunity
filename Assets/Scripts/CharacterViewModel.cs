using Unity.Properties;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class CharacterViewModel : ScriptableObject {
        [CreateProperty]
        public string speech;
    }
}