//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2022/05/25 15:51:36
//  Description: ReferenceCollector
//------------------------------------------------------------
using System;
using System.Collections.Generic;


namespace Azuresong.Runtime
{
    public sealed partial class ReferenceCollector
    {
        public class ReferenceCollectorDataComparer : IComparer<ReferenceCollectorData>
        {
            public int Compare(ReferenceCollectorData x, ReferenceCollectorData y)
            {
                return string.Compare(x.key, y.key, StringComparison.Ordinal);
            }
        }
    }
}
