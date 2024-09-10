using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class SummaryManager : BaseManager
    {
        [SerializeField] private Summary summary;

        public override void Initialize()
        {
            summary.Initialize();
        }

        protected override void Register()
        {
            Signals.GameStateChanged += OnGameStateChanged;
        }

        protected override void Deregister()
        {
            Signals.GameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState != GameState.Finished)
            {
                Reset();
                return;
            }

        }

        private void Reset() => summary.Reset();
        
        private void UpdateContent(int score) => summary.UpdateContent(score);
    }
}