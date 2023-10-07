//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/02 14:00:22
//  Description：UICamera
//------------------------------------------------------------
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.Rendering.Universal;


namespace Azuresong.Runtime
{
    public class UICamera : MonoBehaviour
    {
        private UIComponent _uiComponent = null;
        private EventComponent _eventComponent = null;
        private SceneComponent _sceneComponent = null;
        private bool _isURP = true;


        private void Start()
        {
            _uiComponent = GameEntry.GetComponent<UIComponent>();

            _eventComponent = GameEntry.GetComponent<EventComponent>();
            _eventComponent.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);

            _sceneComponent = GameEntry.GetComponent<SceneComponent>();

            Camera camera = GetComponent<Camera>();
            if (_isURP)
            {
                UniversalAdditionalCameraData cameraData = camera.GetUniversalAdditionalCameraData();
                cameraData.renderType = CameraRenderType.Overlay;
            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            if (_isURP)
            {
                UniversalAdditionalCameraData cameraData = _sceneComponent.MainCamera.GetUniversalAdditionalCameraData();
                if (cameraData.renderType == CameraRenderType.Base)
                {
                    cameraData.cameraStack.Add(_uiComponent.GetUICamera());
                }
            }
        }
    }
}