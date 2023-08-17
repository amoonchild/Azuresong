//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 18:01:00
//  Description：WebGLLoadResourceAgentHelper
//------------------------------------------------------------
using GameFramework;
using GameFramework.FileSystem;
using GameFramework.Resource;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using Utility = GameFramework.Utility;


namespace SarsFramework.Runtime
{
    public class WebGLLoadResourceAgentHelper : LoadResourceAgentHelperBase, IDisposable
    {
        private string _fileFullPath = null;
        private string _assetName = null;
        private float _lastProgress = 0f;
        private bool _disposed = false;
        private ResourceComponent _resourceComponent = null;

        private UnityWebRequest _urlAssetBundleCreateRequest = null;
        private AssetBundleRequest _assetBundleRequest = null;
        private AsyncOperation _asyncOperation = null;

        private EventHandler<LoadResourceAgentHelperUpdateEventArgs> _loadResourceAgentHelperUpdateEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> _loadResourceAgentHelperReadFileCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> _loadResourceAgentHelperReadBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> _loadResourceAgentHelperParseBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> _loadResourceAgentHelperLoadCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperErrorEventArgs> _loadResourceAgentHelperErrorEventHandler = null;

        public override event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate
        {
            add
            {
                _loadResourceAgentHelperUpdateEventHandler += value;
            }
            remove
            {
                _loadResourceAgentHelperUpdateEventHandler -= value;
            }
        }

        public override event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete
        {
            add
            {
                _loadResourceAgentHelperReadFileCompleteEventHandler += value;
            }
            remove
            {
                _loadResourceAgentHelperReadFileCompleteEventHandler -= value;
            }
        }

        public override event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete
        {
            add
            {
                _loadResourceAgentHelperReadBytesCompleteEventHandler += value;
            }
            remove
            {
                _loadResourceAgentHelperReadBytesCompleteEventHandler -= value;
            }
        }

        public override event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> LoadResourceAgentHelperParseBytesComplete
        {
            add
            {
                _loadResourceAgentHelperParseBytesCompleteEventHandler += value;
            }
            remove
            {
                _loadResourceAgentHelperParseBytesCompleteEventHandler -= value;
            }
        }

        public override event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete
        {
            add
            {
                _loadResourceAgentHelperLoadCompleteEventHandler += value;
            }
            remove
            {
                _loadResourceAgentHelperLoadCompleteEventHandler -= value;
            }
        }

        public override event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError
        {
            add
            {
                _loadResourceAgentHelperErrorEventHandler += value;
            }
            remove
            {
                _loadResourceAgentHelperErrorEventHandler -= value;
            }
        }

        public override void ReadFile(string fullPath)
        {
            if (_loadResourceAgentHelperReadFileCompleteEventHandler == null || _loadResourceAgentHelperUpdateEventHandler == null || _loadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            _fileFullPath = fullPath;

#if UNITY_EDITOR
            _urlAssetBundleCreateRequest = UnityWebRequestAssetBundle.GetAssetBundle(fullPath);
#else
            _urlAssetBundleCreateRequest = UnityWebRequestAssetBundle.GetAssetBundle(fullPath, (uint)_resourceComponent.InternalResourceVersion, 0);
#endif
            _urlAssetBundleCreateRequest.SendWebRequest();
        }

        public override void ReadFile(IFileSystem fileSystem, string name)
        {

        }

        public override void ReadBytes(string fullPath)
        {

        }

        public override void ReadBytes(IFileSystem fileSystem, string name)
        {

        }

        public override void ParseBytes(byte[] bytes)
        {

        }

        public override void LoadAsset(object resource, string assetName, Type assetType, bool isScene)
        {
            if (_loadResourceAgentHelperLoadCompleteEventHandler == null || _loadResourceAgentHelperUpdateEventHandler == null || _loadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            AssetBundle assetBundle = resource as AssetBundle;
            if (assetBundle == null)
            {
                LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.TypeError, "Can not load asset bundle from loaded resource which is not an asset bundle.");
                _loadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, "Can not load asset from asset bundle which child name is invalid.");
                _loadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                return;
            }

            _assetName = assetName;
            if (isScene)
            {
                int sceneNamePositionStart = assetName.LastIndexOf('/');
                int sceneNamePositionEnd = assetName.LastIndexOf('.');
                if (sceneNamePositionStart <= 0 || sceneNamePositionEnd <= 0 || sceneNamePositionStart > sceneNamePositionEnd)
                {
                    LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, Utility.Text.Format("Scene name '{0}' is invalid.", assetName));
                    _loadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    return;
                }

                string sceneName = assetName.Substring(sceneNamePositionStart + 1, sceneNamePositionEnd - sceneNamePositionStart - 1);
                _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                if (assetType != null)
                {
                    _assetBundleRequest = assetBundle.LoadAssetAsync(assetName, assetType);
                }
                else
                {
                    _assetBundleRequest = assetBundle.LoadAssetAsync(assetName);
                }
            }
        }

        public override void Reset()
        {
            _fileFullPath = null;
            _assetName = null;
            _lastProgress = 0f;

            if (_urlAssetBundleCreateRequest != null)
            {
                _urlAssetBundleCreateRequest.Dispose();
                _urlAssetBundleCreateRequest = null;
            }

            _urlAssetBundleCreateRequest = null;
            _assetBundleRequest = null;
            _asyncOperation = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_urlAssetBundleCreateRequest != null)
                {
                    _urlAssetBundleCreateRequest.Dispose();
                    _urlAssetBundleCreateRequest = null;
                }
            }

            _disposed = true;
        }

        private void Start()
        {
            _resourceComponent = GameEntry.GetComponent<ResourceComponent>();
        }

        private void Update()
        {
            UpdateUrlAssetBundleCreateRequest();
            UpdateAssetBundleRequest();
            UpdateAsyncOperation();
        }

        private void UpdateUrlAssetBundleCreateRequest()
        {
            if (_urlAssetBundleCreateRequest != null)
            {
                if (_urlAssetBundleCreateRequest.isDone)
                {
                    AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(_urlAssetBundleCreateRequest);
                    if (assetBundle != null)
                    {
                        UnityWebRequest oldUrlAssetBundleCreateRequest = _urlAssetBundleCreateRequest;
                        LoadResourceAgentHelperReadFileCompleteEventArgs loadResourceAgentHelperReadFileCompleteEventArgs = LoadResourceAgentHelperReadFileCompleteEventArgs.Create(assetBundle);
                        _loadResourceAgentHelperReadFileCompleteEventHandler(this, loadResourceAgentHelperReadFileCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperReadFileCompleteEventArgs);
                        if (_urlAssetBundleCreateRequest == oldUrlAssetBundleCreateRequest)
                        {
                            _urlAssetBundleCreateRequest = null;
                            _lastProgress = 0f;
                        }
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, Utility.Text.Format("Can not load asset bundle from file '{0}' which is not a valid asset bundle.", _fileFullPath));
                        _loadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (_urlAssetBundleCreateRequest.downloadProgress != _lastProgress)
                {
                    _lastProgress = _urlAssetBundleCreateRequest.downloadProgress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadResource, _urlAssetBundleCreateRequest.downloadProgress);
                    _loadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }

        private void UpdateAssetBundleRequest()
        {
            if (_assetBundleRequest != null)
            {
                if (_assetBundleRequest.isDone)
                {
                    if (_assetBundleRequest.asset != null)
                    {
                        LoadResourceAgentHelperLoadCompleteEventArgs loadResourceAgentHelperLoadCompleteEventArgs = LoadResourceAgentHelperLoadCompleteEventArgs.Create(_assetBundleRequest.asset);
                        _loadResourceAgentHelperLoadCompleteEventHandler(this, loadResourceAgentHelperLoadCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperLoadCompleteEventArgs);
                        _assetName = null;
                        _lastProgress = 0f;
                        _assetBundleRequest = null;
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, Utility.Text.Format("Can not load asset '{0}' from asset bundle which is not exist.", _assetName));
                        _loadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (_assetBundleRequest.progress != _lastProgress)
                {
                    _lastProgress = _assetBundleRequest.progress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadAsset, _assetBundleRequest.progress);
                    _loadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }

        private void UpdateAsyncOperation()
        {
            if (_asyncOperation != null)
            {
                if (_asyncOperation.isDone)
                {
                    if (_asyncOperation.allowSceneActivation)
                    {
                        LoadResourceAgentHelperLoadCompleteEventArgs loadResourceAgentHelperLoadCompleteEventArgs = LoadResourceAgentHelperLoadCompleteEventArgs.Create(_assetName);
                        _loadResourceAgentHelperLoadCompleteEventHandler(this, loadResourceAgentHelperLoadCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperLoadCompleteEventArgs);
                        _assetName = null;
                        _lastProgress = 0f;
                        _asyncOperation = null;
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, Utility.Text.Format("Can not load scene asset '{0}' from asset bundle.", _assetName));
                        _loadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (_asyncOperation.progress != _lastProgress)
                {
                    _lastProgress = _asyncOperation.progress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadScene, _asyncOperation.progress);
                    _loadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }
    }
}
