using System.Collections.Generic;
using GridManagement;
using LevelManagement;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay
{
    public class GameplayManager : BaseManager
    {
        [SerializeField] private GameObject gameplayBoard;
        
        [SerializeField] private List<Cell> cellList;
        
        private GridManager _gridManager;
        private LevelManager _levelManager;
        private GameManager _gameManager;
        
        private List<int> _rowTargetScores;
        private List<int> _columnTargetScores;
        
        [Inject]
        private void InstallDependencies(GridManager gridManager, LevelManager levelManager, GameManager gameManager)
        {
            _gridManager = gridManager;
            _levelManager = levelManager;
            _gameManager = gameManager;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            _rowTargetScores = new();
            _columnTargetScores = new();
            
            for (var i = 0; i < _gridManager.RowCount; i++)
            {
                _rowTargetScores.Add(_levelManager.GetRowTargetValue(i));
            }
            
            for (var i = 0; i < _gridManager.RowCount; i++)
            {
                _columnTargetScores.Add(_levelManager.GetColumnTargetValue(i));
            }
        }
        
        protected override void Register()
        {
            Signals.CellInteracted += OnCellInteracted;
            Signals.GameStateChanged += OnGameStateChanged;
        }

        protected override void Deregister()
        {
            Signals.CellInteracted -= OnCellInteracted;
            Signals.GameStateChanged -= OnGameStateChanged;
        }
        
        private void OnCellInteracted(Cell cell)
        {
            HandleLogic(cell);
        }

        private void HandleLogic(Cell cell)
        {
            if (CheckWin())
            { 
                Win();
            }
        }

        private void Win()
        {
            _gameManager.SetGameState(GameState.Finished);
        }
        
        private int CalculateRowScore(int row) => CalculateScoreHelper(_gridManager.GetRow(row));
        
        private int CalculateColumnScore(int column) => CalculateScoreHelper(_gridManager.GetColumn(column));
        
        private int CalculateScoreHelper(List<Cell> cells)
        {
            var value = 0;
            foreach (var cell in cells)
            {
                if (cell.IsSelected)
                {
                    value += cell.Value;
                }
            }

            return value;
        }

        public bool CheckIfRowIsCompleted(int row) => _rowTargetScores[row] == CalculateRowScore(row);
        
        public bool CheckIfColumnIsCompleted(int column) => _columnTargetScores[column] == CalculateColumnScore(column);

        private bool CheckWin()
        {
            for (var i = 0; i < _gridManager.RowCount; i++)
            {
                if (!CheckIfRowIsCompleted(i))
                {
                    return false;
                }
            }
            
            for (var i = 0; i < _gridManager.ColumnCount; i++)
            {
                if (!CheckIfColumnIsCompleted(i))
                {
                    return false;
                }
            }

            return true;
        }
        
        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState != GameState.Running)
            {
                return;
            }
            
            gameplayBoard.SetActive(false);
        }
    }
}