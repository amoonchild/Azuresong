//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/18 11:37:15
//  Description：HybirdCLRComponent
//------------------------------------------------------------
using System;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public class HybirdCLRComponent : GameFrameworkComponent
    {
        private bool _isEditor = false;
        private Action<bool, string> _loadCompleteEvent = null;


        public void LoadHotfixAssets(Action<bool, string> loadCompleteEvent)
        {
            ASLog.Info("Init hotfix...");
            _loadCompleteEvent = loadCompleteEvent;

            if(_isEditor)
            {
                ASLog.Info("Init hotfix...");
            }
            else
            {

            }
        }
    }
}