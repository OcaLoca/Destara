using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StarworkGC.Localization
{
    ///<summary>
    ///A component that automatically localizes Unity's and TextMeshPro's dropdowns
    ///</summary>
    public class LocalizedDropdown : Dropdown
    {
        LocalizeText localizedCurrentValue;
        [SerializeField] string prefix;

        protected override void Awake()
        {
            base.Awake();
            localizedCurrentValue = captionText.GetComponent<LocalizeText>();
        }
        protected override void Start()
        {
            base.Start();
            foreach (OptionData option in options)
            {
                if (!option.text.StartsWith(prefix))
                {
                    option.text = prefix + option.text;
                }
            }
            RefreshShownValue();
            onValueChanged.AddListener(OnValueChanged);

        }
        public void OnValueChanged(int index)
        {
            if (captionText.text.StartsWith(prefix))
            {
                localizedCurrentValue.key = captionText.text;
            }
            else
            {
                localizedCurrentValue.key = prefix + captionText.text;
            }
            localizedCurrentValue.OnLocalize();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            foreach (OptionData option in options)
            {
                if (!option.text.StartsWith(prefix))
                {
                    option.text = prefix + option.text;
                }
            }
        }
    }
}