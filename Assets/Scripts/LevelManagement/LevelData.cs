using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    [Serializable, CreateAssetMenu(fileName = "Level Data", menuName = "Level Management/Level Data", order = 1)]
    public class LevelData : ScriptableObject
    {
        public int id;
        public int rowCount;
        public int columnCount;
        public int heartCount;
        public List<CellData> cellDataList;

        public LevelData(int rowCount, int columnCount, List<CellData> cellDataList)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.cellDataList = cellDataList;
        }
    }
}