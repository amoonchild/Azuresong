//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/18 10:20:22
//  Description：WebResponseData
//------------------------------------------------------------
using GameFramework;
using LitJson;
using GFUtility = GameFramework.Utility;


namespace Azuresong.Runtime
{
    /// <summary>
    /// Web消息返回
    /// </summary>
    public class WebResponseData : IReference
    {
        private int _code = 0;
        private string _msg = string.Empty;
        private JsonData _data = null;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public string Msg
        {
            get
            {
                return _msg;
            }
        }

        public JsonData Data
        {
            get
            {
                return _data;
            }
        }


        public bool IsTimeout()
        {
            return _msg == "Timeout";
        }

        public bool IsSuccess()
        {
            return _code == 200;
        }

        public virtual void Clear()
        {
            _code = 0;
            _msg = string.Empty;
            _data = null;
        }


        public static WebResponseData Create(byte[] responseBytes)
        {
            WebResponseData responseData = ReferencePool.Acquire<WebResponseData>();

            // 格式需要和后端统一
            JsonData jsonObject = JsonMapper.ToObject(GFUtility.Converter.GetString(responseBytes));
            responseData._code = (int)jsonObject["code"];
            responseData._msg = (string)jsonObject["msg"];
            if (jsonObject.ContainsKey("data"))
            {
                responseData._data = jsonObject["data"];
            }

            return responseData;
        }

        public static WebResponseData Create(string error)
        {
            WebResponseData responseData = ReferencePool.Acquire<WebResponseData>();

            // 格式需要和后端统一
            JsonData jsonObject = JsonMapper.ToObject(error);
            if (jsonObject != null)
            {
                if (jsonObject.ContainsKey("code"))
                {
                    responseData._code = (int)jsonObject["code"];
                    responseData._msg = (string)jsonObject["msg"];
                    if (jsonObject.ContainsKey("data"))
                    {
                        responseData._data = jsonObject["data"];
                    }
                }
                else
                {
                    responseData._code = -1;
                    responseData._msg = error;
                }
            }
            else
            {
                responseData._code = -2;
                responseData._msg = error;
            }

            return responseData;
        }
    }
}
