//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/18 10:20:22
//  Description：UnityWebRequestAgentHelper
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using GameFramework;
using GameFramework.WebRequest;
using UnityGameFramework.Runtime;
using LitJson;


namespace Azuresong.Runtime
{
    public class UnityWebRequestAgentHelper : WebRequestAgentHelperBase, IDisposable
    {
        private UnityWebRequest _unityWebRequest = null;
        private bool _disposed = false;
        private EventHandler<WebRequestAgentHelperCompleteEventArgs> _webRequestAgentHelperCompleteEventHandler = null;
        private EventHandler<WebRequestAgentHelperErrorEventArgs> _webRequestAgentHelperErrorEventHandler = null;

        public override event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete
        {
            add
            {
                _webRequestAgentHelperCompleteEventHandler += value;
            }
            remove
            {
                _webRequestAgentHelperCompleteEventHandler -= value;
            }
        }
        public override event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError
        {
            add
            {
                _webRequestAgentHelperErrorEventHandler += value;
            }
            remove
            {
                _webRequestAgentHelperErrorEventHandler -= value;
            }
        }


        public override void Request(string webRequestUri, object userData)
        {
            if (_webRequestAgentHelperCompleteEventHandler == null || _webRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            WWWFormInfo wwwFormInfo = (WWWFormInfo)userData;
            UnityWebRequestAgentData agentData = (UnityWebRequestAgentData)wwwFormInfo.UserData;
            WebRequestData requestData = agentData.RequestData;

            if (requestData.Params.Count > 0)
            {
                webRequestUri = requestData.GetFullUrlWithParamsText();
            }

            switch (requestData.Type)
            {
                case UnityWebRequest.kHttpVerbGET:
                    {
                        _unityWebRequest = UnityWebRequest.Get(webRequestUri);
                    }
                    break;
                case UnityWebRequest.kHttpVerbDELETE:
                    {
                        _unityWebRequest = UnityWebRequest.Delete(webRequestUri);
                    }
                    break;
                case UnityWebRequest.kHttpVerbPOST:
                    {
                        if (requestData.UploadFileForm != null)
                        {
                            _unityWebRequest = UnityWebRequest.Post(webRequestUri, requestData.UploadFileForm);
                        }
                        else
                        {
                            _unityWebRequest = new UnityWebRequest(webRequestUri, UnityWebRequest.kHttpVerbPOST);
                            
                            if (requestData.JsonObject.Count > 0)
                            {
                                _unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(requestData.GetJsonBytes());
                                requestData.SetContentTypeToJson();
                            }

                            _unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                        }
                    }
                    break;
                case UnityWebRequest.kHttpVerbPUT:
                    {
                        _unityWebRequest = new UnityWebRequest(webRequestUri, UnityWebRequest.kHttpVerbPUT);
                        
                        if (requestData.JsonObject.Count > 0)
                        {
                            _unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(requestData.GetJsonBytes());
                            requestData.SetContentTypeToJson();
                        }

                        _unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                    }
                    break;
                default:
                    break;
            }

            if (_unityWebRequest == null)
            {
                Log.Fatal("Web request agent request type error.");
                return;
            }

            if (requestData.Headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in requestData.Headers)
                {
                    _unityWebRequest.SetRequestHeader(item.Key, item.Value);
                }
            }

            _unityWebRequest.SendWebRequest();
        }

        public override void Request(string webRequestUri, byte[] postData, object userData)
        {
            
        }

        public override void Reset()
        {
            if (_unityWebRequest != null)
            {
                _unityWebRequest.Dispose();
                _unityWebRequest = null;
            }
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
                if (_unityWebRequest != null)
                {
                    _unityWebRequest.Dispose();
                    _unityWebRequest = null;
                }
            }

            _disposed = true;
        }

        private void Update()
        {
            if (_unityWebRequest == null || !_unityWebRequest.isDone)
            {
                return;
            }

            if (_unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                JsonData jsonObject = new JsonData();
                jsonObject["code"] = _unityWebRequest.responseCode;
                jsonObject["msg"] = _unityWebRequest.downloadHandler.text;
                WebRequestAgentHelperErrorEventArgs webRequestAgentHelperErrorEventArgs = WebRequestAgentHelperErrorEventArgs.Create(jsonObject.ToJson());
                _webRequestAgentHelperErrorEventHandler(this, webRequestAgentHelperErrorEventArgs);
                ReferencePool.Release(webRequestAgentHelperErrorEventArgs);
            }
            else if (_unityWebRequest.downloadHandler.isDone)
            {
                WebRequestAgentHelperCompleteEventArgs webRequestAgentHelperCompleteEventArgs = WebRequestAgentHelperCompleteEventArgs.Create(_unityWebRequest.downloadHandler.data);
                _webRequestAgentHelperCompleteEventHandler(this, webRequestAgentHelperCompleteEventArgs);
                ReferencePool.Release(webRequestAgentHelperCompleteEventArgs);
            }
        }
    }
}
