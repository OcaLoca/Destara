using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
namespace Game
{
    public class DeathController : Controller<GameApplication>
    {
        private void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_DEATH_VIEW, OnDeath);
        }

        private void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.LOAD_PAGE, OnDeath);
        }

        public void OnDeath(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.DeathView;
            app.model.currentView.gameObject.SetActive(true);
        }
    }
}
