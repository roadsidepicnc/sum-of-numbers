using System.Collections.Generic;
using GridManagement;
using LevelManagement;
using UI;
using UI.Popup;
using UnityEngine;
using Utilities;
using Zenject;

namespace Gameplay
{
    public class GameplayManager : Manager
    {
        private GridManager _gridManager;
        private LevelManager _levelManager;
        private PanelManager _panelManager;
        private GameManager _gameManager;
        private SignalManager _signalManager;
        
        private List<int> _rowTargetScores;
        private List<int> _columnTargetScores;
        
        [Inject]
        private void InstallDependencies(GridManager gridManager, LevelManager levelManager, PanelManager panelManager, GameManager gameManager, SignalManager signalManager)
        {
            _gridManager = gridManager;
            _levelManager = levelManager;
            _panelManager = panelManager;
            _gameManager = gameManager;
            _signalManager = signalManager;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            _rowTargetScores = new();
            _columnTargetScores = new();
            
            for (var i = 0; i < _levelManager.CurrentLevelRowCount; i++)
            {
                _rowTargetScores.Add(_levelManager.GetRowTargetValue(i));
            }
            
            for (var i = 0; i < _levelManager.CurrentLevelColumnCount; i++)
            {
                _columnTargetScores.Add(_levelManager.GetColumnTargetValue(i));
            }
            
            IsInitialized = true;
        }
        
        protected override void Register()
        {
            _signalManager.CellInteracted += OnCellInteracted;
        }

        protected override void Deregister()
        {
            _signalManager.CellInteracted -= OnCellInteracted;
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
            for (var i = 0; i < _levelManager.CurrentLevelRowCount; i++)
            {
                if (!CheckIfRowIsCompleted(i))
                {
                    return false;
                }
            }
            
            for (var i = 0; i < _levelManager.CurrentLevelColumnCount; i++)
            {
                if (!CheckIfColumnIsCompleted(i))
                {
                    return false;
                }
            }

            return true;
        }
        
        private void Update()
        {
            if (GameManager.GameState != GameState.Finished && Input.GetKeyDown(KeyCode.A))
            {
                _gameManager.SetGameState(GameState.Finished);
                _panelManager.Show(PanelType.WinPopup);
                _panelManager.Show(PanelType.WinPopup, false);
                _panelManager.Show(PanelType.WinPopup, false);
            }
        }
    }
}