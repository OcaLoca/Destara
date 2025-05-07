using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

namespace Game
{
    public class LibraryView : View<GameApplication>
    {
        [SerializeField] Button[] btnBacks;
        [SerializeField] Button btnVolumeOne, btnVolumeTwo;

        string URLVOLUMEONE = "https://docs.google.com/forms/d/e/1FAIpQLSdHUIXjtOqfb_IbJVQTnPT3tM0yW-7hbzqm_tDixsEQ4kw_uQ/viewform?usp=sharing";
        string URLVOLUMEOTWO = "https://docs.google.com/forms/d/e/1FAIpQLSe4Mu_V-s1qSJ38l7qRTr-OvsxAarYExwwCqcDMEHp6GhphOA/viewform?usp=dialog";


        private void OnEnable()
        {
            foreach (var item in btnBacks)
            {
                item.onClick.RemoveAllListeners();
                item.onClick.AddListener(OnClickBack);
            }

            btnVolumeOne.onClick.RemoveAllListeners();
            btnVolumeOne.onClick.AddListener(OnClickInterestVolumeOne);

            btnVolumeTwo.onClick.RemoveAllListeners();
            btnVolumeTwo.onClick.AddListener(OnClickInterestVolumeTwo);
        }

        void OnClickBack()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

        void OnClickInterestVolumeOne()
        {
            Application.OpenURL(URLVOLUMEONE);
        }

        void OnClickInterestVolumeTwo()
        {
            Application.OpenURL(URLVOLUMEOTWO);
        }

    }
}

