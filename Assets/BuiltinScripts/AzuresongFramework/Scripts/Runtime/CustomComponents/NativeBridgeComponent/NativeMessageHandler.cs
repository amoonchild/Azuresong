//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/07/17 16:12:44
//  Description：NativeMessageHandler
//------------------------------------------------------------
using UnityEngine;
using UnityGameFramework.Runtime;
using LitJson;


namespace Azuresong.Runtime
{
    public class NativeMessageHandler : MonoBehaviour
    {
        private EventComponent _eventComponent;


        private void Start()
        {
            _eventComponent = GameEntry.GetComponent<EventComponent>();
        }

        private void HandleWebMessage(string text)
        {
            try
            {
                ASLog.Info("Handle {0} message, {1}", Application.platform.ToString(), text);

                JsonData jsMessage = JsonMapper.ToObject(text);

                HandleNativeMessageEventArgs ne = HandleNativeMessageEventArgs.Create((string)jsMessage["type"], jsMessage["data"]);
                _eventComponent.Fire(this, ne);
            }
            catch (System.Exception e)
            {
                ASLog.Error("Handle {0} msg error, {1}", Application.platform.ToString(), e.Message);
            }
        }
    }
}