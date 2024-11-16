using System.Collections.Generic;

namespace LevelManagement
{
    public class LevelData
    {
        public int rowCount;
        public int columnCount;
        public int maxHeartCount;
        public int currentHeartCount;
        public List<CellData> cellDataList;

        public LevelData()
        {
            cellDataList = new();
        }
        
        public LevelData(int rowCount, int columnCount, int maxHeartCount, int currentHeartCount, List<CellData> cellDataList) : this()
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.maxHeartCount = maxHeartCount;
            this.currentHeartCount = currentHeartCount;
            
            foreach (var cellData in cellDataList)
            {
                this.cellDataList.Add(new CellData(cellData));
            }
        }
    }
}