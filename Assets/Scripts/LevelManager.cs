using System.Collections.Generic;
using System.Linq;
using Gameplay;
using UnityEngine;
using Utilities;

namespace LevelManagement
{
    public class LevelManager : Manager
    {
        private int LevelId => PlayerPrefs.GetInt(Constants.CurrentLevelIndex);
        public int LevelNumber => LevelId + 1;
        
        private List<LevelData> _levelList;

        private LevelData _levelData; 
        
        public override void Initialize()
        {
            _levelList = Resources.LoadAll<LevelData>("Levels").ToList();
            if (_levelList.Count <= 0)
            {
                Debug.LogError("Levels could not be loaded");
                Application.Quit();
                return;
            }
            
            _levelData = _levelList.Find(x => x.id == LevelId);

            if (_levelData == null)
            {
                Debug.LogError("Required level could not be loaded");
                _levelData = _levelList[0];
            }
            
            IsInitialized = true;
        }

        public CellData GetCellData(int row, int column)
        {
            return _levelData.cellDataList[CoordinateConverter.Convert(row, column, _levelData.columnCount)];
        }

        public void IncreaseLevelId() => PlayerPrefs.SetInt(Constants.CurrentLevelIndex, LevelNumber);
        
        public int CurrentLevelRowCount => _levelData.rowCount;
        
        public int CurrentLevelColumnCount => _levelData.columnCount;
        public int CurrentLevelHeartCount => _levelData.heartCount;
    }
}