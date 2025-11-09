using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Slothsoft.UnityExtensions;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace BloomingCommunity.Runtime {
    sealed class CommunityControl {
        readonly CommunityAsset asset;
        readonly MapControl map;
        ECommunityState state;

        public readonly List<Story> stories = new();
        public readonly Dictionary<Story, string> storyNames = new();
        readonly List<ICommand> commands = new();

        Story currentStory;

        public event Action onCompleteStories;

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
                        if (stories.Count > 0) {
                            state = ECommunityState.StartCutscene;
                        } else {
                            state = ECommunityState.StoriesCompleted;
                        }

                        if (stateTimer < 0) {
                            FixedUpdate(Mathf.Abs(stateTimer));
                        }

                        return;
                    }

                    break;
                case ECommunityState.StartCutscene:
                    if (TryStartCutscene()) {
                        state = ECommunityState.PlayCutscene;
                        ProcessStoryTags();
                    } else {
                        WaitForTravellers();
                    }

                    break;
                case ECommunityState.PlayCutscene:
                    if (TryProcessCommands(deltaTime)) {
                        return;
                    }

                    if (currentStory.canContinue) {
                        currentStory.Continue();
                        ProcessStoryTags();
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
                case ECommunityState.StoriesCompleted:
                    onCompleteStories?.Invoke();
                    state = ECommunityState.Done;
                    break;
                case ECommunityState.Done:
                    break;
            }
        }

        void ProcessStoryTags() {
            foreach (string tag in currentStory.currentTags) {
                if (TryParse(tag, currentStory.currentText, out var command)) {
                    commands.Add(command);
                }
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

        float randomWait => URandom.Range(asset.minWaitForTravellers, asset.maxWaitForTravellers);

        void WaitForTravellers() {
            stateTimer = randomWait;
            state = ECommunityState.None;
        }

        internal void ForceStartCutscene(Story story) {
            SetUpStory(story);
            currentStory = story;
            state = ECommunityState.PlayCutscene;
            ProcessStoryTags();
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
            story.Continue();
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

            switch (args[0]) {
                case "wait":
                    command = new WaitCommand(randomWait);
                    return true;
            }

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
                "harvest" => new HarvestCommand(character, map, args[2]),
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