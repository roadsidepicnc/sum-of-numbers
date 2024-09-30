using System.Collections.Generic;
using GridManagement;
using LevelManagement;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameplayManager : Manager
    {
        [Inject] private GridManager _gridManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private PopupManager _popupManager;
        [Inject] private GameManager _gameManager;
        [Inject] private SignalManager _signalManager;
        [Inject] private HeartManager _heartManager;
        [Inject] private TargetScoreManager _targetScoreManager;
        
        public ClickMode ClickMode { get; private set; }
        
        public override void Initialize()
        {
            base.Initialize();
            
            IsInitialized = true;
            ClickMode = ClickMode.Select;
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

        private async void HandleLogic(Cell cell)
        {
            if (ClickMode == ClickMode.Select)
            {
                if (!cell.IsTarget)
                {
                    await _heartManager.LoseHeart();
                }
                else
                {
                    await cell.PlaceCircle();
                    
                    if (CheckIfRowIsCompleted(cell.Row))
                    {
                        _targetScoreManager.CompleteTargetScore(TargetScoreText.AlignmentType.Row, cell.Row);
                    }
                    else if (CheckIfColumnIsCompleted(cell.Column))
                    {
                        _targetScoreManager.CompleteTargetScore(TargetScoreText.AlignmentType.Column, cell.Column);
                    }
                }
            }
            else if (ClickMode == ClickMode.Erase)
            {
                if (cell.IsTarget)
                {
                    await _heartManager.LoseHeart();
                }
                else
                {
                    await cell.Erase();
                    
                    if (CheckIfRowIsCompleted(cell.Row))
                    {
                        _targetScoreManager.CompleteTargetScore(TargetScoreText.AlignmentType.Row, cell.Row);
                    }
                    else if (CheckIfColumnIsCompleted(cell.Column))
                    {
                        _targetScoreManager.CompleteTargetScore(TargetScoreText.AlignmentType.Column, cell.Column);
                    }
                }
            }
            
            if (CheckWin())
            { 
                Win();
            }
            else if (CheckLose())
            {
                Lose();
            }
        }

        private void Win()
        {
            _gameManager.SetGameState(GameState.Won);
            _popupManager.Show(PopupType.WinPopup);
        }
        
        private void Lose()
        {
            _gameManager.SetGameState(GameState.Lost);
            _popupManager.Show(PopupType.LosePopup);
        }
        
        private bool CheckIfLineIsCompleted(List<Cell> cells)
        {
            var selectedCellsCount = cells.FindAll(x => x.CellState == CellState.Selected).Count;
            var erasedCellsCount = cells.FindAll(x => x.CellState == CellState.Erased).Count;
            var targetCellsCount = cells.FindAll(x => x.IsTarget).Count;
            return selectedCellsCount == targetCellsCount && cells.Count - selectedCellsCount == erasedCellsCount;
        }

        public bool CheckIfRowIsCompleted(int row) => CheckIfLineIsCompleted(_gridManager.GetRow(row));
        
        public bool CheckIfColumnIsCompleted(int column) => CheckIfLineIsCompleted(_gridManager.GetColumn(column));
        
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

        private bool CheckLose()
        {
            return !_heartManager.AreThereAnyActiveHeart;
        }
        
        private void Update()
        {
            if (GameManager.GameState != GameState.Won && Input.GetKeyDown(KeyCode.A))
            {
                _gameManager.SetGameState(GameState.Won);
                _popupManager.Show(PopupType.WinPopup);
                _popupManager.Show(PopupType.WinPopup, false);
                _popupManager.Show(PopupType.WinPopup, false);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (ClickMode == ClickMode.Erase)
                {
                    ClickMode = ClickMode.Select;
                }
                else
                {
                    ClickMode = ClickMode.Erase;
                }
            }            
        }
    }
}