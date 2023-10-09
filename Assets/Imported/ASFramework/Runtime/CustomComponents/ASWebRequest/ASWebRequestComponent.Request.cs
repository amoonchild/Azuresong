//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/08/18 10:20:45
//  Description: ASWebRequestComponent
//------------------------------------------------------------
using UnityEngine;


namespace Azuresong.Runtime
{
    public partial class ASWebRequestComponent
    {
        /// <summary>
        /// 创建web请求
        /// </summary>
        /// <param name="cmd">基础路径</param>
        /// <param name="type">请求类型(GET,POST,PUT,DELETE)</param>
        public WebRequestData CreateRequestData(string cmd, string type)
        {
            return CreateRequestData(cmd, cmd, type);
        }

        /// <summary>
        /// 创建Web请求,自定义路径
        /// </summary>
        /// <param name="cmd">基础路径</param>
        /// <param name="sendCmd">请求路径</param>
        /// <param name="type">请求类型(GET,POST,PUT,DELETE)</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>请求数据</returns>
        public WebRequestData CreateRequestData(string cmd, string sendCmd, string type)
        {
            return WebRequestData.Create(ServerUrl, ServerPort, cmd, sendCmd, type, null);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="requestData">请求消息</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>请求id</returns>
        public int SendRequest(WebRequestData requestData, object userData)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return 0;
            }

            requestData.RequestId = _webRequestComponent.AddWebRequest(requestData.FullUrl, UnityWebRequestAgentData.Create(requestData, userData));
            return requestData.RequestId;
        }
    }
}