/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class EscapeRoomView : View<GameApplication>
    {
        BookModel Model { get { return app.model.BookModel; } }
        [SerializeField] RawImage rawImage;
        [SerializeField] public GameObject panelAlarm;
        [SerializeField] public GameObject animationTransition;
        [SerializeField] public TMP_Text alarmText;
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;
        [SerializeField] GameObject closingView;
        public const string EMPTY = "Empty";

        public GameObject btnPanel;
        public static string MouseSelectedID;

        string ID;

        private void OnEnable()
        {
            animationTransition.gameObject.SetActive(true);
            MouseSelectedID = EMPTY;
            panelAlarm.SetActive(false);
            Model.loadEscapeRoom.SetupEscapeRoom();
            if (gameObject.activeInHierarchy) { rawImage.gameObject.SetActive(false); }

            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(delegate { OpenGameAt(ID); });
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(delegate { TurnOffExitPanel(); });

            RemoveEventListenerFromApp(MVCEvents.OPEN_ESCAPE_ROOM_CLOSE_PANEL, OpenExitPanel);
            AddEventListenerToApp(MVCEvents.OPEN_ESCAPE_ROOM_CLOSE_PANEL, OpenExitPanel);
        }

        public void TurnOnPanelAlarm(string txt, float time)
        {
            alarmText.text = txt;
            panelAlarm.SetActive(true);
            StartCoroutine(TurnOffAlarmPanel(time));
        }
        IEnumerator TurnOffAlarmPanel(float time)
        {
            yield return new WaitForSeconds(time);
            panelAlarm.SetActive(false);
        }


        void OpenExitPanel(params object[] data)
        {
            string pageID = (string)data[0];
            closingView.gameObject.SetActive(true);
            ID = pageID;
        }

        void TurnOffExitPanel()
        {
            ScappaEscapeRoom.scappaEscapeIsReopend = true; // Attenzione qua capire da che escape arriva
            closingView.gameObject.SetActive(false);
        }

        void OpenGameAt(string pageID)
        {
            PlayerManager.Singleton.pagesRead.Add(pageID);

            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.BookView;
            app.model.currentView.gameObject.SetActive(true);
        }

    }

}
