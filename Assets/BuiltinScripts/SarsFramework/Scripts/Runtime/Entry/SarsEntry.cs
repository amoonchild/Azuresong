//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 14:01:00
//  Description：SarsEntry
//------------------------------------------------------------
using UnityEngine;
using UnityGameFramework.Runtime;


namespace SarsFramework.Runtime
{
    public partial class SarsEntry : MonoBehaviour
    {
        public static NativeBridgeComponent NativeBridge
        {
            get;
            private set;
        }


        private void Start()
        {
            InitBuiltinComponents();
            NativeBridge = GameEntry.GetComponent<NativeBridgeComponent>();
        }
    }
}