//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2022/05/25 15:51:36
//  Description: ReferenceCollector
//------------------------------------------------------------
using System;
using UnityEngine;


namespace Azuresong.Runtime
{
    public sealed partial class ReferenceCollector
    {

        [Serializable]
        public class ReferenceCollectorData
        {
            public string key = string.Empty;
            public GameObject obj = null;
        }
    }
}
