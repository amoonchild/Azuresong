//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/09/01 16:50:26
//  Description：JsonEventManager
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using LitJson;


namespace Azuresong.Runtime
{
    public class JsonEventComponent : GameFrameworkComponent
    {
        public class JsonEventListener
        {
            private int _type = 0;
            private Action<JsonData> _handler = null;


            public JsonEventListener(int type)
            {
                _type = type;
            }

            public void AddHandler(Action<JsonData> handler)
            {
                //TODO: strict check
                _handler += handler;
            }

            public void RemoveHandler(Action<JsonData> handler)
            {
                //TODO: strict check
                _handler -= handler;
            }

            public void Excute(JsonData jsonData)
            {
                _handler?.Invoke(jsonData);
            }
        }

        private Dictionary<int, JsonEventListener> _handlers = new Dictionary<int, JsonEventListener>();


        public void AddHandler(int jsonEventType, Action<JsonData> handler)
        {
            if (handler == null)
            {
                return;
            }

            JsonEventListener jsonEventListener;
            if (!_handlers.TryGetValue(jsonEventType, out jsonEventListener))
            {
                jsonEventListener = new JsonEventListener(jsonEventType);
                _handlers.Add(jsonEventType, jsonEventListener);
            }

            jsonEventListener.AddHandler(handler);
        }

        public void RemoveHandler(int jsonEventType, Action<JsonData> handler)
        {
            if (handler == null)
            {
                return;
            }

            if (_handlers.TryGetValue(jsonEventType, out JsonEventListener eventListener))
            {
                eventListener.RemoveHandler(handler);
            }
        }

        public bool Excute(int jsonEventType)
        {
            return Excute(jsonEventType, null);
        }

        public bool Excute(int jsonEventType, JsonData jsonData)
        {
            if (_handlers.TryGetValue(jsonEventType, out JsonEventListener eventListener))
            {
                eventListener.Excute(jsonData);
                return true;
            }

            return false;
        }
    }
}
