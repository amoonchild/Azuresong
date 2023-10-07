//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 14:05:00
//  Description：ProcedureBase
//------------------------------------------------------------
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace Azuresong.Runtime
{
    public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            ASLog.Info("Enter procedure: {0}", GetType().ToString());
        }
    }
}