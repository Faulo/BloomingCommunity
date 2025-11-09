using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BloomingCommunity.Runtime {
    sealed class CommunityControl {
        readonly CommunityAsset asset;
        readonly MapControl map;
        ECommunityState state;

        public readonly List<Story> stories = new();
        public readonly Dictionary<Story, string> storyNames = new();
        readonly List<ICommand> commands = new();

        Story currentStory;

        public CommunityControl(CommunityAsset asset, MapControl map) {
            this.asset = asset;
            this.map = map;

            foreach (var cutscene in asset.cutscenes) {
                var story = new Story(cutscene.text);
                stories.Add(story);
                storyNames[story] = cutscene.name;
            }

            WaitForTravellers();
        }

        float stateTimer;

        public void FixedUpdate(float deltaTime) {
            switch (state) {
                case ECommunityState.None:
                    stateTimer -= deltaTime;
                    if (stateTimer <= 0) {
                        state = ECommunityState.StartCutscene;

                        if (stateTimer < 0) {
                            FixedUpdate(Mathf.Abs(stateTimer));
                        }

                        return;
                    }

                    break;
                case ECommunityState.StartCutscene:
                    if (TryStartCutscene()) {
                        state = ECommunityState.PlayCutscene;
                    } else {
                        WaitForTravellers();
                    }

                    break;
                case ECommunityState.PlayCutscene:
                    if (TryProcessCommands(deltaTime)) {
                        return;
                    }

                    if (currentStory.canContinue) {
                        string text = currentStory.Continue();
                        foreach (string tag in currentStory.currentTags) {
                            if (TryParse(tag, text, out var command)) {
                                commands.Add(command);
                            }
                        }
                    } else {
                        stories.Remove(currentStory);
                        currentStory = null;

                        WaitForDespawn();
                    }

                    break;
                case ECommunityState.EndCutscene:
                    if (TryProcessCommands(deltaTime)) {
                        return;
                    }

                    WaitForTravellers();
                    break;
            }
        }

        bool TryProcessCommands(float deltaTime) {
            if (commands.Count > 0) {
                for (int i = 0; i < commands.Count; i++) {
                    if (commands[i].TryUpdateAndFinish(deltaTime)) {
                        commands.RemoveAt(i);
                        i--;
                    }
                }

                return true;
            }

            return false;
        }

        void WaitForTravellers() {
            stateTimer = Random.Range(asset.minWaitForTravellers, asset.maxWaitForTravellers);
            state = ECommunityState.None;
        }

        internal void ForceStartCutscene(Story story) {
            currentStory = story;
            state = ECommunityState.PlayCutscene;
        }

        bool TryStartCutscene() {
            for (int i = 0; i < stories.Count; i++) {
                SetUpStory(stories[i]);
            }

            currentStory = stories.Where(s => s.canContinue).DefaultIfEmpty().RandomElement();
            return currentStory is not null;
        }

        void SetUpStory(Story story) {
            foreach (var (key, getter) in map.storyVariables) {
                if (story.variablesState.GlobalVariableExistsWithName(key)) {
                    int value = getter();
                    story.variablesState[key] = value;
                }
            }

            story.ChoosePathString("requirements");
        }

        void WaitForDespawn() {
            foreach (var character in map.characters.Where(c => c.isActive)) {
                commands.Add(new DespawnCommand(character, map));
            }

            state = ECommunityState.EndCutscene;
        }

        internal bool TryParse(string tag, string text, out ICommand command) {
            command = default;

            string[] args = tag.ToLower().Split(' ');

            if (!map.TryGetCharacter(args[0], out var character)) {
                Debug.LogWarning($"Unknown character '{args[0]}'!");
                return false;
            }

            command = args[1] switch {
                "spawn" => new SpawnCommand(character, map, args.ElementAtOrDefault(2)),
                "despawn" => new DespawnCommand(character, map, args.ElementAtOrDefault(2)),
                "goto" => new GoToCommand(character, map, args[2]),
                "say" => new SayCommand(character, text),
                "plant" => new PlantCommand(character, map, args[2]),
                _ => default
            };

            if (command is null) {
                Debug.LogWarning($"Unknown character command '{args[1]}' (Full command: '{tag}')!");
                return false;
            }

            return true;
        }
    }
}