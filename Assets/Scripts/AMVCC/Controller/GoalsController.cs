using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class GoalsController : Controller<GameApplication>
    {
        private void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_TROPHY_VIEW, OnTrophyClicked);
        }

        private void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.OPEN_TROPHY_VIEW, OnTrophyClicked);
        }

        void OnTrophyClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.TrophyView;
            app.model.currentView.gameObject.SetActive(true);
        }
    }
}
