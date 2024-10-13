using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LosePopup : Popup
    {
        [Inject] private GameManager _gameManager;
        
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
            _gameManager.SetGameState(GameState.SceneIsReloading);
        }

        private void OnHomeButtonClick()
        {
            _gameManager.SetGameState(GameState.SceneIsChanging);
        }
    }
}