//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/01 16:37:15
//  Description：ASDataTableExtension
//------------------------------------------------------------
using System;
using System.IO;
using UnityEngine;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public static class ASDataTableExtension
    {
        public const string DataRowClassPrefixName = "DR";
        public static readonly char[] DataSplitSeparators = new char[] { '\t' };
        public static readonly char[] DataTrimSeparators = new char[] { '\"' };


        public static void LoadDataTable(this DataTableComponent dataTableComponent, Type dataRowType,
            string dataTableAssetName, object userData = null)
        {
            if(dataRowType == null || string.IsNullOrEmpty(dataTableAssetName))
            {
                ASLog.Error("Datatable type is null '{0}'.", dataTableAssetName);
                return;
            }

            DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, string.Empty);
            dataTable.ReadData(dataTableAssetName, Azuresong.Runtime.Constant.AssetPriority.DataTableAsset, userData);
        }

        public static Vector3 ParseVector3(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
        }

        public static Vector3 ReadVector3(this BinaryReader binaryReader)
        {
            return new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }
    }
}