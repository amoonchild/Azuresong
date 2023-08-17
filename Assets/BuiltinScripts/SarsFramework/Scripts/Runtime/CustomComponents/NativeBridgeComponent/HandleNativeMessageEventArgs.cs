//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/07/17 15:17:41
//  Description：HandleNativeMessageEventArgs
//------------------------------------------------------------
using GameFramework;
using LitJson;


namespace SarsFramework.Runtime
{
    public class HandleNativeMessageEventArgs : SarsEventArgs
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
            // TODO
            //HandleNativeMessageEventArgs ne = ReferencePool.Acquire<HandleNativeMessageEventArgs>();
            HandleNativeMessageEventArgs ne = new HandleNativeMessageEventArgs();
            ne.MsgType = msgType;
            ne.MsgData = msgData;
            return ne;
        }
    }
}