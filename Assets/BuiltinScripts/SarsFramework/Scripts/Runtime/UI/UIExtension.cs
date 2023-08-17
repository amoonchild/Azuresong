//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/06/30 10:45:47
//  Description: UIExtension
//------------------------------------------------------------
using GameFramework.UI;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;


namespace SarsFramework.Runtime
{
    public static class UIExtension
    {
        private static Camera _uiCamera = null;


        public static bool HasUIForm(this UIComponent uiComponent, string uiFromAssetName, string uiGroupName = null)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(uiFromAssetName);
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(uiFromAssetName);
        }

        public static UGUIFormLogic GetUIForm(this UIComponent uiComponent, string uiFromAssetName, string uiGroupName = null)
        {
            UIForm uiForm = null;
            if (string.IsNullOrEmpty(uiGroupName))
            {
                uiForm = uiComponent.GetUIForm(uiFromAssetName);
                if (uiForm == null)
                {
                    return null;
                }

                return (UGUIFormLogic)uiForm.Logic;
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            uiForm = (UIForm)uiGroup.GetUIForm(uiFromAssetName);
            if (uiForm == null)
            {
                return null;
            }

            return (UGUIFormLogic)uiForm.Logic;
        }

        public static void CloseUIForm(this UIComponent uiComponent, string uiFromAssetName)
        {
            if(uiComponent.HasUIForm(uiFromAssetName))
            {
                uiComponent.CloseUIForm(uiComponent.GetUIForm(uiFromAssetName));
            }
        }

        public static void CloseUIForm(this UIComponent uiComponent, UGUIFormLogic uiFormLogic)
        {
            uiComponent.CloseUIForm(uiFormLogic.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, string uiFromAssetName, Type uiFormLogicType,
            string uiGroupName, bool pauseCoveredUIForm, bool allowMultiInstance, object userData = null)
        {
            if (!allowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(uiFromAssetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(uiFromAssetName))
                {
                    return null;
                }
            }

            OpenUIFormInfo openUIFormInfo = OpenUIFormInfo.Create(uiFormId, uiFromAssetName, uiFormLogicType, userData);

            return uiComponent.OpenUIForm(uiFromAssetName, uiGroupName, Constant.AssetPriority.UIFormAsset, pauseCoveredUIForm, openUIFormInfo);
        }

        public static Camera GetUICamera(this UIComponent uiComponent)
        {
            if (_uiCamera == null)
            {
                _uiCamera = uiComponent.GetComponentInChildren<Camera>();
            }

            return _uiCamera;
        }
    }
}