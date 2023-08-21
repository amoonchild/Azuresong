//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:27:00
//  Description：InitXLuaSuccessEventArgs
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    public class InitXLuaSuccessEventArgs : AzuresongEventArgs
    {
        public static readonly int EventId = typeof(InitXLuaSuccessEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        public static InitXLuaSuccessEventArgs Create()
        {
            InitXLuaSuccessEventArgs e = ReferencePool.Acquire<InitXLuaSuccessEventArgs>();
            return e;
        }
    }
}