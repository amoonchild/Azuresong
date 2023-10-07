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
        [BuildSettingsConfigPath]
        public static string BuildSettingsConfig = Utility.Path.GetRegularPath(Path.Combine(EditorBase.SettingsPath, "BuildSettings.xml"));

        [ResourceCollectionConfigPath]
        public static string ResourceCollectionConfig = Utility.Path.GetRegularPath(Path.Combine(EditorBase.SettingsPath, "ResourceCollection.xml"));

        [ResourceEditorConfigPath]
        public static string ResourceEditorConfig = Utility.Path.GetRegularPath(Path.Combine(EditorBase.SettingsPath, "ResourceEditor.xml"));

        [ResourceBuilderConfigPath]
        public static string ResourceBuilderConfig = Utility.Path.GetRegularPath(Path.Combine(EditorBase.SettingsPath, "ResourceBuilder.xml"));
    }
}
