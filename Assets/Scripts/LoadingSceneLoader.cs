using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using LevelManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadingSceneLoader : MonoBehaviour
{
    private LevelManager _levelManager;
    private SignalManager _signalManager;
    private GameManager _gameManager;
    
    private List<Manager> _managers;

    [Inject]
    public void InstallDependencies(LevelManager levelManager, SignalManager signalManager, GameManager gameManager)
    { 
        _signalManager = signalManager;
        _levelManager = levelManager;
        _gameManager = gameManager;
    }

    private void Awake()
    {
        Initialize();
    }

    private async void Initialize()
    {
        _managers = new();
        
        _managers.Add(_signalManager);
        _managers.Add(_gameManager);
        _managers.Add(_levelManager);
        
        foreach (var manager in _managers)
        {
            manager.Initialize();
        }
        
        _gameManager.SetGameState(GameState.SceneLoading);
        await UniTask.WaitUntil(AreAllManagersInitialized);
        await SceneManager.LoadSceneAsync("MainMenu");
        
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
