using System;
using ObjectPool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace GridManagement
{
    public class Cell : PoolObject
    {
        [SerializeField] private Button selectionButton;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Image background;

        public bool IsSelected { get; private set; }
        public int Value { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }

        public override void Initialize(Transform parent, Action<PoolObject> resetAction)
        {
            base.Initialize(parent, resetAction);
        }

        public override void Reset(Transform parent)
        {
            base.Reset(parent);
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
            IsSelected = !IsSelected;
            Signals.CellInteracted?.Invoke(this);
            SetBackground();
        }

        public void Reset()
        {
            IsSelected = false;
        }

        private void SetUI()
        {
            valueText.text = Value.ToString();
            SetBackground();
        }

        private void SetBackground()
        {
            background.color = IsSelected ? Color.green : Color.red;
        }
    }
}