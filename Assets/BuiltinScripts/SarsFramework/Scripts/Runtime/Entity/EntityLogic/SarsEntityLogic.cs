//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/26 16:28:53
//  Description：SarsEntityLogic
//------------------------------------------------------------
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;


namespace SarsFramework.Runtime
{
    public abstract class SarsEntityLogic : EntityLogic
    {
        public int Id
        {
            get
            {
                return Entity.Id;
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

            Name = Utility.Text.Format("[Entity {0}]", Id.ToString());
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
