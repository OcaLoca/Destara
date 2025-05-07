using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SkipElementButtonManager : MonoBehaviour
    {
        public Button button;
        string optionID;
        static bool playerSkipVideo = false;

        private void OnEnable()
        {
            playerSkipVideo = false;

            if (!PlayerManager.Singleton.currentPage.changeByTime)
            {
                button.onClick.AddListener(delegate
                {
                    playerSkipVideo = true;
                    //MapAnimationManager.mapsAnimationIsFinish = true;
                    WaitEffect();
                    StartCoroutine(CheckOfCondition(SetTimeByID()));
                });
            }
            else
            {
                button.interactable = false;
                StartCoroutine(CheckOfCondition(SetTimeByID()));
            }
        }

        internal static bool PlayerSkippedVideo()
        {
            return playerSkipVideo;
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(delegate { CheckOfCondition(0); });
        }

        public void Initialize(string ID)
        {
            optionID = ID;
        }

        public void LoadChapter()
        {
            GameApplication.Singleton.app.Notify(MVCEvents.LOAD_PAGE, optionID);
        }

        public void LoadStatus()
        {
            GameApplication.Singleton.app.Notify(MVCEvents.LOAD_STATUS, optionID);
        }

        public float SetTimeByID()  //questo lo devo caricare alla pagina prima del titolo
        {
            if (PlayerManager.Singleton.currentPage.chapterSection == ScriptablePage.Section.Titolo || PlayerManager.Singleton.currentPage.pageID == "allaSalute")
            {
                return 0;
            }
            else
            {
                return 0;
            }

        }

        IEnumerator CheckOfCondition(float time)    //questo lo carico alla pagina che mi serve
        {
            if (PlayerManager.Singleton.currentPage.pageID == "inVinoVeritas" || PlayerManager.Singleton.currentPage.pageID == "allaSalute")
            {
                //ReturnEffect();
            }
            yield return new WaitForSeconds(time);
            switch (optionID)
            {
                case ("sceltaClasse"):
                    GameApplication.Singleton.app.Notify(MVCEvents.OPEN_CHARACTER_CREATION_VIEW);
                    break;

                default:
                    LoadChapter();
                    LoadStatus();
                    break;
            }
        }

        public void BlockNotifyCoroutine()
        {
            if (BookController.notifyAlarmCoroutine != null)
            {
                StopCoroutine(BookController.notifyAlarmCoroutine);
            }

            if (NotifyTouchManager.Singleton.NotifyPanel.activeSelf)
            {
                NotifyTouchManager.Singleton.DeactiveNotifyPanel();
            }
        }

        public void WaitEffect()
        {
            GameApplication.Singleton.model.BookModel.dissolveEffect.dissolveVelocity = 1;
            GameApplication.Singleton.model.BookModel.dissolveEffect.DissolveImage();
        }
        public void ReturnEffect()
        {
            GameApplication.Singleton.model.BookModel.dissolveEffect.dissolveVelocity = 0.5f;
            GameApplication.Singleton.model.BookModel.dissolveEffect.isDissolving = false;
            GameApplication.Singleton.model.BookModel.dissolveEffect.RewindDissolve();
        }
    }

}


