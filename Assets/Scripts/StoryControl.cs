using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class StoryControl {
        readonly Story story;

        public StoryControl(TextAsset asset) {
            story = new(asset.text);
        }

        internal void SetVariables(IEnumerable<(string, object)> variables) {
            foreach (var (name, value) in variables) {
                if (story.variablesState.GlobalVariableExistsWithName(name)) {
                    story.variablesState[name] = value;
                }
            }
        }
    }
}