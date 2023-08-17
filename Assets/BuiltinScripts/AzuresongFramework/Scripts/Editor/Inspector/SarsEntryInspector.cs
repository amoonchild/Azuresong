//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/27 11:32:59
//  Description：SarsEntryInspector
//------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using Azuresong.Runtime;
using UnityGameFramework.Runtime;
using UnityGameFramework.Editor;


namespace Sars.Editor
{
    [CustomEditor(typeof(Azuresong.Runtime.AzuresongEntry))]
    public class SarsEntryInspector : GameFrameworkInspector
    {
        private static readonly float[] _gameSpeed = new float[] { 0f, 0.01f, 0.1f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };
        private static readonly string[] _gameSpeedForDisplay = new string[] { "0x", "0.01x", "0.1x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            DrawAll();

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                if (!EditorApplication.isPlaying || EditorApplication.isPaused)
                {
                    SceneView.RepaintAll();
                }
            }
        }

        private void DrawAll()
        {
            AzuresongEntry entry = target as AzuresongEntry;

            BaseComponent baseComponent = entry.GetComponentInChildren<BaseComponent>();
            FieldInfo m_EditorResourceMode = baseComponent.GetType().GetField("m_EditorResourceMode", BindingFlags.NonPublic | BindingFlags.Instance);

            EditorGUILayout.BeginVertical("box");
            {
                bool editorResourceMode = (bool)m_EditorResourceMode.GetValue(baseComponent);
                bool editorResourceModeNew = EditorGUILayout.Toggle("使用编辑资源", editorResourceMode);
                if (editorResourceModeNew != editorResourceMode)
                {
                    m_EditorResourceMode.SetValue(baseComponent, editorResourceModeNew);
                    //EditorUtility.SetDirty(baseComponent);

                    //EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
                    //foreach (EditorBuildSettingsScene scene in scenes)
                    //{
                    //    FileInfo fileInfo = new FileInfo(scene.path);
                    //    if (fileInfo.Name == "Launch.unity")
                    //    {
                    //        scene.enabled = true;
                    //    }
                    //    else
                    //    {
                    //        scene.enabled = editorResourceModeNew;
                    //    }
                    //}

                    //EditorBuildSettings.scenes = scenes;
                    if(!EditorApplication.isPlaying)
                    {
                        EditorUtility.SetDirty(baseComponent);
                    }
                }

                FieldInfo m_FrameRate = baseComponent.GetType().GetField("m_FrameRate", BindingFlags.NonPublic | BindingFlags.Instance);
                int frameRate = (int)m_FrameRate.GetValue(baseComponent);
                int newFrameRate = EditorGUILayout.IntSlider("帧率", frameRate, 1, 120);
                if (newFrameRate != frameRate)
                {
                    if (EditorApplication.isPlaying)
                    {
                        baseComponent.FrameRate = frameRate;
                    }
                    else
                    {
                        m_FrameRate.SetValue(baseComponent, newFrameRate);
                        EditorUtility.SetDirty(baseComponent);
                    }
                }

                FieldInfo m_GameSpeed = baseComponent.GetType().GetField("m_GameSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
                float speed = (float)m_GameSpeed.GetValue(baseComponent);
                float newSpeed = EditorGUILayout.Slider("速度", speed, 0f, 8f);
                int selectedGameSpeed = GUILayout.SelectionGrid(GetSelectedGameSpeed(newSpeed), _gameSpeedForDisplay, 5);
                if (selectedGameSpeed >= 0)
                {
                    newSpeed = GetGameSpeed(selectedGameSpeed);
                }

                if (newSpeed != speed)
                {
                    if (EditorApplication.isPlaying)
                    {
                        baseComponent.GameSpeed = newSpeed;
                    }
                    else
                    {
                        m_GameSpeed.SetValue(baseComponent, newSpeed);
                        EditorUtility.SetDirty(baseComponent);
                    }
                }
            }
            EditorGUILayout.EndVertical();

            DebuggerComponent debuggerComponent = entry.GetComponentInChildren<DebuggerComponent>();
            FieldInfo m_ActiveWindow = debuggerComponent.GetType().GetField("m_ActiveWindow", BindingFlags.NonPublic | BindingFlags.Instance);
            DebuggerActiveWindowType m_ActiveWindowValue = (DebuggerActiveWindowType)m_ActiveWindow.GetValue(debuggerComponent);
            DebuggerActiveWindowType m_ActiveWindowValueNew = (DebuggerActiveWindowType)EditorGUILayout.EnumPopup("Debugger Window", m_ActiveWindowValue);
            if (m_ActiveWindowValueNew != m_ActiveWindowValue)
            {
                m_ActiveWindow.SetValue(debuggerComponent, m_ActiveWindowValueNew);
                EditorUtility.SetDirty(debuggerComponent);
            }
        }

        private float GetGameSpeed(int selectedGameSpeed)
        {
            if (selectedGameSpeed < 0)
            {
                return _gameSpeed[0];
            }

            if (selectedGameSpeed >= _gameSpeed.Length)
            {
                return _gameSpeed[_gameSpeed.Length - 1];
            }

            return _gameSpeed[selectedGameSpeed];
        }

        private int GetSelectedGameSpeed(float gameSpeed)
        {
            for (int i = 0; i < _gameSpeed.Length; i++)
            {
                if (gameSpeed == _gameSpeed[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}