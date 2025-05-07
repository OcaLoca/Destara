/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using SmartMVC;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Game
{
    public class DemoView : View<GameApplication>
    {
        [SerializeField] Button btnContinue;
        [SerializeField] Button btnClose;
        [SerializeField] GameObject firstPage, lastPage;
        internal const string RUN_FINISHED = "LASTRUNFINISHEDBYPLAYER";

        private void OnEnable()
        {
            firstPage.SetActive(true);

            btnContinue.onClick.RemoveAllListeners();
            btnContinue.onClick.AddListener(delegate { firstPage.SetActive(false); });

            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(CloseDownloadView);

            PlayerPrefs.SetInt(RUN_FINISHED, 1);
            SoundManager.Singleton.LoadMusicAudioClip();
        }

        const string TRIGGERANIMATIONID = "start_slides";
        void CloseDownloadView()
        {
            Notify(MVCEvents.OPEN_MAINMENU_VIEW);
            gameObject.GetComponentInChildren<Animator>().ResetTrigger(TRIGGERANIMATIONID);
            lastPage.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
