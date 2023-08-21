//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 15:40:57
//  Description：ProcedureChangeScene
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using Azuresong.Runtime;
using GameFramework.Event;
using GameFramework.ObjectPool;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace SarsGame.Runtime
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private string _preSceneName = string.Empty;                    // 前一个场景资源名
        private bool _isUnloadPreSceneComplete = false;                 // 前一个场景是否卸载完毕
        private ProcedureBase _targetProcedure = null;                  // 下一个流程
        private string _targetSceneAssetName = string.Empty;            // 下一个场景资源名
        private float _loadSceneProgress = 0f;                          // 下一个场景加载进度
        private float _loadSceneDependencyProgress = 0f;                // 下一个场景依赖资源加载进度
        private bool _isLoadSceneCompleted = false;                     // 下一个场景是否加载完成
        private string _playerAssetName = string.Empty;                 // 玩家实体资源名
        private float _loadPlayerProgress = 0f;                         // 玩家实体加载进度
        private float _loadPlayerDependencyProgress = 0f;               // 玩家实体依赖资源加载进度
        private bool _isLoadPlayerCompleted = false;                    // 玩家实体是否加载完成


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            AzuresongEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            AzuresongEntry.Event.Subscribe(UnloadSceneSuccessEventArgs.EventId, OnUnloadSceneSuccess);
            AzuresongEntry.Event.Subscribe(UnloadSceneFailureEventArgs.EventId, OnUnloadSceneFailure);
            AzuresongEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            AzuresongEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            AzuresongEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            AzuresongEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            AzuresongEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            AzuresongEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            AzuresongEntry.Event.Subscribe(ShowEntityUpdateEventArgs.EventId, OnShowEntityUpdate);
            AzuresongEntry.Event.Subscribe(ShowEntityDependencyAssetEventArgs.EventId, OnShowEntityDependencyAsset);

            // 隐藏所有实体
            AzuresongEntry.Entity.HideAllLoadingEntities();
            AzuresongEntry.Entity.HideAllLoadedEntities();

            // 关闭所有界面
            AzuresongEntry.UI.CloseAllLoadingUIForms();
            AzuresongEntry.UI.CloseAllLoadedUIForms();

            _targetProcedure = procedureOwner.GetData<VarObject>("NextProcedure").GetValue() as ProcedureBase;
            _targetSceneAssetName = AssetUtility.GetScenePath(_targetProcedure.SceneName);
            _playerAssetName = _targetProcedure.NeedPlayer ? UserData.vmodel : string.Empty;

            _isUnloadPreSceneComplete = false;
            _loadSceneProgress = 0f;
            _loadSceneDependencyProgress = 0f;
            _isLoadSceneCompleted = false;
            _loadPlayerProgress = 0f;
            _loadPlayerDependencyProgress = 0f;
            _isLoadPlayerCompleted = false;

            AzuresongEntry.UI.OpenUIForm(UIFormType.Loading, _targetProcedure);
        }

        protected override void OnUpdateSelf(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (!_isUnloadPreSceneComplete || !_isLoadSceneCompleted || !_isLoadPlayerCompleted)
            {
                return;
            }

            ChangeState(procedureOwner, _targetProcedure.GetType());
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (!isShutdown)
            {
                _targetProcedure = null;
                _targetSceneAssetName = string.Empty;
                _playerAssetName = string.Empty;
                procedureOwner.RemoveData("NextProcedure");

                AzuresongEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
                AzuresongEntry.Event.Unsubscribe(UnloadSceneSuccessEventArgs.EventId, OnUnloadSceneSuccess);
                AzuresongEntry.Event.Unsubscribe(UnloadSceneFailureEventArgs.EventId, OnUnloadSceneFailure);
                AzuresongEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
                AzuresongEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
                AzuresongEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
                AzuresongEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
                AzuresongEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
                AzuresongEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
                AzuresongEntry.Event.Unsubscribe(ShowEntityUpdateEventArgs.EventId, OnShowEntityUpdate);
                AzuresongEntry.Event.Unsubscribe(ShowEntityDependencyAssetEventArgs.EventId, OnShowEntityDependencyAsset);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void ReleasePools(bool performGCCollect)
        {
            List<ObjectPoolBase> pools = new List<ObjectPoolBase>();
            AzuresongEntry.ObjectPool.GetAllObjectPools(true, pools);

            bool canRelease = true;
            while (canRelease)
            {
                canRelease = false;
                for (int i = 0; i < pools.Count; i++)
                {
                    if (pools[i].CanReleaseCount > 0)
                    {
                        pools[i].ReleaseAllUnused();
                        canRelease = true;
                    }
                }
            }

            if (performGCCollect)
            {
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
        }

        private void UnloadPreScene()
        {
            if (!string.IsNullOrEmpty(_preSceneName))
            {
                ASLog.Info("卸载场景 '{0}'.", _preSceneName);
                AzuresongEntry.Scene.UnloadScene(_preSceneName, this);
            }
            else
            {
                _isUnloadPreSceneComplete = true;
                LoadNextScene();
            }
        }

        private void LoadNextScene()
        {
            if (!string.IsNullOrEmpty(_targetProcedure.SceneName))
            {
                ASLog.Info("加载场景 '{0}'.", _targetProcedure.SceneName);
                AzuresongEntry.Scene.LoadScene(AssetUtility.GetScenePath(_targetProcedure.SceneName), this);
            }
            else
            {
                _isLoadSceneCompleted = true;
            }
        }

        private void ShowPlayer()
        {
            if (!string.IsNullOrEmpty(_playerAssetName))
            {
                UserData.playerEntityId = AzuresongEntry.Entity.GenerateSerialId();

                // TODO:
                PlayerEntityData playerEntityData = new PlayerEntityData(_targetProcedure.PlayerInitPosition,
                    Quaternion.Euler(_targetProcedure.PlayerInitRotation), Vector3.one);

                AzuresongEntry.Entity.ShowEntity<PlayerEntityLogic>(UserData.playerEntityId, AssetUtility.GetPlayerPath(_playerAssetName),
                    Constant.EntityGroup.Player, playerEntityData);
            }
            else
            {
                _isLoadPlayerCompleted = true;
            }
        }

        private void UpdateLoadingProgress()
        {
            UpdateLoadingProgressEventArgs e = null;
            if (!string.IsNullOrEmpty(_playerAssetName))
            {
                e = UpdateLoadingProgressEventArgs.Create((_loadSceneProgress + _loadPlayerProgress) * 0.5f);
            }
            else
            {
                e = UpdateLoadingProgressEventArgs.Create(_loadSceneProgress);
            }

            AzuresongEntry.Event.Fire(this, e);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;

            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)ne.UserData;
            if (openUIFormInfo.UIFormType == (int)UIFormType.Loading)
            {
                UnloadPreScene();
            }
        }

        private void OnUnloadSceneSuccess(object sender, GameEventArgs e)
        {
            UnloadSceneSuccessEventArgs ne = (UnloadSceneSuccessEventArgs)e;

            if (ne.UserData != this || ne.SceneAssetName != _preSceneName)
            {
                return;
            }

            _preSceneName = string.Empty;
            _isUnloadPreSceneComplete = true;

            ReleasePools(true);
            LoadNextScene();
        }

        private void OnUnloadSceneFailure(object sender, GameEventArgs e)
        {
            UnloadSceneFailureEventArgs ne = (UnloadSceneFailureEventArgs)e;

            if (ne.UserData != this || ne.SceneAssetName != _preSceneName)
            {
                return;
            }

            ASLog.Error("卸载场景 '{0}' 失败.", _preSceneName);
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;

            if (ne.UserData != this || ne.SceneAssetName != _targetSceneAssetName)
            {
                return;
            }

            _loadSceneProgress = 1f;
            _isLoadSceneCompleted = true;
            UpdateLoadingProgress();

            _preSceneName = _targetSceneAssetName;

            ReleasePools(false);
            ShowPlayer();
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;

            if (ne.UserData != this || ne.SceneAssetName != _targetSceneAssetName)
            {
                return;
            }

            ASLog.Error("加载场景 '{0}' 失败.", _targetSceneAssetName);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;

            if (ne.UserData != this || ne.SceneAssetName != _targetSceneAssetName)
            {
                return;
            }

            if (_loadSceneDependencyProgress > 0f)
            {
                _loadSceneProgress = _loadSceneDependencyProgress + ne.Progress * (1f - _loadSceneDependencyProgress);
            }
            else
            {
                _loadSceneProgress = ne.Progress;
            }

            UpdateLoadingProgress();
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;

            if (ne.UserData != this || ne.SceneAssetName != _targetSceneAssetName)
            {
                return;
            }

            _loadSceneProgress = _loadSceneDependencyProgress = (float)ne.LoadedCount / (float)ne.TotalCount * 0.5f;
            UpdateLoadingProgress();
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;

            if (ne.Entity.Id != UserData.playerEntityId)
            {
                return;
            }

            _loadPlayerProgress = 1f;
            _isLoadPlayerCompleted = true;
            UpdateLoadingProgress();
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;

            if (ne.EntityId != UserData.playerEntityId)
            {
                return;
            }

            ASLog.Error("加载玩家实体 '{0}' 失败.", ne.EntityAssetName);
        }

        private void OnShowEntityUpdate(object sender, GameEventArgs e)
        {
            ShowEntityUpdateEventArgs ne = (ShowEntityUpdateEventArgs)e;

            if (ne.EntityId != UserData.playerEntityId)
            {
                return;
            }

            if (_loadPlayerDependencyProgress > 0f)
            {
                _loadPlayerProgress = _loadPlayerDependencyProgress + ne.Progress * (1f - _loadPlayerDependencyProgress);
            }
            else
            {
                _loadPlayerProgress = ne.Progress;
            }

            UpdateLoadingProgress();
        }

        private void OnShowEntityDependencyAsset(object sender, GameEventArgs e)
        {
            ShowEntityDependencyAssetEventArgs ne = (ShowEntityDependencyAssetEventArgs)e;

            if (ne.EntityId != UserData.playerEntityId)
            {
                return;
            }

            _loadPlayerProgress = _loadPlayerDependencyProgress = (float)ne.LoadedCount / (float)ne.TotalCount * 0.5f;
            UpdateLoadingProgress();
        }
    }
}