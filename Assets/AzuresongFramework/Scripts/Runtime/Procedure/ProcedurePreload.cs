//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/17 17:30:55
//  Description：ProcedurePreload
//------------------------------------------------------------
using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace Azuresong.Runtime
{
    public class ProcedurePreload : ProcedureBase
    {
        //private bool _isInitXluaSuccess = false;
        //private bool _isInitILSuccess = false;
        private bool _isInitHybridCLRSuccess = false;


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            //AzuresongEntry.Event.Subscribe(InitXLuaSuccessEventArgs.EventId, OnInitXLuaSuccess);
            //AzuresongEntry.Event.Subscribe(InitXLuaFailureEventArgs.EventId, OnInitXLuaFailure);
            //AzuresongEntry.Event.Subscribe(InitILSuccessEventArgs.EventId, OnInitILRuntimeSuccess);
            //AzuresongEntry.Event.Subscribe(InitILFailureEventArgs.EventId, OnInitILRuntimeFailure);
            AzuresongEntry.Event.Subscribe(InitHybridCLRSuccessEventArgs.EventId, OnInitHybridCLRSuccess);
            AzuresongEntry.Event.Subscribe(InitHybridCLRFailureEventArgs.EventId, OnInitHybridCLRFailure);

            //_isInitXluaSuccess = !AzuresongEntry.XLua.IsEnable;
            //_isInitILSuccess = !AzuresongEntry.ILRuntime.IsEnable;

            if(AzuresongEntry.HybirdCLR.IsEnable)
            {
                _isInitHybridCLRSuccess = false;
                AzuresongEntry.HybirdCLR.Init();
            }
            else
            {
                _isInitHybridCLRSuccess = true;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (//(!AzuresongEntry.XLua.IsEnable || _isInitXluaSuccess) &&
                //(!AzuresongEntry.ILRuntime.IsEnable || _isInitILSuccess) &&
                (!AzuresongEntry.HybirdCLR.IsEnable || _isInitHybridCLRSuccess))
            {
                ChangeState<ProcedureHotfix>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (!isShutdown)
            {
                //AzuresongEntry.Event.Unsubscribe(InitXLuaSuccessEventArgs.EventId, OnInitXLuaSuccess);
                //AzuresongEntry.Event.Unsubscribe(InitXLuaFailureEventArgs.EventId, OnInitXLuaFailure);
                //AzuresongEntry.Event.Unsubscribe(InitILSuccessEventArgs.EventId, OnInitILRuntimeSuccess);
                //AzuresongEntry.Event.Unsubscribe(InitILFailureEventArgs.EventId, OnInitILRuntimeFailure);
                AzuresongEntry.Event.Unsubscribe(InitHybridCLRSuccessEventArgs.EventId, OnInitHybridCLRSuccess);
                AzuresongEntry.Event.Unsubscribe(InitHybridCLRFailureEventArgs.EventId, OnInitHybridCLRFailure);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        //private void InitXLua()
        //{

        //}

        //private void InitILRuntime()
        //{

        //}

        private void InitHybirdCLR()
        {

        }

        //private void OnInitXLuaSuccess(object sender, GameEventArgs e)
        //{
        //    InitXLuaSuccessEventArgs ne = (InitXLuaSuccessEventArgs)e;

        //    _isInitXluaSuccess = true;

        //    if (AzuresongEntry.HybirdCLR.IsEnable)
        //    {
        //        InitHybirdCLR();
        //    }
        //    else if (AzuresongEntry.ILRuntime.IsEnable)
        //    {
        //        InitILRuntime();
        //    }
        //}

        //private void OnInitXLuaFailure(object sender, GameEventArgs e)
        //{
        //    InitXLuaFailureEventArgs ne = (InitXLuaFailureEventArgs)e;

        //    ASLog.Error("初始化HybridCLR失败, {0}", ne.Error);
        //}

        //private void OnInitILRuntimeSuccess(object sender, GameEventArgs e)
        //{
        //    InitILSuccessEventArgs ne = (InitILSuccessEventArgs)e;

        //    _isInitILSuccess = true;
        //}

        //private void OnInitILRuntimeFailure(object sender, GameEventArgs e)
        //{
        //    InitILFailureEventArgs ne = (InitILFailureEventArgs)e;

        //    ASLog.Error("初始化ILRuntime失败, {0}", ne.Error);
        //}

        private void OnInitHybridCLRSuccess(object sender, GameEventArgs e)
        {
            InitHybridCLRSuccessEventArgs ne = (InitHybridCLRSuccessEventArgs)e;

            _isInitHybridCLRSuccess = true;
        }

        private void OnInitHybridCLRFailure(object sender, GameEventArgs e)
        {
            InitHybridCLRFailureEventArgs ne = (InitHybridCLRFailureEventArgs)e;

            ASLog.Error("初始化HybridCLR失败, {0}", ne.Error);
        }
    }
}