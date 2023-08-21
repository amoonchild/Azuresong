//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:27:00
//  Description：InitHybridCLRSuccessEventArgs
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    public class InitHybridCLRSuccessEventArgs : AzuresongEventArgs
    {
        public static readonly int EventId = typeof(InitHybridCLRSuccessEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        public static InitHybridCLRSuccessEventArgs Create()
        {
            InitHybridCLRSuccessEventArgs e = ReferencePool.Acquire<InitHybridCLRSuccessEventArgs>();
            return e;
        }
    }
}