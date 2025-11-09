namespace BloomingCommunity.Runtime {
    class WaitCommand : ICommand {
        float timer;

        public WaitCommand(float waitTime) {
            timer = waitTime;
        }

        public bool TryUpdateAndFinish(float deltaTime) {
            timer -= deltaTime;
            return timer < 0;
        }
    }
}