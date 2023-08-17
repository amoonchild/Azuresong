using UnityEngine;
using UnityEditor;
using UnityGameFramework.Editor.ResourceTools;


namespace Sars.Editor
{
    class ToolMenuEditor
    {
        [MenuItem("SarsTools/Resource Tools/Resource Builder", false)]
        private static void OpenResourceBuilder()
        {
            ResourceBuilder.Open();
        }

        [MenuItem("SarsTools/Resource Tools/Resource Rule Editor", false)]
        private static void OpenResourceRuleEditor()
        {
            ResourceRuleEditor.Open();
        }
    }
}