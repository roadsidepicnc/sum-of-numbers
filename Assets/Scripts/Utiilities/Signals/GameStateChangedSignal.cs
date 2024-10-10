namespace Utilities.Signals
{
    public struct GameStateChangedSignal
    {
        public GameState GameState { get; private set; }

        public GameStateChangedSignal(GameState gameState)
        {
            GameState = gameState;
        }
    }
}