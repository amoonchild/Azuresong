//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/06/30 11:08:25
//  Description: Constant.Layer
//------------------------------------------------------------
using UnityEngine;


namespace Azuresong.Runtime
{
	public static partial class Constant
	{
		public static class Layer
		{
			public static readonly int Default = LayerMask.NameToLayer("Default");
			public static readonly int UI = LayerMask.NameToLayer("UI");
			public static readonly int UICull = LayerMask.NameToLayer("UI Cull");
        }
	}
}