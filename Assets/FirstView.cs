using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using StarworkGC.Localization;
using StarworkGC.Utils;
using UnityEngine.Rendering;

namespace Game
{
    public class FirstView : View<GameApplication>
    {
        public Button btnContinue;
        public static bool comeFromFirstView;
        public float secondBeforeNotify;

        private void OnEnable()
        {
            app.model.currentView = this;
            app.model.previousView = new Stack<View<GameApplication>>();

            comeFromFirstView = true;
            StartCoroutine(NoTouch());
            StartCoroutine(WaitingForTheInstanceSoundManagerLoading());
        }

        IEnumerator WaitingForTheInstanceSoundManagerLoading()
        {
            yield return CoroutinesHelper.PointOneSeconds;
            app.view.Settings.SetSavedSettings();
            SoundManager.Singleton.LoadMusicAudioClip();
        }
        IEnumerator NoTouch()
        {
            yield return CoroutinesHelper.TwoSeconds;
            CheckPlayerRunGame();
        }

        void CheckPlayerRunGame()
        {
            const string language = "SetLanguage";

            if (PlayerPrefs.GetInt(language, 1) == 1)
            {
                Notify(MVCEvents.OPEN_LANGUAGE_VIEW);
                PlayerPrefs.SetInt(language, 0);
            }
            else
            {
                Notify(MVCEvents.OPEN_MAINMENU_VIEW);
            }
        }
    }
}
