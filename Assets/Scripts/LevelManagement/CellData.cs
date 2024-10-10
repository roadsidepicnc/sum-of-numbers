using System;

namespace LevelManagement
{
    [Serializable]
    public class CellData
    {
        public bool isTarget;
        public int row;
        public int column;
        public int value;

        public CellData(int row, int column, int value, bool isTarget)
        {
            this.row = row;
            this.column = column;
            this.value = value;
            this.isTarget = isTarget;
        }
    }
}