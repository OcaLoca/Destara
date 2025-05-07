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

    public class WeAreController : Controller<GameApplication>
    {
        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_TEAM_VIEW, OnTeamClicked);
            AddEventListenerToApp(MVCEvents.OPEN_MOBYBIT_VIEW, OnMobyBitClicked);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.OPEN_TEAM_VIEW, OnTeamClicked);
            RemoveEventListenerFromApp(MVCEvents.OPEN_MOBYBIT_VIEW, OnMobyBitClicked);
        }

        void OnTeamClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.TeamView;
            app.model.currentView.gameObject.SetActive(true);
        }
        void OnMobyBitClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.MobyBitView;
            app.model.currentView.gameObject.SetActive(true);
        }
    }
}
