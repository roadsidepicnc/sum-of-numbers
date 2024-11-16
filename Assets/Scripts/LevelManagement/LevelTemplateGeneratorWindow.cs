#if UNITY_EDITOR

using System.Collections.Generic;
using GridManagement;
using UnityEditor;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace LevelManagement
{
    public class LevelTemplateGeneratorWindow : EditorWindow
    {
        private string _idText = "0";
        private string _rowCountText = "5";
        private string _columnCountText = "5";
        private string _heartCountText = "3";
        
        [MenuItem("Level Management/Level Template Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelTemplateGeneratorWindow>();
            window.titleContent = new GUIContent("Level Template Generator");
        }
        
        public void OnGUI()
        {
            EditorGUILayout.LabelField("Level Template Generator", EditorStyles.boldLabel);
            _idText = EditorGUILayout.TextField("Level id", _idText, new GUILayoutOption[] {GUILayout.Width(position.width * .985f)});
            _rowCountText = EditorGUILayout.TextField("Row Count", _rowCountText, new GUILayoutOption[] {GUILayout.Width(position.width * .985f)});
            _columnCountText = EditorGUILayout.TextField("Column Count", _columnCountText, new GUILayoutOption[] {GUILayout.Width(position.width * .985f)});
            _heartCountText = EditorGUILayout.TextField("Heart Count", _heartCountText, new GUILayoutOption[] {GUILayout.Width(position.width * .985f)});

            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("GENERATE", new GUILayoutOption[] {GUILayout.Width(position.width * .4875f)}))
            {
                TryToCreate();
            }
            
            if (GUILayout.Button("CLEAR", new GUILayoutOption[] {GUILayout.Width(position.width * .4875f)}))
            {
                TryToCreate();
            }

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
                    cellDataList.Add(new CellData(i, j, Random.Range(1, 10), true, CellState.NotSelected));
                }
            }

            for (var i = 0; i < rowCount; i++)
            {
                var index = Random.Range(0, columnCount); 
                cellDataList[i * rowCount + index].isTarget = false;
            }

            var asset = CreateInstance<LevelTemplate>();
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