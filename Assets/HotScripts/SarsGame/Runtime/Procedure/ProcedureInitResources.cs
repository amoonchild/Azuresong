//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 14:12:57
//  Description：ProcedureInitResources
//------------------------------------------------------------
using SarsFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace SarsGame.Runtime
{
    public class ProcedureInitResources : ProcedureBase
    {
        private bool _initResourcesComplete = false;


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _initResourcesComplete = false;

            SarsEntry.Resource.InitResources(OnInitResourcesComplete);
        }

        protected override void OnUpdateSelf(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (!_initResourcesComplete)
            {
                return;
            }

            ChangeState<ProcedurePreload>(procedureOwner);
        }

        private void OnInitResourcesComplete()
        {
            _initResourcesComplete = true;
            SarsLog.Info("Init resources complete. version:{0}", SarsEntry.Resource.InternalResourceVersion.ToString());
        }
    }
}