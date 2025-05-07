/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    public class ChoiceClassTutorialManager : MonoBehaviour
    {
        [SerializeField] GameObject HandPanel;
        public void OpenHandPanel()
        { 
            HandPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }

    }

}
