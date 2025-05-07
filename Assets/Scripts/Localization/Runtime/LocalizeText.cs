using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StarworkGC.Localization
{
    ///<summary>
    ///A component that automatically localizes Unity's UI labels and TextMeshPro's labels
    ///</summary>
    public class LocalizeText : MonoBehaviour
    {
        /// <summary>
        /// Localization key.
        /// </summary>
        public string key;

        /// <summary>
        /// Manually change the value of whatever the localization component is attached to.
        /// </summary>
        public string value
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Text lbl = GetComponent<Text>();
                    if (lbl)
                    {
                        lbl.text = value;
#if UNITY_EDITOR
                        if (!Application.isPlaying) SetDirty(lbl);
#endif
                        return;
                    }
                    TextMeshProUGUI lblPro = GetComponent<TextMeshProUGUI>();
                    if (lblPro)
                    {
                        lblPro.SetText(value);
#if UNITY_EDITOR
                        if (!Application.isPlaying) SetDirty(lbl);
#endif
                    }
                }
            }
        }

        bool mStarted = false;

        /// <summary>
        /// Localize the widget on enable, but only if it has been started already.
        /// </summary>
        void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (mStarted) OnLocalize();
        }

        /// <summary>
        /// Localize the widget on start.
        /// </summary>

        void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            mStarted = true;
            OnLocalize();
        }

        /// <summary>
        /// This function is called by the Localization manager via a broadcast SendMessage.
        /// </summary>

        public void OnLocalize()
        {
            // If no localization key has been specified, use the label's text as the key
            if (string.IsNullOrEmpty(key))
            {
                Text lbl = GetComponent<Text>();
                if (lbl)
                {
                    key = lbl.text;
                }
                else
                {
                    TextMeshProUGUI lblPro = GetComponent<TextMeshProUGUI>();
                    if (lblPro)
                    {
                        key = lblPro.text;
                    }
                }
            }

            // If we still don't have a key, leave the value as blank
            if (!string.IsNullOrEmpty(key)) value = Localization.Get(key);
        }

        /// <summary>
        /// Convenience function that marks the specified object as dirty in the Unity Editor.
        /// </summary>

        static public void SetDirty(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            if (obj)
            {
                UnityEditor.EditorUtility.SetDirty(obj);
            }
#endif
        }

    }
}