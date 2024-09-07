using System.Collections.Generic;
using Gameplay;

namespace LevelManagement
{
    public class LevelManager : BaseManager
    {
        public int CurrentLevel { get; private set; }

        private List<int> _currentLevelRowTargets;
        private List<int> _currentLevelColumnTargets;
        
        public override void Initialize()
        {
            _currentLevelRowTargets = new();
            _currentLevelColumnTargets = new();
            
            
        }
    }
}