//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/01 10:16:04
//  Description：UIFormConfig
//------------------------------------------------------------
using System;


namespace SarsGame.Runtime
{
    public class UIFormConfig
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetName { get; private set; }
        /// <summary>
        /// Logic代码类型
        /// </summary>
        public Type LogicType { get; private set; }
        /// <summary>
        /// UI组
        /// </summary>
        public string UIGroupName { get; private set; }
        /// <summary>
        /// 打开时暂停相同组层级被遮挡的界面
        /// </summary>
        public bool PauseCoveredUIForm { get; private set; }
        /// <summary>
        /// 可以重复打开多个界面
        /// </summary>
        public bool AllowMultiInstance { get; private set; }


        public UIFormConfig(string assetName, Type logicType, string uiGroupName)
            : this(assetName, logicType, uiGroupName, false, false)
        {

        }

        public UIFormConfig(string assetName, Type logicType, string uiGroupName, bool pauseCoveredUIForm, bool allowMultiInstance)
        {
            AssetName = assetName;
            LogicType = logicType;
            UIGroupName = uiGroupName;
            PauseCoveredUIForm = pauseCoveredUIForm;
            AllowMultiInstance = allowMultiInstance;
        }
    }
}
