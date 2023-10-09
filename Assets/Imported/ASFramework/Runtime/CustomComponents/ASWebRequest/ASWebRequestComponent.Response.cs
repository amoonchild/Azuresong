//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/08/18 10:20:45
//  Description: ASWebRequestComponent
//------------------------------------------------------------
using System;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GFUtility = GameFramework.Utility;


namespace Azuresong.Runtime
{
    public partial class ASWebRequestComponent
    {
        public void RegisterHandler(WebResponseHandlerBase webMessageHandler)
        {
            if(_webResponseHandlers.ContainsKey(webMessageHandler.Cmd))
            {
                return;
            }

            _webResponseHandlers.Add(webMessageHandler.Cmd, webMessageHandler);
        }

        private void InitWebResponseHandlers()
        {
            Type handlerBaseType = typeof(WebResponseHandlerBase);
            Type[] types = GFUtility.Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsClass || types[i].IsAbstract)
                {
                    continue;
                }

                if (types[i].IsSubclassOf(handlerBaseType))
                {
                    WebResponseHandlerBase webMessageHandler = (WebResponseHandlerBase)Activator.CreateInstance(types[i]);
                    if (_webResponseHandlers.ContainsKey(webMessageHandler.Cmd))
                    {
                        ASLog.Error("The handler already exists: {0}", webMessageHandler.Cmd);
                        continue;
                    }

                    _webResponseHandlers.Add(webMessageHandler.Cmd, webMessageHandler);
                }
            }
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;

            UnityWebRequestAgentData agentData = (UnityWebRequestAgentData)ne.UserData;
            if (agentData == null)
            {
                return;
            }

            if (_webResponseHandlers.TryGetValue(agentData.RequestData.Cmd, out WebResponseHandlerBase handler))
            {
                ASLog.Debug("handle response: {0}, \r\n{1}", agentData.RequestData.Cmd, GFUtility.Converter.GetString(ne.GetWebResponseBytes()));

                WebResponseData responseData = WebResponseData.Create(ne.GetWebResponseBytes());

                if (!handler.Handle(agentData.RequestData, responseData, agentData.UserData))
                {
                    ReferencePool.Release(agentData.RequestData);
                    ReferencePool.Release(responseData);
                }
            }
            else
            {
                ASLog.Debug("receive response: {0}, \r\n{1}", agentData.RequestData.Cmd, Utility.Converter.GetString(ne.GetWebResponseBytes()));
            }

            ReferencePool.Release(agentData);
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;

            UnityWebRequestAgentData agentData = (UnityWebRequestAgentData)ne.UserData;
            if (agentData == null)
            {
                return;
            }

            if (_webResponseHandlers.TryGetValue(agentData.RequestData.Cmd, out WebResponseHandlerBase hander))
            {
                ASLog.Error("handle response error: {0}, \r\n{1}", agentData.RequestData.Cmd, ne.ErrorMessage);

                WebResponseData responseData = WebResponseData.Create(ne.ErrorMessage);

                if (!hander.Handle(agentData.RequestData, responseData, agentData.UserData))
                {
                    ReferencePool.Release(agentData.RequestData);
                    ReferencePool.Release(responseData);
                }
            }
            else
            {
                ASLog.Error("receive response error: {0}, \r\n{1}", agentData.RequestData.Cmd, ne.ErrorMessage);
            }

            ReferencePool.Release(agentData);
        }
    }
}
