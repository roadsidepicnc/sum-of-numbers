using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using GridManagement;
using LevelManagement;
using ObjectPoolManagement;
using UI;
using Utilities;
using Zenject;

public class GameManager : Manager
{
    private GridManager _gridManager;
    private ObjectPoolManager _objectPoolManager;
    private UIManager _uiManager;
    private GameplayManager _gameplayManager;
    private LevelManager _levelManager;

    private List<Manager> _managers;

    public GameState GameState { get; private set; }
    
    private void Start()
    {
        Initialize();
    }

    [Inject]
    public void InstallDependencies(GridManager gridManager, ObjectPoolManager objectPoolManager, UIManager uiManager,
        GameplayManager gameplayManager, LevelManager levelManager)
    {
        _levelManager = levelManager;
        _gridManager = gridManager;
        _objectPoolManager = objectPoolManager;
        _uiManager = uiManager;
        _gameplayManager = gameplayManager;
    }

    public override async void Initialize()
    {
        SetGameState(GameState.Initializing);
        
        _managers = new();

        _managers.Add(_objectPoolManager);
        _managers.Add(_levelManager);
        _managers.Add(_gridManager);
        _managers.Add(_gameplayManager);
        _managers.Add(_uiManager);

        foreach (var manager in _managers)
        {
            manager.Initialize();
        }

        await UniTask.WaitUntil(AreAllManagersInitialized);
        
        IsInitialized = true;
        
        SetGameState(GameState.Running);
        
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

    public void SetGameState(GameState gameState)
    {
        GameState = gameState;
        Signals.GameStateChanged?.Invoke(gameState);
    }
}
