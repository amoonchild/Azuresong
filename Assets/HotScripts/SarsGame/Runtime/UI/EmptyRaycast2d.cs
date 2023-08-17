//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/01 10:50:47
//  Description：EmptyRaycast2d
//------------------------------------------------------------
using UnityEngine.UI;


namespace SarsGame.Runtime
{
    public class EmptyRaycast2d : Graphic
    {
        public override void SetMaterialDirty()
        {

        }

        public override void SetVerticesDirty()
        {

        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}