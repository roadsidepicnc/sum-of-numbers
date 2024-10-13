using UnityEngine;
using UnityEngine.UI;
using Utilities.Signals;
using Zenject;

namespace UI
{
    public class LosePopup : Popup
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;

        public override void Initialize()
        {
            base.Initialize();
            
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(OnRestartButtonClick);
            
            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(OnHomeButtonClick);
        }

        private void OnRestartButtonClick()
        {
            _signalBus.Fire(new GameStateChangedSignal(GameState.SceneIsReloading));
        }

        private void OnHomeButtonClick()
        {
            _signalBus.Fire(new GameStateChangedSignal(GameState.SceneIsChanging));
        }
    }
}