using System.Collections.Generic;
using Gameplay;
using GridManagement;
using LevelManagement;
using ObjectPoolManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace UI
{ 
    public class UIManager : Manager
    {
        [SerializeField] private Transform rowTargetScoreTextsParent;
        [SerializeField] private Transform columnTargetScoreTextsParent;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI levelDefinitionText;

        private ObjectPoolManager _objectPoolManager;
        private LevelManager _levelManager;
        private List<TargetScoreText> _rowTargetScoreTexts;
        private List<TargetScoreText> _columnTargetScoreTexts;

        [Inject]
        public void InstallDependencies(ObjectPoolManager objectPoolManager, LevelManager levelManager)
        {
            _objectPoolManager = objectPoolManager;
            _levelManager = levelManager;
        }

        public override void Initialize()
        {
            base.Initialize();

            _rowTargetScoreTexts = new();
            _columnTargetScoreTexts = new();

            CreateRowTargetScoreTexts(_levelManager.CurrentLevelRowCount, _rowTargetScoreTexts, rowTargetScoreTextsParent);
            CreateColumnTargetScoreTexts(_levelManager.CurrentLevelColumnCount, _columnTargetScoreTexts, columnTargetScoreTextsParent);

            SetButtons();
            SetTexts();
            
            IsInitialized = true;
        }

        protected override void Register()
        {
        }

        protected override void Deregister()
        {
        }

        private void CreateRowTargetScoreTexts(int count, List<TargetScoreText> targetScoreTextList, Transform parent)
        {
            for (var i = 0; i < count; i++)
            {
                var poolObject = _objectPoolManager.GetObject(PoolObjectType.TargetScoreText, parent);
                var targetScoreText = (poolObject as TargetScoreText);
                var ratio = (float)_levelManager.CurrentLevelRowCount / Constants.DefaultGridRowCount; 
                targetScoreText?.Set(_levelManager.GetRowTargetValue(i), TargetScoreText.AlignmentType.Row, i, Constants.DefaultTargetScoreTextHeight, Constants.DefaultTargetScoreTextWidth); 
                targetScoreTextList.Add(targetScoreText);
            }
        }

        private void CreateColumnTargetScoreTexts(int count, List<TargetScoreText> targetScoreTextList,
            Transform parent)
        {
            for (var i = 0; i < count; i++)
            {
                var poolObject = _objectPoolManager.GetObject(PoolObjectType.TargetScoreText, parent);
                var targetScoreText = (poolObject as TargetScoreText);
                targetScoreText?.Set(_levelManager.GetColumnTargetValue(i), TargetScoreText.AlignmentType.Column, i, Constants.DefaultTargetScoreTextWidth, Constants.DefaultTargetScoreTextHeight);
                targetScoreTextList.Add(targetScoreText);
            }
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
    }
}