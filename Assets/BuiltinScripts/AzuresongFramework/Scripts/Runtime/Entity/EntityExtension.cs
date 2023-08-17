//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/26 16:31:33
//  Description：EntityExtension
//------------------------------------------------------------
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public static class EntityExtension
    {
        private static int _serialId = 0;


        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --_serialId;
        }

        public static void ResetSerialId()
        {
            _serialId = 0;
        }

        public static AzuresongEntityLogic GetEntityLogic(this EntityComponent entityComponent, int entityId)
        {
            Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (AzuresongEntityLogic)entity.Logic;
        }
    }
}