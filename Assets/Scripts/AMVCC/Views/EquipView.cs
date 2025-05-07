using SmartMVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Game {
    public class EquipView : View<GameApplication>
    {

        public Button btnStats;
        public Button btnLightWeapon;
        public Button btnBackEquipList;
        public Button btnEquip;
        public Button btnBackGame;
        public Button btnInventory;
        public List<Button> btnList = new List<Button>();
        public ScrollRect equipListScrollRect;
        GameObject equipTipeListContentObject, equipCenterListObject, equipComparePanel;
        Vector2 scrollBackupPosition, equipContanerBackupPosition;

        EquipView ew;

        [SerializeField] EquipListButton equipListButtonPrefab;
        [SerializeField] EquippedListButton equippedListButtonPrefab;

        void Start()
        {
            //Recupero varie dimensioni
            scrollBackupPosition = GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().localPosition;
            equipContanerBackupPosition = GameObject.Find("EquipContainer").GetComponent<RectTransform>().localPosition;

            btnStats.onClick.RemoveAllListeners();
            btnStats.onClick.AddListener(OnClickStats);
            //btnLightWeapon.onClick.RemoveAllListeners();
            //btnLightWeapon.onClick.AddListener(OnClickClearList);
            //btnBackEquipList.onClick.RemoveAllListeners();
            //btnBackEquipList.onClick.AddListener(OnClickReturnToEquipList);

            btnBackGame.onClick.RemoveAllListeners();
            btnBackGame.onClick.AddListener(OnClickBack);
            btnInventory.onClick.RemoveAllListeners();
            btnInventory.onClick.AddListener(OnClickInventory);

            //Setto e azzero canvas
            equipCenterListObject = GameObject.Find("EquipCenterList");
            equipTipeListContentObject = GameObject.Find("SingleEquipTypeList");
            equipTipeListContentObject.SetActive(false);
            equipComparePanel = GameObject.Find("WeaponComparePanel");
            equipComparePanel.SetActive(false);

            //TEST DIMENSIONE DINAMICA BOX EQUIP
            SetEquipListSpacing();

            ew = this;
        }

        private void OnEnable()
        {
            EquipmentListLoad();
            //Reset scroll vertical position
            equipListScrollRect.verticalNormalizedPosition = 1f;

            //test bottoni
            LoadEquipListButton();
        }

        //Custom functions

        void OnClickBack()
        {
            ResetActiveStatus();
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }
        void OnClickInventory()
        {
            ResetActiveStatus();
            Notify(MVCEvents.OPEN_INVENTORY_VIEW);
        }

        void OnClickStats()
        {
            ResetActiveStatus();
            Notify(MVCEvents.OPEN_STATS_VIEW);
        }

        void ResetActiveStatus()
        {
            equipCenterListObject.SetActive(true);
            equipTipeListContentObject.SetActive(false);
            equipComparePanel.SetActive(false);
        }

        void TestButton(string stringa)
        {
            Debug.Log(stringa);
        }

        void LoadEquipListButton()
        {

            for (int equipIndex = 1; equipIndex <= 9; equipIndex++)
            {
                string lblBottonList = "";
                string equipListSelectedName = "";
                switch (equipIndex)
                {
                    case 1:
                        lblBottonList = "Arma leggera";
                        equipListSelectedName = "lightWeaponList";
                        break;
                    case 2:
                        lblBottonList = "Arma a distanza";
                        equipListSelectedName = "rangeWeaponList";
                        break;
                    case 3:
                        lblBottonList = "Arma pesante";
                        equipListSelectedName = "heavyWeaponList";
                        break;
                    case 4:
                        lblBottonList = "Arma speciale";
                        equipListSelectedName = "specialWeaponList";
                        break;
                    case 5:
                        lblBottonList = "Elmo";
                        equipListSelectedName = "headEquipmentList";
                        break;
                    case 6:
                        lblBottonList = "Torso";
                        equipListSelectedName = "torsoEquipmentList";
                        break;
                    case 7:
                        lblBottonList = "Scudo";
                        equipListSelectedName = "shieldEquipmentList";
                        break;
                    case 8:
                        lblBottonList = "Accessorio 1";
                        equipListSelectedName = "accessory1List";
                        break;
                    case 9:
                        lblBottonList = "Accessorio 2";
                        equipListSelectedName = "accessory2List";
                        break;
                }
                //Cambio label al bottone
                GameObject.Find("Button" + equipIndex).GetComponentInChildren<Text>().text = lblBottonList;
                GameObject.Find("Button" + equipIndex).GetComponentInChildren<Text>().color = Color.black;
                //Imposto listener al bottone
                string intexString = equipIndex.ToString();
                GameObject.Find("btnEquipList" + equipIndex).GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("btnEquipList" + equipIndex).GetComponent<Button>().onClick.AddListener(delegate () { OnClickClearList(intexString, equipListSelectedName); });
            }
        }

        float GetMaxPageHeight(){
            return GameObject.Find("EquipContainer").GetComponent<RectTransform>().rect.height;
        }

        void SetEquipListSpacing()
        {
            //TEST GESTIONE DINAMICA SPAZI
            float scrollContDim = GameObject.Find("EquipContainer").GetComponent<RectTransform>().rect.height;
            //Debug.Log("Dimensione container = " + scrollContDim);
            float maxBoxHeight = scrollContDim / 9;
            //Debug.Log("Dimensione box (container / 9) = " + maxBoxHeight);
            float firstYPost = GameObject.Find("EquipListItem1").GetComponent<RectTransform>().localPosition.y;
            for (int i = 1; i <= 9; i++)
            {
                //setto posizione label a destra
                float newYPos = firstYPost - (maxBoxHeight * (i - 1));
                Vector2 position = GameObject.Find("EquipListItem" + i).GetComponent<RectTransform>().localPosition;
                //Debug.Log("Box" + i + " --> x = " + position.x + " // y = " + position.y + "  ---  nuova y = "+newYPos);
                GameObject.Find("EquipListItem" + i).GetComponent<RectTransform>().localPosition = new Vector2(position.x, newYPos);
                //setto posizione pulsanti a sinistra
                position = GameObject.Find("Button" + i).GetComponent<RectTransform>().localPosition;
                GameObject.Find("Button" + i).GetComponent<RectTransform>().localPosition = new Vector2(position.x, newYPos);
            }
        }

        void EquipmentListLoad()
        {
            for (int i = 1; i <= 9; i++)
            {
                GameObject.Find("ItemName" + i).GetComponent<TMP_Text>().text = GetEquipName(i);
                if (getRarityColour(i) != null)
                {
                    GameObject.Find("btnEquipList" + i).GetComponent<Image>().sprite = Resources.Load<Sprite>(getRarityColour(i));
                }
            }
        }

        public void OnClickClearList(string intexString, string equipListSelectedName)
        {
            int equipIndex = int.Parse(intexString);
            //Attivo canvas lista equip per tipo
            equipCenterListObject.SetActive(false);
            equipTipeListContentObject.SetActive(true);

            HideLeftButton(equipIndex);

            //Resetto posizione e dimensione scroll
            GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().localPosition = new Vector2(scrollBackupPosition.x, equipContanerBackupPosition.y);
            GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().sizeDelta = new Vector2(GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().rect.width, GetMaxPageHeight());
            var scrollContPosAnch = GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().anchoredPosition;

            //Svuoto lista
            foreach (Transform child in GameObject.Find("EquipTypeListContent").transform)
            {
                Destroy(child.gameObject);
            }

            //TEST DIMENSIONI DINAMICHE LISTA EQUIP 
            //float scrollContDim = GameObject.Find("EquipContainer").GetComponent<RectTransform>().rect.height;

            //Settaggio reflection liste di equip e weapon
            System.Reflection.FieldInfo reflectionEquipPointer = null;
            reflectionEquipPointer = PlayerManager.Singleton.GetType().GetField(equipListSelectedName);
            IEnumerable reflectionEquipList = (IEnumerable)reflectionEquipPointer.GetValue(PlayerManager.Singleton);

            //Cambio dimensione EQUIP TYPE LIST CONTENT
            float newEquipScrollHeight = (GetListSize(reflectionEquipList, equipIndex) + IsEquipped(equipIndex)) * 190;
            if (newEquipScrollHeight < GetMaxPageHeight()) {
                newEquipScrollHeight = GetMaxPageHeight();
            }
            RectTransform provaRect = GameObject.Find("EquipTypeListContent").GetComponent<RectTransform>();
            provaRect.sizeDelta = new Vector2(provaRect.rect.width, newEquipScrollHeight);
            //Imposto nuova posizione EQUIP TYPE LIST CONTENT
            var scrollContPosition = GameObject.Find("EquipTypeListContent").GetComponent<RectTransform>();
            scrollContPosition.anchoredPosition = new Vector2(scrollContPosition.anchoredPosition.x, newEquipScrollHeight);
            float newButtonYpos = -120;

            //TEST cambio colore primo pulsante colonna a SX per back armi

            //Creo elenco per tipo equipaggiamento
            if (equipIndex < 5) {
                if (IsEquipped(equipIndex) == 1)
                {
                    Weapon equipped = (Weapon)GetEquipped(equipIndex);
                    EquippedListButton equipItem = Instantiate(equippedListButtonPrefab);
                    equipItem.Setup(equipped);
                    equipItem.transform.SetParent(GameObject.Find("EquipTypeListContent").GetComponent<Canvas>().transform, false);
                    equipItem.transform.localPosition = new Vector2(scrollContPosAnch.x - 200, newButtonYpos);
                    newButtonYpos = newButtonYpos - 190;
                }
                foreach (Weapon weapon in reflectionEquipList)
                {
                    if (weapon) {
                        EquipListButton equipItem = Instantiate(equipListButtonPrefab);
                        equipItem.Setup(weapon, equipComparePanel, btnEquip, equipIndex, ew);
                        equipItem.transform.SetParent(GameObject.Find("EquipTypeListContent").GetComponent<Canvas>().transform, false);
                        //equipItem.transform.localPosition = new Vector2((rectEquip.x + scrollContPosAnch.x), (float)newButtonYpos);
                        equipItem.transform.localPosition = new Vector2(scrollContPosAnch.x - 200, newButtonYpos);
                        newButtonYpos = newButtonYpos - 190;
                    }
                }
            }
            else
            {
                if (IsEquipped(equipIndex) == 1)
                {
                    Equipment equipped = (Equipment)GetEquipped(equipIndex);
                    EquippedListButton equipItem = Instantiate(equippedListButtonPrefab);
                    equipItem.Setup(equipped);
                    equipItem.transform.SetParent(GameObject.Find("EquipTypeListContent").GetComponent<Canvas>().transform, false);
                    equipItem.transform.localPosition = new Vector2(scrollContPosAnch.x - 200, newButtonYpos);
                    newButtonYpos = newButtonYpos - 190;
                }
                foreach (Equipment equip in reflectionEquipList)
                {
                    if (equip)
                    {
                        EquipListButton equipItem = Instantiate(equipListButtonPrefab);
                        equipItem.Setup(equip, equipComparePanel, btnEquip, equipIndex, ew);
                        equipItem.transform.SetParent(GameObject.Find("EquipTypeListContent").GetComponent<Canvas>().transform, false);
                        //equipItem.transform.localPosition = new Vector2((rectEquip.x + scrollContPosAnch.x), (float)newButtonYpos);
                        equipItem.transform.localPosition = new Vector2(scrollContPosAnch.x - 200, newButtonYpos);
                        newButtonYpos = newButtonYpos - 190;
                    }
                }
            }

            //Reset scroll vertical position
            equipListScrollRect.verticalNormalizedPosition = 1f;
        }

        void OnClickReturnToEquipList() {
            equipCenterListObject.SetActive(true);
            equipTipeListContentObject.SetActive(false);
            equipComparePanel.SetActive(false);
            ShowLeftButton();
            LoadEquipListButton();
            EquipmentListLoad();
        }

        int GetListSize(IEnumerable lista, int index){
            int cont = 0;
            if (index < 5)
            {
                foreach (Weapon weapon in lista)
                {
                    cont++;
                }
            }
            else
            {
                foreach (Equipment equip in lista)
                {
                    cont++;
                }
            }
            return cont;
        }

        ScriptableItem GetEquipped(int pos)
        {
            switch (pos)
            {
                case 1:
                    return PlayerManager.Singleton.playerWeapon.lightWeapon;
                case 2:
                    return PlayerManager.Singleton.playerWeapon.rangeWeapon;
                case 3:
                    return PlayerManager.Singleton.playerWeapon.heavyWeapon;
                case 4:
                    return PlayerManager.Singleton.playerWeapon.specialWeapon;
                case 5:
                    return PlayerManager.Singleton.playerEquipment.equippedLightDefence;
                case 6:
                    return PlayerManager.Singleton.playerEquipment.equippedBalancedDefence;
                case 7:
                    return PlayerManager.Singleton.playerEquipment.equippedHeavyDefence;
                case 8:
                    return PlayerManager.Singleton.playerEquipment.equippedGemstone;
                case 9:
                    return PlayerManager.Singleton.playerEquipment.equippedTalisman;
                case 10:
                    return PlayerManager.Singleton.playerEquipment.equippedRelic;
                default:
                    return null;
            }
        }

        int IsEquipped(int pos)
        {
            switch (pos)
            {
                case 1:
                    if (PlayerManager.Singleton.playerWeapon.lightWeapon)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 2:
                    if (PlayerManager.Singleton.playerWeapon.rangeWeapon)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 3:
                    if (PlayerManager.Singleton.playerWeapon.heavyWeapon)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 4:
                    if (PlayerManager.Singleton.playerWeapon.specialWeapon)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 5:
                    if (PlayerManager.Singleton.playerEquipment.equippedLightDefence)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 6:
                    if (PlayerManager.Singleton.playerEquipment.equippedBalancedDefence)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 7:
                    if (PlayerManager.Singleton.playerEquipment.equippedHeavyDefence)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 8:
                    if (PlayerManager.Singleton.playerEquipment.equippedGemstone)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                case 9:
                    if (PlayerManager.Singleton.playerEquipment.equippedTalisman)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                case 10:
                    if (PlayerManager.Singleton.playerEquipment.equippedRelic)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                default:
                    return 0;

            }
        }

        string GetEquipName(int pos)
        {
            switch (pos)
            {
                case 1:
                    if (PlayerManager.Singleton.playerWeapon.lightWeapon)
                    {
                        return PlayerManager.Singleton.playerWeapon.lightWeapon.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 2:
                    if (PlayerManager.Singleton.playerWeapon.rangeWeapon)
                    {
                        return PlayerManager.Singleton.playerWeapon.rangeWeapon.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 3:
                    if (PlayerManager.Singleton.playerWeapon.heavyWeapon)
                    {
                        return PlayerManager.Singleton.playerWeapon.heavyWeapon.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 4:
                    if (PlayerManager.Singleton.playerWeapon.specialWeapon)
                    {
                        return PlayerManager.Singleton.playerWeapon.specialWeapon.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 5:
                    if (PlayerManager.Singleton.playerEquipment.equippedLightDefence)
                    {
                        return PlayerManager.Singleton.playerEquipment.equippedLightDefence.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 6:
                    if (PlayerManager.Singleton.playerEquipment.equippedBalancedDefence)
                    {
                        return PlayerManager.Singleton.playerEquipment.equippedBalancedDefence.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 7:
                    if (PlayerManager.Singleton.playerEquipment.equippedHeavyDefence)
                    {
                        return PlayerManager.Singleton.playerEquipment.equippedHeavyDefence.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 8:
                    if (PlayerManager.Singleton.playerEquipment.equippedGemstone)
                    {
                        return PlayerManager.Singleton.playerEquipment.equippedGemstone.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                case 9:
                    if (PlayerManager.Singleton.playerEquipment.equippedTalisman)
                    {
                        return PlayerManager.Singleton.playerEquipment.equippedTalisman.name;
                    }
                    else
                    {
                        return "Empty";
                    }
                case 10:
                    if (PlayerManager.Singleton.playerEquipment.equippedRelic)
                    {
                        return PlayerManager.Singleton.playerEquipment.equippedRelic.name;
                    }
                    else
                    {
                        return "Empty";
                    }

                default:
                    return "Empty";

            }
        }

        string getRarityColour(int pos)
        {
            ScriptableItem equip;
            string rarityColor = "";
            switch (pos)
            {
                case 1:
                    equip = PlayerManager.Singleton.playerWeapon.lightWeapon;
                    break;
                case 2:
                    equip = PlayerManager.Singleton.playerWeapon.rangeWeapon;
                    break;

                case 3:
                    equip = PlayerManager.Singleton.playerWeapon.heavyWeapon;
                    break;

                case 4:
                    equip = PlayerManager.Singleton.playerWeapon.specialWeapon;
                    break;

                case 5:
                    equip = PlayerManager.Singleton.playerEquipment.equippedLightDefence;
                    break;

                case 6:
                    equip = PlayerManager.Singleton.playerEquipment.equippedBalancedDefence;
                    break;
                case 7:
                    equip = PlayerManager.Singleton.playerEquipment.equippedHeavyDefence;
                    break;
                case 8:
                    equip = PlayerManager.Singleton.playerEquipment.equippedGemstone;
                    break;
                case 9:
                    equip = PlayerManager.Singleton.playerEquipment.equippedTalisman;
                    break;
                case 10:
                    equip = PlayerManager.Singleton.playerEquipment.equippedRelic;
                    break;
                default:
                    equip = null;
                    break;
            }

            if (equip)
            {
                switch (equip.rarity.ToString())
                {
                    case "Usual":
                        rarityColor = "Button/greenEquip";
                        break;
                    case "Inusual":
                        rarityColor = "Button/blueEquip";
                        break;
                    case "Rare":
                        rarityColor = "Button/purpleEquip";
                        break;
                    case "Legendary":
                        rarityColor = "Button/orangeEquip";
                        break;
                    default:
                        rarityColor = "Button/greyEquip";
                        break;
                }
                return rarityColor;
            }
            else
            {
                return "Button/greyEquip";
            }
        }

        void HideLeftButton(int i)
        {
            GameObject.Find("Button" + i).GetComponent<Image>().color = Color.red;
            GameObject.Find("Button" + i).GetComponentInChildren<Text>().text = "BACK";
            GameObject.Find("Button" + i).GetComponentInChildren<Text>().color = Color.white;
            GameObject.Find("Button" + i).GetComponent<Button>().onClick.RemoveAllListeners();
           GameObject.Find("Button" + i).GetComponent<Button>().onClick.AddListener(delegate () { OnClickReturnToEquipList(); });
        }

        void ShowLeftButton()
        {
            for (int i = 1; i <= 9; i++)
            {
                GameObject.Find("Button" + i).GetComponent<Image>().color = Color.white;
            }
        }
    }
}