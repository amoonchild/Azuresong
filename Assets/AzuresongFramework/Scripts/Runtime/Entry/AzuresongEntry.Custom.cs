//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/17 16:41:29
//  Description：AzuresongEntry.Custom
//------------------------------------------------------------
using UnityEngine;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public partial class AzuresongEntry : MonoBehaviour
    {
        public static NativeBridgeComponent NativeBridge
        {
            get;
            private set;
        }

        public static ASWebRequestComponent ASWebRequest
        {
            get;
            private set;
        }

        public static XLuaComponent XLua
        {
            get;
            private set;
        }

        public static ILRuntimeComponent ILRuntime
        {
            get;
            private set;
        }
        
        public static HybirdCLRComponent HybirdCLR
        {
            get;
            private set;
        }

        public static IMComponent IM
        {
            get;
            private set;
        }

        public static JsonEventComponent JsonEvent
        {
            get;
            private set;
        }
        

        private void InitCustomComponents()
        {
            NativeBridge = GameEntry.GetComponent<NativeBridgeComponent>();
            ASWebRequest = GameEntry.GetComponent<ASWebRequestComponent>();
            XLua = GameEntry.GetComponent<XLuaComponent>();
            ILRuntime = GameEntry.GetComponent<ILRuntimeComponent>();
            HybirdCLR = GameEntry.GetComponent<HybirdCLRComponent>();
            IM = GameEntry.GetComponent<IMComponent>();
            JsonEvent = GameEntry.GetComponent<JsonEventComponent>();
        }
    }
}