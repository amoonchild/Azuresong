//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 15:54:01
//  Description：ProcedureExtension
//------------------------------------------------------------
using UnityGameFramework.Runtime;


namespace SarsGame.Runtime
{
    public static class ProcedureExtension
    {
        public static string GetCurrProcedureName(this ProcedureComponent procedureComponent)
        {
            if (procedureComponent.CurrentProcedure == null)
            {
                return string.Empty;
            }

            return procedureComponent.CurrentProcedure.GetType().Name;
        }

        public static void ChangeScene<T>(this ProcedureComponent procedureComponent) where T : ProcedureBase
        {
            ProcedureBase jinanProcedure = procedureComponent.CurrentProcedure as ProcedureBase;
            jinanProcedure.ChangeScene<T>();
        }
    }
}