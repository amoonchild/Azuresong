//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime: 2022/05/25 15:51:36
//  Description: ReferenceCollector
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Azuresong.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField]
        [HideInInspector]
        private List<ReferenceCollectorData> m_ReferenceObjects = new List<ReferenceCollectorData>();
        private Dictionary<string, GameObject> m_Dict = new Dictionary<string, GameObject>();
        private Transform m_Transform;


        public Transform CachedTransform
        {
            get
            {
                if (m_Transform == null)
                {
                    m_Transform = transform;
                }

                return m_Transform;
            }
        }

        public int Count
        {
            get
            {
                return m_Dict.Count;
            }
        }


        public void SortObjects()
        {
            m_ReferenceObjects.Sort(new ReferenceCollectorDataComparer());
        }

        #region 获取组件
        public bool HasChild(string key)
        {
            return m_Dict.ContainsKey(key);
        }

        public GameObject GetGO(string key)
        {
            Init(false);

            if (!m_Dict.ContainsKey(key))
            {
                return null;
            }

            return m_Dict[key];
        }

        public Transform GetTransform(string key)
        {
            GameObject obj = GetGO(key);
            if (obj == null)
            {
                return null;
            }

            return obj.transform;
        }

        public Component GetComponent(string key, Type type)
        {
            GameObject obj = GetGO(key);
            if (obj == null)
            {
                return null;
            }

            return obj.GetComponent(type);
        }

        public T GetComponent<T>(string key) where T : Component
        {
            GameObject obj = GetGO(key);
            if (obj == null)
            {
                return null;
            }

            return obj.GetComponent<T>();
        }

        public Canvas GetCanvas(string key)
        {
            return (Canvas)GetComponent(key, typeof(Canvas));
        }

        public CanvasGroup GetCanvasGroup(string key)
        {
            return (CanvasGroup)GetComponent(key, typeof(CanvasGroup));
        }

        public Animator GetAnimator(string key)
        {
            return (Animator)GetComponent(key, typeof(Animator));
        }

        public Image GetImage(string key)
        {
            return (Image)GetComponent(key, typeof(Image));
        }

        public RawImage GetRawImage(string key)
        {
            return (RawImage)GetComponent(key, typeof(RawImage));
        }

        public Text GetText(string key)
        {
            return (Text)GetComponent(key, typeof(Text));
        }

        public Button GetButton(string key)
        {
            return (Button)GetComponent(key, typeof(Button));
        }

        public Toggle GetToggle(string key)
        {
            return (Toggle)GetComponent(key, typeof(Toggle));
        }

        public ToggleGroup GetToggleGroup(string key)
        {
            return (ToggleGroup)GetComponent(key, typeof(ToggleGroup));
        }

        public InputField GetInputField(string key)
        {
            return (InputField)GetComponent(key, typeof(InputField));
        }

        public Slider GetSlider(string key)
        {
            return (Slider)GetComponent(key, typeof(Slider));
        }

        public ScrollRect GetScrollRect(string key)
        {
            return (ScrollRect)GetComponent(key, typeof(ScrollRect));
        }

        public void Init(bool reload)
        {
            if (reload)
            {
                m_Dict.Clear();
            }

            if (m_Dict.Count == 0)
            {
                if (m_ReferenceObjects != null)
                {
                    for (int i = 0; i < m_ReferenceObjects.Count; i++)
                    {
                        ReferenceCollectorData data = m_ReferenceObjects[i];
                        if (m_Dict.ContainsKey(data.key))
                        {
                            m_Dict[data.key] = data.obj;
                        }
                        else
                        {
                            m_Dict.Add(data.key, data.obj);
                        }
                    }

                    //m_ReferenceObjects.Clear();
                    //m_ReferenceObjects = null;
                }
            }
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            m_Dict.Clear();

            if (m_ReferenceObjects != null)
            {
                for (int i = 0; i < m_ReferenceObjects.Count; i++)
                {
                    ReferenceCollectorData data = m_ReferenceObjects[i];
                    if (m_Dict.ContainsKey(data.key))
                    {
                        m_Dict[data.key] = data.obj;
                    }
                    else
                    {
                        m_Dict.Add(data.key, data.obj);
                    }
                }

                //m_ReferenceObjects.Clear();
                //m_ReferenceObjects = null;
            }
#endif
        }

        public void Awake()
        {
            m_Dict.Clear();
            Init(false);
        }

        #endregion
    }
}
