using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game
{

    public class InventoryListButton : MonoBehaviour
    {
        [SerializeField]
        Button btnItemList;

        [SerializeField]
        TMP_Text itemName, itemQuantity;

        InventoryView activeIV;
        GameObject itemPanel;

        public void Setup(ScriptableItem item, int quantity, InventoryView iv, GameObject itemDescriptionPanel, Button btnItem)
        {
            activeIV = iv;
            itemPanel = itemDescriptionPanel;
            itemName.text = item.itemNameLocalization;
            itemQuantity.text = quantity.ToString();
            
            if (item.GetItemType().Equals("Consumable"))
            {
                btnItemList.onClick.RemoveAllListeners();
                btnItemList.onClick.AddListener(delegate { OnItemClick(item, btnItem); });
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnItemClick(ScriptableItem item, Button btnTriggerItem)
        {
            itemPanel.SetActive(true);
            GameObject.Find("lblItemName").GetComponent<TMP_Text>().text = "i. " + item.infoDescriptionLocalized;
            GameObject.Find("lblDescription").GetComponent<TMP_Text>().text = item.bioDescriptionLocalized;
            GameObject.Find("btnCloseItemPanel").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("btnCloseItemPanel").GetComponent<Button>().onClick.AddListener(delegate () { activeIV.CloseItemPanel(); });
            btnTriggerItem.onClick.RemoveAllListeners();
            btnTriggerItem.onClick.AddListener(delegate { TriggerItem(item); });
            ModScrollPosition();
        }

        public void OnItemClick(ConsumableItem item, Button btnTriggerItem)
        {
            Debug.Log("Consumable " + item.itemNameLocalization);
            itemPanel.SetActive(true);
            GameObject.Find("lblItemName").GetComponent<TMP_Text>().text = item.infoDescriptionLocalized;
            GameObject.Find("lblDescription").GetComponent<TMP_Text>().text = item.bioDescriptionLocalized;
            GameObject.Find("btnCloseItemPanel").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("btnCloseItemPanel").GetComponent<Button>().onClick.AddListener(delegate () { activeIV.CloseItemPanel(); });
            btnTriggerItem.onClick.RemoveAllListeners();
            btnTriggerItem.onClick.AddListener(delegate { TriggerItem(item); });
            ModScrollPosition();
        }

        public void TriggerItem(ScriptableItem item)
        {
            item.TriggerItem();
            activeIV.LoadItemList();
            activeIV.CloseItemPanel();
        }

        void ModScrollPosition()
        {
            var scrollContDim = GameObject.Find("ItemsList").GetComponent<RectTransform>();

            //Modifico posizione box comparazione armi
            if (scrollContDim.rect.height > (GetMaxPageHeight() - 550))
            {
                GameObject.Find("ItemsList").GetComponent<RectTransform>().sizeDelta = new Vector2(scrollContDim.rect.width, (GetMaxPageHeight() - 550));
                var scrollPosition = GameObject.Find("ItemsList").GetComponent<RectTransform>().localPosition;
                GameObject.Find("ItemsList").GetComponent<RectTransform>().localPosition = new Vector2(scrollPosition.x, scrollPosition.y + 275);
            }
        }

        float GetMaxPageHeight()
        {
            return GameObject.Find("InventoryContainer").GetComponent<RectTransform>().rect.height;
        }

        public void CloseComparaison()
        {
            itemPanel.SetActive(false);
        }
    }
}