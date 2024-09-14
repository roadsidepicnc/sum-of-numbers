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
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI levelDefinitionText;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform topPanel;
        
        private LevelManager _levelManager;
        
        [Inject]
        public void InstallDependencies(LevelManager levelManager)
        {
            _levelManager = levelManager;
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
            Signals.ResetGrid?.Invoke();
        }

        private void SetTexts()
        {
            levelNumberText.text = "Level " + (_levelManager.CurrentLevelId + 1);
            levelDefinitionText.text = "Basic";
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
    }
}