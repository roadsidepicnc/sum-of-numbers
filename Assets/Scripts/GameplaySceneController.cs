using Cysharp.Threading.Tasks;
using LevelManagement;
using ObjectPoolingSystem;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Utilities.Signals;
using Zenject;

public class GameplaySceneController : MonoBehaviour, ISubscribable
{
    [Inject] private SignalBus _signalBus;
    [Inject] private LevelManager _levelManager;
    [Inject] private GameManager _gameManager;
    [Inject] private PopupManager _popupManager;
    [Inject] private ObjectPoolManager _objectPoolManager;
    
    [Header("Buttons")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI levelNumberText;
    
    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();   
    }
    
    private void Awake()
    {
        Initialize();
    }
   
    private void Initialize()
    {
        SetButtons();
        SetTexts();
    }
    
    private void SetButtons()
    {
        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.AddListener(OnHomeButtonClick);
        
        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }
    
    private void SetTexts()
    {
        levelNumberText.text = "Level " + _levelManager.LevelNumber;
    }

    private void OnHomeButtonClick()
    {
        _gameManager.SetGameState(GameState.SceneIsChanging);
    }
    
    private void OnSettingsButtonClick()
    {
        _popupManager.Show(PopupType.SettingsPopup);
    }
    
    private async void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
    {
        switch (gameStateChangedSignal.GameState)
        {
            case GameState.SceneIsChanged:
                _objectPoolManager.ResetPools();
                await SceneManager.LoadSceneAsync("MainMenu");
                break;
            case GameState.SceneIsReloaded:
                _objectPoolManager.ResetPools();
                await SceneManager.LoadSceneAsync("Gameplay");
                break;
        }
    }

    public void Subscribe()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    public void Unsubscribe()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
    }
}