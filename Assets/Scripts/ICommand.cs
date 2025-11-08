namespace BloomingCommunity.Runtime {
    interface ICommand {
        bool TryUpdateAndFinish(float deltaTime);
    }
}