using System.Collections.Generic;
using Gameplay;

namespace LevelManagement
{
    public class LevelManager : Manager
    {
        public int CurrentLevelId { get; private set; }

        private LevelData _levelData; 
        
        public override void Initialize()
        {
            _levelData = new LevelData();
            _levelData.Create();

            IsInitialized = true;
        }

        public LevelData.CellData GetCellData(int row, int column)
        {
            return _levelData.CellsData[row][column];
        }
        
        public int CurrentLevelRowCount => _levelData.RowCount;
        
        public int CurrentLevelColumnCount => _levelData.ColumnCount;
        public int CurrentLevelHeartCount => _levelData.HeartCount;
    }

    public class LevelData
    {
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public int HeartCount { get; private set; }
        public List<List<CellData>> CellsData { get; private set; }

        public void Create()
        {
            RowCount = 5;
            ColumnCount = 5;
            HeartCount = 3;

            CellsData = new();
            
            var counter = 0;
            for (var i = 0; i < RowCount; i++)
            {
                var list = new List<CellData>();
                for (var j = 0; j < RowCount; j++)
                {
                    list.Add(new CellData(++counter, j == 0));
                }
                
                CellsData.Add(list);
            }
        }
        
        public struct CellData
        {
            public bool IsTarget { get; private set; }
            public int Value { get; private set; }

            public CellData(int value, bool isTarget)
            {
                Value = value;
                IsTarget = isTarget;
            }
        }
    }
}