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


    public class TeamView : View<GameApplication>
    {
        public Button btnBack;
        



        void OnEnable()
        {
            app.model.currentView = this;
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnBackClick);
        }

        void OnBackClick()
        {
           Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

        

    }
}
