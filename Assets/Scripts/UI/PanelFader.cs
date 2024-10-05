using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PanelFader : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        [Inject] private SignalManager _signalManager;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameState targetSceneGameState;
        
        [Header("Durations")]
        [SerializeField] private float fadeInDuration = .5f;
        [SerializeField] private float fadeOutDuration = .25f;

        private void OnEnable()
        {
            _signalManager.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _signalManager.GameStateChanged -= OnGameStateChanged;
        }

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        private async void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.SceneLoaded:
                    await PlayFadeInAnimation(fadeInDuration);
                    _gameManager.SetGameState(targetSceneGameState);
                    break;
                case GameState.SceneChanging:
                    await PlayFadeOutAnimation(fadeOutDuration);
                    _gameManager.SetGameState(GameState.SceneFaded);
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