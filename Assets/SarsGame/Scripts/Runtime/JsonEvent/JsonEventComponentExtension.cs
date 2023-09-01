//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/09/01 17:04:50
//  Description：JsonEventComponentExtension
//------------------------------------------------------------
using System;
using Azuresong.Runtime;
using LitJson;


namespace SarsGame.Runtime
{
    public static class JsonEventComponentExtension
    {
        public static void AddHandler(this JsonEventComponent jsonEventComponent, JsonEventType jsonEventType, Action<JsonData> handler)
        {
            jsonEventComponent.AddHandler((int)jsonEventType, handler);
        }

        public static void RemoveHandler(this JsonEventComponent jsonEventComponent, JsonEventType jsonEventType, Action<JsonData> handler)
        {
            jsonEventComponent.RemoveHandler((int)jsonEventType, handler);
        }

        public static void Excute(this JsonEventComponent jsonEventComponent, JsonEventType jsonEventType)
        {
            jsonEventComponent.Excute((int)jsonEventType);
        }

        public static void Excute(this JsonEventComponent jsonEventComponent, JsonEventType jsonEventType, JsonData jsonData)
        {
            jsonEventComponent.Excute((int)jsonEventType, jsonData);
        }
    }
}