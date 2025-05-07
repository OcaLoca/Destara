using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Game
{
    public class OnPressedButton : MonoBehaviour
    {
        public bool buttonPressed;

        private void Update()
        {
            if (!buttonPressed) return;

            Debug.LogError("Is pressed");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            buttonPressed = true;
            Debug.Log("Is pressed");
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            buttonPressed = false;
        }
    }
}