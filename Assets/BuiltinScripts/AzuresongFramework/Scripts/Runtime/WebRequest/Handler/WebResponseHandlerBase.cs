//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/18 10:20:22
//  Description：WebResponseHandlerBase
//------------------------------------------------------------
using GFUtility = GameFramework.Utility;


namespace Azuresong.Runtime
{
    /// <summary>
    /// Web消息返回数据处理
    /// </summary>
    public abstract class WebResponseHandlerBase
    {
        /// <summary>
        /// 完整请求地址
        /// </summary>
        public string FullUrl
        {
            get
            {
                return GFUtility.Text.Format("{0}{1}", ServerUrl, Cmd);
            }
        }

        /// <summary>
        /// 请求地址
        /// </summary>
        public virtual string ServerUrl
        {
            get
            {
                return ASWebRequestComponent.ServerUrl;
            }
        }

        /// <summary>
        /// 请求路径
        /// </summary>
        public abstract string Cmd
        {
            get;
        }


        public virtual bool Handle(WebRequestData requestData, WebResponseData resposeData, object userData)
        {
            return false;
        }
    }
}