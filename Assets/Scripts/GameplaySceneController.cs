using Cysharp.Threading.Tasks;
using LevelManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameplaySceneController : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    
    
    private LevelManager _levelManager;
    private SignalManager _signalManager;
    private GameManager _gameManager;
    
    [Inject]
    private void InstallDependencies(GameManager gameManager, LevelManager levelManager, SignalManager signalManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _signalManager = signalManager;
    }
    
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
        _signalManager.GameStateChanged += OnGameStateChanged;
    }

    private void Deregister()
    {
        _signalManager.GameStateChanged -= OnGameStateChanged;
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
        _gameManager.SetGameState(GameState.Loading);
        await SceneManager.LoadSceneAsync("MainMenu");
    }
    
    private void OnGameStateChanged(GameState gameState)
    {
    }
}