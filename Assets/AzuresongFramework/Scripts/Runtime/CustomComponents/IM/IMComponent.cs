//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/30 11:53:34
//  Description：IMComponent
//------------------------------------------------------------
using System.Text;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using com.tencent.imsdk.unity;
using com.tencent.imsdk.unity.enums;
using com.tencent.imsdk.unity.types;
using Codice.CM.Common;


namespace Azuresong.Runtime
{
    public class IMComponent : GameFrameworkComponent
    {
        private bool _needLog = false;
        // 初始化参数
        private long _sdkAppId = 1400816214L;                                                                                          
        // 登录参数
        private string _userId = string.Empty;
        private string _userSig = string.Empty;
        // 聊天参数
        private string _convId = string.Empty;


        public void InitIM(long sdkAppId)
        {
            if(sdkAppId == 0L)
            {
                return;
            }

            _sdkAppId = sdkAppId;

            SdkConfig sdkConfig = new SdkConfig();
            sdkConfig.sdk_config_config_file_path = AzuresongEntry.Resource.ReadWritePath + "/TIM-Config";
            sdkConfig.sdk_config_log_file_path = AzuresongEntry.Resource.ReadWritePath + "/TIM-Log";

            TIMResult res = TencentIMSDK.Init(_sdkAppId, sdkConfig, _needLog);
            ASLog.Info("IM Init {0}.", res.ToString());

            AddAllEvents();
        }

        public void Uninit()
        {
            RemoveAllEvents();

            TIMResult res = TencentIMSDK.Uninit();
            ASLog.Info("IM Uninit {0}.", res.ToString());
        }

        public void Login(string userId, string userSig)
        {
            _userId = userId;
            _userSig = userSig;
            Login();
        }

        public void Logout()
        {
            if (TencentIMSDK.GetLoginStatus() != TIMLoginStatus.kTIMLoginStatus_Logined)
            {
                return;
            }

            TIMResult res = TencentIMSDK.Logout((int code, string desc, string user_data) => 
            {
                
            });

            ASLog.Info("IM Logout {0}.", res.ToString());
        }

        public void SendTextMessage(string content)
        {
            Message message = new Message()
            {
                message_conv_id = _convId,
                message_conv_type = TIMConvType.kTIMConv_Group,
                message_elem_array = new List<Elem>
                {
                    new Elem()
                    {
                        elem_type = TIMElemType.kTIMElem_Text,
                        text_elem_content = content
                    }
                }
            };

            StringBuilder messageId = new StringBuilder(128);
            TIMResult res = TencentIMSDK.MsgSendMessage(_convId, TIMConvType.kTIMConv_Group,
                message, messageId, (int code, string desc, string data, string user_data) => 
                {
                
                });

            ASLog.Info("IM SendTextMessage {0}.", res.ToString());
        }

        public void SendCustomMessage(string content)
        {
            
        }

        private void Login()
        {
            if (TencentIMSDK.GetLoginStatus() != TIMLoginStatus.kTIMLoginStatus_UnLogined)
            {
                return;
            }

            if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_userSig))
            {
                return;
            }

            TIMResult res = TencentIMSDK.Login(_userId, _userSig, (int code, string desc, string json_param, string user_data) =>
            {
                if (code == 6206 || code == 70001)
                {
                    _userSig = string.Empty;
                }
            });

            ASLog.Info("IM Login {0}.", res.ToString());
        }

        private void AddAllEvents()
        {
            TencentIMSDK.AddRecvNewMsgCallback(OnRecvNewMsg);
            TencentIMSDK.SetUserSigExpiredCallback(OnUserSigExpired);
            TencentIMSDK.SetKickedOfflineCallback(OnKickedOffline);
        }

        private void RemoveAllEvents()
        {
            TencentIMSDK.RemoveRecvNewMsgCallback(OnRecvNewMsg);
            TencentIMSDK.RemoveUserSigExpiredCallback(OnUserSigExpired);
            TencentIMSDK.RemoveKickedOfflineCallback(OnKickedOffline);
        }

        private void OnRecvNewMsg(List<Message> message, string user_data)
        {

        }

        private void OnUserSigExpired(string user_data)
        {
            _userSig = string.Empty;
        }

        private void OnKickedOffline(string user_data)
        {

        }
    }
}