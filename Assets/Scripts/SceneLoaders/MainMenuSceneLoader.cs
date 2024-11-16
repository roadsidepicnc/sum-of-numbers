using System;
using Cysharp.Threading.Tasks;
using InputManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Utilities.Signals;
using Zenject;

public class MainMenuSceneLoader : SceneLoader
{
    [Inject] private SafeSpaceAdjuster _safeSpaceAdjuster;
    [Inject] private ObjectPoolManager _objectPoolManager;
    [Inject] private InputManager _inputManager;
    [Inject] private PopupManager _popupManager;

    protected override async void Initialize()
    {
        SignalBus.Fire(new GameStateChangedSignal(GameState.ManagersAreInitializing));
        _safeSpaceAdjuster.Initialize();
            
        try
        {
            await InitializeManagers();
        }
        catch (OperationCanceledException e)
        {
            Debug.LogError("Main Menu Scene managers couldn't be initialized");
            return;
        }
        
        await UniTask.WaitUntil(() => _safeSpaceAdjuster.IsInitialized);
        
        SignalBus.Fire(new SceneStateChangedSignal(SceneState.MainMenu));
        SignalBus.Fire(new GameStateChangedSignal(GameState.ManagersAreInitialized));
    }

    protected override void AddManagers()
    {
        Managers.Add(_inputManager);
        Managers.Add(_objectPoolManager);
        Managers.Add(_popupManager);
    }
}