using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using GridManagement;
using LevelManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Utilities.Signals;
using Zenject;

public class GameplaySceneLoader : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [Inject] private ObjectPoolManager _objectPoolManager;
    [Inject] private PopupManager _popupManager;
    [Inject] private GridManager _gridManager;
    [Inject] private GameplayManager _gameplayManager;
    [Inject] private HeartManager _heartManager;
    [Inject] private TargetScoreManager _targetScoreManager;
    [Inject] private CircleManager _circleManager;
    [Inject] private SafeSpaceAdjuster _safeSpaceAdjuster;
    
    private List<Manager> _managers;
    
    private void Start()
    {
        Initialize();
    }

    private async void Initialize()
    {
        _signalBus.Fire(new GameStateChangedSignal(GameState.ManagersAreInitializing));
        
        _safeSpaceAdjuster.Initialize();
        
        _managers = new();
        
        _managers.Add(_objectPoolManager);
        _managers.Add(_popupManager);
        _managers.Add(_gridManager);
        _managers.Add(_gameplayManager);
        _managers.Add(_heartManager);
        _managers.Add(_circleManager);
        _managers.Add(_targetScoreManager);
        
        foreach (var manager in _managers)
        {
            manager.Initialize();
        }
        
        await UniTask.WaitUntil(AreAllManagersInitialized);
        await UniTask.WaitUntil(() => _safeSpaceAdjuster.IsInitialized);
        
        _signalBus.Fire(new GameStateChangedSignal(GameState.ManagersAreInitialized));
        
        return;

        bool AreAllManagersInitialized()
        {
            foreach (var manager in _managers)
            {
                if (!manager.IsInitialized)
                {
                    return false;
                }
            }

            return true;
        }
    }
}