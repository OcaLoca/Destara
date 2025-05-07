using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
namespace Game
{
    public class DemoController : Controller<GameApplication>
    {
        private void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_DEMO_VIEW, OnFinishDemo);
        }
        private void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.LOAD_PAGE, OnFinishDemo);
        }

        public void OnFinishDemo(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.DemoView;
            app.model.currentView.gameObject.SetActive(true);
        }
    }
}
