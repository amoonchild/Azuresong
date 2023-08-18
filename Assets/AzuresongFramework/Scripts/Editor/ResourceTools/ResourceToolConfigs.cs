//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  #CreationTime：2022-05-30 17:07:34
//  Description：ResourceToolConfigs
//------------------------------------------------------------
using GameFramework;
using System.IO;
using UnityEngine;
using UnityGameFramework.Editor;
using UnityGameFramework.Editor.ResourceTools;


namespace Azuresong.Editor
{
    public static class ResourceToolConfigs
    {
        public const string ConfigPath = "Assets/AzuresongFramework/Scripts/Editor/Settings/";

        [BuildSettingsConfigPath]
        public static string BuildSettingsConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, ConfigPath, "BuildSettings.xml"));

        [ResourceCollectionConfigPath]
        public static string ResourceCollectionConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, ConfigPath, "ResourceCollection.xml"));

        [ResourceEditorConfigPath]
        public static string ResourceEditorConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, ConfigPath, "ResourceEditor.xml"));

        [ResourceBuilderConfigPath]
        public static string ResourceBuilderConfig = Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, ConfigPath, "ResourceBuilder.xml"));
    }
}
