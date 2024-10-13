using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utilities;
using Utilities.Signals;
using Zenject;

namespace UI
{
    public class PanelFader : MonoBehaviour, ISubscribable
    {
        [Inject] private GameManager _gameManager;
        [Inject] private SignalBus _signalBus;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameState targetGameState;
        
        [Header("Durations")]
        [SerializeField] private float fadeInDuration = .5f;
        [SerializeField] private float fadeOutDuration = .25f;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        public void Subscribe()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }
        
        public void Unsubscribe()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        private async void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
        {
            switch (gameStateChangedSignal.GameState)
            {
                case GameState.ManagersAreInitialized:
                    await PlayFadeInAnimation(fadeInDuration);
                    _gameManager.SetGameState(targetGameState);
                    break;
                case GameState.SceneIsChanging:
                    await PlayFadeOutAnimation(fadeOutDuration);
                    _gameManager.SetGameState(GameState.SceneIsChanged);
                    break;
                case GameState.SceneIsReloading:
                    await PlayFadeOutAnimation(fadeOutDuration);
                    _gameManager.SetGameState(GameState.SceneIsReloaded);
                    break;
            }
        }

        private async UniTask PlayFadeInAnimation(float duration)
        {
            await canvasGroup.DOFade(1f, duration).From(0f);
        }
        
        private async UniTask PlayFadeOutAnimation(float duration)
        {
            await canvasGroup.DOFade(0f, duration).From(1f);
        }
    }
}