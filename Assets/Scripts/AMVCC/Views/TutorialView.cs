using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;

namespace Game
{
    public class TutorialView : View<GameApplication>   
    {
        public Button btnBack;

        private void OnEnable()
        {
            btnBack.onClick.AddListener(OnBackClick);
        }

        private void OnDisable()
        {
            btnBack.onClick.RemoveAllListeners();
        }

        void OnBackClick()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }
    }
}


