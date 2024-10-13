using System.Collections.Generic;
using System.Linq;
using Gameplay;
using UnityEngine;
using Utilities;
using Utilities.Signals;
using Zenject;

namespace LevelManagement
{
    public class LevelManager : Manager
    {
        [Inject] private SignalBus _signalBus;
        
        private int LevelId => PlayerPrefs.GetInt(Constants.CurrentLevelIndex);
        public int LevelNumber => LevelId + 1;
        
        private List<LevelData> _levelList;

        private LevelData _levelData; 
        
        public override void Initialize()
        {
            base.Initialize();
            
            _levelList = Resources.LoadAll<LevelData>("Levels").ToList();
            if (_levelList.Count <= 0)
            {
                Debug.LogError("Levels could not be loaded");
                Application.Quit();
                return;
            }

            var index = LevelId % _levelList.Count;
            _levelData = _levelList.Find(x => x.id == index);
            
            IsInitialized = true;
        }

        public override void Subscribe()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public override void Unsubscribe()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public CellData GetCellData(int row, int column)
        {
            return _levelData.cellDataList[CoordinateConverter.Convert(row, column, _levelData.columnCount)];
        }

        private void IncreaseLevelId() => PlayerPrefs.SetInt(Constants.CurrentLevelIndex, LevelNumber);
        
        public int CurrentLevelRowCount => _levelData.rowCount;
        
        public int CurrentLevelColumnCount => _levelData.columnCount;
        public int CurrentLevelHeartCount => _levelData.heartCount;
        
        private void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
        {
            if (gameStateChangedSignal.GameState == GameState.Won)
            {
                IncreaseLevelId();
                var index = LevelId % _levelList.Count;
                _levelData = _levelList.Find(x => x.id == index);
            }
        }
    }
}