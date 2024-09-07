using System.Collections.Generic;
using GridManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace Gameplay
{
    public class GameplayManager : BaseManager
    {
        [SerializeField] private TextMeshProUGUI mainScoreText;
        [SerializeField] private TextMeshProUGUI sideScoreText;
        [SerializeField] private Button resetButton;
        [SerializeField] private GameObject gameplayBoard;
        
        [SerializeField] private List<Cell> cellList;
        
        private int _sideScore;
        private int _mainScore;
        
        private GridManager _gridManager;
        
        private List<int> _rowTargetScores;
        private List<int> _columnTargetScores;
        private List<int> _rowCurrentScores;
        private List<int> _columnCurrentScores;

        [Inject]
        private void InstallDependencies(GridManager gridManager)
        {
            _gridManager = gridManager;
        }
        
        public override void Initialize()
        {
            _rowTargetScores = new();
            _columnTargetScores = new();
            _rowCurrentScores = new();
            _columnCurrentScores = new();
            
            _sideScore = 0;
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(OnResetButtonClick);

            for (var i  = 0 ; i < _gridManager.RowCount; i++)
            {
                _rowTargetScores.Add(100);
                _rowCurrentScores.Add(CalculateRowScore(i));
            }
            
            for (var i  = 0 ; i < _gridManager.ColumnCount; i++)
            {
                _columnTargetScores.Add(100);
                _columnCurrentScores.Add(CalculateColumnScore(i));
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
        
        private void UpdateMainScoreText()
        {
            mainScoreText.text = _mainScore.ToString();
        }
        
        private void UpdateSideScoreText()
        {
            sideScoreText.text = _sideScore.ToString();
        }
        
        private void OnCellInteracted(Cell cell)
        {
            HandleLogic(cell);
        }

        private void HandleLogic(Cell cell)
        {
            var row = cell.Row;
            var column = cell.Column;
            
            var rowScore = CalculateRowScore(cell.Row);
            var columnScore = CalculateColumnScore(cell.Column);
            
            _rowCurrentScores[row] = rowScore;
            _columnCurrentScores[column] = columnScore;
            
            if (CheckIfRowIsCompleted(rowScore, row) && CheckIfColumnIsCompleted(columnScore, column))
            {
                CheckWin();
            }
        }
        
        private void OnResetButtonClick()
        {
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

        private bool CheckIfRowIsCompleted(int row, int rowScore) => rowScore == _rowTargetScores[row];
        
        private bool CheckIfColumnIsCompleted(int column, int columnScore) => columnScore == _columnTargetScores[column];

        private bool CheckWin()
        {
            for (var i = 0; i < _gridManager.RowCount; i++)
            {
                if (!CheckIfRowIsCompleted(i, _rowCurrentScores[i]))
                {
                    return false;
                }
            }
            
            for (var i = 0; i < _gridManager.ColumnCount; i++)
            {
                if (!CheckIfColumnIsCompleted(i, _columnCurrentScores[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        private void OnGameStateChanged(GameState gameState, int score)
        {
            if (gameState != GameState.Running)
            {
                
                return;
            }
            
            gameplayBoard.SetActive(false);
        }
    }
}