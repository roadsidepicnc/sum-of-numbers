using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine.SceneManagement;
using Utilities.Signals;
using Zenject;

public class GameManager : Manager
{
    [Inject] private SignalBus _signalBus;
    
    public static GameState GameState { get; private set; }
    public static SceneState SceneState { get; private set; }
    
    public override void Initialize()
    {
        base.Initialize();
        
        IsInitialized = true;
    }
    

    public override void Subscribe()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        _signalBus.Subscribe<SceneStateChangedSignal>(OnSceneStateChanged);
    }

    public override void Unsubscribe()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        _signalBus.Unsubscribe<SceneStateChangedSignal>(OnSceneStateChanged);
    }

    private async void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
    {
        switch (SceneState)
        {
            case SceneState.MainMenu:
                switch (gameStateChangedSignal.GameState)
                {
                    case GameState.SceneIsChanged:
                        await SceneManager.LoadSceneAsync("Gameplay");
                        break;
                }
                break;
            case SceneState.Gameplay:
                switch (gameStateChangedSignal.GameState)
                {
                    case GameState.SceneIsChanged:
                        await SceneManager.LoadSceneAsync("MainMenu");
                        break;
                    case GameState.SceneIsReloaded:
                        await SceneManager.LoadSceneAsync("Gameplay");
                        break;
                }
                break;
        }

        GameState = gameStateChangedSignal.GameState;
    }

    private void OnSceneStateChanged(SceneStateChangedSignal sceneStateChangedSignal)
    {
        SceneState = sceneStateChangedSignal.SceneState;
    }
}