using FMODUnity;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class FMODEventPlayer : MonoBehaviour {
        Coroutine coroutine;

        public void PlayRepeatedly(in EventReference reference, float interval) {
            if (coroutine is not null) {
                StopCoroutine(coroutine);
            }

            coroutine = reference.PlayRepeatedly(this, interval);
        }

        public void StopPlaying() {
            if (coroutine is not null) {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}