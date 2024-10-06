using Cysharp.Threading.Tasks;
using LevelManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Utilities.Signals;
using Zenject;

public class GameplaySceneController : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [Inject] private LevelManager _levelManager;
    [Inject] private GameManager _gameManager;
    [Inject] private ObjectPoolManager _objectPoolManager;
    
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    
    private void OnEnable()
    {
        Register();
    }

    private void OnDisable()
    {
        Deregister();   
    }
    
    private void Awake()
    {
        Initialize();
    }

    private void Register()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    private void Deregister()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
    }
    
    private void Initialize()
    {
        SetButtons();
    }
    
    private void SetButtons()
    {
        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.AddListener(OnHomeButtonClick);
    }

    private async void OnHomeButtonClick()
    {
        _gameManager.SetGameState(GameState.SceneChanging);
        await UniTask.WaitUntil(() => GameManager.GameState == GameState.SceneFaded);
        _objectPoolManager.ResetPools();
        await SceneManager.LoadSceneAsync("MainMenu");
    }
    
    private void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
    {
    }
}