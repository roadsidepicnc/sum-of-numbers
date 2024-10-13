using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class WinPopup : Popup
    {
        [Inject] private GameManager _gameManager;
        
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
            _gameManager.SetGameState(GameState.SceneIsReloading);
        }
        
        private void OnHomeButtonClick()
        {
            _gameManager.SetGameState(GameState.SceneIsChanging);
        }
    }
}