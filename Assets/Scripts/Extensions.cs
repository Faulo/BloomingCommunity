using System.Collections;
using FMOD.Studio;
using FMODUnity;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    static class Extensions {
        public static EventInstance PlayOnce(this in EventReference reference) {
            if (reference.IsNull) {
                return default;
            }

            var instance = RuntimeManager.CreateInstance(reference);
            instance.start();
            return instance;
        }
        public static Coroutine PlayRepeatedly(this in EventReference reference, MonoBehaviour context, float interval) {
            return context.StartCoroutine(PlayRepeatedly_Co(reference, interval));
        }
        static IEnumerator PlayRepeatedly_Co(EventReference reference, float interval) {
            while (true) {
                var instance = RuntimeManager.CreateInstance(reference);
                instance.start();
                yield return Wait.forSeconds[interval];
            }
        }
    }
}