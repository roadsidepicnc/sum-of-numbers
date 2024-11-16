using UnityEngine;
using UnityEngine.UI;
using Utilities.Signals;
using Zenject;

namespace UI
{
    public class ConfirmationPopup : Popup
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button homeButton;

        public override void Initialize()
        {
            base.Initialize();
            
            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(OnHomeButtonClick);
            
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
        }
        
        private void OnNextLevelButtonClick()
        {
            _signalBus.Fire(new GameStateChangedSignal(GameState.SceneIsReloading));
        }
        
        private void OnHomeButtonClick()
        {
            _signalBus.Fire(new GameStateChangedSignal(GameState.SceneIsChanging));
        }
    }
}