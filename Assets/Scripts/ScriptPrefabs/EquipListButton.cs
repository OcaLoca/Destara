using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game
{
    public class EquipListButton : MonoBehaviour
    {

        [SerializeField]
        Button btnEquipList;

        [SerializeField]
        TMP_Text itemName;

        Vector2 scrollBackupPosition, equipContanerBackupPosition;

        GameObject equipComparePanelBackup;

        public EquipView ew;

        //Setup per Weapon
        public void Setup(Weapon equipData, GameObject equipComparePanel, Button btnEquip, int index, EquipView eqV)
        {
            BackupPosition();
            itemName.text = equipData.name;
            setEquipRarity(btnEquipList, equipData);
            btnEquipList.onClick.RemoveAllListeners();
            btnEquipList.onClick.AddListener(delegate { OnItemClick(equipData, equipComparePanel, btnEquip, index); });
            equipComparePanelBackup = equipComparePanel;
            ew = eqV;
        }
        //Setup per Equip
        public void Setup(Equipment equipData, GameObject equipComparePanel, Button btnEquip, int index, EquipView eqV)
        {
            BackupPosition();
            itemName.text = equipData.name;
            setEquipRarity(btnEquipList, equipData);
            btnEquipList.onClick.RemoveAllListeners();
            btnEquipList.onClick.AddListener(delegate { OnItemClick(equipData, equipComparePanel, btnEquip, index); });
            ew = eqV;
        }
        //Backup posizioni utili
        public void BackupPosition()
        {
            scrollBackupPosition = GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().localPosition;
            equipContanerBackupPosition = GameObject.Find("EquipContainer").GetComponent<RectTransform>().localPosition;
        }

        //Per WEAPON
        void OnItemClick(Weapon equip, GameObject equipComparePanel, Button btnEquip, int index)
        {
            //Attivo pulsante per equipaggiare nuova arma
            btnEquip.onClick.RemoveAllListeners();
            btnEquip.onClick.AddListener(delegate { EquipItem(equip, index); });
            equipComparePanel.SetActive(true);
            setCompare(0);
            GameObject.Find("btnCloseCompare").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("btnCloseCompare").GetComponent<Button>().onClick.AddListener(delegate () { CloseComparaison(); });
            GameObject.Find("lblEquipCompareName").GetComponent<TMP_Text>().text = equip.name;
            GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().text = equip.damage.ToString();
            GameObject.Find("lblEquipBuff").GetComponent<TMP_Text>().text = "";
            GameObject.Find("lblEquipElement").GetComponent<TMP_Text>().text = "";
            Weapon equippdetItem = (Weapon)GetEquippedItem(index);
            if (equippdetItem)
            {
                GameObject.Find("lblEquippedName").GetComponent<TMP_Text>().text = equippdetItem.name;
                GameObject.Find("lblEquippedDamage").GetComponent<TMP_Text>().text = equippdetItem.damage.ToString();
                //Cambio colore danno arma comparata
                if (equippdetItem.damage < equip.damage)
                {
                    GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    if (equippdetItem.damage > equip.damage)
                    {
                        GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().color = Color.red;
                    }
                    else
                    {
                        GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().color = Color.white;
                    }
                }
                GameObject.Find("lblEquippedBuff").GetComponent<TMP_Text>().text = "";
                GameObject.Find("lblEquippedElement").GetComponent<TMP_Text>().text = "";
            }
            else
            {
                GameObject.Find("lblEquippedName").GetComponent<TMP_Text>().text = "Empty";
                GameObject.Find("lblEquippedDamage").GetComponent<TMP_Text>().text = "";
                GameObject.Find("lblEquippedBuff").GetComponent<TMP_Text>().text = "";
                GameObject.Find("lblEquippedElement").GetComponent<TMP_Text>().text = "";
            }

            ModScrollPosition();
        }
        //Per EQUIPMENT
        void OnItemClick(Equipment equip, GameObject equipComparePanel, Button btnEquip, int index)
        {
            //Attivo pulsante per equipaggiare nuova arma
            btnEquip.onClick.RemoveAllListeners();
            btnEquip.onClick.AddListener(delegate { EquipItem(equip, index); });
            equipComparePanel.SetActive(true);
            equipComparePanelBackup = equipComparePanel;
            setCompare(1);
            GameObject.Find("btnCloseCompare").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("btnCloseCompare").GetComponent<Button>().onClick.AddListener(delegate () { CloseComparaison(); });
            GameObject.Find("lblEquipCompareName").GetComponent<TMP_Text>().text = equip.name;
            GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().text = equip.defence.ToString();
            GameObject.Find("lblEquipBuff").GetComponent<TMP_Text>().text = "";
            GameObject.Find("lblEquipElement").GetComponent<TMP_Text>().text = "";
            Equipment equippdetItem = (Equipment)GetEquippedItem(index);
            if (equippdetItem)
            {
                GameObject.Find("lblEquippedName").GetComponent<TMP_Text>().text = equippdetItem.name;
                GameObject.Find("lblEquippedDamage").GetComponent<TMP_Text>().text = equippdetItem.defence.ToString();
                if (equippdetItem.defence < equip.defence)
                {
                    GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    if (equippdetItem.defence > equip.defence)
                    {
                        GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().color = Color.red;
                    }
                    else
                    {
                        GameObject.Find("lblEquipDamage").GetComponent<TMP_Text>().color = Color.white;
                    }
                }
                GameObject.Find("lblEquippedDamage").GetComponent<TMP_Text>().text = "";
                GameObject.Find("lblEquippedBuff").GetComponent<TMP_Text>().text = "";
                GameObject.Find("lblEquippedElement").GetComponent<TMP_Text>().text = "";
            }
            else
            {
                GameObject.Find("lblEquippedName").GetComponent<TMP_Text>().text = "Empty";
                GameObject.Find("lblEquippedDamage").GetComponent<TMP_Text>().text = "";
                GameObject.Find("lblEquippedBuff").GetComponent<TMP_Text>().text = "";
                GameObject.Find("lblEquippedElement").GetComponent<TMP_Text>().text = "";
            }

            ModScrollPosition();            
        }
        void ModScrollPosition()
        {
            //Modifico dimensioni scroll rect
            var scrollContDim = GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>();
            //var scrollContPos = GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().position;
            //var scrollContPosAnch = GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().anchoredPosition;

            //Modifico posizione box comparazione armi
            if (scrollContDim.rect.height > (GetMaxPageHeight() - 550))
            {
                GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().sizeDelta = new Vector2(scrollContDim.rect.width, (GetMaxPageHeight() - 550));
                var scrollPosition = GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().localPosition;
                GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().localPosition = new Vector2(scrollPosition.x, scrollPosition.y + 275);
            }
        }

        void setCompare(int value)
        {
            //da fare in inglese
            if (value == 0)
            {
                GameObject.Find("lblEquipped1").GetComponent<TMP_Text>().text = "Atk";
                GameObject.Find("lblEquipped2").GetComponent<TMP_Text>().text = "Buff";
                GameObject.Find("lblEquipped3").GetComponent<TMP_Text>().text = "Elem";
                GameObject.Find("lblEquip1").GetComponent<TMP_Text>().text = "Atk";
                GameObject.Find("lblEquip2").GetComponent<TMP_Text>().text = "Buff";
                GameObject.Find("lblEquip3").GetComponent<TMP_Text>().text = "Elem";
            }
            else
            {
                GameObject.Find("lblEquipped1").GetComponent<TMP_Text>().text = "Def";
                GameObject.Find("lblEquipped2").GetComponent<TMP_Text>().text = "Buff";
                GameObject.Find("lblEquipped3").GetComponent<TMP_Text>().text = "Elem";
                GameObject.Find("lblEquip1").GetComponent<TMP_Text>().text = "Def";
                GameObject.Find("lblEquip2").GetComponent<TMP_Text>().text = "Buff";
                GameObject.Find("lblEquip3").GetComponent<TMP_Text>().text = "Elem";
            }
        }

        //Per WEAPON
        void EquipItem (Weapon equip, int index)
        {
            switch (index)
            {
                case 1:
                    PlayerManager.Singleton.lightWeaponList.Add(PlayerManager.Singleton.playerWeapon.lightWeapon);
                    PlayerManager.Singleton.playerWeapon.lightWeapon = equip;
                    //PlayerManager.Singleton.GetEquippedWeaponID[0] = equip.ID;
                    PlayerManager.Singleton.lightWeaponList.Remove(equip);
                    break;
                case 2:
                    PlayerManager.Singleton.rangeWeaponList.Add(PlayerManager.Singleton.playerWeapon.rangeWeapon);
                    PlayerManager.Singleton.playerWeapon.rangeWeapon = equip;
                    //PlayerManager.Singleton.GetEquippedWeaponID[1] = equip.ID;
                    PlayerManager.Singleton.rangeWeaponList.Remove(equip);
                    break;
                case 3:
                    PlayerManager.Singleton.heavyWeaponList.Add(PlayerManager.Singleton.playerWeapon.heavyWeapon);
                    PlayerManager.Singleton.playerWeapon.heavyWeapon = equip;
                    //PlayerManager.Singleton.GetEquippedWeaponID[2] = equip.ID;
                    PlayerManager.Singleton.heavyWeaponList.Remove(equip);
                    break;
                case 4:
                    PlayerManager.Singleton.specialWeaponList.Add(PlayerManager.Singleton.playerWeapon.specialWeapon);
                    PlayerManager.Singleton.playerWeapon.specialWeapon = equip;
                    //PlayerManager.Singleton.GetEquippedWeaponID[3] = equip.ID;
                    PlayerManager.Singleton.specialWeaponList.Remove(equip);
                    break;
            }
            ew.OnClickClearList(index.ToString(), ListSelectedName(index));
            CloseComparaison();
        }
        //Per EQUIP
        void EquipItem(Equipment equip, int index)
        {
            switch (index)
            {
                case 5:
                    PlayerManager.Singleton.lightDefenceList.Add(PlayerManager.Singleton.playerEquipment.equippedLightDefence);
                    PlayerManager.Singleton.playerEquipment.equippedLightDefence = equip;
                    PlayerManager.Singleton.lightDefenceList.Remove(equip);
                    break;
                case 6:
                    PlayerManager.Singleton.balancedDefenceList.Add(PlayerManager.Singleton.playerEquipment.equippedBalancedDefence);
                    PlayerManager.Singleton.playerEquipment.equippedBalancedDefence = equip;
                    PlayerManager.Singleton.balancedDefenceList.Remove(equip);
                    break;
                case 7:
                    PlayerManager.Singleton.heavyDefenceList.Add(PlayerManager.Singleton.playerEquipment.equippedHeavyDefence);
                    PlayerManager.Singleton.playerEquipment.equippedHeavyDefence = equip;
                    PlayerManager.Singleton.heavyDefenceList.Remove(equip);
                    break;
                case 8:
                    PlayerManager.Singleton.talismansList.Add(PlayerManager.Singleton.playerEquipment.equippedTalisman);
                    PlayerManager.Singleton.playerEquipment.equippedTalisman = equip;
                    PlayerManager.Singleton.talismansList.Remove(equip);
                    break;
                case 9:
                    PlayerManager.Singleton.relicsList.Add(PlayerManager.Singleton.playerEquipment.equippedRelic);
                    PlayerManager.Singleton.playerEquipment.equippedRelic = equip;
                    PlayerManager.Singleton.relicsList.Remove(equip);
                    break;
                case 10:
                    PlayerManager.Singleton.gemstonesList.Add(PlayerManager.Singleton.playerEquipment.equippedGemstone);
                    PlayerManager.Singleton.playerEquipment.equippedGemstone = equip;
                    PlayerManager.Singleton.gemstonesList.Remove(equip);
                    break;
            }
            ew.OnClickClearList(index.ToString(), ListSelectedName(index));
            CloseComparaison();
        }

        ScriptableItem GetEquippedItem(int pos)
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

        void CloseComparaison()
        {
            equipComparePanelBackup.SetActive(false);
            GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().localPosition = new Vector2(scrollBackupPosition.x, equipContanerBackupPosition.y);
            GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().sizeDelta = new Vector2(GameObject.Find("SingleEquipTypeList").GetComponent<RectTransform>().rect.width, GetMaxPageHeight());
        }

        float GetMaxPageHeight(){
            return GameObject.Find("EquipContainer").GetComponent<RectTransform>().rect.height;
        }
        string ListSelectedName(int equipIndex)
        {
            string equipListSelectedName = "";
            switch (equipIndex)
            {
                case 1:
                    equipListSelectedName = "lightWeaponList";
                    break;
                case 2:
                    equipListSelectedName = "rangeWeaponList";
                    break;
                case 3:
                    equipListSelectedName = "heavyWeaponList";
                    break;
                case 4:
                    equipListSelectedName = "specialWeaponList";
                    break;
                case 5:
                    equipListSelectedName = "headEquipmentList";
                    break;
                case 6:
                    equipListSelectedName = "torsoEquipmentList";
                    break;
                case 7:
                    equipListSelectedName = "shieldEquipmentList";
                    break;
                case 8:
                    equipListSelectedName = "accessory1List";
                    break;
                case 9:
                    equipListSelectedName = "accessory2List";
                    break;
            }
            return equipListSelectedName;
        }

        void setEquipRarity(Button bottone, ScriptableItem equip)
        {
            Debug.Log(equip.rarity);
            switch (equip.rarity.ToString())
            {
                case "Usual":
                    bottone.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/greenEquip");
                    break;
                case "Inusual":
                    bottone.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/blueEquip");
                    break;
                case "Rare":
                    bottone.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/purpleEquip");
                    break;
                case "Legendary":
                    bottone.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/orangeEquip");
                    break;
                default:
                    break;
            }
        }
    }

}