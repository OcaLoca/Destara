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
    public class EscapeRoomController : Controller<GameApplication>
    {
        private void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_ESCAPE_ROOM_VIEW, OpenEscapeRoom);
        }

        public void OpenEscapeRoom(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.EscapeRoomView;
            app.model.currentView.gameObject.SetActive(true);
        }

    }

}
