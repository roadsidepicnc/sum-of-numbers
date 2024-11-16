using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using LevelManagement;
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
        [Inject] private SignalBus _signalBus;
        [Inject] private CircleManager _circleManager;
        
        [Header("UI Components")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Image background;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private CellData _cellData;
        private Circle _circle;
        private RectTransform _rectTransform;

        public CellState CellState => _cellData.cellState;
        public bool IsTarget => _cellData.isTarget;
        public int Value => _cellData.value;
        public int Row => _cellData.row;
        public int Column => _cellData.column;

        public override void Initialize(Transform parent, Action<PoolObject> resetAction)
        {
            base.Initialize(parent, resetAction);
            _rectTransform = transform as RectTransform;
        }

        public void Set(string name, CellData cellData, float cellSize)
        {
            _cellData = cellData;
            gameObject.name = name;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClick);
            SetUI(cellSize);
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

        public void SetUI(float cellSize)
        {
            valueText.text = Value.ToString();
            canvasGroup.alpha = 1f;
            
            switch (CellState)
            {
                case CellState.Selected:
                    PlaceCircle(cellSize, false);
                    break;
                case CellState.Erased:
                    Erase(0f);
                    break;
            }
        }
        
        public async UniTask PlaceCircle(float cellSize, bool withAnimation = true)
        {
            if (_rectTransform == null)
            {
                return;
            }
            
            _circle = _circleManager.Create(transform, cellSize, cellSize);
            if (_circle != null)
            {
                if (withAnimation)
                {
                    await _circle.PlayAnimation(.35f);
                }
                
                _cellData.cellState = CellState.Selected;
            }
        }

        private void RemoveCircle()
        {
            if (_circle == null)
            {
                return;
            }
            
            _circleManager.Reset(_circle);
            _circle = null;
        }

        public async UniTask Erase(float duration = .2f)
        {
            await canvasGroup.DOFade(0f, duration).From(1f);
            _cellData.cellState = CellState.Erased;
        }
    }
}