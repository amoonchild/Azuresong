//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 14:24:23
//  Description：ProcedureHotfix
//------------------------------------------------------------
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace Azuresong.Runtime
{
    public class ProcedureHotfix : ProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            AzuresongEntry.HybirdCLR.RunHotfix();
        }
    }
}