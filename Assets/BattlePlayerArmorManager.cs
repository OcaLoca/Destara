/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.PlayerManager;

namespace Game
{
    public class BattlePlayerArmorManager : MonoBehaviour
    {
        public static BattlePlayerArmorManager SingletonBattlePlayerArmorManager { get; set; }

        [Header("ArmorButtons")]
        [SerializeField] private GameObject headEquipButton;
        [SerializeField] Button headEquipInfoButton;
        [SerializeField] private GameObject torsoEquipButton;
        [SerializeField] Button torsoEquipInfoButton;
        [SerializeField] private GameObject shieldEquipButton;
        [SerializeField] Button shieldEquipInfoButton;

        [Header("UIMenuEquipaggiamento")]
        [SerializeField] public GameObject equipListRect;
        [SerializeField] private Transform equipListContainer;
        [SerializeField] TMP_Text categoryTitle;
        [SerializeField] private EquipPrefab equipItemPrefab;

        [Header("ComparePanelUI")]
        [SerializeField] private GameObject equipComparePanel;
        [SerializeField] private Button closeEquipPanelBtn;
        [SerializeField] private Button setNewEquippedBtn;
        [SerializeField] ComparePanel equippedItemPanel;
        [SerializeField] ComparePanel comparedItemPanel;

        private void Awake()
        {
            SingletonBattlePlayerArmorManager = this;    
        }

        private void OnEnable()
        {
            headEquipButton.GetComponent<Button>().onClick.RemoveAllListeners();
            torsoEquipButton.GetComponent<Button>().onClick.RemoveAllListeners();
            shieldEquipButton.GetComponent<Button>().onClick.RemoveAllListeners();

            torsoEquipButton.GetComponent<Button>().onClick.AddListener(delegate { InBattleOpenOwnedPlayerEquipmentOfType(Equipment.EquipPlaceType.BalancedDefence); });
            headEquipButton.GetComponent<Button>().onClick.AddListener(delegate { InBattleOpenOwnedPlayerEquipmentOfType(Equipment.EquipPlaceType.LightDefence); });
            shieldEquipButton.GetComponent<Button>().onClick.AddListener(delegate { InBattleOpenOwnedPlayerEquipmentOfType(Equipment.EquipPlaceType.HeavyDefence); });
        }

        public void InBattleOpenOwnedPlayerEquipmentOfType(Equipment.EquipPlaceType type)
        {
            PlayerEquipment currentEquippedEquipment = Singleton.playerEquipment;
            equipListRect.SetActive(true);

            DeleteAllEquipListChildren();
            switch (type)
            {
                //Equips
                case Equipment.EquipPlaceType.LightDefence:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.LIGHTDEFENCE);

                    if (currentEquippedEquipment.equippedLightDefence)
                    {
                        IstantiateEquipmentButton(currentEquippedEquipment.equippedLightDefence, true, equipListContainer);
                    }
                    foreach (ScriptableItem index in Singleton.lightDefenceList)
                    {
                        IstantiateEquipmentButton((Equipment)index, false, equipListContainer);
                    }
                    break;
                case Equipment.EquipPlaceType.BalancedDefence:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.BALANCEDDEFENCE);
                    if (currentEquippedEquipment.equippedBalancedDefence)
                    {
                        IstantiateEquipmentButton(currentEquippedEquipment.equippedBalancedDefence, true, equipListContainer);
                    }
                    foreach (ScriptableItem index in Singleton.balancedDefenceList)
                    {
                        IstantiateEquipmentButton((Equipment)index, false, equipListContainer);
                    }
                    break;
                case Equipment.EquipPlaceType.HeavyDefence:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.HEAVYDEFENCE);
                    if (currentEquippedEquipment.equippedHeavyDefence)
                    {
                        IstantiateEquipmentButton(currentEquippedEquipment.equippedHeavyDefence, true, equipListContainer);
                    }
                    foreach (ScriptableItem index in Singleton.heavyDefenceList)
                    {
                        IstantiateEquipmentButton((Equipment)index, false, equipListContainer);
                    }
                    break;
                default:
                    CloseCategoryList();
                    break;
            }
        }

        void IstantiateEquipmentButton(Equipment index, bool isEquipped, Transform parent)
        {
            EquipPrefab item = Instantiate(equipItemPrefab, parent);
            if (isEquipped)
            {
                item.SetupEquippedEquipment(index);
            }
            else
            {
                item.SetupNotEquppedArmorOrShield(index, equipComparePanel, equippedItemPanel, comparedItemPanel, setNewEquippedBtn);
            }
            item.transform.localScale = Vector3.one;
        }

        void CloseCategoryList()
        {
            equipListRect.SetActive(false);
            DeleteAllEquipListChildren();
        }
        void DeleteAllEquipListChildren()
        {
            for (int i = equipListContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(equipListContainer.GetChild(i).gameObject);
            }
        }
    }

}
