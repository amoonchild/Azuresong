//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2023/08/18 10:20:45
//  Description: ASWebRequestComponent
//------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;


namespace Azuresong.Runtime
{
    public partial class ASWebRequestComponent : GameFrameworkComponent
    {
        public static string ServerUrl
        {
            get;
            private set;
        }

        public static int ServerPort
        {
            get;
            private set;
        }


        private Dictionary<string, WebResponseHandlerBase> _webResponseHandlers = new Dictionary<string, WebResponseHandlerBase>();
        private EventComponent _eventComponent = null;
        private WebRequestComponent _webRequestComponent = null;
        [SerializeField]
        private string _debugServerUrl = "localhost";
        [SerializeField]
        private int _debugServerPort = 0;
        [SerializeField]
        private string _releaseServerUrl = "https://www.";
        [SerializeField]
        private int _releaseServerPort = 0;
        [SerializeField]
        private bool _isDebug = false;

        public bool IsDebug
        {
            get
            {
                return _isDebug;
            }
            set
            {
                _isDebug = value;
                ServerUrl = _isDebug ? _debugServerUrl : _releaseServerUrl;
                ServerPort = _isDebug ? _debugServerPort : _releaseServerPort;
            }
        }


        private void Start()
        {
            ServerUrl = _isDebug ? _debugServerUrl : _releaseServerUrl;
            ServerPort = _isDebug ? _debugServerPort : _releaseServerPort;

            InitWebResponseHandlers();

            _eventComponent = GameEntry.GetComponent<EventComponent>();
            _eventComponent.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            _eventComponent.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            _webRequestComponent = GameEntry.GetComponent<WebRequestComponent>();
        }
    }
}
