using System.Collections.Generic;
using GridManagement;
using LevelManagement;
using UI;
using UnityEngine;
using Utilities.Signals;
using Zenject;

namespace Gameplay
{
    public class GameplayManager : Manager
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private GridManager _gridManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private PopupManager _popupManager;
        [Inject] private GameManager _gameManager;
        [Inject] private HeartManager _heartManager;
        [Inject] private TargetScoreManager _targetScoreManager;
        
        public ClickMode ClickMode { get; private set; }
        
        public override void Initialize()
        {
            base.Initialize();
            
            IsInitialized = true;
            ClickMode = ClickMode.Select;
        }
        
        public override void Subscribe()
        {
            _signalBus.Subscribe<CellInteractedSignal>(OnCellInteracted);
            _signalBus.Subscribe<ClickModeChangedSignal>(OnClickModeChanged);
        }

        public override void Unsubscribe()
        {
            _signalBus.Unsubscribe<CellInteractedSignal>(OnCellInteracted);
            _signalBus.Unsubscribe<ClickModeChangedSignal>(OnClickModeChanged);
        }
        
        private void OnCellInteracted(CellInteractedSignal signal)
        {
            HandleLogic(signal.Cell);
        }

        private void OnClickModeChanged()
        {
            ClickMode = ClickMode switch
            {
                ClickMode.Erase => ClickMode.Select,
                ClickMode.Select => ClickMode.Erase,
                _ => ClickMode
            };
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
                }
            }
            
            TryToCompleteTargetScores(cell);
            
            if (CheckWin())
            { 
                Win();
            }
            else if (CheckLose())
            {
                Lose();
            }
        }
        
        private void TryToCompleteTargetScores(Cell cell)
        {
            var rowCompleted = CheckIfRowIsCompleted(cell.Row);
            var columnCompleted = CheckIfColumnIsCompleted(cell.Column);
            
            if (rowCompleted)
            {
                _targetScoreManager.CompleteTargetScore(TargetScoreText.AlignmentType.Row, cell.Row);
            }
            
            if (columnCompleted)
            {
                _targetScoreManager.CompleteTargetScore(TargetScoreText.AlignmentType.Column, cell.Column);
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