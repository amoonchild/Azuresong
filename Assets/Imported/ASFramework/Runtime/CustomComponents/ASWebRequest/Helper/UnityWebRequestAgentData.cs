//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/08/18 10:20:45
//  Description: UnityWebRequestAgentData
//------------------------------------------------------------
using GameFramework;


namespace Azuresong.Runtime
{
    /// <summary>
    /// Web请求信息
    /// </summary>
    public class UnityWebRequestAgentData : IReference
    {
        private WebRequestData _requestData = null;
        private object _userData = null;

        /// <summary>
        /// 请求消息
        /// </summary>
        public WebRequestData RequestData
        {
            get
            {
                return _requestData;
            }
        }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData
        {
            get
            {
                return _userData;
            }
        }


        public void Clear()
        {
            _requestData = null;
            _userData = null;
        }


        public static UnityWebRequestAgentData Create(WebRequestData requestData, object userData)
        {
            UnityWebRequestAgentData agentData = ReferencePool.Acquire<UnityWebRequestAgentData>();
            agentData._requestData = requestData;
            agentData._userData = userData;
            return agentData;
        }
    }
}