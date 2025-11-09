using System;
using System.Collections;
using NativeWebSocket;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    [CreateAssetMenu]
    sealed class APICall : ScriptableObject {
        [SerializeField]
        string url = "";
        [SerializeField]
        SerializableKeyValuePairs<string, string> parameters = new();

        string fullUrl {
            get {
                string fullUrl = url;

                bool first = true;
                foreach (var (key, value) in parameters) {
                    if (first) {
                        first = false;
                        fullUrl += "?";
                    } else {
                        fullUrl += "&";
                    }

                    fullUrl += Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value);
                }

                return fullUrl;
            }
        }

        public void CallAPI() {
            var obj = new GameObject(fullUrl);
            DontDestroyOnLoad(obj);

            var runner = obj.AddComponent<ObjectRunner>();
            runner.StartCoroutine(CallWeckSocket_Co(fullUrl, obj));
        }

        static IEnumerator CallWeckSocket_Co(string url, GameObject obj) {
            var socket = new WebSocket(url);
            socket.OnError += Debug.LogError;

            var connect = socket.Connect();
            yield return new WaitUntil(() => connect.IsCompleted);

            var close = socket.Close();
            yield return new WaitUntil(() => close.IsCompleted);

            Destroy(obj);
        }
    }
}