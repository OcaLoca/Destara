/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class CuriosityController : Controller<GameApplication>
    {
        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_WEARE_VIEW, OnWeAreClicked);
            AddEventListenerToApp(MVCEvents.OPEN_GOALS_VIEW, OnGoalsClicked);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.OPEN_WEARE_VIEW, OnWeAreClicked);
            RemoveEventListenerFromApp(MVCEvents.OPEN_GOALS_VIEW, OnGoalsClicked);
        }

      
        void OnWeAreClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.WeAreView;
            app.model.currentView.gameObject.SetActive(true);
        }

        void OnGoalsClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.GoalsView;
            app.model.currentView.gameObject.SetActive(true);

        }
    }
}