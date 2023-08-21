//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/21 16:39:08
//  Description：AVProVideoController
//------------------------------------------------------------
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using System.Security.Policy;

namespace Azuresong.Runtime
{
    public class AVProVideoController : MonoBehaviour
    {
        [SerializeField]
        private string _url = string.Empty;
        [SerializeField]
        private MediaPlayer _mediaPlayer = null;
        [SerializeField]
        private DisplayUGUI _displayUGUI = null;


        private void Awake()
        {
            _mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
            _mediaPlayer.Stop();
        }

        private void Start()
        {
        }

        private void OnMediaPlayerEvent(MediaPlayer arg0, MediaPlayerEvent.EventType arg1, ErrorCode arg2)
        {
            ASLog.Info("OnMediaPlayerEvent  " + arg1.ToString());
            switch (arg1)
            {
                case MediaPlayerEvent.EventType.ReadyToPlay:
                    {
                        //_mediaPlayer.Play();
                    }
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _mediaPlayer.OpenMedia(new MediaPath(_url, MediaPathType.AbsolutePathOrURL));
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _mediaPlayer.Play();
            }
        }
    }
}