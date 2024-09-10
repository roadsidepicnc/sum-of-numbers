using System.Collections.Generic;
using LevelManagement;
using ObjectPoolManagement;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GridManagement
{
    public class GridCreator
    {
        private readonly ObjectPoolManager _objectPoolManager;
        private readonly LevelManager _levelManager;

        private const float BoldLineThickness = 5f;
        private const float ThinLineThickness = 2.5f;

        [Inject]
        public GridCreator(ObjectPoolManager objectPoolManager, LevelManager levelManager)
        {
            _objectPoolManager = objectPoolManager;
            _levelManager = levelManager;
        }
        
        public void Create(int rowCount, int columnCount, GridLayoutGroup cellsParent, float cellSize, float gridSize, List<Cell> cells, GridLayoutGroup rowLinesParent, GridLayoutGroup columnLinesParent)
        {
            PlaceLines(rowLinesParent, columnLinesParent, rowCount, columnCount, cellSize, gridSize);
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
                var lineThickness = i == 0 || i == lineCount - 1  ? BoldLineThickness : ThinLineThickness;
                var lineColor = i == 0 || i == lineCount - 1 ? Color.black : new Color(75f / 255f, 74f / 255f, 75f / 255f);
                (line as GridLine)?.Set(gridSize + lineThickness, lineThickness, lineColor);
            }

            columnLinesParent.cellSize = Vector2.up * gridSize;
            columnLinesParent.spacing = Vector2.right * cellSize;
            columnLinesParent.constraintCount = lineCount;
            
            for (var i = 0; i < lineCount; i++)
            {
                var line = _objectPoolManager.GetObject(PoolObjectType.GridLine, columnLinesParent.transform);
                var lineThickness = i == 0 || i == lineCount - 1  ? BoldLineThickness : ThinLineThickness;
                (line as GridLine)?.Set(lineThickness, gridSize + lineThickness, Color.black);
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
                        Object.Destroy(cellPoolObject);
                        Debug.LogError("Cell couldn't be created");

                        foreach (var createdCell in cells)
                        {
                            Object.Destroy(createdCell);
                        }
                        
                        cells.Clear();
                        return;
                    }
                    
                    cell.Set("Cell (" + i + "," + j + ")", _levelManager.GetCellValue(i, j), i, j);
                    cells.Add(cell);
                }
            }
        }
    }
}