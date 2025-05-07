/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TriggerableZoomObj : MonoBehaviour
    {
        [SerializeField] GameObject imgZoom;
        [SerializeField] float alarmTime;
        [SerializeField] string storyID;

        public void OnMouseDown()
        {
            GameApplication.Singleton.view.EscapeRoomView.TurnOnPanelAlarm(storyID, alarmTime);
            imgZoom.gameObject.SetActive(true);
        }

    }

}
