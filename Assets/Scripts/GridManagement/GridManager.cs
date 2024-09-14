using System.Collections.Generic;
using Gameplay;
using LevelManagement;
using UI;
using Utilities;
using Zenject;

namespace GridManagement
{
    public class GridManager : Manager
    {
        private List<Cell> _cellList;
        private List<TargetScoreText> _rowTargetScoreTexts;
        private List<TargetScoreText> _columnTargetScoreTexts;
        
        private GridCreator _gridCreator;
        private LevelManager _levelManager;
        
        [Inject]
        private void InstallDependencies(GridCreator gridCreator, LevelManager levelManager)
        {
            _gridCreator = gridCreator;
            _levelManager = levelManager;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            _cellList = new();
            _rowTargetScoreTexts = new();
            _columnTargetScoreTexts = new();
            
            _gridCreator.Create(_levelManager.CurrentLevelRowCount, _levelManager.CurrentLevelColumnCount, _cellList, _rowTargetScoreTexts, _columnTargetScoreTexts);
            
            IsInitialized = true;
        }

        protected override void Register()
        {
            Signals.ResetGrid += ResetGrid;
        }

        protected override void Deregister()
        {
            Signals.ResetGrid -= ResetGrid;
        }

        private void ResetGrid()
        {
            foreach (var cell in _cellList)
            {
                cell.Reset();
            }
        }
        
        public Cell GetCell(int row, int column) => _cellList.Find(x => x.Row == row && x.Column == column);
        
        public List<Cell> GetRow(int row) => _cellList.FindAll(x => x.Row == row);
        
        public List<Cell> GetColumn(int column) => _cellList.FindAll(x => x.Column == column);
    }
}