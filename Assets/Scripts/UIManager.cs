using Gameplay;
using LevelManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace UI
{ 
    public class UIManager : Manager
    {
        public float CanvasWidth { get; private set; }
        public float CanvasHeight { get; private set; }
        public float UnitSize { get; private set; }
        
        [SerializeField] private Button homeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button levelFinishedPanelNextLevelButton;
        [SerializeField] private Button levelFinishedPanelHomeButton;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI levelDefinitionText;
        [SerializeField] private TextMeshProUGUI playButtonLevelDefinitionText;
        [SerializeField] private TextMeshProUGUI playButtonLevelNumberText;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform topPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject levelFinishedPanel;
        
        private LevelManager _levelManager;
        private SignalManager _signalManager;
        
        [Inject]
        public void InstallDependencies(LevelManager levelManager, SignalManager signalManager)
        {
            _levelManager = levelManager;
            _signalManager = signalManager;
        }

        public override void Initialize()
        {
            base.Initialize();

            CalculateUnitSize();
            SetButtons();
            SetTexts();
            
            topPanel.anchoredPosition += Vector2.down * ScreenSafeAreaTopDifferenceInPixels() / 2;
            
            IsInitialized = true;
        }
        
        private void SetButtons()
        {
            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(() => {});
            
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(() => {});
            
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(OnResetButtonClick);
        }

        private void OnResetButtonClick()
        {
            _signalManager.ResetGrid?.Invoke();
        }

        private void SetTexts()
        {
            levelNumberText.text = "Level " + (_levelManager.CurrentLevelId + 1);
            levelDefinitionText.text = "Basic";
            playButtonLevelNumberText.text = "Level " + (_levelManager.CurrentLevelId + 1);
            playButtonLevelDefinitionText.text = "Basic";
        }

        private void CalculateUnitSize()
        {
            CanvasHeight = canvas.GetComponent<RectTransform>().sizeDelta.y;
            CanvasWidth = canvas.GetComponent<RectTransform>().sizeDelta.x;
            UnitSize = canvas.referencePixelsPerUnit * ( CanvasHeight / Constants.DefaultScreenHeight);
        }
        
        private float SafeAreaChangeInUnits()
        {
            return ScreenSafeAreaTopDifferenceInPixels() * 0.5f / UnitSize;
        }

        private float ScreenSafeAreaTopDifferenceInPixels()
        {
            var screenSafeAreaTopDifferenceInPixels = Mathf.Abs(Screen.safeArea.max.y - Screen.height);
            if (screenSafeAreaTopDifferenceInPixels > 0f)
            {
                screenSafeAreaTopDifferenceInPixels += UnitSize / 2f; 
            }
        
            return screenSafeAreaTopDifferenceInPixels;
        }
        
        private void OnGameStateChanged(GameState gameState)
        {
            /*
            switch (gameState)
            {
                case GameState.Running:
                    gameplayPanel.SetActive(true);
                    mainMenuPanel.SetActive(false);
                    levelFinishedPanel.SetActive(false);
                    break;
                case GameState.OnMenu:
                    mainMenuPanel.SetActive(true);
                    gameplayPanel.SetActive(false);
                    levelFinishedPanel.SetActive(false);
                    break;
                case GameState.Finished:
                    levelFinishedPanel.SetActive(true);
                    mainMenuPanel.SetActive(false);
                    gameplayPanel.SetActive(false);
                    break;
                    
            }*/
        }
    }
}