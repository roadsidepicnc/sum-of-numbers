using Gameplay;
using Utilities.Signals;
using Zenject;

public class GameManager : Manager
{
    [Inject] private SignalBus _signalBus;
    
    public static GameState GameState { get; private set; }
    
    public override void Initialize()
    {
        base.Initialize();
        
        IsInitialized = true;
    }

    public override void Subscribe()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    public override void Unsubscribe()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
    {
        GameState = gameStateChangedSignal.GameState;
    }
}