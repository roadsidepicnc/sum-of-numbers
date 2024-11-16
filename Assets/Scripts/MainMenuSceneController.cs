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

public class MainMenuSceneController : MonoBehaviour, ISubscribable
{
    [Inject] private SignalBus _signalBus;
    [Inject] private LevelManager _levelManager;
    [Inject] private GameManager _gameManager;
    [Inject] private ObjectPoolManager _objectPoolManager;
    [Inject] private PopupManager _popupManager;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button achievementsButton;
    
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

    private void Start()
    {
        Initialize();
    }

    public void Subscribe()
    {
        _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    public void Unsubscribe()
    {
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
    }
    
    private void Initialize()
    {
        SetTexts();
        SetButtons();
    }

    private void SetTexts()
    {
        levelNumberText.text = "Level " + _levelManager.LevelNumber;
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

    private void OnPlayButtonClick()
    {
        _signalBus.Fire(new GameStateChangedSignal(GameState.SceneIsChanging));
    }

    private void OnSettingsButtonClick()
    {
        _popupManager.Show(PopupType.SettingsPopup);
    }
    
    private void OnAchievementsButtonClick()
    {
    }
    
    private async void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
    {
        if (gameStateChangedSignal.GameState == GameState.SceneIsChanged)
        {
            _objectPoolManager.ResetPools();
            await SceneManager.LoadSceneAsync("Gameplay");
        }
    }
}