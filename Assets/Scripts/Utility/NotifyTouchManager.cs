/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class NotifyTouchManager : MonoBehaviour
    {
        public GameObject NotifyPanel;

        public static NotifyTouchManager Singleton { get; set; }

        private void Awake()
        {
            Singleton = this;
        }

        public void ActiveNotifyPanel()
        {
            NotifyPanel.SetActive(true);
        }
        public void DeactiveNotifyPanel()
        {
            NotifyPanel.SetActive(false);
        }
        internal bool NotifyAlreadyActive()
        {
            return NotifyPanel.activeSelf;
        }

    }

}
