using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    [Serializable, CreateAssetMenu(fileName = "Level Template", menuName = "Level Management/Level Template", order = 1)]
    public class LevelTemplate : ScriptableObject
    {
        public int id;
        public int rowCount;
        public int columnCount;
        public int heartCount;
        public List<CellData> cellDataList;

        public LevelTemplate(int rowCount, int columnCount, List<CellData> cellDataList)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.cellDataList = cellDataList;
        }
    }
}