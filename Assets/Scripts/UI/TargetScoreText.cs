using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using ObjectPoolingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class TargetScoreText : PoolObject
    {
        [SerializeField] private TextMeshProUGUI targetScoreText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image background;
        
        [Inject] private GameplayManager _gameplayManager;
        
        public void Set(int value, AlignmentType alignmentType, int alignmentIndex, float width, float height)
        {
            targetScoreText.text = value.ToString();
            canvasGroup.alpha = 1f;
            
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            var backgroundTransform = background.GetComponent<RectTransform>();
            switch (alignmentType)
            {
                case AlignmentType.Column:
                    rectTransform.pivot = new Vector2(.5f, 1f);
                    rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0f);
                    break;
                case AlignmentType.Row:
                    rectTransform.pivot = new Vector2(0f, .5f);
                    rectTransform.localPosition = new Vector3(0f, rectTransform.localPosition.y);
                    break;
            }
        }

        public override void Reset(Transform parent)
        {
            base.Reset(parent);
            canvasGroup.alpha = 1f;
        }
        
        private async UniTask Fade(float duration = .2f)
        {
            await canvasGroup.DOFade(0f, duration).From(1f);
        }

        public async UniTask Complete()
        {
            await Fade();
        }
        
        public enum AlignmentType
        {
            Row,
            Column
        }
    }
}