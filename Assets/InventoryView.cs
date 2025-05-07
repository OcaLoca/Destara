using SmartMVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Game
{
    public class InventoryView : View<GameApplication>
    {
        public Button btnStats;
        public Button btnEquip;
        public Button btnTriggerItem;
        public Button btnBackGame;
        public Button testItem, testItem2;
        GameObject inventoryContainer;
        GameObject itemCenterListObject;
        int activeCategory;
        InventoryView activeIV;

        [SerializeField]
        GameObject itemPanel;

        Vector2 scrollBackupPosition, inventoryContanerBackupPosition;

        [SerializeField] InventoryListButton inventoryListButtonPrefab;
        
        [SerializeField]
         AudioClip audioClickOnEquip;
         [SerializeField]
         AudioClip audioClickOnStats;
         [SerializeField]
         AudioClip audioClickOnOpenItemPanel;
         [SerializeField]  
         AudioClip audioClickOnCloseItemPanel;


        //InventoryListItem
        void Start()
        {
            activeCategory = 1;
            activeIV = this;

            //Recupero varie dimensioni
            scrollBackupPosition = GameObject.Find("ItemsList").GetComponent<RectTransform>().localPosition;
            inventoryContanerBackupPosition = GameObject.Find("InventoryContainer").GetComponent<RectTransform>().localPosition;
            //itemPanel = GameObject.Find("SelectedItemPanel");
            inventoryContainer = GameObject.Find("InventoryContainer");

            btnStats.onClick.RemoveAllListeners();
            btnStats.onClick.AddListener(OnClickStats);
            btnBackGame.onClick.RemoveAllListeners();
            btnBackGame.onClick.AddListener(OnClickBack);
            btnEquip.onClick.RemoveAllListeners();
            btnEquip.onClick.AddListener(OnClickEquip);

            //test item
            /*
            testItem.onClick.RemoveAllListeners();
            testItem.onClick.AddListener(OpenItemPanel);
            testItem2.onClick.RemoveAllListeners();
            testItem2.onClick.AddListener(CloseItemPanel);
            */
            
        }

        private void OnEnable()
        {
            activeIV = this;
            //itemPanel = GameObject.Find("SelectedItemPanel");
            LoadCategoryList(1);
        }

        void OnClickBack()
        {
            OpenItemPanel();
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
        }
        void OnClickEquip()
        {
            OpenItemPanel();
            Notify(MVCEvents.OPEN_EQUIP_VIEW);
            UISoundManager.Singleton.PlayAudioClip(audioClickOnEquip);
        }

        void OnClickStats()
        {
            OpenItemPanel();
            Notify(MVCEvents.OPEN_STATS_VIEW);
            UISoundManager.Singleton.PlayAudioClip(audioClickOnStats);
        }

        void OpenItemPanel()
        {
            itemPanel.SetActive(true);
            UISoundManager.Singleton.PlayAudioClip(audioClickOnOpenItemPanel);
        }

        public void CloseItemPanel() {
            if (itemPanel)
            {
                itemPanel.SetActive(false);
                GameObject.Find("ItemsList").GetComponent<RectTransform>().localPosition = new Vector2(scrollBackupPosition.x, inventoryContanerBackupPosition.y);
                GameObject.Find("ItemsList").GetComponent<RectTransform>().sizeDelta = new Vector2(GameObject.Find("ItemsList").GetComponent<RectTransform>().rect.width, GetMaxPageHeight());
            }
            UISoundManager.Singleton.PlayAudioClip(audioClickOnCloseItemPanel);
        }

        void LoadCategoryList(int categoryToLoad)
        {
            //Debug.Log("Carico categoria "+categoryToLoad);
            activeCategory = categoryToLoad;
            for (int i = 0; i < 3; i++)
            {
                int categoryIndex = i + 1;
                string lblBottonList = "";
                string equipListSelectedName = "";
                switch (categoryIndex)
                {
                    case 1:
                        lblBottonList = "Consum";
                        equipListSelectedName = "item1";
                        break;
                    case 2:
                        lblBottonList = "Story";
                        equipListSelectedName = "item2";
                        break;
                    case 3:
                        lblBottonList = "Collez";
                        equipListSelectedName = "item3";
                        break;
                }

                //Cambio label al bottone della lista e imposto listener
                GameObject.Find("Button" + categoryIndex).GetComponentInChildren<Text>().text = lblBottonList;
                if(activeCategory == categoryIndex){
                    Color temp = GameObject.Find("Button" + categoryIndex).GetComponent<Button>().image.color;
                    temp.a = 1f;
                    GameObject.Find("Button" + categoryIndex).GetComponent<Button>().image.color = temp;
                }
                else{
                    Color temp = GameObject.Find("Button" + categoryIndex).GetComponent<Button>().image.color;
                    temp.a = 0.5f;
                    GameObject.Find("Button" + categoryIndex).GetComponent<Button>().image.color = temp;
                    GameObject.Find("Button" + categoryIndex).GetComponent<Button>().onClick.RemoveAllListeners();
                    GameObject.Find("Button" + categoryIndex).GetComponent<Button>().onClick.AddListener(delegate () { LoadCategoryList(categoryIndex); });
                }
                activeCategory = categoryToLoad;
                CloseItemPanel();
                LoadItemList();
                //Imposto listener al bottone dell'item
                //GameObject.Find("btnItemList" + categoryIndex).GetComponent<Button>().onClick.RemoveAllListeners();
                //GameObject.Find("btnItemList" + categoryIndex).GetComponent<Button>().onClick.AddListener(delegate () { OnClickOpenItemPanel(categoryIndex, equipListSelectedName); });
            }
        }

        public void LoadItemList()
        {
            int categoryToLoad = activeCategory;
            //Svuoto lista
            foreach (Transform child in GameObject.Find("ItemListContent").transform)
            {
                Destroy(child.gameObject);
            }

            //Resetto posizione e dimensione scroll
            GameObject.Find("ItemsList").GetComponent<RectTransform>().localPosition = new Vector2(inventoryContanerBackupPosition.x + 130, inventoryContanerBackupPosition.y);
            GameObject.Find("ItemsList").GetComponent<RectTransform>().sizeDelta = new Vector2(GameObject.Find("ItemsList").GetComponent<RectTransform>().rect.width, GetMaxPageHeight());
            var scrollContPosAnch = GameObject.Find("ItemsList").GetComponent<RectTransform>().anchoredPosition;

            //Cambio dimensione EQUIP TYPE LIST CONTENT
            float newItemScrollHeight = (GetListSize(categoryToLoad)) * 190;
            if (newItemScrollHeight < GetMaxPageHeight())
            {
                newItemScrollHeight = GetMaxPageHeight();
            }
            RectTransform provaRect = GameObject.Find("ItemListContent").GetComponent<RectTransform>();
            provaRect.sizeDelta = new Vector2(provaRect.rect.width, newItemScrollHeight);
            //provaRect.sizeDelta = new Vector2((GameObject.Find("ItemsList").GetComponent<RectTransform>().rect.width - GameObject.Find("InventoryLeftList").GetComponent<RectTransform>().rect.width), newItemScrollHeight);
            //Imposto nuova posizione EQUIP TYPE LIST CONTENT
            var scrollContPosition = GameObject.Find("ItemListContent").GetComponent<RectTransform>();
            scrollContPosition.anchoredPosition = new Vector2(scrollContPosition.anchoredPosition.x, newItemScrollHeight);
            float newButtonYpos = -120;

            //Aggiunto item alla lista
            foreach (KeyValuePair<ScriptableItem, int> entry in PlayerManager.Singleton.GetInventoryItems())
            {
                if (entry.Value > 0) 
                {
                    if (categoryToLoad == EnumItemTypeToCategoryInt(entry.Key.itemType.ToString()))
                    {
                        InventoryListButton equipItem = Instantiate(inventoryListButtonPrefab);
                        equipItem.Setup(entry.Key, entry.Value, activeIV, itemPanel, btnTriggerItem);
                        equipItem.transform.SetParent(GameObject.Find("ItemListContent").GetComponent<Canvas>().transform, false);
                        equipItem.transform.localPosition = new Vector2(scrollContPosAnch.x - 250, newButtonYpos);
                        newButtonYpos = newButtonYpos - 190;
                    }
                }
            }
        }

        public void OnClickOpenItemPanel(int indexString, string equipListSelectedName)
        { 

            Debug.Log("Prova " + indexString);

            Dictionary<ScriptableItem, int> inventoryItems = PlayerManager.Singleton.GetInventoryItems();

            int cont = 0;
            foreach (KeyValuePair<ScriptableItem,int> entry in inventoryItems)
            {
                cont++;
                Debug.Log(cont + " -> " + entry.Key.itemNameLocalized + " : "+ entry.Key.itemType +" #" + entry.Value);
            }
        }

        float GetMaxPageHeight()
        {
            return GameObject.Find("InventoryContainer").GetComponent<RectTransform>().rect.height;
        }

        int GetListSize(int categoryToLoad)
        {
            Dictionary<ScriptableItem, int> inventoryItems = PlayerManager.Singleton.GetInventoryItems();
            int cont = 0;
            foreach (KeyValuePair<ScriptableItem, int> entry in inventoryItems)
            {
                if (categoryToLoad == EnumItemTypeToCategoryInt(entry.Key.itemType.ToString()))
                {
                    cont++;
                }
            }
            return cont;
        }

        int EnumItemTypeToCategoryInt(string value) {
            switch (value)
            {
                case "Consumable":
                    return 1;
                case "Story":
                    return 2;
                case "Collectible":
                    return 3;
            }
            return 0;        
        }

    }
}