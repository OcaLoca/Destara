/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InventoryTutorialManager : MonoBehaviour
    {
        public Button btn;
        public GameObject[] TutorialPanelsDatabase;
        int panelIndexNumber = 0;
        int firstDataLenght;

        private void OnEnable()
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClickButton);
            firstDataLenght = TutorialPanelsDatabase.Length - 1;
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
            if (panelIndexNumber > firstDataLenght)
            {
                panelIndexNumber = 0;
                StartCoroutine(TurnOffPanel());
                return;
            }
            TutorialPanelsDatabase[panelIndexNumber].SetActive(true);
        }

        IEnumerator TurnOffPanel()
        {
            yield return new WaitForEndOfFrame();
            gameObject.SetActive(false);
        }


        public void OpenDefenceSubmenuAndActivateTutorial() //richiamato da inspector 
        {
            GameApplication.Singleton.view.EquipmentView.OpenDefenceListMenu(true);
            GameApplication.Singleton.view.EquipmentView.ShowPreparationDefenceBeforeFight();
        }
    }

}
