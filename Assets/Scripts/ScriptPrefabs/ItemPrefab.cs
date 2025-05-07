using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using StarworkGC.Localization;


namespace Game
{
    public class ItemPrefab : MonoBehaviour
    {
        [SerializeField] TMP_Text itemName;
        [SerializeField] TMP_Text itemQuantity;
        [SerializeField] Button equipButton;

        private GameObject selectedItemPanel;
        private EquipmentView currentEquipmentView;
        private NewInventoryView currentInventoryView;
        private Button itemTriggerBtn;

        public Color grayColor;
        public Color redColor;

        //equippedLight

        public void SetupItemButton(ScriptableItem item, int quantity, GameObject SelecteditemPanelRef, Button itemTriggerButton, NewInventoryView inventoryView)
        {
            itemName.text =  Localization.Get(item.itemNameLocalization);
            itemQuantity.text = "x" + quantity.ToString();
            selectedItemPanel = SelecteditemPanelRef;
            itemTriggerBtn = itemTriggerButton;
            currentInventoryView = inventoryView;

            equipButton.gameObject.GetComponent<RawImage>().color = redColor;

            equipButton.interactable = true;

            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(delegate 
            { 
                OnItemClick(item, itemTriggerBtn); 
            });
        }

        public void SetupEmptyItem(NewInventoryView inventoryView)
        {
            itemName.text = "";
            itemQuantity.text = "";
            selectedItemPanel = null;
            itemTriggerBtn = null;
            currentInventoryView = inventoryView;

            equipButton.onClick.RemoveAllListeners();
            equipButton.gameObject.GetComponent<RawImage>().color = grayColor;
            equipButton.interactable = false;
        }

        void OnItemClick(ScriptableItem item, Button itemTriggerBtn)
        {
            Debug.Log("ITEM: " + item.itemType);
            string story = Localization.Get(item.GetLocalizedBio());
            string info = Localization.Get(item.GetLocalizedInfo());

            if (item.itemType == ScriptableItem.ItemType.Consumable)
            {
                currentInventoryView.ForceClosePanelAlarm();
                currentInventoryView.selectedItemDescription.text = story;
                currentInventoryView.selectedItemInfo.text = info;
            }
            else 
            {
                currentInventoryView.selectedCollectibleStoryItemBio.text = story;
                currentInventoryView.selectedCollectibleStoryItemInfo.text = info;
            }

            selectedItemPanel.SetActive(true);

            itemTriggerBtn.onClick.RemoveAllListeners();
            itemTriggerBtn.onClick.AddListener(delegate { OnClickInInventoryBook(item); });
        }

        void OnClickInInventoryBook(ScriptableItem item)
        {
            item.TriggerItem();
            selectedItemPanel.SetActive(false);
            currentInventoryView.OpenItemList(item.itemType);
        }
    }
}
