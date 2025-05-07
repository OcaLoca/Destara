/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BattleTutorialSystem : MonoBehaviour
    {
        public static BoxCollider2D TutorialBoxCollider;
        public GameObject[] TutorialPanelsDatabase;
        public GameObject buttonBlocker;
        // public BoxCollider2D [] TutorialColliders;
        int panelIndexNumber = 0;
        int firstDataLenght;

        private void Awake()
        {
            buttonBlocker.gameObject.SetActive(true);
            TutorialBoxCollider = GetComponent<BoxCollider2D>();
            TutorialBoxCollider.enabled = true;
        }

        private void OnEnable()
        {
            firstDataLenght = TutorialPanelsDatabase.Length - 1;

            //if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1 ||
            //    PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1)
            //{
            //    gameObject.SetActive(true);
            //}
        }

        public void OnMouseDown()
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
                gameObject.SetActive(false);
                return;
            }
            TutorialBoxCollider = GetComponent<BoxCollider2D>();
            TutorialBoxCollider.gameObject.SetActive(false);
            TutorialBoxCollider.gameObject.SetActive(true);
            TutorialPanelsDatabase[panelIndexNumber].SetActive(true);

            if (panelIndexNumber == firstDataLenght)
            {
                StartCoroutine(TurnOffBattleTutorial());
            }
        }

        IEnumerator TurnOffBattleTutorial()
        {
            yield return new WaitForEndOfFrame();
            buttonBlocker.SetActive(false);
        }
    }
}
