//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/07/17 14:51:35
//  Description：NativeBridgeComponent
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework.Event;
using LitJson;


namespace SarsFramework.Runtime
{
    public class NativeBridgeComponent : GameFrameworkComponent
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _InitNativeInfo(string handlerObjName, string handlerFuncName);
        [DllImport("__Internal")]
        private static extern string _SendNativeData(string type, string msgData);
#else
        private static void _InitNativeInfo(string handlerObjName, string handlerFuncName)
        {

        }

        private static string _SendNativeData(string type, string msgData)
        {
            return string.Empty;
        }
#endif

        private GameObject _messageHandlerObj = null;
        private Dictionary<string, Action<JsonData>> _messageHandlers = new Dictionary<string, Action<JsonData>>();


        public void AddHandler(string msgType, Action<JsonData> handler)
        {
            if (!_messageHandlers.ContainsKey(msgType))
            {
                _messageHandlers.Add(msgType, handler);
            }
            else
            {
                _messageHandlers[msgType] += handler;
            }
        }

        public void RemoveHandler(string msgType, Action<JsonData> handler)
        {
            if (_messageHandlers.TryGetValue(msgType, out System.Action<JsonData> callback))
            {
                callback -= handler;
                if (callback == null)
                {
                    _messageHandlers.Remove(msgType);
                }
            }
        }

        public string SendData(string msgType, string msgData = "")
        {
            SarsLog.Info("Send message to {0}, type:{1}, data:{2}", Application.platform.ToString(), msgType, msgData);

#if UNITY_WEBGL && !UNITY_EDITOR
            return _SendNativeData(msgType, msgData);
#else
            return string.Empty;
#endif
        }


        private void Start()
        {
            EventComponent eventComponent = GameEntry.GetComponent<EventComponent>();
            eventComponent.Subscribe(HandleNativeMessageEventArgs.EventId, OnHandleNativeMessage);

            // TODO: 
            _messageHandlerObj = new GameObject("WebJSUtility");
            _messageHandlerObj.AddComponent<NativeMessageHandler>();
            DontDestroyOnLoad(_messageHandlerObj);

#if !UNITY_EDITOR
            _InitNativeInfo(_messageHandlerObj.name, "HandleNativeMessage");
#endif
        }

        private void OnDestroy()
        {
            if (_messageHandlerObj != null)
            {
                Destroy(_messageHandlerObj);
                _messageHandlerObj = null;
            }
        }

        private void OnHandleNativeMessage(object sender, GameEventArgs e)
        {
            HandleNativeMessageEventArgs ne = (HandleNativeMessageEventArgs)e;

            if(_messageHandlers.TryGetValue(ne.MsgType, out Action<JsonData> handlers))
            {
                handlers?.Invoke(ne.MsgData);
            }
        }
    }
}