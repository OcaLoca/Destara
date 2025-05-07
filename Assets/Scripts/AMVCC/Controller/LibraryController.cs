using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class LibraryController : Controller<GameApplication>
    {
        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_MAINMENU_VIEW, OnClickBook);
            AddEventListenerToApp(MVCEvents.OPEN_LIBRARY, OnClickLibrary);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.OPEN_MAINMENU_VIEW, OnClickBook);
            RemoveEventListenerFromApp(MVCEvents.OPEN_LIBRARY, OnClickLibrary);
        }

        public void OnClickBook(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.MainMenu;
            app.model.currentView.gameObject.SetActive(true);
        }

        public void OnClickLibrary(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.LibraryView;
            app.model.currentView.gameObject.SetActive(true);
        }



    }
}
