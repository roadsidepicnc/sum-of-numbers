using Cysharp.Threading.Tasks;
using LevelManagement;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private RectTransform topPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button achievementsButton;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI levelNumberText;
    
    [SerializeField] private GameObject content;

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

    private void Start()
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
        SetTexts();
        SetButtons();
    }

    private void SetTexts()
    {
        levelNumberText.text = "Level " + (_levelManager.CurrentLevelId + 1);
    }

    private void SetButtons()
    {
        playButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        achievementsButton.onClick.RemoveAllListeners();
        
        playButton.onClick.AddListener(OnPlayButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        achievementsButton.onClick.AddListener(OnAchievementsButtonClick);
    }

    private async void OnPlayButtonClick()
    {
        _gameManager.SetGameState(GameState.Loading);
        await SceneManager.LoadSceneAsync("Gameplay");
    }

    private void OnSettingsButtonClick()
    {
        
    }
    
    private void OnAchievementsButtonClick()
    {
        
    }
    
    private void OnGameStateChanged(GameState gameState)
    {
    }
}