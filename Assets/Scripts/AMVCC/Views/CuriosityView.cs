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
using UnityEngine.UI;

namespace Game
{
    public class CuriosityView : View<GameApplication>
    {
        public Button btnBack;
        public Button btnGoals;
        ///public Button btnMuseum;
        //public Button btnTutorial;
        public Button btnTeam;

        void OnEnable()
        {
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);

            btnGoals.onClick.RemoveAllListeners();
            btnGoals.onClick.AddListener(OnClickGoals);

            btnTeam.onClick.RemoveAllListeners();
            btnTeam.onClick.AddListener(OnClickTeam);
        }

        void OnClickBack()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

        void OnClickGoals()
        {
            Notify(MVCEvents.OPEN_GOALS_VIEW);
        }

        void OnClickTeam()
        {
            Notify(MVCEvents.OPEN_TEAM_VIEW);
        }

    }
}