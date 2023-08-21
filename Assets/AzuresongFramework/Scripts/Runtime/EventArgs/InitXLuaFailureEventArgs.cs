//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:30:15
//  Description：InitXLuaFailureEventArgs
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    public class InitXLuaFailureEventArgs : AzuresongEventArgs
    {
        public static readonly int EventId = typeof(InitXLuaFailureEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string Error
        {
            get;
            private set;
        }


        public override void Clear()
        {
            Error = string.Empty;
        }


        public static InitXLuaFailureEventArgs Create(string error)
        {
            InitXLuaFailureEventArgs e = ReferencePool.Acquire<InitXLuaFailureEventArgs>();
            e.Error = error;
            return e;
        }
    }
}