/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class StatsViewSystemManager : MonoBehaviour
    {
        public Button btn;
        public GameObject[] TutorialPanelsDatabase;
        int panelIndexNumber = 0;
        int dataLenght;

        private void OnEnable()
        {
            dataLenght = TutorialPanelsDatabase.Length - 1;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClickButton);
            RefreshFirstPanel();
        }

        public void RefreshFirstPanel()
        {
            TutorialPanelsDatabase[0].SetActive(true);
        }

        public void OnClickButton()
        {
            foreach (var panel in TutorialPanelsDatabase)
            {
                panel.gameObject.SetActive(false);
            }
            OpenNextFirstTutorialPanel();
        }

        void OpenNextFirstTutorialPanel()
        {
            panelIndexNumber++;
            if (panelIndexNumber > dataLenght)
            {
                panelIndexNumber = 0;
                GameApplication.Singleton.view.StatsView.OnClickEquipButton(true);
                gameObject.SetActive(false);
                return;
            }
            TutorialPanelsDatabase[panelIndexNumber].SetActive(true);
        }

    }
}
