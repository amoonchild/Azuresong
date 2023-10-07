//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/18 10:20:22
//  Description：WebRequestData
//------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using LitJson;
using GFUtility = GameFramework.Utility;


namespace Azuresong.Runtime
{
    /// <summary>
    /// Web消息请求
    /// </summary>
    public class WebRequestData : IReference
    {
        protected string _serverUrl = string.Empty;
        protected string _cmd = string.Empty;
        protected string _sendCmd = string.Empty;
        protected string _type = string.Empty;
        protected Dictionary<string, string> _header = new Dictionary<string, string>();
        protected Dictionary<string, object> _params = new Dictionary<string, object>();
        protected JsonData _jsonObject = new JsonData();
        protected WWWForm _uploadFileForm = null;
        protected object _userData = null;
        protected int _requestId = 0;

        /// <summary>
        /// 完整请求地址
        /// </summary>
        public virtual string FullUrl
        {
            get
            {
                return GFUtility.Text.Format("{0}{1}", ServerUrl, SendCmd);
            }
        }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public virtual string ServerUrl
        {
            get
            {
                return _serverUrl;
            }
        }

        /// <summary>
        /// 请求路径
        /// </summary>
        public virtual string Cmd
        {
            get
            {
                return _cmd;
            }
        }

        /// <summary>
        /// 实际请求路径
        /// </summary>
        public virtual string SendCmd
        {
            get
            {
                return _sendCmd;
            }
        }

        /// <summary>
        /// 请求类型(GET、POST、PUT、DELETE)
        /// </summary>
        public virtual string Type
        {
            get
            {
                return _type;
            }
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                return _header;
            }
        }

        public Dictionary<string, object> Params
        {
            get
            {
                return _params;
            }
        }

        public JsonData JsonObject
        {
            get
            {
                return _jsonObject;
            }
            set
            {
                _jsonObject = value;
            }
        }

        public WWWForm UploadFileForm
        {
            get
            {
                return _uploadFileForm;
            }
        }

        public object UserData
        {
            get
            {
                return _userData;
            }
        }

        public int RequestId
        {
            get
            {
                return _requestId;
            }
            set
            {
                _requestId = value;
            }
        }


        public string GetParamsText()
        {
            string text = string.Empty;
            if (_params.Count > 0)
            {
                text = "?";
                int index = 0;
                foreach (var item in _params)
                {
                    text = GFUtility.Text.Format("{0}{1}{2}={3}", text, index == 0 ? string.Empty : "&", item.Key, item.Value.ToString());
                    index++;
                }
            }

            return text;
        }

        public string GetFullUrlWithParamsText()
        {
            string text = FullUrl;
            if (_params.Count > 0)
            {
                text = GFUtility.Text.Format("{0}?", text);
                int index = 0;
                foreach (var item in _params)
                {
                    text = GFUtility.Text.Format("{0}{1}{2}={3}", text, index == 0 ? string.Empty : "&", item.Key, item.Value.ToString());
                    index++;
                }
            }

            return text;
        }

        public byte[] GetJsonBytes()
        {
            if (_jsonObject.Count > 0)
            {
                return GFUtility.Converter.GetBytes(_jsonObject.ToJson());
            }

            return null;
        }

        public void AddWWWField(string fieldName, string value)
        {
            if (_uploadFileForm == null)
            {
                _uploadFileForm = new WWWForm();
            }

            _uploadFileForm.AddField(fieldName, value);
        }

        public void AddFile(string fieldName, byte[] datas, string fileName, string mineType)
        {
            if (_uploadFileForm == null)
            {
                _uploadFileForm = new WWWForm();
            }

            _uploadFileForm.AddBinaryData(fieldName, datas, fileName, mineType);
        }

        public void SetContentTypeToJson()
        {
            _header["Content-Type"] = "application/json";
        }

        public void SetContentTypeToFile()
        {
            //_header["Content-Type"] = "application/octet-stream";
            _header["Content-Type"] = "multipart/form-data";
        }

        public virtual void Clear()
        {
            _serverUrl = string.Empty;
            _cmd = string.Empty;
            _sendCmd = string.Empty;
            _type = string.Empty;
            _header.Clear();
            _params.Clear();
            _jsonObject.Clear();
            _uploadFileForm = null;
            _userData = null;
            _requestId = 0;
        }


        public static WebRequestData Create(string serverUrl, int serverPort, string cmd, string sendCmd, string type, object userData = null)
        {
            WebRequestData csMessage = ReferencePool.Acquire<WebRequestData>();
            if (serverPort > 0)
            {
                csMessage._serverUrl = GFUtility.Text.Format("{0}:{1}", serverUrl, serverPort.ToString());
            }
            else
            {
                csMessage._serverUrl = serverUrl;
            }
            csMessage._cmd = cmd;
            csMessage._sendCmd = sendCmd;
            csMessage._type = type;
            csMessage._userData = userData;

            return csMessage;
        }
    }
}
