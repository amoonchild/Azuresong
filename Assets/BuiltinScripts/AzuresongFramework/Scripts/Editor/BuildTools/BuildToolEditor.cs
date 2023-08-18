//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/07/18 15:34:01
//  Description：BuildToolEditor
//------------------------------------------------------------
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityGameFramework.Editor;
using UnityGameFramework.Editor.ResourceTools;
using System;
using System.Text;
using GameFramework;
using GameFramework.Resource;
using Azuresong.Editor.DataTableTools;


namespace Azuresong.Editor.BuildTools
{
    public partial class BuildToolEditor : EditorWindow
    {
        private static readonly string m_ConfigurationDir = "Assets/BuiltinScripts/AzuresongFramework/Scripts/Editor/Settings";
        private static readonly string m_ConfigurationName = "BuildToolEditorData.asset";

        private static bool _isVal = false;

        [MenuItem("AzuresongTools/Build Tools/Build Tool", false)]
        private static void Open()
        {
            BuildToolEditor window = GetWindow<BuildToolEditor>(false, "Build Tool", true);
        }


        private ResourceRuleEditorData _resourceRuleConfiguration = null;
        private ResourceBuilderController _resourceBuilderController = null;
        private BuildToolEditorData _configuration;
        private EditorBuildSettingsScene[] _scenesInBuild = null;
        private bool _orderBuildWebGL = false;
        private int _resourceVersion = 0;


        private void OnEnable()
        {
            _resourceRuleConfiguration = null;
            _resourceBuilderController = null;
            _configuration = null;
            _scenesInBuild = null;
            _orderBuildWebGL = false;
            _isVal = false;

            OnSceneListChanged();
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        }

        private void OnDisable()
        {
            EditorBuildSettings.sceneListChanged -= OnSceneListChanged;
        }

        private void OnValidate()
        {
            _isVal = false;
        }

        private void OnFocus()
        {
            _isVal = false;
        }

        private void OnGUI()
        {

            if (!_isVal)
            {
                _isVal = true;
                _resourceRuleConfiguration = AssetDatabase.LoadAssetAtPath<ResourceRuleEditorData>(ResourceRuleEditor.ConfigurationPath);
                _resourceBuilderController = new ResourceBuilderController();
                if (!_resourceBuilderController.Load())
                {
                    _resourceBuilderController = null;
                }

                OnSceneListChanged();
                LoadResourceVersion();
            }

            if (_configuration == null)
            {
                LoadConfiguration();
            }

            GUILayout.Space(10f);
            DrawResourceSettings();

            GUILayout.Space(10f);
            DrawBuildSettings();

            GUILayout.Space(10f);
            EditorGUILayout.LabelField("App Version:", Application.version);
            EditorGUILayout.LabelField("Resource Version:", _resourceVersion.ToString());

            if (GUILayout.Button("Build App"))
            {
                _orderBuildWebGL = true;
            }

            GUILayout.Space(5f);
            if (GUILayout.Button("Build Resource And App"))
            {
                if (ResourceRuleEditor.RefreshResourceCollection())
                {
                    if (ResourceBuilder.OpenAndBuild())
                    {
                        _orderBuildWebGL = true;
                    }
                }
            }
        }

        private void Update()
        {
            if (_orderBuildWebGL)
            {
                _orderBuildWebGL = false;
                BuildWebGL(_configuration);
            }
        }

        private void LoadConfiguration()
        {
            string path = Path.Combine(m_ConfigurationDir, m_ConfigurationName);
            _configuration = AssetDatabase.LoadAssetAtPath<BuildToolEditorData>(path);
            if (_configuration == null)
            {
                _configuration = ScriptableObject.CreateInstance<BuildToolEditorData>();

                if (!Directory.Exists(m_ConfigurationDir))
                {
                    Directory.CreateDirectory(m_ConfigurationDir);
                }

                AssetDatabase.CreateAsset(_configuration, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void SaveConfiguration()
        {
            //string path = Path.Combine(m_ConfigurationDir, m_ConfigurationName);
            //if (AssetDatabase.LoadAssetAtPath<BuildToolEditorData>(path) == null)
            //{
            //    if (!Directory.Exists(m_ConfigurationDir))
            //    {
            //        Directory.CreateDirectory(m_ConfigurationDir);
            //    }

            //    AssetDatabase.CreateAsset(_configuration, path);
            //    AssetDatabase.Refresh();
            //}
            //else
            {
                EditorUtility.SetDirty(_configuration);
                AssetDatabase.SaveAssets();
            }
        }

        private void DrawResourceSettings()
        {
            GUILayout.Label("Resource Settings:");

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Open Resource Builder"))
                    {
                        ResourceBuilder.Open();
                    }

                    if (GUILayout.Button("Open Resource Rule Editor"))
                    {
                        ResourceRuleEditor.Open();
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (_resourceBuilderController != null && _resourceBuilderController.IsValidOutputDirectory && GUILayout.Button("Open Output directory"))
                {
                    OpenFolder.Execute(_resourceBuilderController.OutputDirectory);
                }

                GUILayout.Space(10f);

                string warningMsg = string.Empty;
                string errorMsg = string.Empty;

                if (_resourceRuleConfiguration == null)
                {
                    AppendMsg(ref errorMsg, "Resource rule data does not exist.");
                }
                else if (_resourceRuleConfiguration.rules.Count == 0)
                {
                    AppendMsg(ref warningMsg, "Configure resource rules first.");
                }

                if (_resourceBuilderController == null)
                {
                    AppendMsg(ref errorMsg, "Platform is invalid.");
                    AppendMsg(ref errorMsg, "Compression helper is invalid.");
                    AppendMsg(ref errorMsg, "Output directory is invalid.");
                }
                else
                {
                    if (_resourceBuilderController.Platforms == 0)
                    {
                        AppendMsg(ref errorMsg, "Platform is invalid.");
                    }

                    if (string.IsNullOrEmpty(_resourceBuilderController.CompressionHelperTypeName))
                    {
                        AppendMsg(ref errorMsg, "Compression helper is invalid.");
                    }

                    if (!_resourceBuilderController.IsValidOutputDirectory)
                    {
                        AppendMsg(ref errorMsg, "Output directory is invalid.");
                    }
                }

                if (!string.IsNullOrEmpty(warningMsg))
                {
                    EditorGUILayout.HelpBox(warningMsg, MessageType.Warning);
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    EditorGUILayout.HelpBox(errorMsg, MessageType.Error);
                }

                if (GUILayout.Button("Build Resource"))
                {
                    DataTableGeneratorMenu.GenerateDataTables();

                    if (ResourceRuleEditor.RefreshResourceCollection())
                    {

                        ResourceBuilder.OpenAndBuild();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawBuildSettings()
        {
            GUILayout.Label("Build Settings:");

            DrawLog();

            GUILayout.Space(5f);
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Scenes In Build:");
                GUILayout.Space(3f);

                bool isChanged = false;
                if (_scenesInBuild != null)
                {
                    for (int i = 0; i < _scenesInBuild.Length; i++)
                    {
                        EditorBuildSettingsScene scene = _scenesInBuild[i];
                        bool isEnable = _configuration.scenes.Contains(scene.path);
                        bool isEnableChanged = EditorGUILayout.ToggleLeft(scene.path.Replace("Assets/", string.Empty), isEnable);
                        if (isEnableChanged != isEnable)
                        {
                            if (isEnableChanged && !isEnable)
                            {
                                _configuration.scenes.Add(scene.path);
                                isChanged = true;
                            }
                            else if (!isEnableChanged && isEnable)
                            {
                                _configuration.scenes.Remove(scene.path);
                                isChanged = true;
                            }
                        }
                    }
                }
                else
                {
                    if (_configuration.scenes.Count > 0)
                    {
                        _configuration.scenes.Clear();
                        isChanged = true;
                    }
                }

                if (isChanged)
                {
                    SaveConfiguration();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal("box");
            {
                _configuration.outputDirectory = EditorGUILayout.TextField("Output Directory", _configuration.outputDirectory);
                if (GUILayout.Button("Browse..."))
                {
                    _configuration.outputDirectory = EditorUtility.OpenFolderPanel("Output Directory", _configuration.outputDirectory, string.Empty);
                    SaveConfiguration();
                }
            }
            EditorGUILayout.EndHorizontal();

            DrawUntiySettings();

            string errorMsg = string.Empty;

            if (_configuration.scenes.Count == 0)
            {
                AppendMsg(ref errorMsg, "No startup scene selected.");
            }

            if (string.IsNullOrEmpty(_configuration.outputDirectory) || !Directory.Exists(_configuration.outputDirectory))
            {
                AppendMsg(ref errorMsg, "Output directory is invalid.");
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                EditorGUILayout.HelpBox(errorMsg, MessageType.Error);
            }
        }

        private void DrawLog()
        {
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Log Level:");
                GUILayout.Space(3f);

                int isChanged = DrawLog("All", "ENABLE_LOG", ref _configuration.isEnableAllLog) ? 1 : 0;
                if (isChanged == 0)
                {
                    isChanged += DrawLog("Debug", "ENABLE_DEBUG_LOG", ref _configuration.isEnableDebugLog) ? 1 : 0;
                    isChanged += DrawLog("Info", "ENABLE_INFO_LOG", ref _configuration.isEnableInfoLog) ? 1 : 0;
                    isChanged += DrawLog("Warning", "ENABLE_WARNING_LOG", ref _configuration.isEnableWarningLog) ? 1 : 0;
                    isChanged += DrawLog("Error", "ENABLE_ERROR_LOG", ref _configuration.isEnableErrorLog) ? 1 : 0;
                    isChanged += DrawLog("Fatal", "ENABLE_FATAL_LOG", ref _configuration.isEnableFateLog) ? 1 : 0;
                }

                if (isChanged != 0)
                {
                    SaveConfiguration();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private bool DrawLog(string logLevel, string symbol, ref bool isEnable)
        {
            bool isChanged = false;
            bool isLogEnabledChanged = EditorGUILayout.ToggleLeft(logLevel, isEnable);
            if (isEnable != isLogEnabledChanged)
            {
                isEnable = isLogEnabledChanged;
                isChanged = true;
            }

            if (isEnable && !ScriptingDefineSymbols.HasScriptingDefineSymbol(EditorUserBuildSettings.selectedBuildTargetGroup, symbol))
            {
                ScriptingDefineSymbols.AddScriptingDefineSymbol(EditorUserBuildSettings.selectedBuildTargetGroup, symbol);
            }
            else if (!isEnable && ScriptingDefineSymbols.HasScriptingDefineSymbol(EditorUserBuildSettings.selectedBuildTargetGroup, symbol))
            {
                ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EditorUserBuildSettings.selectedBuildTargetGroup, symbol);
            }

            return isChanged;
        }

        private void DrawUntiySettings()
        {
            //bool developmentBuild = m_Configuration.developmentBuild;
            //bool developmentBuildNew = EditorGUILayout.Toggle("DevelopmentBuild", developmentBuild);
            //if (developmentBuildNew != developmentBuild)
            //{
            //    m_Configuration.developmentBuild = developmentBuildNew;
            //    Save();
            //}
        }

        private void AppendMsg(ref string currMsg, string newMsg)
        {
            if (!string.IsNullOrEmpty(currMsg))
            {
                currMsg += Environment.NewLine;
            }

            currMsg += newMsg;
        }

        private void OnSceneListChanged()
        {
            _scenesInBuild = EditorBuildSettings.scenes;
        }

        private void BuildWebGL(BuildToolEditorData configuration)
        {
            if (configuration == null)
            {
                return;
            }

            if (configuration.scenes.Count == 0)
            {
                Debug.LogError("No startup scene selected.");
                return;
            }

            if (string.IsNullOrEmpty(configuration.outputDirectory) || !Directory.Exists(configuration.outputDirectory))
            {
                Debug.LogError("Output directory is invalid.");
                return;
            }

            UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = configuration.outputDirectory;

            buildPlayerOptions.scenes = configuration.scenes.ToArray();
            buildPlayerOptions.target = configuration.target;
            buildPlayerOptions.targetGroup = BuildPipeline.GetBuildTargetGroup(configuration.target);
            buildPlayerOptions.options = BuildOptions.None;

            if (configuration.developmentBuild || EditorUserBuildSettings.development)
            {
                buildPlayerOptions.options |= BuildOptions.Development;
            }

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.LogFormat("Build {0} succeeded.", buildPlayerOptions.target.ToString());

                OpenFolder.Execute(buildPlayerOptions.locationPathName);
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogErrorFormat("Build {0} failed.", buildPlayerOptions.target.ToString());
            }
            else if (summary.result == BuildResult.Cancelled)
            {
                Debug.LogFormat("Build {0} cancelled.", buildPlayerOptions.target.ToString());
            }
        }

        private void LoadResourceVersion()
        {
            _resourceVersion = 0;
            string versionDatPath = Utility.Path.GetRegularPath((Path.Combine(Application.dataPath, "StreamingAssets/GameFrameworkVersion.dat")));
            if (!File.Exists(versionDatPath))
            {
                return;
            }

            using (FileStream fs = new FileStream(versionDatPath, FileMode.Open))
            {
                int totalBytes = (int)fs.Length;
                byte[] bytes = new byte[totalBytes];
                int bytesRead = 0;

                while (bytesRead < totalBytes)
                {
                    int len = fs.Read(bytes, bytesRead, totalBytes);
                    bytesRead += len;
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    PackageVersionListSerializer packageVersionListSerializer = new PackageVersionListSerializer();
                    packageVersionListSerializer.RegisterDeserializeCallback(0, (stream) => { return default(PackageVersionList); });
                    packageVersionListSerializer.RegisterDeserializeCallback(1, (stream) => { return default(PackageVersionList); });
                    packageVersionListSerializer.RegisterDeserializeCallback(2, (stream) =>
                    {
                        using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);
                            string applicableGameVersion = binaryReader.ReadEncryptedString(encryptBytes);
                            int internalResourceVersion = binaryReader.Read7BitEncodedInt32();
                            int assetCount = binaryReader.Read7BitEncodedInt32();

                            return new PackageVersionList(Application.version, internalResourceVersion, null, null, null, null);
                        }
                    });

                    PackageVersionList versionList = packageVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new GameFrameworkException("Deserialize package version list failure.");
                    }

                    _resourceVersion = versionList.InternalResourceVersion;
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Deserialize package version list error: {0}", e.Message);
                    _resourceVersion = 0;
                }
            }
        }
    }
}