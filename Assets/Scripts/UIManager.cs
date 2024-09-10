using System.Collections.Generic;
using Gameplay;
using LevelManagement;
using ObjectPoolManagement;
using UnityEngine;
using Zenject;

namespace UI
{ 
    public class UIManager : BaseManager
    {
        [SerializeField] private Transform rowTargetScoreTextsParent;
        [SerializeField] private Transform columnTargetScoreTextsParent;

        private ObjectPoolManager _objectPoolManager;
        private GameplayManager _gameplayManager;
        private LevelManager _levelManager;
        private List<TargetScoreText> _rowTargetScoreTexts;
        private List<TargetScoreText> _columnTargetScoreTexts;

        [Inject]
        public void InstallDependencies(ObjectPoolManager objectPoolManager, GameplayManager gameplayManager,
            LevelManager levelManager)
        {
            _objectPoolManager = objectPoolManager;
            _gameplayManager = gameplayManager;
            _levelManager = levelManager;
        }

        public override void Initialize()
        {
            base.Initialize();

            _rowTargetScoreTexts = new();
            _columnTargetScoreTexts = new();

            CreateRowTargetScoreTexts(4, _rowTargetScoreTexts, rowTargetScoreTextsParent);
            CreateColumnTargetScoreTexts(4, _columnTargetScoreTexts, columnTargetScoreTextsParent);

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
                targetScoreText?.Set(_levelManager.GetRowTargetValue(i), TargetScoreText.AlignmentType.Row, i, 100, 200); 
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
                targetScoreText?.Set(_levelManager.GetColumnTargetValue(i), TargetScoreText.AlignmentType.Column, i, 200, 100);
                targetScoreTextList.Add(targetScoreText);
            }
        }
    }
}