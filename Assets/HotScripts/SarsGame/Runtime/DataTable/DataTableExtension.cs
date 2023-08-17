//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/14 10:15:44
//  Description：DataTableExtension
//------------------------------------------------------------
using System;
using UnityGameFramework.Runtime;
using Azuresong.Runtime;


namespace SarsGame.Runtime
{
    public static class DataTableExtension
    {
        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName,
               bool useBytes, object userData = null)
        {
            dataTableComponent.LoadDataTable(Type.GetType(Azuresong.Runtime.DataTableExtension.DataRowClassPrefixName + dataTableName),
                AssetUtility.GetDataTablePath(dataTableName, useBytes), userData);
        }
    }
}