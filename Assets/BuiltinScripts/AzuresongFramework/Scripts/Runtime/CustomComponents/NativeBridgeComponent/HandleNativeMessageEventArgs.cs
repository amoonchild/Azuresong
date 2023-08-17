//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/07/17 15:17:41
//  Description：HandleNativeMessageEventArgs
//------------------------------------------------------------
using GameFramework;
using LitJson;


namespace Azuresong.Runtime
{
    public class HandleNativeMessageEventArgs : AzuresongEventArgs
    {
        public static readonly int EventId = typeof(HandleNativeMessageEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string MsgType
        {
            get;
            private set;
        }

        public JsonData MsgData
        {
            get;
            private set;
        }


        public static HandleNativeMessageEventArgs Create(string msgType, JsonData msgData)
        {
            HandleNativeMessageEventArgs ne = ReferencePool.Acquire<HandleNativeMessageEventArgs>();
            ne.MsgType = msgType;
            ne.MsgData = msgData;
            return ne;
        }
    }
}