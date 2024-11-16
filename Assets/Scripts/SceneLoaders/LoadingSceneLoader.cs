using System;
using Cysharp.Threading.Tasks;
using LevelManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Signals;
using Zenject;

public class LoadingSceneLoader : SceneLoader
{
    [Inject] private LevelManager _levelManager;
    [Inject] private GameManager _gameManager;

    protected override async void Initialize()
    {
        SignalBus.Fire(new GameStateChangedSignal(GameState.ManagersAreInitializing));
        
        try
        {
            await InitializeManagers();
        }
        catch (OperationCanceledException e)
        {
            Debug.LogError("Loading Scene managers couldn't be initialized");
            return;
        }
        
        SignalBus.Fire(new GameStateChangedSignal(GameState.ManagersAreInitialized));
        
        await SceneManager.LoadSceneAsync("MainMenu");
    }

    protected override void AddManagers()
    {
        Managers.Add(_levelManager);
        Managers.Add(_gameManager);
    }
}