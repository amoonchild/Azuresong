//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 18:45:52
//  Description：SarsLogHelper
//------------------------------------------------------------
using GameFramework;
using UnityEngine;


namespace SarsFramework.Runtime
{ 
    public class SarsLogHelper : GameFrameworkLog.ILogHelper
    {
        public void Log(GameFrameworkLogLevel level, object message)
        {
            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    {
                        Debug.Log(Utility.Text.Format("Debug> {0}", message));
                        break;
                    }
                case GameFrameworkLogLevel.Info:
                    {
                        Debug.Log(message);
                        break;
                    }
                case GameFrameworkLogLevel.Warning:
                    {
                        Debug.LogWarning(message);
                    }
                    break;
                case GameFrameworkLogLevel.Error:
                    {
                        Debug.LogError(message);
                    }
                    break;
                case GameFrameworkLogLevel.Fatal:
                    {
                        Debug.LogError(message);
                    }
                    break;
                default:
                    {
                        throw new GameFrameworkException(message.ToString());
                    }
                    break;
            }
        }
    }
}