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

        public int GetCellValue(int row, int column)
        {
            return _levelData.CellsData[row][column];
        }
        
        public int GetRowTargetValue(int row)
        {
            return _levelData.RowTargets[row];
        }
        
        public int GetColumnTargetValue(int column)
        {
            return _levelData.ColumnTargets[column];
        }
        
        public int CurrentLevelRowCount => _levelData.RowCount;
        
        public int CurrentLevelColumnCount => _levelData.ColumnCount;
    }

    public class LevelData
    {
        public int RowCount;
        public int ColumnCount;
        public List<int> RowTargets;
        public List<int> ColumnTargets;
        public List<List<int>> CellsData;

        public void Create()
        {
            RowCount = 4;
            ColumnCount = 4;
            RowTargets = new();
            ColumnTargets = new();
            CellsData = new();
            
            var counter = 0;
            for (var i = 0; i < 4; i++)
            {
                var list = new List<int>();
                list.Add(++counter);
                list.Add(++counter);
                list.Add(++counter);
                list.Add(++counter);
                CellsData.Add(list);
            }

            for (var i = 0; i < 4; i++)
            {
                RowTargets.Add(CellsData[i][0] + CellsData[i][1]);
                ColumnTargets.Add(CellsData[0][i] + CellsData[1][i]);
            }
        }
    }
}