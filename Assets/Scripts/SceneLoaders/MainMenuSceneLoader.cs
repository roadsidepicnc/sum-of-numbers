using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using InputManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Utilities.Signals;
using Zenject;

public class MainMenuSceneLoader : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [Inject] private ObjectPoolManager _objectPoolManager;
    [Inject] private InputManager _inputManager;
    [Inject] private PopupManager _popupManager;
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
        
        _managers.Add(_inputManager);
        _managers.Add(_objectPoolManager);
        _managers.Add(_popupManager);
        
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