using DG.Tweening;
using Gameplay;
using GridManagement;
using ObjectPoolManagement;
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
            _signalManager.ResetGrid += OnResetGrid;
        }

        private void Deregister()
        {
            _signalManager.CellInteracted -= OnCellInteracted;
            _signalManager.ResetGrid -= OnResetGrid;
        }

        public void Set(int value, AlignmentType alignmentType, int alignmentIndex, float width, float height)
        {
            Register();
            targetScoreText.text = value.ToString();
            _alignmentType = alignmentType;
            _alignmentIndex = alignmentIndex;
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            var backgroundTransform = background.GetComponent<RectTransform>();
            switch (alignmentType)
            {
                case AlignmentType.Column:
                    backgroundTransform.offsetMin = new Vector2(5f, 0f);
                    backgroundTransform.offsetMax = new Vector2(-5f, 0f);
                    rectTransform.pivot = new Vector2(.5f, 1f);
                    rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0f);
                    break;
                case AlignmentType.Row:
                    backgroundTransform.offsetMin = new Vector2(0f, 5f);
                    backgroundTransform.offsetMax = new Vector2(0f, -5f);
                    rectTransform.pivot = new Vector2(0f, .5f);
                    rectTransform.localPosition = new Vector3(0f, rectTransform.localPosition.y);
                    break;
            }
        }

        public override void Reset(Transform parent)
        {
            base.Reset(parent);
            Deregister();
        }

        private void UpdateUI(bool isCompleted)
        {
            if (isCompleted)
            {
                Fade(Constants.TargetScoreColorChangeDuration, Constants.TargetScoreBackgroundCompletedColor, Constants.TargetScoreTextCompletedColor);
            }
            else
            {
                Fade(Constants.TargetScoreColorChangeDuration, Constants.TargetScoreBackgroundNotCompletedColor, Constants.TargetScoreTextNotCompletedColor);
            }
        }

        private void OnCellInteracted(Cell cell)
        {
            switch (_alignmentType)
            {
                case AlignmentType.Row when cell.Row == _alignmentIndex:
                {
                    var x = _gameplayManager.CheckIfRowIsCompleted(cell.Row);
                    UpdateUI(x);
                    break;
                }
                case AlignmentType.Column when cell.Column == _alignmentIndex:
                {
                    var x = _gameplayManager.CheckIfColumnIsCompleted(cell.Column);
                    UpdateUI(x);
                    break;
                }
            }
        }

        private void Fade(float duration, Color backgroundTargetColor, Color textTargetColor)
        {
            background.DOComplete();
            background.DOColor(backgroundTargetColor, duration);
            targetScoreText.DOComplete();
            targetScoreText.DOColor(textTargetColor, duration);
        }

        private void OnResetGrid()
        {
            Fade(0f, Constants.TargetScoreBackgroundNotCompletedColor, Constants.TargetScoreTextNotCompletedColor);
        }

        public enum AlignmentType
        {
            Row,
            Column
        }
    }
}