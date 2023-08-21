//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:43:46
//  Description：ILRuntimeComponent
//------------------------------------------------------------
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public class ILRuntimeComponent : GameFrameworkComponent
    {
        private bool _isEnable = false;

        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
        }
    }
}