using FMODUnity;
using MyBox;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class CharacterAsset : ScriptableObject {
        [SerializeField]
        public GameObject prefab;
        [SerializeField]
        public RuntimeAnimatorController animator;
        [Tag]
        [SerializeField]
        public string tag;

        [Header("Speech")]
        [SerializeField]
        public float letterDuration = 0.1f;
        [SerializeField]
        public float speechPause = 2;

        [Header("Movement")]
        [SerializeField]
        public float moveDuration = 1;
        [SerializeField]
        public float facingDuration = 10;
        [SerializeField]
        public float blockedDuration = 0;

        [Header("Audio")]
        [SerializeField]
        public EventReference stepEvent = new();
        [SerializeField]
        internal float stepInterval = 0.1f;
        [SerializeField]
        public EventReference bonkEvent = new();
        [SerializeField]
        public EventReference growEvent = new();
    }
}