using System.Collections.Generic;
using Gameplay;
using LevelManagement;
using UI;
using Zenject;

namespace GridManagement
{
    public class GridManager : Manager
    {
        [Inject] private GridCreator _gridCreator;
        [Inject] private LevelManager _levelManager;
        
        private List<Cell> _cellList;
        private List<TargetScoreText> _rowTargetScoreTexts;
        private List<TargetScoreText> _columnTargetScoreTexts;

        private float _cellSize;
        public float CellSize => _cellSize;
        
        public override void Initialize()
        {
            base.Initialize();
            
            _cellList = new();
            _rowTargetScoreTexts = new();
            _columnTargetScoreTexts = new();
            
            _gridCreator.Create(_levelManager.CurrentLevelRowCount, _levelManager.CurrentLevelColumnCount, _cellList, ref _cellSize);
            
            IsInitialized = true;
        }
        
        public Cell GetCell(int row, int column) => _cellList.Find(x => x.Row == row && x.Column == column);
        
        public List<Cell> GetRow(int row) => _cellList.FindAll(x => x.Row == row);
        
        public List<Cell> GetColumn(int column) => _cellList.FindAll(x => x.Column == column);

        public int GetRowTarget(int row)
        {
            var count = 0;
            foreach (var cell in GetRow(row))
            {
                count += cell.Value;
            }

            return count;
        }
        
        public int GetColumnTarget(int column)
        {
            var count = 0;
            foreach (var cell in GetColumn(column))
            {
                count += cell.Value;
            }

            return count;
        }
    }
}