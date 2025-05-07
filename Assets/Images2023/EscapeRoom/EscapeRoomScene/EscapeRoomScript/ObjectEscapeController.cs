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

    public class ObjectEscapeController : View<GameApplication>
    {
        [SerializeField] string ID;
        [SerializeField] Sprite sprite;
        [SerializeField] bool fusion;
        [SerializeField] float alarmTime;
        [SerializeField] string storyID;

        public void OnMouseDown()
        {
            GameApplication.Singleton.view.EscapeRoomView.TurnOnPanelAlarm(storyID, alarmTime);
            Notify(MVCEvents.OBJECT_PUSHED_SET_ID, ID, fusion);
            Notify(MVCEvents.OBJECT_PUSHED_SET_SPRITE, sprite);
            gameObject.SetActive(false);
        }


    }

}
