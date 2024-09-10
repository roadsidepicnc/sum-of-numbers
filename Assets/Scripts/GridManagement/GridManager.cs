using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GridManagement
{
    public class GridManager : BaseManager
    {
        [SerializeField] private int rowCount;
        [SerializeField] private int columnCount;
        [SerializeField] private GridLayoutGroup cellsParent;
        [SerializeField] private GridLayoutGroup rowLinesParent;
        [SerializeField] private GridLayoutGroup columnLinesParent;
        
        [Inject] private GridCreator _gridCreator;

        private List<Cell> _cellList;
        
        public override void Initialize()
        {
            base.Initialize();

            var horizontalGridSize = cellsParent.GetComponent<RectTransform>().rect.width;
            var verticalGridSize = cellsParent.GetComponent<RectTransform>().rect.height;
            
            if (horizontalGridSize != verticalGridSize || rowCount != columnCount)
            {
                Debug.LogError("Grid cell size is not proper");
                return;
            }

            var cellSize = horizontalGridSize / rowCount; 
            
            _cellList = new();
            
            _gridCreator.Create(rowCount, columnCount, cellsParent, cellSize, horizontalGridSize, _cellList, rowLinesParent, columnLinesParent);
            IsInitialized = true;
        }

        public Cell GetCell(int row, int column) => _cellList.Find(x => x.Row == row && x.Column == column);
        
        public List<Cell> GetRow(int row) => _cellList.FindAll(x => x.Row == row);
        
        public List<Cell> GetColumn(int column) => _cellList.FindAll(x => x.Column == column);

        public int RowCount => rowCount;
        
        public int ColumnCount => columnCount;
    }
}