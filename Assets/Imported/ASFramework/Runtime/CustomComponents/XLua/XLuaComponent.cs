//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:44:24
//  Description：XLuaComponent
//------------------------------------------------------------
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public class XLuaComponent : GameFrameworkComponent
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