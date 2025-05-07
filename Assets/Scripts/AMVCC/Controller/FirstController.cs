using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
namespace Game
{
    public class FirstController : Controller<GameApplication>
    {
        private void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_LANGUAGE_VIEW, OnClickContinue);
        }

        private void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.OPEN_LANGUAGE_VIEW, OnClickContinue);
        }

        void OnClickContinue(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.LanguageView;
            app.model.currentView.gameObject.SetActive(true);
        }


    }
}

