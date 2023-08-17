using GameFramework;
using GameFramework.DataTable;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace Sars.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        [MenuItem("SarsTools/DataTable/Generate DataTables")]
        public static void GenerateDataTables()
        {
            GenerateDataTablesAndCodes(false);
        }

        [MenuItem("SarsTools/DataTable/Generate DataTables And Codes")]
        public static void GenerateDataTablesAndCodes()
        {
            GenerateDataTablesAndCodes(true);
        }

        private static void GenerateDataTablesAndCodes(bool generateCode)
        {
            string dataTablePath = Type.GetConfigurationPath<DataTableSourceFilePathAttribute>();
            if (string.IsNullOrEmpty(dataTablePath))
            {
                return;
            }

            string[] dataTables = AssetDatabase.FindAssets(string.Empty, new string[] { dataTablePath });
            for (int i = 0; i < dataTables.Length; i++)
            {
                FileInfo file = new FileInfo(AssetDatabase.GUIDToAssetPath(dataTables[i]));
                if(file.Extension != ".txt")
                {
                    continue;
                }

                string dataTableName = file.Name.Replace(file.Extension, "");
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                if(generateCode)
                {
                    DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
