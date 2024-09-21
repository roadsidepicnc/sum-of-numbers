using Gameplay;
using Zenject;

public class GameManager : Manager
{
    [Inject] private SignalManager _signalManager;
    
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
        _signalManager.GameStateChanged?.Invoke(gameState);
    }
}