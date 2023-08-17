//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 14:10:07
//  Description：ProcedurePreload
//------------------------------------------------------------
using SarsFramework.Runtime;
using System.Collections.Generic;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace SarsGame.Runtime
{
    public class ProcedurePreload : ProcedureBase
    {
        private string[] _dataTableNames = null;
        private Dictionary<string, bool> _dataTableLoadStates = null;


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            SarsEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            SarsEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);

            PreLoadDataTables();
        }

        protected override void OnUpdateSelf(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (_dataTableLoadStates != null)
            {
                foreach (KeyValuePair<string, bool> item in _dataTableLoadStates)
                {
                    if (item.Value == false)
                    {
                        return;
                    }
                }
            }

            UserData.vmodel = "1001";
            TryChangeScene(procedureOwner, typeof(ProcedureFloor1));
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (!isShutdown)
            {
                SarsEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
                SarsEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);

                _dataTableNames = null;
                _dataTableLoadStates = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void PreLoadDataTables()
        {
            return;

            _dataTableNames = new string[]
            {
            "InteractiveNpc",
            };

            _dataTableLoadStates = new Dictionary<string, bool>();

            for (int i = 0; i < _dataTableNames.Length; i++)
            {
                _dataTableLoadStates.Add(_dataTableNames[i], false);
                SarsEntry.DataTable.LoadDataTable(_dataTableNames[i], true, this);
            }
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;

            if (ne.UserData != this)
            {
                return;
            }

            _dataTableLoadStates[ne.DataTableAssetName] = true;

            SarsLog.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;

            if (ne.UserData != this)
            {
                return;
            }

            SarsLog.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.",
                ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
        }
    }
}