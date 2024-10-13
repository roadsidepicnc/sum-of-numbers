#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace LevelManagement
{
    public class LevelDataGeneratorWindow : EditorWindow
    {
        private string _rowCountText;
        private string _columnCountText;
        private string _heartCountText;
        private string _idText;
        
        [MenuItem("Level Management/Level Data Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelDataGeneratorWindow>();
            window.titleContent = new GUIContent("Level Data Generator");
        }
        
        public void OnGUI()
        {
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("LEVEL GENERATOR", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Id:");
            _idText = GUILayout.TextField(_idText, GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Row Count:");
            _rowCountText = GUILayout.TextField(_rowCountText, GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Column Count:");
            _columnCountText = GUILayout.TextField(_columnCountText, GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Heart Count:");
            _heartCountText = GUILayout.TextField(_heartCountText, GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(25);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("GENERATE", GUILayout.Width(300), GUILayout.Height(50)))
            {
                TryToCreate();
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void TryToCreate()
        {
            if (!int.TryParse(_idText, out var id) || id < 0)
            {
                Debug.Log("Invalid id is entered");
                return;
            }
            
            if (!int.TryParse(_rowCountText, out var rowCount) || rowCount <= 0)
            {
                Debug.Log("Invalid row count is entered");
                return;
            }
                
            if (!int.TryParse(_columnCountText, out var columnCount) || columnCount <= 0 || rowCount != columnCount)
            {
                Debug.Log("Invalid column count is entered");
                return;
            }
                
            if (!int.TryParse(_heartCountText, out var heartCount) || heartCount <= 0)
            {
                Debug.Log("Invalid heart count is entered");
                return;
            }
            
            Create(id, rowCount, columnCount, heartCount);
        }

        private void Create(int id, int rowCount, int columnCount, int heartCount)
        {
            var cellDataList = new List<CellData>();
            
            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                {
                    cellDataList.Add(new CellData(i, j, Random.Range(1, 10), true));
                }
            }

            for (var i = 0; i < rowCount; i++)
            {
                var index = Random.Range(0, columnCount); 
                cellDataList[i * rowCount + index].isTarget = false;
            }

            var asset = CreateInstance<LevelData>();
            asset.id = id;
            asset.rowCount = rowCount;
            asset.columnCount = columnCount;
            asset.heartCount = heartCount;
            asset.cellDataList = cellDataList;
            
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Levels/Level-" + id + ".asset");
            AssetDatabase.SaveAssets();
            
            Debug.Log("Level-" + id + " is successfully created");
            DisplayGrid(rowCount, columnCount, heartCount, cellDataList);
        }

        private void DisplayGrid(int rowCount, int columnCount, int heartCount, List<CellData> cellDataList)
        {
            var str = "";
            for (var i = 0; i < rowCount; i++)
            {
                var rowTotal = 0;
                for (var j = 0; j < columnCount; j++)
                {
                    if (cellDataList[i * columnCount + j].isTarget)
                    {
                        var index = CoordinateConverter.Convert(i, j, columnCount);
                        str += cellDataList[index].value + " ";
                        rowTotal += cellDataList[index].value;
                    }
                    else
                    {
                        str += "X ";
                    }
                }

                str += "|" + rowTotal;
                str += "\n";
            }

            for (var i = 0; i < columnCount; i++)
            {
                str += "- ";
            }
            
            str += "\n";
            
            for (var i = 0; i < columnCount; i++)
            {
                var columnTotal = 0;
                for (var j = 0; j < rowCount; j++)
                {
                    if (cellDataList[j * rowCount + i].isTarget)
                    {
                        
                        columnTotal += cellDataList[CoordinateConverter.Convert(j, i, columnCount)].value;
                    }
                }
                
                str += columnTotal + " ";
            }

            str += "\nHeart Count: " + heartCount;
            
            Debug.Log(str);
        }
    }
}

#endif