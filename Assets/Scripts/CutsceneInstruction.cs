namespace BloomingCommunity.Runtime {
    sealed class CutsceneInstruction {
        readonly MapControl map;
        public CutsceneInstruction(CharacterControl character, MapControl map) {
            this.map = map;
        }
    }
}