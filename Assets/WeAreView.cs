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
    public class WeAreView : View<GameApplication>
    {
        [SerializeField] Button btnMembersView;
        [SerializeField] Button btnMobyBitView;
        [SerializeField] Button btnBack;


        // Start is called before the first frame update
        void Start()
        {
            btnMembersView.onClick.RemoveAllListeners();
            btnMembersView.onClick.AddListener(OnClickMembers);

            btnMobyBitView.onClick.RemoveAllListeners();
            btnMobyBitView.onClick.AddListener(OnClickMobytBit);

            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);

        }

        void OnClickMembers()
        {
            Notify(MVCEvents.OPEN_TEAM_VIEW);
        }

        void OnClickMobytBit()
        {
            Notify(MVCEvents.OPEN_MOBYBIT_VIEW);
        }

        void OnClickBack()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }
    }

}

