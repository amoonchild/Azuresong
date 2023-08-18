//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/18 10:20:22
//  Description：HandleWebResponseEventArgs
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    public abstract class HandleWebResponseEventArgs : AzuresongEventArgs
    {
        public WebRequestData RequestData
        {
            get;
            set;
        }

        public WebResponseData ResponseData
        {
            get;
            set;
        }


        public override void Clear()
        {
            ReferencePool.Release(RequestData);
            RequestData = null;
            ReferencePool.Release(ResponseData);
            ResponseData = null;
        }
    }
}
