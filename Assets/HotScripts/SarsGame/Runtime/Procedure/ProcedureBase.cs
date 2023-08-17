//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/06/29 15:55:34
//  Description：JinanProcedure
//------------------------------------------------------------
using Azuresong.Runtime;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace SarsGame.Runtime
{
    public abstract class ProcedureBase : Azuresong.Runtime.ProcedureBase
    {
        protected Type _nextProcedureType = null;

        public virtual string SceneName
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual bool NeedPlayer
        {
            get
            {
                return false;
            }
        }

        public virtual Vector3 PlayerInitPosition
        {
            get
            {
                return Vector3.zero;
            }
        }

        public virtual Vector3 PlayerInitRotation
        {
            get
            {
                return Vector3.zero;
            }
        }


        public void ChangeScene<T>() where T : ProcedureBase
        {
            _nextProcedureType = typeof(T);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (_nextProcedureType != null)
            {
                TryChangeScene(procedureOwner, _nextProcedureType);
            }
            else
            {
                OnUpdateSelf(procedureOwner, elapseSeconds, realElapseSeconds);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            _nextProcedureType = null;

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected virtual void OnUpdateSelf(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {

        }

        protected void TryChangeScene(ProcedureOwner procedureOwner, Type procedureType)
        {
            if (procedureType == null)
            {
                ASLog.Error("Unknow procedure type.");
                return;
            }

            ProcedureBase procedure = AzuresongEntry.Procedure.GetProcedure(procedureType) as ProcedureBase;
            if (procedure == null)
            {
                ASLog.Error("Unknow procedure type '{0}'.", procedureType.Name);
                return;
            }

            if (!string.IsNullOrEmpty(procedure.SceneName))
            {
                VarObject varObject = new VarObject();
                varObject.SetValue(procedure);

                procedureOwner.SetData<VarObject>("NextProcedure", varObject);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            else
            {
                ChangeState(procedureOwner, procedureType);
            }
        }

        protected void SetSceneRootObjsActive(bool active)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene != null)
            {
                GameObject[] objs = scene.GetRootGameObjects();
                if (objs != null && objs.Length > 0)
                {
                    for (int i = 0; i < objs.Length; i++)
                    {
                        objs[i].SetActive(active);
                    }
                }
            }
        }
    }
}