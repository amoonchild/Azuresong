//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/26 16:28:53
//  Description：PlayerEntityLogic
//------------------------------------------------------------
using UnityEngine;
using UnityGameFramework.Runtime;
using SarsFramework.Runtime;


namespace SarsGame.Runtime
{
    public class PlayerEntityLogic : SarsEntityLogic
    {
        [SerializeField]
        private PlayerEntityData _entityData = null;

        public PlayerEntityData EntityData
        {
            get
            {
                return _entityData;
            }
        }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            _entityData = userData as PlayerEntityData;
            if (_entityData == null)
            {
                Log.Error("Entity data is invalid.");
                return;
            }

            CachedTransform.localScale = _entityData.Scale;
            CachedTransform.localRotation = _entityData.Rotation;
            CachedTransform.localPosition = _entityData.Position;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            if (!isShutdown)
            {
                _entityData = null;
            }

            base.OnHide(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}