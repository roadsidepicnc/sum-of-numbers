namespace Utilities.Signals
{
    public class GameStateChangedSignal
    {
        public GameState GameState { get; private set; }

        public GameStateChangedSignal(GameState gameState)
        {
            GameState = gameState;
        }
    }
}