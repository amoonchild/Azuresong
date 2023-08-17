//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/01 10:05:16
//  Description：UIExtension
//------------------------------------------------------------
using Azuresong.Runtime;
using UnityGameFramework.Runtime;


namespace SarsGame.Runtime
{
    public static class UIExtension
    {
        public static int? OpenUIForm(this UIComponent uiComponent, UIFormType uiFormType)
        {
            return uiComponent.OpenUIForm(uiFormType, null);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormType uiFormType, object userData)
        {
            if (UIFormConfigList.Configs.TryGetValue(uiFormType, out UIFormConfig uiFormInitData))
            {
                return uiComponent.OpenUIForm((int)uiFormType, uiFormInitData.AssetName, uiFormInitData.LogicType, uiFormInitData.UIGroupName,
                    uiFormInitData.PauseCoveredUIForm, uiFormInitData.AllowMultiInstance, userData);
            }

            return null;
        }

        public static void CloseUIForm(this UIComponent uiComponent, UIFormType uiFormType)
        {
            if (UIFormConfigList.Configs.TryGetValue(uiFormType, out UIFormConfig uiFormInitData))
            {
                uiComponent.CloseUIForm(uiFormInitData.AssetName);
            }
        }
    }
}