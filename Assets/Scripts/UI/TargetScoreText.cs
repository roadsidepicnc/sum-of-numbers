using Gameplay;
using GridManagement;
using ObjectPoolManagement;
using TMPro;
using UnityEngine;
using Utilities;
using Zenject;

namespace UI
{
    public class TargetScoreText : PoolObject
    {
        [SerializeField] private TextMeshProUGUI targetScoreText;

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
        }

        private void Deregister()
        {
            Signals.CellInteracted -= OnCellInteracted;
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
            targetScoreText.color = isCompleted ? Color.green : Color.white;
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

        public enum AlignmentType
        {
            Row,
            Column
        }
    }
}