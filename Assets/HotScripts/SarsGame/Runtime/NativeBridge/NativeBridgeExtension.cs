//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/07/19 14:28:57
//  Description：NativeBridgeExtension
//------------------------------------------------------------
using System;
using LitJson;
using Azuresong.Runtime;


namespace SarsGame.Runtime
{
    public static class NativeBridgeExtension
    {
        public static void AddHandler(this NativeBridgeComponent nativeBridgeComponent, W2UMsgType msgType, Action<JsonData> handler)
        {
            nativeBridgeComponent.AddHandler(msgType.ToString(), handler);
        }

        public static void RemoveHandler(this NativeBridgeComponent nativeBridgeComponent, W2UMsgType msgType, Action<JsonData> handler)
        {
            nativeBridgeComponent.RemoveHandler(msgType.ToString(), handler);
        }

        public static void SendData(this NativeBridgeComponent nativeBridgeComponent, U2WMsgType msgType)
        {
            nativeBridgeComponent.SendData(msgType.ToString(), string.Empty);
        }

        public static void SendData(this NativeBridgeComponent nativeBridgeComponent, U2WMsgType msgType, string msgData)
        {
            nativeBridgeComponent.SendData(msgType.ToString(), msgData);
        }
    }
}