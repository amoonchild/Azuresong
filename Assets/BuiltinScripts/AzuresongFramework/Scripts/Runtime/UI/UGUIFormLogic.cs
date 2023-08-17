//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/06/30 10:44:36
//  Description: UGUIFormLogic
//------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public class UGUIFormLogic : UIFormLogic
    {
        public const int DepthFactor = 100;

        protected int _uiFormType = 0;
        protected Canvas _canvas = null;
        protected int _originalCanvasDepth = 0;
        protected CanvasGroup _canvasGroup = null;
        protected List<Canvas> _cachedCanvasContainer = new List<Canvas>();
        protected OpenUIFormInfo _openInfo = null;

        public int UIFormType
        {
            get { return _uiFormType; }
        }


        public void Close()
        {
            AzuresongEntry.UI.CloseUIForm(UIForm);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            _canvas = gameObject.GetOrAddComponent<Canvas>();
            _canvas.worldCamera = AzuresongEntry.UI.GetUICamera();
            _canvas.planeDistance = 1f;
            _canvas.overrideSorting = true;
            _originalCanvasDepth = _canvas.sortingOrder;

            _canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = false;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            RectTransform rc = gameObject.GetComponent<RectTransform>();
            rc.anchorMin = Vector2.zero;
            rc.anchorMax = Vector2.one;
            rc.anchoredPosition = Vector2.zero;
            rc.sizeDelta = Vector2.zero;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _openInfo = (OpenUIFormInfo)userData;

            gameObject.SetLayerRecursively(Constant.Layer.UI);

            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            if(!isShutdown)
            {
                if(_openInfo != null)
                {
                    ReferencePool.Release(_openInfo);
                    _openInfo = null;
                }
            }

            base.OnClose(isShutdown, userData);
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnPause()
        {
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetLayerRecursively(Constant.Layer.UICull);

            //base.OnPause();
        }

        protected override void OnResume()
        {
            //base.OnResume();

            gameObject.SetLayerRecursively(Constant.Layer.UI);
            _canvasGroup.blocksRaycasts = true;
        }
        
        protected override void OnCover()
        {
            base.OnCover();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            
            int deltaDepth = uiGroupDepth + DepthFactor * depthInUIGroup - _canvas.sortingOrder + _originalCanvasDepth;
            
            GetComponentsInChildren(true, _cachedCanvasContainer);
            for (int i = 0; i < _cachedCanvasContainer.Count; i++)
            {
                _cachedCanvasContainer[i].sortingOrder += deltaDepth;
            }
            _cachedCanvasContainer.Clear();
        }
    }
}