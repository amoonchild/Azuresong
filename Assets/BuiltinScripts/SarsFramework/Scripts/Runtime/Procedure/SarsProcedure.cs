//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/19 14:05:00
//  Description：ProcedureBase
//------------------------------------------------------------
using GameFramework.Fsm;
using GameFramework.Procedure;


namespace SarsFramework.Runtime
{
    public abstract class SarsProcedure : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            SarsLog.Info("Enter procedure: {0}", GetType().ToString());
        }
    }
}