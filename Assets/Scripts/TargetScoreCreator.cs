using System.Collections.Generic;
using GridManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class TargetScoreCreator : MonoBehaviour
    {
        [Inject] private ObjectPoolManager _objectPoolManager;
        [Inject] private GridManager _gridManager;
        
        [SerializeField] private Transform rowTargetScoreTextsParent;
        [SerializeField] private Transform columnTargetScoreTextsParent;

        private const float SizeMultiplier = .9f;
        
        public void Create(int rowCount, int columnCount, List<TargetScoreText> rowTargetScoreTexts, List<TargetScoreText> columnTargetScoreTexts)
        {
            CreateRowTargetScoreTexts(rowCount, rowTargetScoreTexts, rowTargetScoreTextsParent, _gridManager.CellSize * SizeMultiplier);
            CreateColumnTargetScoreTexts(columnCount, columnTargetScoreTexts, columnTargetScoreTextsParent, _gridManager.CellSize * SizeMultiplier);
        }
        
        private void CreateRowTargetScoreTexts(int count, List<TargetScoreText> targetScoreTextList, Transform parent, float cellSize)
        {
            for (var i = 0; i < count; i++)
            {
                var poolObject = _objectPoolManager.GetObject(PoolObjectType.TargetScoreText, parent);
                var targetScoreText = (poolObject as TargetScoreText);
                targetScoreText?.Set(_gridManager.GetRowTarget(i), TargetScoreText.AlignmentType.Row, i, cellSize, cellSize); 
                targetScoreTextList.Add(targetScoreText);
            }
        }

        private void CreateColumnTargetScoreTexts(int count, List<TargetScoreText> targetScoreTextList, Transform parent, float cellSize)
        {
            for (var i = 0; i < count; i++)
            {
                var poolObject = _objectPoolManager.GetObject(PoolObjectType.TargetScoreText, parent);
                var targetScoreText = poolObject as TargetScoreText;
                targetScoreText?.Set(_gridManager.GetColumnTarget(i), TargetScoreText.AlignmentType.Column, i, cellSize, cellSize);
                targetScoreTextList.Add(targetScoreText);
            }
        }
    }
}