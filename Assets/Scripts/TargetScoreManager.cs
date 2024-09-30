using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LevelManagement;
using UI;
using Zenject;

namespace Gameplay
{
    public class TargetScoreManager : Manager
    {
        [Inject] private TargetScoreCreator _targetScoreCreator;
        [Inject] private LevelManager _levelManager;
        
        private List<TargetScoreText> _rowTargetScoreTexts;
        private List<TargetScoreText> _columnTargetScoreTexts;

        public override void Initialize()
        {
            base.Initialize();
            
            _rowTargetScoreTexts = new();
            _columnTargetScoreTexts = new();
            
            _targetScoreCreator.Create(_levelManager.CurrentLevelRowCount, _levelManager.CurrentLevelColumnCount, _rowTargetScoreTexts, _columnTargetScoreTexts);
            
            IsInitialized = true;
        }

        public async UniTask CompleteTargetScore(TargetScoreText.AlignmentType alignmentType, int index)
        {
            var targetScoreText = alignmentType switch
            {
                TargetScoreText.AlignmentType.Row => _rowTargetScoreTexts[index],
                TargetScoreText.AlignmentType.Column => _columnTargetScoreTexts[index],
                _ => null
            };

            if (targetScoreText== null)
            {
                return;
            }

            await targetScoreText.Complete();
        }
    }
}