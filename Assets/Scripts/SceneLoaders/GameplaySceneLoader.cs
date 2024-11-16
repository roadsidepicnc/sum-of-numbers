using System;
using Cysharp.Threading.Tasks;
using Gameplay;
using GridManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Utilities.Signals;
using Zenject;

public class GameplaySceneLoader : SceneLoader
{
    [Inject] private SafeSpaceAdjuster _safeSpaceAdjuster;
    [Inject] private ObjectPoolManager _objectPoolManager;
    [Inject] private PopupManager _popupManager;
    [Inject] private GridManager _gridManager;
    [Inject] private GameplayManager _gameplayManager;
    [Inject] private HeartManager _heartManager;
    [Inject] private TargetScoreManager _targetScoreManager;
    [Inject] private CircleManager _circleManager;

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
            Debug.LogError("Gameplay Scene managers couldn't be initialized");
            return;
        }
        
        await UniTask.WaitUntil(() => _safeSpaceAdjuster.IsInitialized);
        
        SignalBus.Fire(new GameStateChangedSignal(GameState.ManagersAreInitialized));
    }

    protected override void AddManagers()
    {
        Managers.Add(_objectPoolManager); 
        Managers.Add(_popupManager);
        Managers.Add(_gridManager);
        Managers.Add(_gameplayManager);
        Managers.Add(_heartManager);
        Managers.Add(_circleManager);
        Managers.Add(_targetScoreManager);
    }
}