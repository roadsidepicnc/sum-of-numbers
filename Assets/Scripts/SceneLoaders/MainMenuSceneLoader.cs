using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Zenject;

public class MainMenuSceneLoader : MonoBehaviour
{
    [Inject] private ObjectPoolManager _objectPoolManager;
    [Inject] private GameManager _gameManager;
    [Inject] private SignalManager _signalManager;
    [Inject] private SafeSpaceAdjuster _safeSpaceAdjuster;
    
    private List<Manager> _managers;
    
    private void Awake()
    {
        Initialize();
    }

    private async void Initialize()
    {
        _gameManager.SetGameState(GameState.SceneLoading);
        
        _safeSpaceAdjuster.Initialize();
        
        _managers = new();
        
        _managers.Add(_objectPoolManager);
        
        foreach (var manager in _managers)
        {
            manager.Initialize();
        }
        
        await UniTask.WaitUntil(AreAllManagersInitialized);
        await UniTask.WaitUntil(() => _safeSpaceAdjuster.IsInitialized);
        
        _gameManager.SetGameState(GameState.SceneLoaded);
        
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