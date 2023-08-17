//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 14:01:00
//  Description：AzuresongEntry
//------------------------------------------------------------
using UnityEngine;


namespace Azuresong.Runtime
{
    public partial class AzuresongEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}