//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/18 11:37:15
//  Description：HybirdCLRComponent
//------------------------------------------------------------
using System;
using System.Reflection;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public class HybirdCLRComponent : GameFrameworkComponent
    {
        private string _hotfixDllListFileName = "HotfixDlls.txt";

        private bool _isEnable = false;
        private bool _isEditor = false;
        private Action<bool, string> _loadCompleteEvent = null;


        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
        }


        public void Init()
        {
            AzuresongEntry.Event.Fire(this, InitHybridCLRSuccessEventArgs.Create());
        }

        public void RunHotfix()
        {
            AzuresongEntry.Resource.LoadAsset("Assets/SarsGame/Entities/HotfixEntry.prefab",
                new LoadAssetCallbacks(OnLoadHotfixEntryPrefabSuccess, OnLoadHotfixEntryPrefabFailure));
        }

        private void LoadMetadata()
        {

        }

        private void LoadHotfixDlls()
        {

        }

        private void OnLoadHotfixDllAssetsSuccess()
        {
            
        }

        private void OnLoadHotfixEntryPrefabSuccess(string assetName, object asset, float duration, object userData)
        {
            GameObject obj = GameObject.Instantiate<GameObject>(asset as GameObject);
            DontDestroyOnLoad(obj);
        }

        private void OnLoadHotfixEntryPrefabFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            ASLog.Error("load hotfix entry error");
        }
    }
}