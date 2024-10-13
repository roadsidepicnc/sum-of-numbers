using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using LevelManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadingSceneLoader : MonoBehaviour
{
    [Inject] private LevelManager _levelManager;
    [Inject] private GameManager _gameManager;
    
    private List<Manager> _managers;
    
    private void Awake()
    {
        Initialize();
    }

    private async void Initialize()
    {
        _managers = new();
        
        _managers.Add(_gameManager);
        _managers.Add(_levelManager);
        
        _gameManager.SetGameState(GameState.ManagersAreInitializing);
        
        foreach (var manager in _managers)
        {
            manager.Initialize();
        }
        
        await UniTask.WaitUntil(AreAllManagersInitialized);
        
        _gameManager.SetGameState(GameState.ManagersAreInitialized);
        
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
