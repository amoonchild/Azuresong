//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 15:40:57
//  Description：ProcedureInitResources
//------------------------------------------------------------
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.ObjectPool;
using System.Collections.Generic;
using System;
using LitJson;
using SarsFramework.Runtime;


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

            SarsEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            SarsEntry.Event.Subscribe(UnloadSceneSuccessEventArgs.EventId, OnUnloadSceneSuccess);
            SarsEntry.Event.Subscribe(UnloadSceneFailureEventArgs.EventId, OnUnloadSceneFailure);
            SarsEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            SarsEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            SarsEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            SarsEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            SarsEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            SarsEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            SarsEntry.Event.Subscribe(ShowEntityUpdateEventArgs.EventId, OnShowEntityUpdate);
            SarsEntry.Event.Subscribe(ShowEntityDependencyAssetEventArgs.EventId, OnShowEntityDependencyAsset);

            // 隐藏所有实体
            SarsEntry.Entity.HideAllLoadingEntities();
            SarsEntry.Entity.HideAllLoadedEntities();

            // 关闭所有界面
            SarsEntry.UI.CloseAllLoadingUIForms();
            SarsEntry.UI.CloseAllLoadedUIForms();

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

            // 界面逻辑已移动到unity,所以需要等待Loading界面打开
            SarsEntry.UI.OpenUIForm(UIFormType.Loading, _targetProcedure);
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

                SarsEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
                SarsEntry.Event.Unsubscribe(UnloadSceneSuccessEventArgs.EventId, OnUnloadSceneSuccess);
                SarsEntry.Event.Unsubscribe(UnloadSceneFailureEventArgs.EventId, OnUnloadSceneFailure);
                SarsEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
                SarsEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
                SarsEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
                SarsEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
                SarsEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
                SarsEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
                SarsEntry.Event.Unsubscribe(ShowEntityUpdateEventArgs.EventId, OnShowEntityUpdate);
                SarsEntry.Event.Unsubscribe(ShowEntityDependencyAssetEventArgs.EventId, OnShowEntityDependencyAsset);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void ReleasePools(bool performGCCollect)
        {
            List<ObjectPoolBase> pools = new List<ObjectPoolBase>();
            SarsEntry.ObjectPool.GetAllObjectPools(true, pools);

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
                SarsLog.Info("卸载场景 '{0}'.", _preSceneName);
                SarsEntry.Scene.UnloadScene(_preSceneName, this);
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
                SarsLog.Info("加载场景 '{0}'.", _targetProcedure.SceneName);
                SarsEntry.Scene.LoadScene(AssetUtility.GetScenePath(_targetProcedure.SceneName), this);
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
                UserData.playerEntityId = SarsEntry.Entity.GenerateSerialId();

                PlayerEntityData playerEntityData = new PlayerEntityData(_targetProcedure.PlayerInitPosition,
                    Quaternion.Euler(_targetProcedure.PlayerInitRotation), Vector3.one);

                SarsEntry.Entity.ShowEntity<PlayerEntityLogic>(UserData.playerEntityId, AssetUtility.GetPlayerPath(_playerAssetName),
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

            SarsEntry.Event.Fire(this, e);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;

            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)ne.UserData;
            if (openUIFormInfo.UIFormId == (int)UIFormType.Loading)
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

            SarsLog.Error("卸载场景 '{0}' 失败.", _preSceneName);
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

            SarsLog.Error("加载场景 '{0}' 失败.", _targetSceneAssetName);
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

            SarsLog.Error("加载玩家实体 '{0}' 失败.", ne.EntityAssetName);
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