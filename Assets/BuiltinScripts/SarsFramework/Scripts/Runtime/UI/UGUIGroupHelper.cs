//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/06/30 10:42:59
//  Description: UGuiGroupHelper
//------------------------------------------------------------
using UnityGameFramework.Runtime;


namespace SarsFramework.Runtime
{
    public class UGuiGroupHelper : UIGroupHelperBase
	{
        public const int DepthFactor = 10000;

        private int _depth = 0;


        public override void SetDepth(int depth)
        {
            _depth = depth;
        }
    }
}