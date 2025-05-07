/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class TriggerableObjectEscapeRoom : Controller<GameApplication>
    {
        [SerializeField] string unlockID;
        [SerializeField] string infoText;
        [SerializeField] float alarmTime;
        public void OnMouseDown()
        {
            if (EscapeRoomView.MouseSelectedID == unlockID)
            {
                Notify(MVCEvents.DELETE_BUTTON_IMAGE);
                DestroyGameObject();
                EscapeRoomView.MouseSelectedID = EscapeRoomView.EMPTY;
            }
            else if (EscapeRoomView.MouseSelectedID != EscapeRoomView.EMPTY)
            {
                app.view.EscapeRoomView.TurnOnPanelAlarm("Non funziona...", alarmTime);
                EscapeRoomView.MouseSelectedID = EscapeRoomView.EMPTY;
            }
            else
            {
                app.view.EscapeRoomView.TurnOnPanelAlarm(infoText, alarmTime);
            }
            EscapeRoomButtonsManager.playerClicked = true;
            EscapeRoomBackground.BoxCollider2D.enabled = false;
        }

        void DestroyGameObject()
        {
            gameObject.SetActive(false);
        }

    }

}
