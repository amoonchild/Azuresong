//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/01 09:54:56
//  Description：UIFormConfigList
//------------------------------------------------------------
using System.Collections.Generic;


namespace SarsGame.Runtime
{
    public static class UIFormConfigList
    {
        // TODO: create datatable file
        public static Dictionary<UIFormType, UIFormConfig> Configs = new Dictionary<UIFormType, UIFormConfig>()
        {
            {UIFormType.Loading, new UIFormConfig(AssetUtility.GetUIFormPath("LoadingForm"), typeof(UILoadingForm), Constant.UIGroup.Top)},
        };
    }
}