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
    public class ZoomView : MonoBehaviour
    {
        [SerializeField] GameObject animation;
        [SerializeField] Button btnClose;
        [SerializeField] GameObject[] objToTurnOff;
        [SerializeField] bool panelButton;
        bool animationIsActive;

        private void OnEnable()
        {
            if(!panelButton)
            {
                GameApplication.Singleton.view.EscapeRoomView.btnPanel.SetActive(false);
            }

            if (animation.gameObject.activeSelf)
            {
                animation.gameObject.SetActive(false);
                animationIsActive = true;
            }
            else
            {
                animationIsActive = false;
            }
            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(CheckAnimation);
            TurnOff();
        }

        List<GameObject> objToTurnOn = new List<GameObject>();
        void TurnOff()
        {
            objToTurnOn.Clear();

            foreach (GameObject gameObject in objToTurnOff)
            {
                if(gameObject.activeInHierarchy)
                {
                    objToTurnOn.Add(gameObject);
                }
                gameObject.SetActive(false);
            }
        }

        void TurnOn()
        {
            foreach (GameObject gameObject in objToTurnOn)
            {
                gameObject.SetActive(true);
            }
        }

        void CheckAnimation()
        {
            if (animationIsActive) { animation.gameObject.SetActive(true); }
            TurnOn();
            GameApplication.Singleton.view.EscapeRoomView.btnPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
