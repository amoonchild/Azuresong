//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/06/30 10:43:45
//  Description: OpenUIFormInfo
//------------------------------------------------------------
using System;
using GameFramework;


namespace Azuresong.Runtime
{
    public class OpenUIFormInfo : IReference
    {
        public int UIFormType
        {
            get;
            private set;
        }

        public string UIFormAssetName
        {
            get;
            private set;
        }

        public Type UIFormLogicType
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }


        public void Clear()
        {
            UIFormType = 0;
            UIFormAssetName = string.Empty;
            UIFormLogicType = null;
            UserData = null;
        }


        public static OpenUIFormInfo Create(int uiFormId, string uiFormAssetName, Type uiFormLogicType, object userData)
        {
            OpenUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenUIFormInfo>();
            openUIFormInfo.UIFormType = uiFormId;
            openUIFormInfo.UIFormAssetName = uiFormAssetName;
            openUIFormInfo.UIFormLogicType = uiFormLogicType;
            openUIFormInfo.UserData = userData;
            return openUIFormInfo;
        }
    }
}