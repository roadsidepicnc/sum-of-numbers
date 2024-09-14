using System;
using ObjectPoolManagement;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace GridManagement
{
    public class Cell : PoolObject
    {
        [SerializeField] private Button selectionButton;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Image background;

        [Inject] private GameManager _gameManager;
        [Inject] private ObjectPoolManager _objectPoolManager;

        private Cross _cross;
        private RectTransform _rectTransform;
        
        public bool IsSelected { get; private set; }
        public int Value { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }

        public override void Initialize(Transform parent, Action<PoolObject> resetAction)
        {
            base.Initialize(parent, resetAction);
            _rectTransform = transform as RectTransform;
        }

        public void Set(string name, int value, int row, int column)
        {
            IsSelected = true;
            Value = value;
            gameObject.name = name;
            Row = row;
            Column = column;
            selectionButton.onClick.RemoveAllListeners();
            selectionButton.onClick.AddListener(OnButtonClick);
            SetUI();
        }

        private void OnButtonClick()
        {
            if (_gameManager.GameState != GameState.Running)
            {
                return;
            }
            
            IsSelected = !IsSelected;
            Signals.CellInteracted?.Invoke(this);
            if (!IsSelected)
            {
                PlaceCross();
            }
            else
            {
                RemoveCross();
            }
        }

        public void Reset()
        {
            RemoveCross();
            IsSelected = true;
        }

        private void SetUI()
        {
            valueText.text = Value.ToString();
            if (!IsSelected)
            {
                PlaceCross();
            }
            else
            {
                RemoveCross();
            }
        }

      

        private void PlaceCross()
        {
            if (_rectTransform == null)
            {
                return;
            }
            
            _cross = _objectPoolManager.GetObject(PoolObjectType.Cross, transform) as Cross;
            _cross?.Set(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y);
        }

        private void RemoveCross()
        {
            if (_cross == null)
            {
                return;
            }
            
            _objectPoolManager.ResetObject(_cross);
            _cross = null;
        }
    }
}