using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class SettingsPopup : Popup
    {
        [Inject] private GameManager _gameManager;
        
        [SerializeField] private Button backButton;
        [SerializeField] private SoundButton soundButton;

        private GameState _previousGameState;
        
        public override void Initialize()
        {
            base.Initialize();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => Close());
            
            soundButton.Initialize();
        }

        public override async UniTask Open(Action callback = null)
        {
            _previousGameState = GameManager.GameState;
            _gameManager.SetGameState(GameState.Paused);
            base.Open(callback);
        }

        public override async UniTask Close(Action callback = null, bool ignoreCommand = false)
        {
            await base.Close(callback, ignoreCommand);
            _gameManager.SetGameState(_previousGameState);
        }
    }
}