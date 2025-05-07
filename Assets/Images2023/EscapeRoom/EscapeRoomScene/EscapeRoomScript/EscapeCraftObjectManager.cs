/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{

    public class EscapeCraftObjectManager : Controller<GameApplication>
    {
        [SerializeField] Sprite firstSprite;
        [SerializeField] Sprite secondSprite;
        [SerializeField] int objToCraft;
        [SerializeField] string[] solutions;
        [SerializeField] string infoText;
        bool firstItemActivated;
        bool secondItemActivated;
        [SerializeField] GameObject unlockedObj;
        [SerializeField] float alarmTime;


        public void OnMouseDown()
        {
            if (EscapeRoomView.MouseSelectedID == solutions[0])
            {
                firstItemActivated = true;
                Notify(MVCEvents.DELETE_BUTTON_IMAGE);
                Check();
                return;
            }
            else if (EscapeRoomView.MouseSelectedID == solutions[1])
            {
                secondItemActivated = true;
                Notify(MVCEvents.DELETE_BUTTON_IMAGE);
                Check();
                return;
            }
            else
            {
                app.view.EscapeRoomView.TurnOnPanelAlarm(infoText, alarmTime); 
            }

            //   StartCoroutine(AlreadyCraft());
        }


        bool craftedCompleted;
        void Check()
        {
            if (firstItemActivated && secondItemActivated)
            {
                Debug.LogWarning("Hai craftato");
                craftedCompleted = true;
                unlockedObj.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else if (firstItemActivated && !secondItemActivated)
            {
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = firstSprite;
                craftedCompleted = false;
            }
            else
            {
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = secondSprite;
                craftedCompleted = false;
            }
            EscapeRoomView.MouseSelectedID = EscapeRoomView.EMPTY;
            EscapeRoomButtonsManager.playerClicked = true;
            EscapeRoomBackground.BoxCollider2D.enabled = false;
        }

        public IEnumerator AlreadyCraft()
        {
            yield return new WaitUntil(Crafted);
        }

        public bool Crafted()
        {
            return craftedCompleted;
        }

    }

}
