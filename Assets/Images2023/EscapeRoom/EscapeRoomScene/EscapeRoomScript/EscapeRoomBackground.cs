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
    public class EscapeRoomBackground : MonoBehaviour
    {
        public static BoxCollider2D BoxCollider2D;
        [SerializeField] float alarmTime;
        [SerializeField] string storyID;
        [SerializeField] string introStoryID;

        private void Awake()
        {
            BoxCollider2D = GetComponent<BoxCollider2D>();
            BoxCollider2D.enabled = false;
        }

        private void OnEnable()
        {
            GameApplication.Singleton.view.EscapeRoomView.TurnOnPanelAlarm(introStoryID, alarmTime);
        }

        public void OnMouseDown()
        {
            EscapeRoomButtonsManager.playerClicked = true;
            BoxCollider2D.enabled = false;
            GameApplication.Singleton.view.EscapeRoomView.TurnOnPanelAlarm(storyID, alarmTime);
            EscapeRoomView.MouseSelectedID = EscapeRoomView.EMPTY;
        }
    }

}
