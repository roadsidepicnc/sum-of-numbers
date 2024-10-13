using Gameplay;
using Utilities.Signals;
using Zenject;

public class GameManager : Manager
{
    [Inject] private SignalBus _signalBus;
    
    public static GameState GameState { get; private set; }
    public static InputState InputState { get; private set; }
    
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
        _signalBus.Fire(new GameStateChangedSignal(GameState));
    }
    
    public void SetInputState(InputState inputState)
    {
        InputState = inputState;
        _signalBus.Fire(new InputStateChangedSignal(InputState));
    }
}