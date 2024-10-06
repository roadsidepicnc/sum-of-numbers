using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ObjectPoolingSystem;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Signals;
using Zenject;

namespace GridManagement
{
    public class Cell : PoolObject
    {
        [Inject] private ObjectPoolManager _objectPoolManager;
        [Inject] private SignalBus _signalBus;
        
        [Header("UI Components")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Image background;
        [SerializeField] private CanvasGroup canvasGroup;

        private const float CircleSizeMultiplier = .85f; 
        
        private Circle _circle;
        private RectTransform _rectTransform;
        
        public CellState CellState { get; private set; }
        public bool IsTarget { get; private set; }
        public int Value { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }

        public override void Initialize(Transform parent, Action<PoolObject> resetAction)
        {
            base.Initialize(parent, resetAction);
            _rectTransform = transform as RectTransform;
        }

        public void Set(string name, int value, int row, int column, CellState cellState, bool isTarget)
        {
            CellState = cellState;
            IsTarget = isTarget;
            Value = value;
            gameObject.name = name;
            Row = row;
            Column = column;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClick);
            SetUI();
        }

        private void OnButtonClick()
        {
            if (GameManager.GameState != GameState.OnGameplay)
            {
                return;
            }

            if (CellState == CellState.Selected || CellState == CellState.Erased)
            {
                return;
            }

            _signalBus.Fire(new CellInteractedSignal(this));
        }
        
        public override void Reset(Transform parent)
        {
            RemoveCircle();
            base.Reset(parent);
        }

        private void SetUI()
        {
            valueText.text = Value.ToString();
        }
        
        public async UniTask PlaceCircle()
        {
            if (_rectTransform == null)
            {
                return;
            }
            
            _circle = _objectPoolManager.GetObject(PoolObjectType.Circle, transform) as Circle;
            _circle?.Set(Color.black, _rectTransform.sizeDelta.x * CircleSizeMultiplier, _rectTransform.sizeDelta.y * CircleSizeMultiplier);
            if (_circle != null)
            {
                await _circle.PlayAnimation(.35f);
                CellState = CellState.Selected;
            }
        }

        private void RemoveCircle()
        {
            if (_circle == null)
            {
                return;
            }
            
            _objectPoolManager.ResetObject(_circle);
            _circle = null;
        }

        public async UniTask Erase(float duration = .2f)
        {
            await canvasGroup.DOFade(0f, duration).From(1f);
            CellState = CellState.Erased;
        }
    }
}