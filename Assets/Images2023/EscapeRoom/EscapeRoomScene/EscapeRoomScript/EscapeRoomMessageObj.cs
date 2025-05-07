/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EscapeRoomMessageObj : MonoBehaviour
    {
        [SerializeField] float alarmTime;
        [SerializeField] string storyID;

        private void OnMouseDown()
        {
            GameApplication.Singleton.view.EscapeRoomView.TurnOnPanelAlarm(storyID, alarmTime);
        }
    }
}
