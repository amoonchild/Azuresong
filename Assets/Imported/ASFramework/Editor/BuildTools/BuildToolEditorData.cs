using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Azuresong.Editor.BuildTools
{
    public class BuildToolEditorData : ScriptableObject
    {
        public bool isEnableAllLog = false;
        public bool isEnableDebugLog = false;
        public bool isEnableInfoLog = false;
        public bool isEnableWarningLog = false;
        public bool isEnableErrorLog = false;
        public bool isEnableFateLog = false;
        public string outputDirectory = string.Empty;
        public List<string> scenes = new List<string>();
        public BuildTarget target = BuildTarget.WebGL;
        public bool developmentBuild = false;
    }
}