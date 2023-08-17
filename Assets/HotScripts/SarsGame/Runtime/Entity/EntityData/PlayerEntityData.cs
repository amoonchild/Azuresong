//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/26 16:26:44
//  Description：PlayerEntityData
//------------------------------------------------------------
using System;
using UnityEngine;
using SarsFramework.Runtime;


namespace SarsGame.Runtime
{
    [Serializable]
    public class PlayerEntityData : SarsEntityData
    {
        public Vector3 Position
        {
            get;
            private set;
        }

        public Quaternion Rotation
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            private set;
        }


        public PlayerEntityData(Vector3 position, Quaternion rotation)
            : this(position, rotation, Vector3.zero)
        {

        }

        public PlayerEntityData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}