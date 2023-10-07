//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 10:24:03
//  Description：ProcedureInitPackageResource
//------------------------------------------------------------
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace Azuresong.Runtime
{
    public class ProcedureInitPackageResource : ProcedureBase
    {
        private bool _isInitResourceComplete = false;


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _isInitResourceComplete = false;
            AzuresongEntry.Resource.InitResources(OnInitResourcesComplete);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (_isInitResourceComplete)
            {
                ChangeState<ProcedurePreload>(procedureOwner);
            }
        }

        private void OnInitResourcesComplete()
        {
            _isInitResourceComplete = true;
        }
    }
}