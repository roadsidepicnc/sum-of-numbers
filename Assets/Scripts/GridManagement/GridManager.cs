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
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private Transform parent;
        
        [Inject] private GridCreator _gridCreator;

        private List<Cell> _cellList;
        
        public override void Initialize()
        {
            base.Initialize();
            _cellList = new();
            
            var horizontalCellSize = gridLayout.cellSize.x;
            var verticalCellSize = gridLayout.cellSize.y;

            if (horizontalCellSize != verticalCellSize)
            {
                Debug.LogError("Grid cell size is not proper");
                return;
            }
            
            _gridCreator.Create(rowCount, columnCount, parent, horizontalCellSize, _cellList);
            IsInitialized = true;
        }

        public Cell GetCell(int row, int column) => _cellList.Find(x => x.Row == row && x.Column == column);
        
        public List<Cell> GetRow(int row) => _cellList.FindAll(x => x.Row == row);
        
        public List<Cell> GetColumn(int column) => _cellList.FindAll(x => x.Column == column);

        public int RowCount => rowCount;
        
        public int ColumnCount => columnCount;
    }
}