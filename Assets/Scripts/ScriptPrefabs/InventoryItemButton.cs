using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InventoryItemButton : MonoBehaviour
    {
        [SerializeField]
        Button itemButton;

        [SerializeField]
        TMP_Text lblQuantity;

        [SerializeField]
        TMP_Text lblItemName;

        [SerializeField]
        Image imgItem;

        void OnEnable()
        {
            itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(delegate { OnItemClick(); });
        }

        public void Setup(ScriptableItem itemData, int quantity)
        {
            //imgItem = SET;
            lblQuantity.text = "x" + quantity.ToString();
            lblItemName.text = itemData.itemNameLocalization;
            if(Localization.Get(itemData.itemNameLocalization) != string.Empty)
            {
                lblItemName.text = Localization.Get(itemData.itemNameLocalization);
            }
        }

        void OnItemClick()
        {
            //TODO
        }
    }
}

