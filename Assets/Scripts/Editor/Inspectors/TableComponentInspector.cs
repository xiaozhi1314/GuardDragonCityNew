/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TableComponentInspector.cs
 * author:    taoye
 * created:   2021/1/12
 * descrip:   Table组件编辑器面板定制
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


namespace BigDream
{
    [CustomEditor(typeof(TableComponent))]
    internal sealed class TableComponentInspector : Editor
    {

        private string ExcelTablePath = "";
        private string outputJsonPath= "";

        private List<string> allExcelFiles;
        private void OnEnable()
        {
            ExcelTablePath =  Txt.Format("{0}/../Table", Application.dataPath);
            outputJsonPath = Application.dataPath + "/StreamingAssets/Json";
            allExcelFiles = Directory.GetFiles(ExcelTablePath, "*.xlsm").Where(file => !System.IO.Path.GetFileName(file).StartsWith("~$", StringComparison.Ordinal)).ToList();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            if (GUILayout.Button("导出全部"))
            {
                foreach (var excelFiles in allExcelFiles)
                {
                    string excelFileName = System.IO.Path.GetFileNameWithoutExtension(excelFiles);
                    TableExportEditorUtility.ExportExcelToJsonArrayList(excelFiles, $"{outputJsonPath}/{excelFileName}.json");
                }

                GUIUtility.ExitGUI();
            }
            
            EditorGUILayout.BeginVertical("box");
            {
                foreach (var excelFiles in allExcelFiles)
                {

                    string excelFileName = System.IO.Path.GetFileNameWithoutExtension(excelFiles);
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.LabelField(string.Concat($"{excelFileName}.xlsm"), GUILayout.MaxWidth(250));

                        if (GUILayout.Button("打开自定义Excel"))
                        {
                            TableExportEditorUtility.OpenExcel(excelFiles);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }
    }
}
