//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/17 17:27:59
//  Description：UIEmptyRaycast
//------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;


namespace Azuresong.Runtime
{
    public class UIEmptyRaycast : MaskableGraphic
    {
        protected UIEmptyRaycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}
