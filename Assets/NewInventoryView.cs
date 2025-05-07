using SmartMVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using StarworkGC.Localization;

namespace Game
{
    public class NewInventoryView : View<GameApplication>
    {
        //Navigation buttons
        public Button backButton;
        public Button closeItemListPanel;
        public Button itemTriggerButton;

        //Category type buttons
        public Button consumblesButton;
        public Button storyButton;
        public Button collectiblesButton;

        private ScriptableItem selectedItem;

        [SerializeField] private GameObject itemListPanel;
        [SerializeField]
        public GameObject consumableBorderContainer, storyCollectibleBorderContainer,
            consumableInfoItemContainer, alarmPanelPlayerCantUseObject, storyCollectibleInfoItemContainer;

        [SerializeField] private Transform itemListContainer;
        [SerializeField] private EquipPrefab equipItemPrefab;
        [SerializeField] private ItemPrefab itemPrefab;
        //[SerializeField] private TMP_Text selectedItemNameLabel;
        [SerializeField] public TMP_Text selectedItemDescription, selectedCollectibleStoryItemBio;
        [SerializeField] public TMP_Text selectedItemInfo, selectedCollectibleStoryItemInfo;
        [SerializeField] public TMP_Text txtPlayerCantUseObject;
        [SerializeField] private TMP_Text categoryTitleLabel;

        // Start is called before the first frame update
        void Start()
        {
            //Navigation buttons
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnClickBack);

            closeItemListPanel.onClick.RemoveAllListeners();
            closeItemListPanel.onClick.AddListener(CloseItemList);
            itemTriggerButton.onClick.RemoveAllListeners();

            //Category type buttons
            consumblesButton.onClick.RemoveAllListeners();
            consumblesButton.onClick.AddListener(delegate
            {
                ForceClosePanelAlarm();
                OpenItemList(ScriptableItem.ItemType.Consumable);
            });
            storyButton.onClick.RemoveAllListeners();
            storyButton.onClick.AddListener(delegate
            {
                ForceClosePanelAlarm();
                OpenItemList(ScriptableItem.ItemType.Story);
            });
            collectiblesButton.onClick.RemoveAllListeners();
            collectiblesButton.onClick.AddListener(delegate
            {
                OpenItemList(ScriptableItem.ItemType.Collectible);
                ForceClosePanelAlarm();
            });

            itemListPanel.SetActive(false);
            consumableInfoItemContainer.SetActive(false);

            selectedItem = null;
        }

        void OnClickBack()
        {
            Notify(MVCEvents.OPEN_STATS_VIEW);
        }

        public void OpenItemList(ScriptableItem.ItemType itemType)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);

            TurnOffAllUIElement();

            itemListPanel.SetActive(true);
            //objSecondBottomButtons.gameObject.SetActive(true);
            DeleteAllListChildren();

            //Change label title by type
            switch (itemType)
            {
                case ScriptableItem.ItemType.Collectible:
                    categoryTitleLabel.text = Localization.Get("Collectible");
                    SetCollectibleAndStoryElement();
                    break;
                case ScriptableItem.ItemType.Story:
                    categoryTitleLabel.text = Localization.Get("Story");
                    SetCollectibleAndStoryElement();
                    break;
                case ScriptableItem.ItemType.Consumable:
                    itemTriggerButton.gameObject.SetActive(true);
                    categoryTitleLabel.text = Localization.Get("Consumable");
                    consumableBorderContainer.SetActive(true);
                    consumableInfoItemContainer.gameObject.SetActive(true);
                    break;
            }

            //controlliamo quanti item consumbili abbiamo per poi creare i campi vuoti
            int nItems = 0;
            int nItemsToShow = 6;

            foreach (KeyValuePair<ScriptableItem, int> entry in PlayerManager.Singleton.GetInventoryItems())
            {
                if (entry.Value > 0 && entry.Key.itemType == itemType)
                {
                    ItemPrefab item = Instantiate(itemPrefab, itemListContainer);
                    item.SetupItemButton(entry.Key, entry.Value, consumableInfoItemContainer, itemTriggerButton, this);
                }
            }

            for (int i = nItems; i < nItemsToShow; i++) //genera i tasti fake vuoti
            {
                ItemPrefab item = Instantiate(itemPrefab, itemListContainer);
                item.SetupEmptyItem(this);
            }
        }

        void TurnOffAllUIElement()
        {
            consumableBorderContainer.SetActive(false);
            storyCollectibleBorderContainer.SetActive(false);
            storyCollectibleInfoItemContainer.gameObject.SetActive(false);
            consumableInfoItemContainer.gameObject.SetActive(false);
            selectedCollectibleStoryItemInfo.text = string.Empty;
            selectedCollectibleStoryItemBio.text = string.Empty;
            selectedItemInfo.text = string.Empty;
            selectedItemDescription.text = string.Empty;

        }
        void SetCollectibleAndStoryElement()
        {
            itemTriggerButton.gameObject.SetActive(false);
            storyCollectibleBorderContainer.SetActive(true);
            storyCollectibleInfoItemContainer.gameObject.SetActive(true);
        }

        void CloseItemList()
        {
            itemListPanel.SetActive(false);
        }

        void DeleteAllListChildren()
        {
            for (int i = itemListContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(itemListContainer.GetChild(i).gameObject);
            }
        }

        public void ShowPanelCantUseObj(string txtAlert)
        {
            panelAlarm = StartCoroutine(PlayerCantUseObject(txtAlert));
        }

        Coroutine panelAlarm;
        public IEnumerator PlayerCantUseObject(string txtAlert)
        {
            alarmPanelPlayerCantUseObject.SetActive(true);
            txtPlayerCantUseObject.text = txtAlert;
            yield return new WaitForSeconds(2);

            alarmPanelPlayerCantUseObject.SetActive(false);
        }

        public void ForceClosePanelAlarm()
        {
            if (panelAlarm != null)
            {
                StopCoroutine(panelAlarm);
            }
            alarmPanelPlayerCantUseObject.SetActive(false);

        }

    }

}