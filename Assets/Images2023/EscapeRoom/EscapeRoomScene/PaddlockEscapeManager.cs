/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    
    public class PaddlockEscapeManager : MonoBehaviour
    {
        public TMP_InputField InputField;
        [SerializeField] int rightKey;

        private void OnEnable()
        {
            InputField.onValueChanged.AddListener(delegate { CheckSolution(); });
        }

        void CheckSolution()
        {
            if(rightKey.ToString() == InputField.text)
            {
                ChangePaperUI();
            }
        }

        [SerializeField] GameObject layoutComponent;
        [SerializeField] GameObject DropItemAnimation;
        [SerializeField] DroppedItem tmpDroppedItem;
        [SerializeField] ScriptableItem item;
        [SerializeField] int quantity;
        bool firstTime = true;
        public void ChangePaperUI()
        {
            if (!firstTime) { return; }
            firstTime = false;
            DestroyFindedObj();
            DropItemAnimation.gameObject.SetActive(true);
            DroppedItem DroppedItem = Instantiate(tmpDroppedItem);
            DroppedItem.SetupItem(item, quantity = 1);
            DroppedItem.transform.SetParent(layoutComponent.transform);
            DroppedItem.transform.localPosition = new Vector3(0, 0, 0);
            DroppedItem.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            gameObject.SetActive(false);
        }

        public void DestroyFindedObj()
        {
            foreach (Transform child in layoutComponent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
