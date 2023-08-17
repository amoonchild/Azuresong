//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/14 10:15:44
//  Description：UpdateLoadingProgressEventArgs
//------------------------------------------------------------
using GameFramework;
using SarsFramework.Runtime;


namespace SarsGame.Runtime
{
    public class UpdateLoadingProgressEventArgs : SarsEventArgs
    {
        public static readonly int EventId = typeof(UpdateLoadingProgressEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public float Progress
        {
            get;
            private set;
        }


        public static UpdateLoadingProgressEventArgs Create(float progress)
        {
            UpdateLoadingProgressEventArgs e = ReferencePool.Acquire<UpdateLoadingProgressEventArgs>();//new UpdateLoadingProgressEventArgs();
            e.Progress = progress;
            return e;
        }
    }
}