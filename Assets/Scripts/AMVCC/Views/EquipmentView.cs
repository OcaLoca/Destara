using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartMVC;
using StarworkGC.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.PlayerManager;
using static Game.ScriptableItem;
using static Game.TypeDatabase;

namespace Game
{
    public class EquipmentView : View<GameApplication>
    {
        [Header("UIMenuEquipaggiamento")]
        [SerializeField] public GameObject equipListRect;
        [SerializeField] Button btnEquipBack, btnBackEquipmentSubmenu, btnBackAccessoriesSubmenu, btnBackDefencesList;
        [SerializeField] Button btnOpenEquipmentSubmenu, btnOpenWeaponsMenu, btnOpenAccessoriesSubmenu, btnAbiliyMenu;
        [SerializeField] Button btnOpenWeaponsList, btnOpenDefencesList;
        [SerializeField] List<Button> btnBackFromSubMenuList;
        [SerializeField] GameObject inventoryMainMenu, battleEquipmentSubmenu, accessoriesEquipmentSubmenu;
        [SerializeField] GameObject weaponsEquippedBattleListMenu, accessoriesListMenu, defencesListMenu, menuAbilities;
        public ScrollRect equipListScroll;


        [Header("WeaponButtons")]
        [SerializeField] private GameObject lightWeaponButton;
        [SerializeField] Button lightWeaponInfoButton;
        [SerializeField] private GameObject heavyWeaponButton;
        [SerializeField] Button heavyWeaponInfoButton;
        [SerializeField] private GameObject rangedWeaponButton;
        [SerializeField] Button rangedWeaponInfoButton;
        [SerializeField] private GameObject specialWeaponButton;
        [SerializeField] Button specialWeaponInfoButton;

        [Header("ArmorButtons")]
        [SerializeField] Image DefaultDefenceIcon;
        [SerializeField] TMP_Text txtDefaultDefenceName, txtDefaultDefenceLevel, txtDefaultDefenceValue, txtDefaultDefenceStaminaCost;
        [SerializeField] RawImage imgDefaultDefenceContainer;
        [SerializeField] private GameObject lightDefenceButton;
        [SerializeField] Button lightDefenceInfoButton;
        [SerializeField] private GameObject balancedDefenceButton;
        [SerializeField] Button balancedDefenceInfoButton;
        [SerializeField] private GameObject heavyDefenceButton;
        [SerializeField] Button heavyDefenceInfoButton;

        [Header("AccessoriesButtons")]
        [SerializeField] private GameObject accessoryEquipButton;
        [SerializeField] Button accessoryEquipInfoButton;
        [SerializeField] private GameObject talismanEquipButton;
        [SerializeField] Button talismanEquipInfoButton;
        [SerializeField] private GameObject relicEquipButton;
        [SerializeField] Button relicEquipInfoButton;
        //[SerializeField] private TMP_Text defenceType, defenceValue, defenceWeight;


        [Header("AbilityInfoPanelUI")]
        [SerializeField] private GameObject firstAbilityEquipButton, secondAbilityEquipButton, thirdAbilityEquipButton, fourthAbilityEquipButton;
        // [SerializeField] Button firstAbilityInfoEquipButton, secondAbilityInfoEquipButton, thirdAbilityInfoEquipButton, fourthAbilityInfoEquipButton;
        [SerializeField] private TMP_Text abilityDescription, abilityName, abilityCost;

        [Header("ComparePanelUI")]
        [SerializeField] private GameObject equipComparePanel;
        [SerializeField] private Button closeEquipPanelBtn;
        [SerializeField] private Button setNewEquippedBtn;
        [SerializeField] ComparePanel equippedItemPanel;
        [SerializeField] ComparePanel comparedItemPanel;
        [SerializeField] GameObject alarmPanelLevelToLow;

        [SerializeField] private Button closeCategoryList;
        [SerializeField] private EquipPrefab equipItemPrefab;
        [SerializeField] private Transform equipListContainer;
        [SerializeField] private EquipmentView equipmentView;
        [SerializeField] TMP_Text categoryTitle;


        [Header("InfoPanels")]
        [SerializeField] private Image abilityIcon;
        [SerializeField] private GameObject infoPanelAbility;
        [SerializeField] private GameObject infoAccessoryTalismanPanel;
        [SerializeField] private GameObject panelShowWeaponInfo;
        [SerializeField] private GameObject panelShowEquipmentInfo;
        [SerializeField] Button btnCloseAbilityInfoPanel, btnCloseAccessoryTalismanInfoPanel;

        [Header("Tutorial")]
        [SerializeField] GameObject tutorial, tutorialPreparation, tutorialPreparationDefence;


        void Start()
        {
            equippedItemPanel.Setup();
            comparedItemPanel.Setup();
            equipListRect.SetActive(false);
            equipComparePanel.SetActive(false);

            btnEquipBack.onClick.RemoveAllListeners();
            btnEquipBack.onClick.AddListener(OnClickBackOnEquipMenu);
            foreach (Button button in btnBackFromSubMenuList)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OnCloseSubMenu);
            }
            btnBackEquipmentSubmenu.onClick.RemoveAllListeners();
            btnBackEquipmentSubmenu.onClick.AddListener(delegate
            {
                UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
                battleEquipmentSubmenu.SetActive(false);
            });

            btnBackAccessoriesSubmenu.onClick.RemoveAllListeners();
            btnBackAccessoriesSubmenu.onClick.AddListener(delegate
            {
                UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
                accessoriesEquipmentSubmenu.SetActive(false);
            });

            closeEquipPanelBtn.onClick.RemoveAllListeners();
            closeEquipPanelBtn.onClick.AddListener(CloseComparePanel);

            btnOpenEquipmentSubmenu.onClick.RemoveAllListeners();
            btnOpenAccessoriesSubmenu.onClick.RemoveAllListeners();
            btnAbiliyMenu.onClick.RemoveAllListeners();
            btnOpenWeaponsList.onClick.RemoveAllListeners();
            btnOpenWeaponsList.onClick.AddListener(OpenWeaponsMenu);
            btnOpenDefencesList.onClick.RemoveAllListeners();
            btnOpenDefencesList.onClick.AddListener(delegate { OpenDefenceListMenu(false); });
            btnBackDefencesList.onClick.RemoveAllListeners();
            btnBackDefencesList.onClick.AddListener(delegate
            {
                UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
                defencesListMenu.SetActive(false);
            });

            btnOpenEquipmentSubmenu.onClick.AddListener(OpenBattleEquipmentSubmenu);
            btnOpenAccessoriesSubmenu.onClick.AddListener(OpenEquipmentAccessoriesSubmenu);
            btnAbiliyMenu.onClick.AddListener(OpenAbilitiesSubmenu);

            closeCategoryList.onClick.RemoveAllListeners();
            closeCategoryList.onClick.AddListener(CloseCategoryList);
        }


        List<ScriptableAbility> equippedAbilities;
        List<Weapon> equippedWeapons;

        [SerializeField] Image LightWeaponIcon, HeavyWeaponIcon, RangedWeaponIcon, SpecialWeaponIcon;


        private void RemoveAllListenersFromButtons()
        {
            RemoveListeners(lightWeaponButton.GetComponent<Button>(), heavyWeaponButton.GetComponent<Button>(), rangedWeaponButton.GetComponent<Button>(), specialWeaponButton.GetComponent<Button>());
            RemoveListeners(lightDefenceButton.GetComponent<Button>(), balancedDefenceButton.GetComponent<Button>(), heavyDefenceButton.GetComponent<Button>());
            RemoveListeners(accessoryEquipButton.GetComponent<Button>(), talismanEquipButton.GetComponent<Button>(), relicEquipButton.GetComponent<Button>());
            RemoveListeners(firstAbilityEquipButton.GetComponent<Button>(), secondAbilityEquipButton.GetComponent<Button>(), thirdAbilityEquipButton.GetComponent<Button>(), fourthAbilityEquipButton.GetComponent<Button>());
            RemoveListeners(lightWeaponInfoButton, heavyWeaponInfoButton, rangedWeaponInfoButton, specialWeaponInfoButton);
            RemoveListeners(lightDefenceInfoButton, balancedDefenceInfoButton, heavyDefenceInfoButton);
            RemoveListeners(accessoryEquipInfoButton, talismanEquipInfoButton, relicEquipInfoButton);
            RemoveListeners(btnCloseAbilityInfoPanel, btnCloseAccessoryTalismanInfoPanel);
        }

        private void RemoveListeners(params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        public void ShowPreparationBeforeFight()
        {
            tutorialPreparation.SetActive(true);
        }

        public void ShowPreparationDefenceBeforeFight()
        {
            tutorialPreparationDefence.SetActive(true);
        }

        private void OnEnable()
        {
            if(TutorialSystemManager.InventoryTutorialPartNotShowed)
            {
                if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1 ||
               PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1)
                {
                    tutorial.SetActive(true);
                    TutorialSystemManager.InventoryTutorialPartNotShowed = false;
                }
            }
            else
            {
                tutorial.SetActive(false);
            }

            equippedAbilities = Singleton.playerAbility;
            equippedWeapons = Singleton.EquippedWeaponToList();

            LoadEquippedLabels();

            LightWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LIGHTWEAPONICONID);
            HeavyWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.HEAVYWEAPONICONID);
            RangedWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.RANGEDWEAPONICONID);
            SpecialWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.SPECIALWEAPONICONID);

            SetDefaultDefence();

            RemoveAllListenersFromButtons();

            lightWeaponButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfAttackType(TypeDatabase.AttackType.Light); });
            heavyWeaponButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfAttackType(TypeDatabase.AttackType.Heavy); });
            rangedWeaponButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfAttackType(TypeDatabase.AttackType.Ranged); });
            specialWeaponButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfAttackType(TypeDatabase.AttackType.Special); });

            balancedDefenceButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfEquipPlaceType(Equipment.EquipPlaceType.BalancedDefence); });
            lightDefenceButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfEquipPlaceType(Equipment.EquipPlaceType.LightDefence); });
            heavyDefenceButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfEquipPlaceType(Equipment.EquipPlaceType.HeavyDefence); });

            accessoryEquipButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfEquipPlaceType(Equipment.EquipPlaceType.GemStone); });
            talismanEquipButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfEquipPlaceType(Equipment.EquipPlaceType.Talisman); });
            relicEquipButton.GetComponent<Button>().onClick.AddListener(delegate { OpenListOfEquipPlaceType(Equipment.EquipPlaceType.Relic); });

            firstAbilityEquipButton.GetComponent<Button>().onClick.AddListener(delegate { OpenAbilityInfo(equippedAbilities[0]); });
            secondAbilityEquipButton.GetComponent<Button>().onClick.AddListener(delegate { OpenAbilityInfo(equippedAbilities[1]); });
            thirdAbilityEquipButton.GetComponent<Button>().onClick.AddListener(delegate { OpenAbilityInfo(equippedAbilities[2]); });
            fourthAbilityEquipButton.GetComponent<Button>().onClick.AddListener(delegate { OpenAbilityInfo(equippedAbilities[3]); });

            btnCloseAbilityInfoPanel.onClick.AddListener(delegate
            {
                UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
                infoPanelAbility.SetActive(false);
            });
            btnCloseAccessoryTalismanInfoPanel.onClick.AddListener(delegate
            {
                UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
                infoAccessoryTalismanPanel.SetActive(false);
            });

            lightWeaponInfoButton.onClick.AddListener(delegate { OpenWeaponInfoPanel(Singleton.playerWeapon.lightWeapon); });
            heavyWeaponInfoButton.onClick.AddListener(delegate { OpenWeaponInfoPanel(Singleton.playerWeapon.heavyWeapon); });
            rangedWeaponInfoButton.onClick.AddListener(delegate { OpenWeaponInfoPanel(Singleton.playerWeapon.rangeWeapon); });
            specialWeaponInfoButton.onClick.AddListener(delegate { OpenWeaponInfoPanel(Singleton.playerWeapon.specialWeapon); });

            lightDefenceInfoButton.onClick.AddListener(delegate { OpenEquipmentInfoPanel(Singleton.playerEquipment.equippedLightDefence); });
            balancedDefenceInfoButton.onClick.AddListener(delegate { OpenEquipmentInfoPanel(Singleton.playerEquipment.equippedBalancedDefence); });
            heavyDefenceInfoButton.onClick.AddListener(delegate { OpenEquipmentInfoPanel(Singleton.playerEquipment.equippedHeavyDefence); });
            accessoryEquipInfoButton.onClick.AddListener(delegate { OpenAccessoryInfoPanel(Singleton.playerEquipment.equippedGemstone); });
            talismanEquipInfoButton.onClick.AddListener(delegate { OpenAccessoryInfoPanel(Singleton.playerEquipment.equippedTalisman); });
            relicEquipInfoButton.onClick.AddListener(delegate { OpenAccessoryInfoPanel(Singleton.playerEquipment.equippedRelic); });
        }

        public void SetDefaultDefence()
        {
            DefaultDefenceIcon.sprite = IconsDatabase.Singleton.GetArmorSpriteByDefenceType(Singleton.GetDefaultClassDefence().defenseType);
            txtDefaultDefenceName.text = Localization.Get(Singleton.GetDefaultClassDefence().itemNameLocalized);
            txtDefaultDefenceLevel.text = "Lv." + Singleton.GetDefaultClassDefence().level.ToString();
            txtDefaultDefenceValue.text = Singleton.GetDefaultClassDefence().defence.ToString();
            txtDefaultDefenceStaminaCost.text = "St" + Singleton.GetDefaultClassDefence().StaminaCost.ToString();
            imgDefaultDefenceContainer.color = ColorDatabase.Singleton.GetRarityColor(Singleton.GetDefaultClassDefence().rarity);
        }

        public void ShowMessage(string weapon_name, string level)
        {
            StartCoroutine(ShowMessagePlayerLevelLow(2, weapon_name, level));
        }
        public IEnumerator ShowMessagePlayerLevelLow(float time, string weapon_name, string level)
        {
            alarmPanelLevelToLow.SetActive(true);
            string text = string.Format(Localization.Get(LocalizationIDDatabase.CANT_USE_LEVEL_LOW), weapon_name, level);
            alarmPanelLevelToLow.GetComponentInChildren<TMP_Text>().text = text;

            yield return new WaitForSeconds(time);
            alarmPanelLevelToLow.SetActive(false);
        }

        void OpenBattleEquipmentSubmenu()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            battleEquipmentSubmenu.SetActive(true);
        }
        void OpenEquipmentAccessoriesSubmenu()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            accessoriesEquipmentSubmenu.SetActive(true);
        }

        void OpenListOfAttackType(TypeDatabase.AttackType attackType)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            OpenWeaponInventoryList(attackType);
        }
        void OpenListOfEquipPlaceType(Equipment.EquipPlaceType equipPlaceType)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            OpenEquipCategoryList(equipPlaceType);
        }

        const string AP = "AP";
        void OpenAbilityInfo(ScriptableAbility ability)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            abilityIcon.color = ColorDatabase.Singleton.GetRarityColor(ability.rarity);
            abilityName.text = ability.GetAbilityLocalizedName();
            abilityDescription.text = Localization.Get(ability.abilityDescription);
            abilityCost.text = ability.GetAbilityCostToString();
            infoPanelAbility.SetActive(true);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        void OpenAccessoryInfoPanel(Equipment e)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);

            string objName = Localization.Get(e.itemNameLocalization);
            string objInfoBio = e.GetLocalizedBio();
            string objInfoDsc = e.GetLocalizedInfo();

            //ShowAccessoryInfoPanel.Singleton.SetBioText(objInfoBio);
            Sprite objSprite;
            e.SetEquipmentIcon();
            objSprite = e.icon;
            if (objInfoDsc == null) { objInfoDsc = Localization.Get("noInformation"); }

            infoAccessoryTalismanPanel.SetActive(true);
            ShowAccessoryInfoPanel.Singleton.ShowTalismanInfo(objName, Localization.Get(e.itemType.ToString()), e.rarity.ToString(), UIUtility.GetNumberForUI(e.level), objInfoDsc, objInfoBio,
                objSprite, e.GetEquipmentRarityColor());
        }

        void OpenWeaponInfoPanel(Weapon w)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);

            panelShowWeaponInfo.gameObject.SetActive(true);
            w.SetWeaponIcon();

            ShowWeaponInfoPanel.Singleton.ShowWeaponInfo(w.attackType, w.itemNameLocalization, w.rarity.ToString(),
                                                          UIUtility.GetNumberForUI(w.level), w.weaponConstDamage.ToString(),
                                                         w.scaleType.ToString(), UIUtility.GetNumberForUI((int)w.criticalDamageChance),
                                                         UIUtility.GetNumberForUI((int)w.hitChance), w.icon, w.GetWeaponRarityColor());
        }
        void OpenEquipmentInfoPanel(Equipment e)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);

            panelShowEquipmentInfo.gameObject.SetActive(true);
            e.SetEquipmentIcon();

            ShowEquipmentInfoPanel.Singleton.ShowEquipmentInfo(e.defenseType, e.itemNameLocalization, e.rarity.ToString(),
                                                               UIUtility.GetNumberForUI(e.level), UIUtility.GetNumberForUI((int)e.defence),
                                                               UIUtility.GetNumberForUI((int)e.GetWeight()), e.icon, e.GetEquipmentRarityColor());
        }

        void OnClickBackOnEquipMenu()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            DeleteAllEquipListChildren();
            equipListRect.SetActive(false);
            equipComparePanel.SetActive(false);
            Notify(MVCEvents.OPEN_STATS_VIEW);
        }

        void OpenWeaponsMenu()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            weaponsEquippedBattleListMenu.gameObject.SetActive(true);
        }
        public void OpenDefenceListMenu(bool tutorial = false)
        {
            defencesListMenu.gameObject.SetActive(true);
            if (tutorial) return;
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
        }
        void OpenAbilitiesSubmenu()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            menuAbilities.gameObject.SetActive(true);
        }
        void OnCloseSubMenu()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            DeleteAllEquipListChildren();
            equipListRect.SetActive(false);
            equipComparePanel.SetActive(false);
            weaponsEquippedBattleListMenu.gameObject.SetActive(false);
            accessoriesListMenu.gameObject.SetActive(false);
            menuAbilities.gameObject.SetActive(false);
        }

        /// <summary>
        /// Apre la lista di accessori dello stesso tipo
        /// </summary>
        /// <param name="attackType"></param>
        public void OpenWeaponInventoryList(TypeDatabase.AttackType attackType) ///apro la lista di armi dello stesso tipo che ho
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);

            equipListRect.SetActive(true);
            DeleteAllEquipListChildren();

            EquippedWeapon currentEquippedWeapons = Singleton.playerWeapon;
            switch (attackType)
            {
                //Weapons
                case TypeDatabase.AttackType.Light:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.LIGHTWEAPON);
                    if (currentEquippedWeapons.lightWeapon)
                    {
                        InstantiateWeaponButton(currentEquippedWeapons.lightWeapon, true, equipListContainer);
                    }
                    foreach (ScriptableItem weapon in Singleton.lightWeaponList)
                    {
                        InstantiateWeaponButton((Weapon)weapon, false, equipListContainer);
                    }
                    break;
                case TypeDatabase.AttackType.Heavy:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.HEAVYWEAPON);
                    if (currentEquippedWeapons.heavyWeapon)
                    {
                        InstantiateWeaponButton(currentEquippedWeapons.heavyWeapon, true, equipListContainer);
                    }
                    foreach (ScriptableItem weapon in Singleton.heavyWeaponList)
                    {
                        InstantiateWeaponButton((Weapon)weapon, false, equipListContainer);
                    }
                    break;
                case TypeDatabase.AttackType.Ranged:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.RANGEDWEAPON);
                    if (currentEquippedWeapons.rangeWeapon)
                    {
                        InstantiateWeaponButton(currentEquippedWeapons.rangeWeapon, true, equipListContainer);
                    }
                    foreach (ScriptableItem weapon in Singleton.rangeWeaponList)
                    {
                        InstantiateWeaponButton((Weapon)weapon, false, equipListContainer);
                    }
                    break;
                case TypeDatabase.AttackType.Special:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.SPECIALWEAPON);
                    if (currentEquippedWeapons.specialWeapon)
                    {
                        InstantiateWeaponButton(currentEquippedWeapons.specialWeapon, true, equipListContainer);
                    }
                    foreach (ScriptableItem weapon in Singleton.specialWeaponList)
                    {
                        InstantiateWeaponButton((Weapon)weapon, false, equipListContainer);
                    }
                    break;
            }
        }

        public void OpenEquipCategoryList(Equipment.EquipPlaceType equipType)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            PlayerEquipment currentEquippedEquipment = Singleton.playerEquipment;

            equipListRect.SetActive(true);
            //equipListContainer.DetachChildren();
            DeleteAllEquipListChildren();
            switch (equipType)
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

                case Equipment.EquipPlaceType.GemStone:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.GEMSTONE);
                    if (currentEquippedEquipment.equippedGemstone)
                    {
                        IstantiateEquipmentButton(currentEquippedEquipment.equippedGemstone, true, equipListContainer);
                    }
                    foreach (ScriptableItem index in Singleton.gemstonesList)
                    {
                        IstantiateEquipmentButton((Equipment)index, false, equipListContainer);
                    }
                    break;
                case Equipment.EquipPlaceType.Talisman:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.TALISMAN);
                    if (currentEquippedEquipment.equippedTalisman)
                    {
                        IstantiateEquipmentButton(currentEquippedEquipment.equippedTalisman, true, equipListContainer);
                    }
                    foreach (ScriptableItem index in Singleton.talismansList)
                    {
                        IstantiateEquipmentButton((Equipment)index, false, equipListContainer);
                    }
                    break;
                case Equipment.EquipPlaceType.Relic:
                    categoryTitle.text = Localization.Get(LocalizationDatabase.RELIC);
                    if (currentEquippedEquipment.equippedRelic)
                    {
                        IstantiateEquipmentButton(currentEquippedEquipment.equippedRelic, true, equipListContainer);
                    }
                    foreach (ScriptableItem index in Singleton.relicsList)
                    {
                        IstantiateEquipmentButton((Equipment)index, false, equipListContainer);
                    }
                    break;
                default:
                    CloseCategoryList();
                    break;
            }
        }

        public async void LoadEquippedLabels(ScriptableItem newObj = null)
        {
            EquippedWeapon equippedWeapon = Singleton.playerWeapon;
            PlayerEquipment equippedEquipment = Singleton.playerEquipment;

            // Aggiorna l'equipaggiamento se un nuovo oggetto è stato passato
            if (newObj)
            {
                UpdateEquippedItems(newObj);
            }

            // Aggiorna le etichette delle armi equipaggiate
            if (equippedWeapon.lightWeapon)
                await SetCategoryButtonLabelAsync(lightWeaponButton, equippedWeapon.lightWeapon.rarity, equippedWeapon.lightWeapon);

            if (equippedWeapon.heavyWeapon)
                await SetCategoryButtonLabelAsync(heavyWeaponButton, equippedWeapon.heavyWeapon.rarity, equippedWeapon.heavyWeapon);

            if (equippedWeapon.rangeWeapon)
                await SetCategoryButtonLabelAsync(rangedWeaponButton, equippedWeapon.rangeWeapon.rarity, equippedWeapon.rangeWeapon);

            if (equippedWeapon.specialWeapon)
                await SetCategoryButtonLabelAsync(specialWeaponButton, equippedWeapon.specialWeapon.rarity, equippedWeapon.specialWeapon);

            // Aggiorna le etichette delle difese equipaggiate
            if (equippedEquipment.equippedLightDefence)
                await SetCategoryButtonLabelAsync(lightDefenceButton, equippedEquipment.equippedLightDefence.rarity, equippedEquipment.equippedLightDefence);

            if (equippedEquipment.equippedBalancedDefence)
                await SetCategoryButtonLabelAsync(balancedDefenceButton, equippedEquipment.equippedBalancedDefence.rarity, equippedEquipment.equippedBalancedDefence);

            if (equippedEquipment.equippedHeavyDefence)
                await SetCategoryButtonLabelAsync(heavyDefenceButton, equippedEquipment.equippedHeavyDefence.rarity, equippedEquipment.equippedHeavyDefence);

            if (equippedEquipment.equippedGemstone)
                await SetCategoryButtonLabelAsync(accessoryEquipButton, equippedEquipment.equippedGemstone.rarity, equippedEquipment.equippedGemstone);

            if (equippedEquipment.equippedTalisman)
                await SetCategoryButtonLabelAsync(talismanEquipButton, equippedEquipment.equippedTalisman.rarity, equippedEquipment.equippedTalisman);

            if (equippedEquipment.equippedRelic)
                await SetCategoryButtonLabelAsync(relicEquipButton, equippedEquipment.equippedRelic.rarity, equippedEquipment.equippedRelic);

            // Aggiorna le etichette delle abilità equipaggiate
            List<GameObject> abilityButtons = new List<GameObject> { firstAbilityEquipButton, secondAbilityEquipButton, thirdAbilityEquipButton, fourthAbilityEquipButton };
            for (int i = 0; i < equippedAbilities.Count; i++)
            {
                if (equippedAbilities[i])
                {
                    await SetCategoryButtonLabelAsync(abilityButtons[i], equippedAbilities[i].rarity, null, equippedAbilities[i]);
                }
            }
        }

        private async Task SetCategoryButtonLabelAsync(GameObject selectedButton, Rarity rarity, ScriptableItem equippedItem = null, ScriptableAbility ability = null)
        {
            // Ottieni riferimenti ai componenti del bottone una sola volta
            var nameLabel = selectedButton.transform.Find("EquipDetailPanel/EquipNameCategoryLbl").GetComponent<TMP_Text>();
            var levelLabel = selectedButton.transform.Find("EquipDetailPanel/EquipLevelLbl").GetComponent<TMP_Text>();
            SetRarityColor(rarity, selectedButton);

            if (ability != null)
            {
                nameLabel.text = ability.GetAbilityLocalizedName(); // Usa traduzioni asincrone
                selectedButton.transform.Find("EquipDetailPanel/txtAP").GetComponent<TMP_Text>().text = ability.GetAbilityCostToString();
                levelLabel.text = "Lv " + SetLevelText(ability.abilityLevel);
                return;
            }

            levelLabel.text = "Lv " + SetLevelText(equippedItem.level);
            nameLabel.text = equippedItem.GetLocalizedObjName();
            if (equippedItem is Weapon wpn)
            {
                selectedButton.transform.Find("EquipDetailPanel/EquipLevelPanel/EquipDmgContainer/Image").GetComponent<Image>().gameObject.SetActive(true);
                selectedButton.transform.Find("EquipDetailPanel/EquipLevelPanel/EquipDmgContainer/EquipDmgLbl").GetComponent<TMP_Text>().text = wpn.weaponConstDamage.ToString();
                selectedButton.transform.Find("EquipDetailPanel/EquipLevelPanel/EquipStaminaCost").GetComponent<TMP_Text>().text = "ST" + wpn.staminaCost.ToString();
            }
            else if (equippedItem is Equipment equip)
            {
                if (equip.itemType == ItemType.Armor)
                {
                    var damageLabel = selectedButton.transform.Find("EquipDetailPanel/EquipLevelPanel/EquipDmgContainer/EquipDmgLbl").GetComponent<TMP_Text>();
                    var imageComponent = selectedButton.transform.Find("EquipDetailPanel/EquipLevelPanel/EquipDmgContainer/Image").GetComponent<Image>();
                    damageLabel.text = equip.defence.ToString();
                    imageComponent.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.DEFENCEICONID);
                    selectedButton.transform.Find("EquipDetailPanel/EquipLevelPanel/EquipStaminaCost").GetComponent<TMP_Text>().text = "ST" + equip.StaminaCost.ToString();
                }
            }

        }

        private void UpdateEquippedItems(ScriptableItem newObj)
        {
            if (newObj is Weapon wpn)
            {
                switch (wpn.attackType)
                {
                    case AttackType.Light:
                        Singleton.playerWeapon.lightWeapon = wpn;
                        break;
                    case AttackType.Heavy:
                        Singleton.playerWeapon.heavyWeapon = wpn;
                        break;
                    case AttackType.Ranged:
                        Singleton.playerWeapon.rangeWeapon = wpn;
                        break;
                    case AttackType.Special:
                        Singleton.playerWeapon.specialWeapon = wpn;
                        break;
                }
            }
            else if (newObj is Equipment equip)
            {
                switch (equip.equipPlaceType)
                {
                    case Equipment.EquipPlaceType.LightDefence:
                        Singleton.playerEquipment.equippedLightDefence = equip;
                        break;
                    case Equipment.EquipPlaceType.BalancedDefence:
                        Singleton.playerEquipment.equippedBalancedDefence = equip;
                        break;
                    case Equipment.EquipPlaceType.HeavyDefence:
                        Singleton.playerEquipment.equippedHeavyDefence = equip;
                        break;
                    case Equipment.EquipPlaceType.GemStone:
                        Singleton.playerEquipment.equippedGemstone = equip;
                        break;
                    case Equipment.EquipPlaceType.Talisman:
                        Singleton.playerEquipment.equippedTalisman = equip;
                        break;
                    case Equipment.EquipPlaceType.Relic:
                        Singleton.playerEquipment.equippedRelic = equip;
                        break;
                }

                SetDefaultDefence();
            }
        }

        string SetLevelText(int level)
        {
            string levelText = level >= 10 ? level.ToString() : "0" + level.ToString();
            return levelText;
        }

        void SetRarityColor(Rarity rarity, GameObject selectedButton)
        {
            selectedButton.transform.Find("EquipDetailPanel").GetComponent<RawImage>().color = ColorDatabase.Singleton.GetRarityColor(rarity);
        }

        void InstantiateWeaponButton(Weapon index, bool isEquipped, Transform parent)
        {
            EquipPrefab item = Instantiate(equipItemPrefab, parent);

            if (isEquipped)
            {
                item.SetupEquippedWeapon(index);
            }
            else
            {
                item.SetupNotEquippedWeapon(index, equipComparePanel, equippedItemPanel, comparedItemPanel, setNewEquippedBtn, this);
            }
            item.transform.localScale = Vector3.one;
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
                item.SetupNotEquppedArmorOrShield(index, equipComparePanel, equippedItemPanel, comparedItemPanel, setNewEquippedBtn, this);
            }
            item.transform.localScale = Vector3.one;
        }

        void DeleteAllEquipListChildren()
        {
            for (int i = equipListContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(equipListContainer.GetChild(i).gameObject);
            }
        }

        void CloseCategoryList()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            equipListRect.SetActive(false);
            DeleteAllEquipListChildren();
        }

        void CloseComparePanel()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            equipComparePanel.SetActive(false);
        }
    }

}