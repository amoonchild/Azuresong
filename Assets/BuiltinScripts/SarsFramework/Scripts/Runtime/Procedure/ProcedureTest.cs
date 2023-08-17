//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 14:05:00
//  Description：ProcedureTest
//------------------------------------------------------------
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;


namespace SarsFramework.Runtime
{
    public class ProcedureTest : SarsProcedure
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            //SarsEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);

            //SarsEntry.Resource.InitResources(OnInitResourcesCompleted);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            if(!isShutdown)
            {
                //SarsEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnInitResourcesCompleted()
        {
            Log.Info("OnInitResourcesCompleted");
            TestLoadScene();
        }

        private void TestLoadAsset()
        {
            bool isTestSwitch = false;

            SarsEntry.Resource.LoadAsset("Assets/Test/sprite.prefab", new GameFramework.Resource.LoadAssetCallbacks(
                (string assetName, object asset, float duration, object userData) =>
                {
                    if(isTestSwitch)
                    {
                        return;
                    }

                    Log.Info("asset success");
                    GameObject obj = GameObject.Instantiate<GameObject>(asset as GameObject);
                },
                (string assetName, LoadResourceStatus status, string errorMessage, object userData) =>
                {
                    Log.Info("asset failed");
                }), isTestSwitch);
        }

        private void TestLoadScene()
        {
            SarsEntry.Scene.LoadScene("Assets/Test/SampleScene.unity");
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;

            Log.Info("scene success");
            TestLoadAsset();
        }
    }
}