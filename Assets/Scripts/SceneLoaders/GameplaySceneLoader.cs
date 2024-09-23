using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using GridManagement;
using LevelManagement;
using ObjectPoolingSystem;
using UI.Popup;
using UnityEngine;
using Zenject;

public class GameplaySceneLoader : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager;
    private LevelManager _levelManager;
    private PanelManager _panelManager;
    private SignalManager _signalManager;
    private GridManager _gridManager;
    private GameplayManager _gameplayManager;
    private GameManager _gameManager;
    
    private List<Manager> _managers;

    [Inject]
    public void InstallDependencies(LevelManager levelManager, ObjectPoolManager objectPoolManager,
        PanelManager panelManager, SignalManager signalManager, GridManager gridManager,
        GameplayManager gameplayManager, GameManager gameManager)
    {
        _signalManager = signalManager;
        _levelManager = levelManager;
        _panelManager = panelManager;
        _gridManager = gridManager;
        _gameplayManager = gameplayManager;
        _gameManager = gameManager;
        _objectPoolManager = objectPoolManager;
    }

    private void Awake()
    {
        Initialize();
    }

    private async void Initialize()
    {
        _objectPoolManager.Initialize();
        await UniTask.WaitUntil(() => _objectPoolManager.IsInitialized);
        
        _managers = new();
        
        _managers.Add(_signalManager);
        _managers.Add(_gameManager);
        _managers.Add(_levelManager);
        _managers.Add(_panelManager);
        _managers.Add(_gridManager);
        _managers.Add(_gameplayManager);
        
        foreach (var manager in _managers)
        {
            manager.Initialize();
        }
        
        await UniTask.WaitUntil(AreAllManagersInitialized);
        
        _gameManager.SetGameState(GameState.OnGameplay);
        
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