using System;
using GridManagement;

namespace LevelManagement
{
    [Serializable]
    public class CellData
    {
        public bool isTarget;
        public CellState cellState;
        public int row;
        public int column;
        public int value;

        public CellData()
        {
        }
        
        public CellData(int row, int column, int value, bool isTarget, CellState cellState)
        {
            this.row = row;
            this.column = column;
            this.value = value;
            this.isTarget = isTarget;
            this.cellState = cellState;
        }
        
        public CellData(CellData cellData)
        {
            row = cellData.row;
            column = cellData.column;
            value = cellData.value;
            isTarget = cellData.isTarget;
            cellState = cellData.cellState;
        }
    }
}