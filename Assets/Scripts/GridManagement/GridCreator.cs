using System.Collections.Generic;
using LevelManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GridManagement
{
    public class GridCreator : MonoBehaviour
    {
        [Inject] private ObjectPoolManager _objectPoolManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private GridManager _gridManager;

        [SerializeField] private float boldLineThickness;
        [SerializeField] private float thinLineThickness;
        [SerializeField] private GridLayoutGroup cellsParent;
        [SerializeField] private GridLayoutGroup rowLinesParent;
        [SerializeField] private GridLayoutGroup columnLinesParent;
        
        [Header("Colors")]
        [SerializeField] private Color boldLineColor;
        [SerializeField] private Color thinLineColor;
        
        public void Create(int rowCount, int columnCount, List<Cell> cells, ref float cellSize)
        {
            var horizontalGridSize = cellsParent.GetComponent<RectTransform>().rect.width;
            var verticalGridSize = cellsParent.GetComponent<RectTransform>().rect.height;
            
            if (horizontalGridSize != verticalGridSize || rowCount != columnCount)
            {
                Debug.LogError("Grid cell size is not proper");
                return;
            }

            cellSize = horizontalGridSize / rowCount; 
            PlaceLines(rowLinesParent, columnLinesParent, rowCount, columnCount, cellSize, horizontalGridSize);
            PlaceCells(cellsParent, rowCount, columnCount, cellSize, cells);
        }

        private void PlaceLines(GridLayoutGroup rowLinesParent, GridLayoutGroup columnLinesParent, int rowCount, int columnCount, float cellSize, float gridSize)
        {
            var lineCount = rowCount + 1;
            rowLinesParent.cellSize = Vector2.right * gridSize;
            rowLinesParent.spacing = Vector2.up * cellSize;
            rowLinesParent.constraintCount = lineCount;
            
            for (var i = 0; i < lineCount; i++)
            {
                var line = _objectPoolManager.GetObject(PoolObjectType.GridLine, rowLinesParent.transform);
                var lineThickness = i == 0 || i == lineCount - 1  ? boldLineThickness : thinLineThickness;
                var lineColor = i == 0 || i == lineCount - 1 ? boldLineColor : thinLineColor;
                (line as GridLine)?.Set(i == 0 || i == lineCount - 1 ? gridSize + lineThickness : gridSize - boldLineThickness, lineThickness, lineColor);
            }

            columnLinesParent.cellSize = Vector2.up * gridSize;
            columnLinesParent.spacing = Vector2.right * cellSize;
            columnLinesParent.constraintCount = lineCount;
            
            for (var i = 0; i < lineCount; i++)
            {
                var line = _objectPoolManager.GetObject(PoolObjectType.GridLine, columnLinesParent.transform);
                var lineThickness = i == 0 || i == lineCount - 1  ? boldLineThickness : thinLineThickness;
                var lineColor = i == 0 || i == lineCount - 1 ? boldLineColor : thinLineColor;
                (line as GridLine)?.Set(lineThickness, i == 0 || i == lineCount - 1 ? gridSize + lineThickness : gridSize - boldLineThickness, lineColor);
            }
        }
        
        private void PlaceCells(GridLayoutGroup parent, int rowCount, int columnCount, float cellSize, List<Cell> cells)
        {
            parent.cellSize = Vector2.one * cellSize;
            parent.spacing = Vector2.zero;
            parent.constraintCount = columnCount;
            
            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                {
                    var cellPoolObject = _objectPoolManager.GetObject(PoolObjectType.Cell, parent.transform);
                    var cell = cellPoolObject as Cell;
                    
                    if (cell == null)
                    {
                        Destroy(cellPoolObject);
                        Debug.LogError("Cell couldn't be created");

                        foreach (var createdCell in cells)
                        {
                            Destroy(createdCell);
                        }
                        
                        cells.Clear();
                        return;
                    }
                    
                    var cellData = _levelManager.GetCellData(i, j);
                    cell.Set("Cell (" + i + "," + j + ")", cellData.value, i, j, CellState.NotSelected, cellData.isTarget);
                    cells.Add(cell);
                }
            }
        }
    }
}