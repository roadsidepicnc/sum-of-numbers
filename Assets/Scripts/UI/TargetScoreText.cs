using DG.Tweening;
using Gameplay;
using GridManagement;
using ObjectPoolingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace UI
{
    public class TargetScoreText : PoolObject
    {
        [SerializeField] private TextMeshProUGUI targetScoreText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image background;

        private AlignmentType _alignmentType;
        private int _alignmentIndex;
        
        [Inject] private GameplayManager _gameplayManager;
        [Inject] private SignalManager _signalManager;
        
        private void Register()
        {
            _signalManager.CellInteracted += OnCellInteracted;
        }

        private void Deregister()
        {
            _signalManager.CellInteracted -= OnCellInteracted;
        }

        public void Set(int value, AlignmentType alignmentType, int alignmentIndex, float width, float height)
        {
            Register();
            targetScoreText.text = value.ToString();
            _alignmentType = alignmentType;
            _alignmentIndex = alignmentIndex;

            var isErased = alignmentType switch
            {
                AlignmentType.Row => _gameplayManager.CheckIfRowIsCompleted(_alignmentIndex),
                AlignmentType.Column => _gameplayManager.CheckIfColumnIsCompleted(_alignmentIndex),
                _ => false
            };

            canvasGroup.alpha = isErased ? 0f : 1f;
            
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
            Deregister();
            canvasGroup.alpha = 1f;
        }
        
        private void OnCellInteracted(Cell cell)
        {
            var isCompleted = _alignmentType switch
            {
                AlignmentType.Row when cell.Row == _alignmentIndex => _gameplayManager.CheckIfRowIsCompleted(cell.Row),
                AlignmentType.Column when cell.Column == _alignmentIndex => _gameplayManager.CheckIfColumnIsCompleted(cell.Column),
                _ => false
            };

            if (isCompleted)
            {
                Fade();
            }
        }

        private void Fade(float duration = .2f)
        {
            canvasGroup.DOFade(0f, duration).From(1f);
        }
        
        public enum AlignmentType
        {
            Row,
            Column
        }
    }
}