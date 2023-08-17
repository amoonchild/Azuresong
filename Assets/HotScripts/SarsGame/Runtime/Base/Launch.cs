//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/14 10:15:44
//  Description：Launch
//------------------------------------------------------------
using System.Collections;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework;
using GameFramework.Fsm;


namespace SarsGame.Runtime
{
    public partial class Launch : MonoBehaviour
    {
        private IProcedureManager _procedureManager = null;


        private void Awake()
        {
            _procedureManager = GameFrameworkEntry.GetModule<IProcedureManager>();
        }

        private IEnumerator Start()
        {
            InitCustomComponents();

            // 初始化流程
            ProcedureBase[] procedures = new ProcedureBase[]
            {
                new ProcedureLaunch(),
                new ProcedureInitResources(),
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