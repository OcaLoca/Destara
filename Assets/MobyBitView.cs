/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */

using SmartMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MobyBitView : View<GameApplication>
    {
        [SerializeField] Button btnMobyBitWebSite;
        [SerializeField] Button btnBack;

        const string URL = "https://www.mobybit.it/";

        // Start is called before the first frame update
        void Start()
        {
            btnMobyBitWebSite.onClick.RemoveAllListeners();
            btnMobyBitWebSite.onClick.AddListener(OnClickMobytBitWebSite);

            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);
        }

        void OnClickMobytBitWebSite()
        {
            Application.OpenURL(URL);
        }

        void OnClickBack()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

    }
}

