using System;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class ObjectRunner : MonoBehaviour {
        internal event Action<float> onUpdate;
        internal event Action<float> onFixedUpdate;

        void Update() {
            onUpdate?.Invoke(Time.deltaTime);
        }

        void FixedUpdate() {
            onFixedUpdate?.Invoke(Time.deltaTime);
        }
    }
}