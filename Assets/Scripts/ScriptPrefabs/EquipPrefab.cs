using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarworkGC.Localization;
//using static UnityEditor.Progress;

namespace Game
{
    public class EquipPrefab : MonoBehaviour
    {
        [SerializeField] Button equipButton;
        public Texture equippedTexture;
        public Color32 test;
        public RawImage equipPanelRI;
        public Image DamageIcon;
        public Image DefenceIcon;

        [SerializeField] public TMP_Text level;
        [SerializeField] TMP_Text equipName;
        [SerializeField] TMP_Text equipStrenght;
        [SerializeField] TMP_Text txtStaminaCost;

        public Button setNewItemBtn;

        [SerializeField] private GameObject equipComparePanel;
        [SerializeField] private GameObject selectedItemPanel;
        [SerializeField] private ComparePanel comparedPanelPrefab;
        [SerializeField] private ComparePanel equippedPanelPrefab;
        [SerializeField] private EquipmentView currentEquipmentView;
        [SerializeField] private NewInventoryView currentInventoryView;
        private Button itemTriggerBtn;

        //equippedLight
        [SerializeField] public GameObject equippedLight;
        public void SetupNotEquppedArmorOrShield(Equipment equip, GameObject equipComparePanelRef, ComparePanel equippedItemPanel, ComparePanel comparedItemPanel, Button setNewEquippedBtn, EquipmentView equipmentView = null)
        {
            equipName.text = equip.GetLocalizedObjName();

            SetLevel(equip.level);

            setNewItemBtn = setNewEquippedBtn;
            DamageIcon.gameObject.SetActive(false);
            DefenceIcon.gameObject.SetActive(false);
            equipStrenght.gameObject.SetActive(false);
            txtStaminaCost.gameObject.SetActive(false);

            if ((equip.equipPlaceType != Equipment.EquipPlaceType.Talisman) && (equip.equipPlaceType != Equipment.EquipPlaceType.GemStone) && (equip.equipPlaceType != Equipment.EquipPlaceType.Relic))
            {
                equipStrenght.gameObject.SetActive(true);
                equipStrenght.text = equip.defence.ToString();
                DefenceIcon.gameObject.SetActive(true);
                txtStaminaCost.gameObject.SetActive(true);
            }

            SetRarityColor(equip.rarity);
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(delegate { OnArmorOrShieldItemClick(equip); });

            equipComparePanel = equipComparePanelRef;
            comparedPanelPrefab = comparedItemPanel;
            equippedPanelPrefab = equippedItemPanel;

            currentEquipmentView = equipmentView;
        }

        public void SetupNotEquippedWeapon(Weapon equip, GameObject equipComparePanelRef, ComparePanel equippedItemPanel, ComparePanel comparedItemPanel, Button setNewEquippedBtn, EquipmentView equipmentView)
        {
            equipName.text = equip.GetLocalizedObjName();
            setNewItemBtn = setNewEquippedBtn;
            equipStrenght.text = equip.weaponConstDamage.ToString();
            SetLevel(equip.level);

            DamageIcon.gameObject.SetActive(true);
            DefenceIcon.gameObject.SetActive(false);

            SetRarityColor(equip.rarity);

            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(delegate { OnWeaponClicked(equip); });

            equipComparePanel = equipComparePanelRef;
            comparedPanelPrefab = comparedItemPanel;
            equippedPanelPrefab = equippedItemPanel;

            currentEquipmentView = equipmentView;
        }
        const string ST = "St";

        public void SetupEquippedWeapon(Weapon weapon, bool isEquipped = true)
        {
            equipName.text = weapon.GetLocalizedObjName();
            equipPanelRI.texture = equippedTexture;
            equippedLight.SetActive(true);

            DamageIcon.gameObject.SetActive(true);
            DefenceIcon.gameObject.SetActive(false);

            txtStaminaCost.text = ST + weapon.staminaCost.ToString();
            SetLevel(weapon.level);
            SetRarityColor(weapon.rarity);
            equipStrenght.text = weapon.weaponConstDamage.ToString();
        }

        void SetLevel(int level)
        {
            string txt = level >= 10 ? "Lv" + level.ToString() : "Lv 0" + level.ToString();
            this.level.text = txt;
        }

        public void SetupEquippedEquipment(Equipment equip)
        {
            equipName.text = equip.GetLocalizedObjName();
            equipPanelRI.texture = equippedTexture;
            equippedLight.SetActive(true);

            DefenceIcon.gameObject.SetActive(false);
            DamageIcon.gameObject.SetActive(false);
            equipStrenght.gameObject.SetActive(false);
            txtStaminaCost.gameObject.SetActive(false);

            if (equip.IsDefence())
            {
                equipStrenght.gameObject.SetActive(true);
                equipStrenght.text = equip.defence.ToString();
                DefenceIcon.gameObject.SetActive(true);
                txtStaminaCost.gameObject.SetActive(true);
                txtStaminaCost.text = ST + equip.StaminaCost.ToString();
            }
            SetLevel(equip.level);
            SetRarityColor(equip.rarity);
        }
        //public void SetupObjectInventoryButton(ScriptableItem item, int quantity, TMP_Text itemNameLabel, TMP_Text itemDescriptionLabel, GameObject SelecteditemPanelRef, Button itemTriggerButton, NewInventoryView inventoryView)
        //{
        //    equipName.text = Localization.Get(item.itemNameLocalization);
        //    level.text = quantity.ToString();
        //    selectedItemPanel = SelecteditemPanelRef;
        //    itemTriggerBtn = itemTriggerButton;
        //    currentInventoryView = inventoryView;

        //    equipButton.onClick.RemoveAllListeners();
        //    equipButton.onClick.AddListener(delegate { OnItemClick(item, itemNameLabel, itemDescriptionLabel, itemTriggerBtn); });
        //}

        void SetRarityColor(ScriptableItem.Rarity rarity)
        {
            transform.Find("ButtonBackground").GetComponent<RawImage>().color = ColorDatabase.Singleton.GetRarityColor(rarity);
        }

        void OnWeaponClicked(Weapon weaponToEquip)
        {
            equipComparePanel.SetActive(true);
            comparedPanelPrefab.SetItem(weaponToEquip);
            currentEquipmentView.equipListRect.SetActive(false);
            switch (weaponToEquip.attackType)
            {
                case TypeDatabase.AttackType.Light:
                    if (PlayerManager.Singleton.playerWeapon.lightWeapon)
                    {
                        //equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerWeapon.lightWeapon);
                        equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerWeapon.lightWeapon, weaponToEquip);
                        comparedPanelPrefab.SetItem(weaponToEquip, PlayerManager.Singleton.playerWeapon.lightWeapon);
                    }
                    else
                    {
                        equippedPanelPrefab.SetItem();
                    }
                    break;
                case TypeDatabase.AttackType.Heavy:
                    if (PlayerManager.Singleton.playerWeapon.heavyWeapon)
                    {
                        //equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerWeapon.heavyWeapon);
                        equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerWeapon.heavyWeapon, weaponToEquip);
                        comparedPanelPrefab.SetItem(weaponToEquip, PlayerManager.Singleton.playerWeapon.heavyWeapon);
                    }
                    else
                    {
                        equippedPanelPrefab.SetItem();
                    }
                    break;
                case TypeDatabase.AttackType.Ranged:
                    if (PlayerManager.Singleton.playerWeapon.rangeWeapon)
                    {
                        equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerWeapon.rangeWeapon, weaponToEquip);
                        comparedPanelPrefab.SetItem(weaponToEquip, PlayerManager.Singleton.playerWeapon.rangeWeapon);
                    }
                    else
                    {
                        equippedPanelPrefab.SetItem();
                    }
                    break;
                case TypeDatabase.AttackType.Special:
                    if (PlayerManager.Singleton.playerWeapon.specialWeapon)
                    {
                        equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerWeapon.specialWeapon, weaponToEquip);
                        comparedPanelPrefab.SetItem(weaponToEquip, PlayerManager.Singleton.playerWeapon.specialWeapon);
                    }
                    else
                    {
                        equippedPanelPrefab.SetItem();
                    }
                    break;
            }

            setNewItemBtn.onClick.RemoveAllListeners();
            setNewItemBtn.onClick.AddListener(delegate
            {
                if (PlayerIsToWeak(weaponToEquip))
                {
                    GameApplication.Singleton.view.EquipmentView.ShowMessage(weaponToEquip.level.ToString(), weaponToEquip.GetLocalizedObjName());
                }
                else
                {
                    SetEquippedWeapon(weaponToEquip);
                }

            });
        }
        void OnArmorOrShieldItemClick(Equipment armor)
        {
            equipComparePanel.SetActive(true);
            currentEquipmentView.equipListRect.SetActive(false);

            switch (armor.equipPlaceType)
            {
                case Equipment.EquipPlaceType.LightDefence:
                    equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerEquipment.equippedLightDefence, armor);
                    comparedPanelPrefab.SetItem(armor, PlayerManager.Singleton.playerEquipment.equippedLightDefence);
                    break;
                case Equipment.EquipPlaceType.BalancedDefence:
                    equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerEquipment.equippedBalancedDefence, armor);
                    comparedPanelPrefab.SetItem(armor, PlayerManager.Singleton.playerEquipment.equippedBalancedDefence);
                    break;
                case Equipment.EquipPlaceType.HeavyDefence:
                    equippedPanelPrefab.SetItem(PlayerManager.Singleton.playerEquipment.equippedHeavyDefence, armor);
                    comparedPanelPrefab.SetItem(armor, PlayerManager.Singleton.playerEquipment.equippedHeavyDefence);
                    break;
                case Equipment.EquipPlaceType.GemStone:
                    equippedPanelPrefab.SetAccessoryItem(PlayerManager.Singleton.playerEquipment.equippedGemstone, armor);
                    comparedPanelPrefab.SetAccessoryItem(armor, PlayerManager.Singleton.playerEquipment.equippedGemstone);
                    break;
                case Equipment.EquipPlaceType.Talisman:
                    equippedPanelPrefab.SetAccessoryItem(PlayerManager.Singleton.playerEquipment.equippedTalisman, armor);
                    comparedPanelPrefab.SetAccessoryItem(armor, PlayerManager.Singleton.playerEquipment.equippedTalisman);
                    break;
                case Equipment.EquipPlaceType.Relic:
                    equippedPanelPrefab.SetAccessoryItem(PlayerManager.Singleton.playerEquipment.equippedRelic, armor);
                    comparedPanelPrefab.SetAccessoryItem(armor, PlayerManager.Singleton.playerEquipment.equippedRelic);
                    break;
            }

            setNewItemBtn.onClick.RemoveAllListeners();
            setNewItemBtn.onClick.AddListener(delegate {

                if (PlayerIsToWeak(armor))
                {
                    GameApplication.Singleton.view.EquipmentView.ShowMessage(armor.level.ToString(), armor.GetLocalizedObjName());
                }
                else
                {
                    SetEquippedEquipment(armor);
                }
            });
        }

        void OnItemClick(ScriptableItem item, TMP_Text itemNameLabel, TMP_Text itemDescriptionLabel, Button itemTriggerBtn)
        {
            if(item.itemType == ScriptableItem.ItemType.Consumable)
            {
                currentInventoryView.consumableInfoItemContainer.SetActive(true);
                currentInventoryView.selectedItemDescription.text = item.GetLocalizedBio();
                currentInventoryView.selectedItemInfo.text = item.GetLocalizedInfo();
            }
            

            selectedItemPanel.SetActive(true);
            itemNameLabel.text = item.GetLocalizedObjName();
            itemDescriptionLabel.text = item.GetLocalizedInfo();
            itemTriggerBtn.onClick.RemoveAllListeners();
            itemTriggerBtn.onClick.AddListener(delegate { OnClickItemTrigger(item); });
        }

        void SetEquippedWeapon(Weapon toEquip)
        {
            switch (toEquip.attackType)
            {
                case TypeDatabase.AttackType.Light:
                    if (PlayerManager.Singleton.playerWeapon.lightWeapon)
                    {
                        PlayerManager.Singleton.lightWeaponList.Add(PlayerManager.Singleton.playerWeapon.lightWeapon);
                    }
                    PlayerManager.Singleton.playerWeapon.lightWeapon = toEquip;
                    PlayerManager.Singleton.lightWeaponList.Remove(toEquip);
                    currentEquipmentView.OpenWeaponInventoryList(TypeDatabase.AttackType.Light);
                    break;
                case TypeDatabase.AttackType.Heavy:
                    if (PlayerManager.Singleton.playerWeapon.heavyWeapon)
                    {
                        PlayerManager.Singleton.heavyWeaponList.Add(PlayerManager.Singleton.playerWeapon.heavyWeapon);
                    }
                    PlayerManager.Singleton.playerWeapon.heavyWeapon = toEquip;
                    PlayerManager.Singleton.heavyWeaponList.Remove(toEquip);
                    currentEquipmentView.OpenWeaponInventoryList(TypeDatabase.AttackType.Heavy);
                    break;
                case TypeDatabase.AttackType.Ranged:
                    if (PlayerManager.Singleton.playerWeapon.rangeWeapon)
                    {
                        PlayerManager.Singleton.rangeWeaponList.Add(PlayerManager.Singleton.playerWeapon.rangeWeapon);
                    }
                    PlayerManager.Singleton.playerWeapon.rangeWeapon = toEquip;
                    PlayerManager.Singleton.rangeWeaponList.Remove(toEquip);
                    currentEquipmentView.OpenWeaponInventoryList(TypeDatabase.AttackType.Ranged);
                    break;
                case TypeDatabase.AttackType.Special:
                    if (PlayerManager.Singleton.playerWeapon.specialWeapon)
                    {
                        PlayerManager.Singleton.specialWeaponList.Add(PlayerManager.Singleton.playerWeapon.specialWeapon);
                    }
                    PlayerManager.Singleton.playerWeapon.specialWeapon = toEquip;
                    PlayerManager.Singleton.specialWeaponList.Remove(toEquip);
                    currentEquipmentView.OpenWeaponInventoryList(TypeDatabase.AttackType.Special);
                    break;
            }
            GameApplication.Singleton.view.EquipmentView.LoadEquippedLabels(toEquip);
            equipComparePanel.SetActive(false);
        }
        void SetEquippedEquipment(Equipment toEquip)
        {
            switch (toEquip.equipPlaceType)
            {
                case Equipment.EquipPlaceType.LightDefence:
                    PlayerManager.Singleton.lightDefenceList.Add(PlayerManager.Singleton.playerEquipment.equippedLightDefence);
                    PlayerManager.Singleton.playerEquipment.equippedLightDefence = toEquip;
                    PlayerManager.Singleton.lightDefenceList.Remove(toEquip);
                    UpdateDefaultDefense(toEquip);
                    currentEquipmentView.OpenEquipCategoryList(toEquip.equipPlaceType);
                    break;
                case Equipment.EquipPlaceType.BalancedDefence:
                    PlayerManager.Singleton.balancedDefenceList.Add(PlayerManager.Singleton.playerEquipment.equippedBalancedDefence);
                    PlayerManager.Singleton.playerEquipment.equippedBalancedDefence = toEquip;
                    PlayerManager.Singleton.balancedDefenceList.Remove(toEquip);
                    UpdateDefaultDefense(toEquip);
                    currentEquipmentView.OpenEquipCategoryList(toEquip.equipPlaceType);
                    break;
                case Equipment.EquipPlaceType.HeavyDefence:
                    PlayerManager.Singleton.heavyDefenceList.Add(PlayerManager.Singleton.playerEquipment.equippedHeavyDefence);
                    PlayerManager.Singleton.playerEquipment.equippedHeavyDefence = toEquip;
                    PlayerManager.Singleton.heavyDefenceList.Remove(toEquip);
                    UpdateDefaultDefense(toEquip);
                    currentEquipmentView.OpenEquipCategoryList(toEquip.equipPlaceType);
                    break;
                case Equipment.EquipPlaceType.GemStone:
                    PlayerManager.Singleton.gemstonesList.Add(PlayerManager.Singleton.playerEquipment.equippedGemstone);
                    PlayerManager.Singleton.gemstonesList.Remove(toEquip);
                    PlayerManager.Singleton.playerEquipment.equippedGemstone = toEquip;
                    currentEquipmentView.OpenEquipCategoryList(toEquip.equipPlaceType);
                    break;
                case Equipment.EquipPlaceType.Talisman:
                    PlayerManager.Singleton.talismansList.Add(PlayerManager.Singleton.playerEquipment.equippedTalisman);
                    PlayerManager.Singleton.talismansList.Remove(toEquip);
                    PlayerManager.Singleton.playerEquipment.equippedTalisman = toEquip;
                    currentEquipmentView.OpenEquipCategoryList(toEquip.equipPlaceType);
                    break;
                case Equipment.EquipPlaceType.Relic:
                    PlayerManager.Singleton.relicsList.Add(PlayerManager.Singleton.playerEquipment.equippedRelic);
                    PlayerManager.Singleton.relicsList.Remove(toEquip);
                    PlayerManager.Singleton.playerEquipment.equippedRelic = toEquip;
                    currentEquipmentView.OpenEquipCategoryList(toEquip.equipPlaceType);
                    break;
            }
            GameApplication.Singleton.view.EquipmentView.LoadEquippedLabels(toEquip);
            equipComparePanel.SetActive(false);
        }

        public void UpdateDefaultDefense(Equipment newDefense)
        {
            if (PlayerManager.Singleton.initialEquipmentDefenseType == newDefense.defenseType)
            {
                PlayerManager.Singleton.playerEquipment.equippedClassDefaultDefence = newDefense;
                currentEquipmentView.SetDefaultDefence();
            }

        }
        bool PlayerIsToWeak(ScriptableItem equip)
        {
            return equip.level > PlayerManager.Singleton.GetPlayerLevel;
        }

        void OnClickItemTrigger(ScriptableItem item)
        {
            item.TriggerItem();
            selectedItemPanel.SetActive(false);
            currentInventoryView.OpenItemList(item.itemType);
        }
    }
}
