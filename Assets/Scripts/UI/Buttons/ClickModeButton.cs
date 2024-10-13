using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Signals;
using Zenject;

namespace UI
{
    public class ClickModeButton : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private GameplayManager _gameplayManager;

        [Header("Targets")]
        [SerializeField] private Transform eraserTarget;
        [SerializeField] private Transform pencilTarget;
        
        [Header("UI Components")]
        [SerializeField] private Button button;
        [SerializeField] private Transform circle;
        [SerializeField] private Image eraserIcon;
        [SerializeField] private Image pencilIcon;
        
        [Header("Colors")]
        [SerializeField] private Color activeColor;
        [SerializeField] private Color passiveColor;
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);

            circle.localPosition = pencilTarget.localPosition;
            pencilIcon.color = activeColor;
            eraserIcon.color = passiveColor;
        }
        
        private void OnClick()
        {
            _signalBus.Fire<ClickModeChangedSignal>();
            var clickMode = _gameplayManager.ClickMode;
            
            switch (clickMode)
            {
                case ClickMode.Erase:
                    break;
                case ClickMode.Select:
                    break;
            }

            PlayScaleAnimation(eraserIcon, clickMode == ClickMode.Erase, .2f);
            PlayScaleAnimation(pencilIcon, clickMode == ClickMode.Select, .2f);
            PlayCircleAnimation(clickMode == ClickMode.Select ?  pencilTarget : eraserTarget, .2f);
        }
        
        private async UniTask PlayScaleAnimation(Image target, bool isActive, float duration)
        {
            target.DOColor(isActive ? activeColor : passiveColor, duration);
            target.transform.DOComplete();
            await target.transform.DOScale(Vector3.one * .2f, duration);
            await target.transform. DOScale(Vector3.one, duration);
        }

        private void PlayCircleAnimation(Transform target, float duration)
        {
            circle.DOComplete();
            circle.DOLocalMove(target.localPosition, duration);
        }
        
    }
}