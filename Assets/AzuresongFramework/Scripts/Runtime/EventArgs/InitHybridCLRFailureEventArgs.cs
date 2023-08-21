//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:27:00
//  Description：InitHybridCLRFailureEventArgs
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    public class InitHybridCLRFailureEventArgs : AzuresongEventArgs
    {
        public static readonly int EventId = typeof(InitHybridCLRFailureEventArgs).GetHashCode();

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


        public static InitHybridCLRFailureEventArgs Create(string error)
        {
            InitHybridCLRFailureEventArgs e = ReferencePool.Acquire<InitHybridCLRFailureEventArgs>();
            e.Error = error;
            return e;
        }
    }
}