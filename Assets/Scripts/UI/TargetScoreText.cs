using Cysharp.Threading.Tasks;
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
        private GameplayManager _gameplayManager;

        [Inject]
        private void InstallDependencies(GameplayManager gameplayManager)
        {
            _gameplayManager = gameplayManager;
        }

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Deregister();
        }

        private void Register()
        {
            Signals.CellInteracted += OnCellInteracted;
            Signals.ResetGrid += OnResetGrid;
        }

        private void Deregister()
        {
            Signals.CellInteracted -= OnCellInteracted;
            Signals.ResetGrid -= OnResetGrid;

        }

        public void Set(int value, AlignmentType alignmentType, int alignmentIndex, float width, float height)
        {
            targetScoreText.text = value.ToString();
            _alignmentType = alignmentType;
            _alignmentIndex = alignmentIndex;
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }

        private void UpdateUI(bool isCompleted)
        {
            if (isCompleted)
            {
                Fade(.4f, .3f, new Color(226f / 255f, 226f / 255f, 226f / 255f, 1f));
            }
            else
            {
                Fade(1f, .3f, new Color(188f / 255f, 1986f / 255f, 204f / 255f, 1f));
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

        public async UniTask Fade(float target, float duration, Color backgroundColor)
        {
            await canvasGroup.DOFade(target, duration);
            background.color = backgroundColor;
        }

        private void OnResetGrid()
        {
            Fade(1f, 0f, new Color(188f / 255f, 1986f / 255f, 204f / 255f, 1f));
        }

        public enum AlignmentType
        {
            Row,
            Column
        }
    }
}