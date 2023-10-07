//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/01 17:01:20
//  Description：DataTableGenerator
//------------------------------------------------------------
using Azuresong.Editor.DataTableTools;


namespace SarsGame.Editor
{
    public static class DataTableGeneratorConfig
    {
        [DataTableSourceFilePath]
        public static string DataTableSourceFile = "Assets/SarsGame/DataTables";

        [DataTableOutputFilePath]
        public static string DataTableOutputFile = "Assets/SarsGame/DataTables/BinaryFiles";

        [DataTableCSharpCodePath]
        public static string DataTableCSharpCode = "Assets/SarsGame/Scripts/Runtime/DataTable";
    }
}
