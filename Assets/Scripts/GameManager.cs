using Gameplay;
using Utilities.Signals;
using Zenject;

public class GameManager : Manager
{
    [Inject] private SignalBus _signalBus;
    
    public static GameState GameState { get; private set; }
    
    private void Start()
    {
        Initialize();
    }
    
    public override void Initialize()
    {
        IsInitialized = true;
    }

    public void SetGameState(GameState gameState)
    {
        GameState = gameState;
        _signalBus.Fire(new GameStateChangedSignal(gameState));
    }
}