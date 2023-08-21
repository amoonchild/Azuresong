//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/17 17:27:59
//  Description：ProcedureSplash
//------------------------------------------------------------
using GameFramework.Resource;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace Azuresong.Runtime
{
    public class ProcedureSplash : ProcedureBase
    {
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (AzuresongEntry.Base.EditorResourceMode)
            {
                ChangeState(procedureOwner, typeof(ProcedurePreload));
            }
            else if (AzuresongEntry.Resource.ResourceMode == ResourceMode.Package)
            {
                ChangeState(procedureOwner, typeof(ProcedureInitPackageResource));
            }
            else if (AzuresongEntry.Resource.ResourceMode == ResourceMode.Updatable)
            {
                ChangeState(procedureOwner, typeof(ProcedureCheckVersion));
            }
            else
            {

            }
        }
    }
}