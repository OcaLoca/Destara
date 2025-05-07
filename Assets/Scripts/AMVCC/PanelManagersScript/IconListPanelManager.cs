/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Game
{

    public class IconListPanelManager : MonoBehaviour
    {
        [SerializeField] List<Button> iconsButton;

        private void OnEnable()
        {
            foreach (Button btn in iconsButton)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(delegate { SetInfoPanel(btn.gameObject.name); });
            }
        }

        void SetInfoPanel(string btnName)
        {
           InfoIconPanelManager.Instance.SetPanelByButtonName(btnName);
        }
    }

}
