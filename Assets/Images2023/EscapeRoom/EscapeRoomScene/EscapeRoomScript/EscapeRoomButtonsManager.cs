/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;
using TMPro;

namespace Game
{
    public class EscapeRoomButtonsManager : Controller<GameApplication>
    {
        public Button[] btns;
        public RawImage[] images;
        public GameObject imgBlockButton;

        private void OnEnable()
        {
            imgBlockButton.gameObject.SetActive(true);
            for (int i = 0; i < images.Length; i++)
            {
                images[i].texture = null;
            }
            RemoveEventListenerFromApp(MVCEvents.OBJECT_PUSHED_SET_SPRITE, SetSprite);
            AddEventListenerToApp(MVCEvents.OBJECT_PUSHED_SET_SPRITE, SetSprite);
            RemoveEventListenerFromApp(MVCEvents.DELETE_BUTTON_IMAGE, RemoveSpriteFromButton);
            AddEventListenerToApp(MVCEvents.DELETE_BUTTON_IMAGE, RemoveSpriteFromButton);
            RemoveEventListenerFromApp(MVCEvents.OBJECT_PUSHED_SET_ID, SetID);
            AddEventListenerToApp(MVCEvents.OBJECT_PUSHED_SET_ID, SetID);

            foreach (Button btn in btns)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(delegate { SetTmpID(btn); });
                btn.onClick.AddListener(delegate { LockAllButtons(); });
            }
        }

        void SetID(params object[] data)
        {
            string ID = (string)data[0];
            bool craft = (bool)data[1];

            for (int i = 0; i < images.Length; i++)
            {
                if (btns[i].gameObject.name == EscapeRoomView.EMPTY)
                {
                    btns[i].gameObject.name = ID;

                    if (craft)
                    {
                        btns[i].GetComponentInChildren<TMP_Text>().text = "Craftable";
                    }
                    return;
                }
            }
        }

        void SetSprite(params object[] data)
        {
            if (imgBlockButton.activeSelf) { imgBlockButton.gameObject.SetActive(false); }
            Sprite sprite = (Sprite)data[0];

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].texture == null)
                {
                    float alpha = 1.0f; //1 is opaque, 0 is transparent
                    Color currColor = images[i].color;
                    currColor.a = alpha;
                    images[i].color = currColor;
                    images[i].texture = sprite.texture;
                    return;
                }
            }
        }

        public static bool playerClicked = false;
        void SetTmpID(Button btn)
        {
            EscapeRoomView.MouseSelectedID = btn.name;
            btn.interactable = false;
            EscapeRoomBackground.BoxCollider2D.enabled = true;
            playerClicked = false;
            StartCoroutine(WaitAnotherInput(btn));
        }

        void LockAllButtons()
        {
            for (int i = 0; i < btns.Length; i++)
            {
                if (btns[i].interactable == true)
                {
                    btns[i].enabled = false;
                }
            }
        }
        void UnlockAllButtons(Button btn)
        {
            for (int i = 0; i < btns.Length; i++)
            {
                if (btns[i].enabled == false)
                {
                    btns[i].enabled = true;
                }
            }
            btn.interactable = true;
        }
        IEnumerator WaitAnotherInput(Button btn)
        {
            yield return new WaitUntil(HaveClicked);
            UnlockAllButtons(btn);
            playerClicked = false;
        }

        bool HaveClicked()
        {
            return playerClicked;
        }

        void RemoveSpriteFromButton(params object[] data)
        {
            for (int i = 0; i < btns.Length; i++)
            {
                if (btns[i].interactable == false)
                {
                    float alpha = 0f; //1 is opaque, 0 is transparent
                    Color currColor = images[i].color;
                    currColor.a = alpha;
                    images[i].color = currColor;
                    images[i].texture = null;
                    btns[i].gameObject.name = EscapeRoomView.EMPTY;
                }
            }

        }
    }

}
