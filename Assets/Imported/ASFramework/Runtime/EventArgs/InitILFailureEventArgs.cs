//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:30:15
//  Description：InitILFailureEventArgs
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    public class InitILFailureEventArgs : AzuresongEventArgs
    {
        public static readonly int EventId = typeof(InitILFailureEventArgs).GetHashCode();
        
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



        public static InitILFailureEventArgs Create(string error)
        {
            InitILFailureEventArgs e = ReferencePool.Acquire<InitILFailureEventArgs>();
            e.Error = error;
            return e;
        }
    }
}