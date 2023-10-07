//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 18:45:52
//  Description：SarsLogHelper
//------------------------------------------------------------
using GameFramework;
using UnityEngine;


namespace Azuresong.Runtime
{
    public class ASLogHelper : GameFrameworkLog.ILogHelper
    {
        public void Log(GameFrameworkLogLevel level, object message)
        {
            string msg = string.Empty;

            if (level == GameFrameworkLogLevel.Debug)
            {
                msg = Utility.Text.Format("<color=#888888>[{0}]{1}</color>", Time.realtimeSinceStartup.ToString("F2"), message);
            }
            else
            {
                msg = Utility.Text.Format("[{0}]{1}", Time.realtimeSinceStartup.ToString("F2"), message);
            }

            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    {
                        Debug.Log(msg);
                        break;
                    }
                case GameFrameworkLogLevel.Info:
                    {
                        Debug.Log(msg);
                        break;
                    }
                case GameFrameworkLogLevel.Warning:
                    {
                        Debug.LogWarning(msg);
                    }
                    break;
                case GameFrameworkLogLevel.Error:
                    {
                        Debug.LogError(msg);
                    }
                    break;
                case GameFrameworkLogLevel.Fatal:
                    {
                        throw new GameFrameworkException(msg);
                    }
                    break;
                default:
                    {
                        throw new GameFrameworkException(msg);
                    }
                    break;
            }
        }
    }
}