//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:27:00
//  Description：InitILSuccessEventArgs
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    public class InitILSuccessEventArgs : AzuresongEventArgs
    {
        public static readonly int EventId = typeof(InitILSuccessEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        public static InitILSuccessEventArgs Create()
        {
            InitILSuccessEventArgs e = ReferencePool.Acquire<InitILSuccessEventArgs>();
            return e;
        }
    }
}