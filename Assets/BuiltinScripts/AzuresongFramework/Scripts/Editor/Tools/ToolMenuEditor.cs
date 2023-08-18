//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  #CreationTime：2022-05-30 17:07:34
//  Description：ToolMenuEditor
//------------------------------------------------------------
using UnityEditor;
using UnityGameFramework.Editor.ResourceTools;


namespace Azuresong.Editor
{
    class ToolMenuEditor
    {
        [MenuItem("AzuresongTools/Resource Tools/Resource Builder", false)]
        private static void OpenResourceBuilder()
        {
            ResourceBuilder.Open();
        }

        [MenuItem("AzuresongTools/Resource Tools/Resource Rule Editor", false)]
        private static void OpenResourceRuleEditor()
        {
            ResourceRuleEditor.Open();
        }
    }
}