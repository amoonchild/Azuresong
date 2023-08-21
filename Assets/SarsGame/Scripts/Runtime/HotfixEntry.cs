//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/14 10:15:44
//  Description：HotfixEntry
//------------------------------------------------------------
using System.Collections;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using Azuresong.Runtime;


namespace SarsGame.Runtime
{
    public class HotfixEntry : MonoBehaviour
    {
        private IProcedureManager _procedureManager = null;


        private void Awake()
        {
            _procedureManager = GameFrameworkEntry.GetModule<IProcedureManager>();
        }

        private IEnumerator Start()
        {
            ASLog.Info("SarsGame.Runtime start.");

            AzuresongEntry.Fsm.DestroyFsm<IProcedureManager>();

            ProcedureBase[] procedures = new ProcedureBase[]
            {
                new ProcedureLaunch(),
                new ProcedurePreload(),
                new ProcedureChangeScene(),

                new ProcedureFloor1(),
            };

            _procedureManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), procedures);
            yield return new WaitForEndOfFrame();

            _procedureManager.StartProcedure(procedures[0].GetType());
        }
    }
}