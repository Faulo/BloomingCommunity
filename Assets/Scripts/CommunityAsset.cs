using System;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class CommunityAsset : ScriptableObject {
        [SerializeField]
        public float minWaitForTravellers = 1;
        [SerializeField]
        public float maxWaitForTravellers = 3;

        [Space]
        [SerializeField]
        public TextAsset[] cutscenes = Array.Empty<TextAsset>();
    }
}