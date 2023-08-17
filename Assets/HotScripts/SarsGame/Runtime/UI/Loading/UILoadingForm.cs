//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/14 10:40:57
//  Description：UILoadingForm
//------------------------------------------------------------
using SarsFramework.Runtime;
using GameFramework;
using GameFramework.Event;
using TMPro;


namespace SarsGame.Runtime
{
    public class UILoadingForm : UGUIFormLogic
    {
        private float _progress = 0f;
        private float _targetProgress = 0f;
        private TextMeshProUGUI _txtProgress = null;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            _txtProgress = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            SarsEntry.Event.Subscribe(UpdateLoadingProgressEventArgs.EventId, OnUpdateLoadingProgress);

            _progress = _targetProgress = 0f;
            _txtProgress.text = string.Empty;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //_progress = Mathf.Min(_progress + Time.deltaTime, _targetProgress);
            _progress = _targetProgress;
            _txtProgress.text = Utility.Text.Format("{0}%", (_progress * 100f).ToString("F0"));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            if (!isShutdown)
            {
                _txtProgress.text = string.Empty;

                SarsEntry.Event.Unsubscribe(UpdateLoadingProgressEventArgs.EventId, OnUpdateLoadingProgress);
            }

            base.OnClose(isShutdown, userData);
        }

        private void OnUpdateLoadingProgress(object sender, GameEventArgs e)
        {
            UpdateLoadingProgressEventArgs ne = (UpdateLoadingProgressEventArgs)e;

            _targetProgress = ne.Progress;
        }
    }
}