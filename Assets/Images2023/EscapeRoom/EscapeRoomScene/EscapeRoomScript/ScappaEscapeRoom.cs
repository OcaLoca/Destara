/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;

namespace Game
{
    public class ScappaEscapeRoom : View<GameApplication>
    {
        [SerializeField] Button rightButton;
        [SerializeField] string ID;
        public static bool scappaEscapeIsReopend;

        private void OnEnable()
        {
            rightButton.onClick.RemoveAllListeners();
            rightButton.onClick.AddListener(delegate { OpenClosePanel(); });
        }

        public void OnMouseDown()
        {
            EscapeRoomButtonsManager.playerClicked = true;
        }

        void OpenClosePanel()
        {
            TurnOff();
            Notify(MVCEvents.OPEN_ESCAPE_ROOM_CLOSE_PANEL, ID);
            scappaEscapeIsReopend = false;
            StartCoroutine(ReactivateAllObj());
        }

        IEnumerator ReactivateAllObj()
        {
            yield return new WaitUntil(IsScappaEscapeReopend);
            TurnOn();
        }

        bool IsScappaEscapeReopend()
        {
            return scappaEscapeIsReopend;
        }


        [SerializeField] GameObject[] objToTurnOff;
        List<GameObject> objToTurnOn = new List<GameObject>();
        void TurnOff()
        {
            objToTurnOn.Clear();

            foreach (GameObject gameObject in objToTurnOff)
            {
                if (gameObject.activeInHierarchy)
                {
                    objToTurnOn.Add(gameObject);
                }
                gameObject.SetActive(false);
            }
        }

        public void TurnOn()
        {
            foreach (GameObject gameObject in objToTurnOn)
            {
                gameObject.SetActive(true);
            }
        }

    }

}
