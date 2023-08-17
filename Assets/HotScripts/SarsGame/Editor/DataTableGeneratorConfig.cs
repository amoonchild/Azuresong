//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/01 17:01:20
//  Description：DataTableGenerator
//------------------------------------------------------------
using Sars.Editor.DataTableTools;


namespace SarsGame.Editor
{
    public static class DataTableGeneratorConfig
    {
        public const string ConfigPath = "SarsFramework/Scripts/Editor/ResourceTools/ResourceBuilderSettings/";


        [DataTableSourceFilePath]
        public static string DataTableSourceFile = "Assets/SarsAssets/DataTables";

        [DataTableOutputFilePath]
        public static string DataTableOutputFile = "Assets/SarsAssets/DataTables/BinaryFiles";

        [DataTableCSharpCodePath]
        public static string DataTableCSharpCode = "Assets/Scripts/Runtime/DataTable";

        [DataTableCSharpCodeTemplatePath]
        public static string DataTableCSharpCodeTemplate = "Assets/Resources/DataTableCodeTemplate.txt";
    }
}
