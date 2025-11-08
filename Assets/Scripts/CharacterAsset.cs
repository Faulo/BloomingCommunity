using UnityEngine;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class CharacterAsset : ScriptableObject {
        [SerializeField]
        public float moveDuration = 1;
        [SerializeField]
        public float facingDuration = 10;
        [SerializeField]
        public float blockedDuration = 0;
    }
}