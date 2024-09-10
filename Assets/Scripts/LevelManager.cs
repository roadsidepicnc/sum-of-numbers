using System.Collections.Generic;
using Gameplay;

namespace LevelManagement
{
    public class LevelManager : BaseManager
    {
        public int CurrentLevel { get; private set; }

        private List<int> _currentLevelRowTargets;
        private List<int> _currentLevelColumnTargets;
        private List<List<int>> _gridData;
        
        public override void Initialize()
        {
            _gridData = new();
            _currentLevelRowTargets = new();
            _currentLevelColumnTargets = new();
            
            var counter = 0;
            for (var i = 0; i < 4; i++)
            {
                var list = new List<int>();
                list.Add(++counter);
                list.Add(++counter);
                list.Add(++counter);
                list.Add(++counter);
                _gridData.Add(list);
            }

            for (var i = 0; i < 4; i++)
            {
                _currentLevelRowTargets.Add(_gridData[i][0] + _gridData[i][1]);
                _currentLevelColumnTargets.Add(_gridData[0][i] + _gridData[1][i]);
            }
        }

        public int GetCellValue(int row, int column)
        {
            return _gridData[row][column];
        }
        
        public int GetRowTargetValue(int row)
        {
            return _currentLevelRowTargets[row];
        }
        
        public int GetColumnTargetValue(int column)
        {
            return _currentLevelColumnTargets[column];
        }
    }
}