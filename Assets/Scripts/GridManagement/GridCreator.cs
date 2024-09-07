using System.Collections.Generic;
using ObjectPool;
using UnityEngine;
using Zenject;

namespace GridManagement
{
    public class GridCreator
    {
        [Inject] private ObjectPoolManager _objectPoolManager;
        
        public void Create(int rowCount, int columnCount, Transform parent, float cellSize, List<Cell> cells)
        {
            PlaceTiles(parent, rowCount, columnCount, cellSize, cells);
        }
        
        private void PlaceTiles(Transform parent, int rowCount, int columnCount, float tileSize, List<Cell> cells)
        {
            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                {
                    var cellPoolObject = _objectPoolManager.GetObject(PoolObjectType.Cell, parent);
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
                    
                    cell.Set("Cell (" + i + "," + j + ")", Random.Range(1, 13), i, j);
                    cells.Add(cell);
                }
            }
        }
    }
}