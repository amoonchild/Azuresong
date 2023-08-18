//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/06/30 10:41:10
//  Description: UGuiFormHelper
//------------------------------------------------------------
using UnityEngine;
using GameFramework.UI;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public class UGuiFormHelper : UIFormHelperBase
    {
        private ResourceComponent _resourceComponent = null;


        private void Start()
        {
            _resourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (_resourceComponent == null)
            {
                ASLog.Fatal("Resource component is invalid.");
                return;
            }
        }

        public override object InstantiateUIForm(object uiFormAsset)
        {
            return Instantiate((Object)uiFormAsset);
        }

        public override IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            GameObject gameObject = uiFormInstance as GameObject;
            if (gameObject == null)
            {
                ASLog.Error("UI form instance is invalid.");
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(((MonoBehaviour)uiGroup.Helper).transform);
            transform.localScale = Vector3.one;

            OpenUIFormInfo uiOpenUserData = (OpenUIFormInfo)userData;
            if (gameObject.GetComponent(uiOpenUserData.UIFormLogicType) == null)
            {
                gameObject.AddComponent(uiOpenUserData.UIFormLogicType);
            }

            return gameObject.GetOrAddComponent<UIForm>();
        }

        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
        {
            DestroyImmediate((Object)uiFormInstance);
            _resourceComponent.UnloadAsset(uiFormAsset);
        }
    }
}