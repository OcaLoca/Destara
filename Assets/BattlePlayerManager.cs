/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */

using AV;
using StarworkGC.Localization;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Game.BattleController;
using Unit = Game.BattleController.Unit;

namespace Game
{
    public class BattlePlayerManager : MonoBehaviour
    {
        [Header("PlayerGameobject")]
        [SerializeField] GameObject[] classWeaponContainer;
        [SerializeField] GameObject transformPositionForTargetPlayer;
        List<GameObject> allies = new List<GameObject>();
        List<ScriptableAbility> abilities = new List<ScriptableAbility>();
        List<Weapon> unitWeapons = new List<Weapon>();
        [SerializeField] GameObject bagContainer;
        [SerializeField] GameObject targetPanel;
        [SerializeField] BattleController battleController;
        [SerializeField] BattleView battleView;

        [Header("UI")]
        public Button[] fightButtons, weaponsButtons, abilityButtons, shieldsButtons, inventoryButtons;
        public GameObject abilityPanel, weaponsPanel, inventoryPanel, shieldsPanel;
        [SerializeField] Button btnBackAbilitiesPanel, btnBackWeaponPanel, btnCloseInventoryPanel, btnCloseShieldsPanel;
        public TMP_Text txtAvaiablePowerUpCount;
        [SerializeField] public TMP_Text currentTxtAPCount;
        [SerializeField] TMP_Text maxTxtAPCount;
        [SerializeField] TMP_Text txtPopupDamage, txtPopupDescription;

        [SerializeField] Image ClassIcon;
        [SerializeField] internal Image defencePlayerBattleImage;
        [SerializeField] List<Sprite> iconClassDatabase;

        [SerializeField] GameObject BagNoItemsMsg;

        [Header("BagManager")]
        [SerializeField] Button btnCloseBag;
        [SerializeField] Button btnSelectItem;
        [SerializeField] TMP_Text txtItemDescription;
        Vector3 bufforiginalPosition;
        [SerializeField] BagButtonManager tmpBagButtonManager;

        public List<Unit> targets;
        public static Vector3 PlayerTransformPosition;
        public static Vector3 AllyTransformPosition;

        [SerializeField] internal GameObject[] effectStatsIcon;

        private void Awake()
        {
            allies.Add(transformPositionForTargetPlayer);
            PlayerTransformPosition = transformPositionForTargetPlayer.transform.localPosition;
        }

        Vector3 weaponContainerOriginalPosition = Vector3.zero;

        public void SetClassBattleUI(byte classIndex)
        {
            this.classIndex = classIndex;
        }

        int classIndex;
        bool mainPlayer;
        public bool IsMainPlayer { get => mainPlayer; }
        public void ChangeUICharacterInBattle(Unit unit)
        {
            DestroyBagButtonInstance();
            if (unit.mainCharacter)
            {
                mainPlayer = true;
                Vector3 starterPosition = new Vector3(-375f, -377f, 0);
            }
            else
            {
                mainPlayer = false;
                Vector3 vector3 = new Vector3(339f, -377f, 0);
                foreach (ScriptableItem item in unit.equippedItem)
                {
                    AddAllyItemToInventory(unit, item, item.GetObjectQuantity);
                }
            }
            LoadPlayerConsumableForBagInTheBattle(unit);
        }
        public void DestroyBagButtonInstance()
        {
            foreach (Transform child in bagContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void AddAllyItemToInventory(Unit unit, ScriptableItem item, int quantity)
        {
            if (unit.bag.ContainsKey(item))
            {
                unit.bag[item] += quantity;
            }
            else
            {
                unit.bag.Add(item, quantity);
            }
        }

        private void OnEnable()
        {
            ClassIcon.sprite = iconClassDatabase[classIndex];

            btnBackAbilitiesPanel.onClick.RemoveAllListeners();
            btnBackWeaponPanel.onClick.RemoveAllListeners();
            btnCloseInventoryPanel.onClick.RemoveAllListeners();
            btnCloseShieldsPanel.onClick.RemoveAllListeners();
            btnCloseBag.onClick.RemoveAllListeners();

            btnBackAbilitiesPanel.onClick.AddListener(() => CloseButton(abilityPanel));
            btnBackWeaponPanel.onClick.AddListener(() => CloseButton(weaponsPanel));
            btnCloseInventoryPanel.onClick.AddListener(() => CloseButton(inventoryPanel));
            btnCloseShieldsPanel.onClick.AddListener(() => CloseButton(shieldsPanel));
            btnCloseBag.onClick.AddListener(() => CloseTargetPanel());

            Debug.LogWarning("Riattivare questo tasto se si volesse baffare");

            switch (battleController.allies.Count)
            {
                case 1:
                    // allyPlayer.gameObject.SetActive(false);
                    break;
                case 2:
                    //  allyPlayer.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }

            foreach (Button btn in fightButtons)
            {
                btn.onClick.RemoveAllListeners();
            }
            foreach (Button btn in abilityButtons)
            {
                btn.onClick.RemoveAllListeners();
            }
            foreach (Button btn in weaponsButtons)
            {
                btn.onClick.RemoveAllListeners();
            }
            foreach (Button btn in shieldsButtons)
            {
                btn.onClick.RemoveAllListeners();
            }
            foreach (Button btn in inventoryButtons)
            {
                btn.onClick.RemoveAllListeners();
            }

            fightButtons[0].onClick.AddListener(delegate { InBattleOpenPlayerWeaponsPanel(); });
            fightButtons[1].onClick.AddListener(delegate { InBattleOpenPlayerAbilitiesPanel(); });
            fightButtons[2].onClick.AddListener(delegate { PlayerRest(); });
            fightButtons[3].onClick.AddListener(delegate { InBattleOpenPlayerInventoryPanel(); });

            inventoryButtons[0].onClick.AddListener(delegate { InBattleOpenPlayerDefencePanel(); });
            inventoryButtons[1].onClick.AddListener(delegate { OpenBag(); });

            weaponsButtons[0].onClick.AddListener(delegate { battleController.OnPlayerClickAttack(isMainPlayer, 0); });
            weaponsButtons[1].onClick.AddListener(delegate { battleController.OnPlayerClickAttack(isMainPlayer, 1); });
            weaponsButtons[2].onClick.AddListener(delegate { battleController.OnPlayerClickAttack(isMainPlayer, 2); });
            weaponsButtons[3].onClick.AddListener(delegate { battleController.OnPlayerClickAttack(isMainPlayer, 3); });

            shieldsButtons[0].onClick.AddListener(delegate { battleController.OnPlayerChangeDefence(0); });
            shieldsButtons[1].onClick.AddListener(delegate { battleController.OnPlayerChangeDefence(1); });
            shieldsButtons[2].onClick.AddListener(delegate { battleController.OnPlayerChangeDefence(2); });

            abilityButtons[0].onClick.AddListener(delegate { ReturnFirstAbility(); });
            abilityButtons[1].onClick.AddListener(delegate { ReturnSecondAbility(); });
            abilityButtons[2].onClick.AddListener(delegate { ReturnThirdAbility(); });
            abilityButtons[3].onClick.AddListener(delegate { ReturnFourthAbility(); });

            //btnFinalMove.onClick.RemoveAllListeners();
            //btnFinalMove.onClick.AddListener(delegate { FinalMove(); });
        }

        void CloseButton(GameObject panelToClose)
        {
            panelToClose.gameObject.SetActive(false);
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
        }


        [SerializeField] ScrollRect scrollRect;
        [SerializeField] Scrollbar verticalScrollbar;
        [SerializeField] RectTransform bagContent;
        [SerializeField] RectTransform targetContent;
        Vector2 newHeight = new Vector2(0, 0);
        List<BagButtonManager> bagButtonsList = new List<BagButtonManager>();

        bool noItem;
        public void LoadPlayerConsumableForBagInTheBattle(Unit unit)
        {
            Dictionary<ScriptableItem, int> playerItems = unit.bag;
            List<ConsumableItem> consumablesItems = new List<ConsumableItem>();
            List<int> numberOfItem = new List<int>();

            foreach (ConsumableItem item in playerItems.Keys.OfType<ConsumableItem>())
            {
                if (item.consumableInFight)
                {
                    consumablesItems.Add(item);
                    numberOfItem.Add(playerItems[item]);
                }
            }

            bagButtonsList.Clear();
            newHeight.y = 0;
            bagContent.sizeDelta = newHeight;

            int bagCount = consumablesItems.Count;

            if (bagCount <= 0)
            {
                noItem = true;
                Debug.Log("Mettere un immagine per dire che è vuota");
            }
            else
            {
                noItem = false;
                verticalScrollbar.gameObject.SetActive(true);
                scrollRect.enabled = true;
                for (int i = 0; i < bagCount; i++)
                {
                    newHeight.y = newHeight.y + 180;
                }
                bagContent.sizeDelta = newHeight;

                byte indexOfNumberList = 0;
                foreach (ConsumableItem consumable in consumablesItems)
                {
                    BagButtonManager bagButtonManager = Instantiate(tmpBagButtonManager);
                    bagButtonsList.Add(bagButtonManager);
                    bagButtonManager.Setup(consumable, numberOfItem[indexOfNumberList]);
                    indexOfNumberList++;
                    bagButtonManager.transform.SetParent(battleView.GetItemContainer.GetComponent<RectTransform>().transform, false);
                    Button button = bagButtonManager.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate
                    {
                        OnClickItemInBattleBag(consumable, bagButtonManager, unit);
                        ShowItemDescriptionInBattleBag(consumable);
                    });
                }
            }
        }

        void TurnOffWeaponButtonClickAdvice(int index)
        {
            weaponsButtons[index].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(false);
        }
        void TurnOnWeaponButtonClickAdvice(int index)
        {
            weaponsButtons[index].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(true);
        }

        void TurnOffAbilityButtonClickAdvice(int index)
        {
            abilityButtons[index].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(false);
        }
        void TurnOnAbilityButtonClickAdvice(int index)
        {
            abilityButtons[index].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(true);
        }

        void TurnOffDefencesButtonClickAdvice(int index)
        {
            shieldsButtons[index].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(false);
        }
        void TurnOnDefencesButtonClickAdvice(int index)
        {
            shieldsButtons[index].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(true);
        }

        void TurnOffRestButtonClickAdvice()
        {
            fightButtons[2].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(false);
        }
        void TurnOnRestButtonClickAdvice()
        {
            fightButtons[2].GetComponentInChildren<Transform>().Find("ButtonBorderEffect").gameObject.SetActive(true);
        }

        void OnClickItemInBattleBag(ConsumableItem item, BagButtonManager btn, Unit unit)
        {
            foreach (BagButtonManager bgButton in bagButtonsList)
            {
                bgButton.TurnOffBagButton();
            }
            btn.TurnOnSelectedBagButton(item);

            int mainPlayer = battleController.allUnits.FindIndex(u => u.isControllable && u.mainCharacter);

            btnSelectItem.onClick.RemoveAllListeners();
            btnSelectItem.onClick.AddListener(delegate { ClickOnUse(item, battleController.allUnits[mainPlayer]); });
        }

        internal void ActivatePlayerIconStatsEffect()
        {
            DeactivateEffectStatsIconContainerChild();

            if (battleController.GetIsBurned)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.burned);
            }
            if (battleController.GetIsConfused)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.confused);
            }
            if (battleController.GetIsParalyzed)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.paralyzed, ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.AP_COLOR));
            }
            if (battleController.GetIsPoisoned)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.poisoned, ColorDatabase.Singleton.GetRarityColor(ScriptableItem.Rarity.Legendary));
            }
            if (battleController.GetIsFreezed)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.freezed);
            }
            if (battleController.GetIsInvulnerable)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.invurneable);
            }
        }
        internal void DeactivateEffectStatsIconContainerChild()
        {
            foreach (var obj in effectStatsIcon)
            {
                obj.gameObject.SetActive(false);
                Transform childTransform = obj.transform.GetChild(0);
                childTransform.GetComponentInChildren<Image>().sprite = null;
            }
        }

        public void AssignSpriteToEmptyImages(PlayerManager.Stats stat, Color32? color = null)
        {
            Sprite spriteToAssign = IconsDatabase.Singleton.GetEffectStatSpriteByStatusType(stat);

            if (spriteToAssign == null)
            {
                return;
            }

            foreach (GameObject obj in effectStatsIcon)
            {
                if (obj == null)
                    continue;

                Transform childTransform = obj.transform.GetChild(0);
                Image icon = childTransform.GetComponentInChildren<Image>();

                if (icon.sprite == null)
                {
                    obj.SetActive(true);
                    icon.sprite = spriteToAssign;

                    if (color != null)
                    {
                        icon.color = (Color32)color;
                    }
                    break;
                }
            }
        }


        internal static string effectObjectLog;
        void ClickOnUse(ConsumableItem item, Unit unit)
        {

            switch (item.tartgetType)
            {
                case ConsumableItem.TargetType.Player:

                    if (BattleControllerHelper.Instance.BattleUselessItem(unit, item))
                    {
                        txtItemDescription.text = Localization.Get(LocalizationIDDatabase.DONT_USE_OBJECT);
                        OpenTargetPanel();
                        return;
                    }

                    unit = PlayerUseObjectOnHimself(ref unit, item);
                    battleController.UpdateActorStats(unit);
                    bagContainer.gameObject.SetActive(false);
                    break;
                default:
                    Debug.Log("Nessun tipo di target specificato nell'oggetto");
                    break;
            }

            battleController.PlayerUseObject = true;
            UpdateQuantityNumberOfTheObj(unit, item);
            RefreshInfodescription();

            battleController.playerPlayed = true;
            string useLog = string.Format(Localization.Get(LocalizationIDDatabase.USE_BATTLE_LOG), unit.name, item.GetLocalizedObjName());
            consoleLog = useLog + "\n" + effectObjectLog;

            StartCoroutine(battleController.ClosePlayerCommandPanel(0));
        }

        Unit PlayerUseObjectOnHimself(ref Unit unit, ConsumableItem item)
        {
            unit = item.TriggerItemInBattle(ref unit, item);
            //GameApplication.Singleton.model.playerHealthbarManager.UpdateCurrentLife(unit.hp);
            return unit;
        }

        void UpdateQuantityNumberOfTheObj(Unit unit, ScriptableItem item)
        {
            unit.bag[item]--; // abbasso di uno la quantità dell'oggetto.

            if (unit.bag[item] <= 0)
            {
                unit.bag.Remove(item);
            }
        }

        void OpenTargetPanel()
        {
            targetPanel.gameObject.SetActive(true);
        }
        void CloseTargetPanel()
        {
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
            targetPanel.gameObject.SetActive(false);
            bagContainer.gameObject.SetActive(false);
        }

        void ShowItemDescriptionInBattleBag(ScriptableItem item)
        {
            item.SetLocalizedText();
            txtItemDescription.text = Localization.Get(item.infoDescriptionLocalized);
            OpenTargetPanel();
            //txtItemValue.text = Localization.Get(item.infoDescriptionLocalized);
        }

        void RefreshInfodescription()
        {
            txtItemDescription.text = string.Empty;
            //txtItemValue.text = string.Empty;
        }

        public void OpenBag()
        {
            if (noItem)
            {
                BagNoItemsMsg.SetActive(true);
                Debug.Log("Non hai oggetti");
            }
            else
            {
                UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericMenuButton);
                bagContainer.gameObject.SetActive(true);
            }
        }

        public void PlayerRest()
        {
            StartCoroutine(battleController.OnClickDuringBattleRest());
        }

        public void ReturnFirstAbility()
        {
            battleController.WaitInput(0);  //aspetto input
            StartCoroutine(battleController.ChoiceAbility()); //okey è stata scelta
            battleController.UseAbility();
        }
        public void ReturnSecondAbility()
        {
            battleController.WaitInput(1);
            StartCoroutine(battleController.ChoiceAbility());
            battleController.UseAbility();
        }
        public void ReturnThirdAbility()
        {
            battleController.WaitInput(2);
            StartCoroutine(battleController.ChoiceAbility());
            battleController.UseAbility();
        }
        public void ReturnFourthAbility()
        {
            battleController.WaitInput(3);
            StartCoroutine(battleController.ChoiceAbility());
            battleController.UseAbility();
        }

        public void Destroy(Unit unit)
        {
            foreach (GameObject actor in allies)
            {
                if (unit.placeID == actor.name)
                {
                    actor.SetActive(false);
                }
            }
        }

        void ChangeInfoItem()
        {
            if (txtItemDescription.gameObject.activeSelf)
            {
                //txtItemValue.gameObject.SetActive(true);
                txtItemDescription.gameObject.SetActive(false);
            }
            else
            {
                //txtItemValue.gameObject.SetActive(false);
                txtItemDescription.gameObject.SetActive(true);
            }
        }

        public ScriptableAbility SearchAbility()
        {
            foreach (Button button in abilityButtons)
            {
                string text = button.GetComponentInChildren<TMP_Text>().text;

                for (int i = 0; i < abilities.Count; i++)
                {
                    if (text == abilities[i].abilityName)
                    {
                        return abilities[i];
                    }
                }
            }
            return null;
        }

        bool isMainPlayer = true;

        void InBattleOpenPlayerWeaponsPanel()
        {
            weaponsPanel.gameObject.SetActive(true);
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericMenuButton);
            battleController.UnlockPlayerAbility();
        }

        void InBattleOpenPlayerAbilitiesPanel()
        {
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericMenuButton);
            abilityPanel.gameObject.SetActive(true);
            battleController.UnlockPlayerAbility();
        }

        void InBattleOpenPlayerInventoryPanel()
        {
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericMenuButton);
            inventoryPanel.gameObject.SetActive(true);
        }
        void InBattleOpenPlayerDefencePanel()
        {
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericMenuButton);
            shieldsPanel.gameObject.SetActive(true);
        }

        Unit tmpLastUnit = new Unit();
        public void CheckAvaiableAbility(Unit unit)
        {
            tmpLastUnit = unit;

            foreach (Button button in abilityButtons)
            {
                string text = button.GetComponentInChildren<TMP_Text>().text;

                for (int i = 0; i < unit.ability.Count; i++)
                {
                    if (unit.ability[i] == null) { TurnOffAbilityButtonClickAdvice(i); }
                    else
                    {
                        if (text == unit.ability[i].abilityName)
                        {
                            if (unit.ability[i].APcost >= unit.abilityPoints)
                            {
                                TurnOnAbilityButtonClickAdvice(i);
                            }
                            else
                            {
                                TurnOnAbilityButtonClickAdvice(i);
                            }
                        }
                    }
                }
            }
        }

        public void CheckAvaiableWeapon(Unit unit)
        {
            tmpLastUnit = unit;

            foreach (Button button in weaponsButtons)
            {
                string text = button.GetComponentInChildren<TMP_Text>().text;

                for (int i = 0; i < unit.equippedWeapon.Count; i++)
                {
                    if (unit.equippedWeapon[i] == null) { TurnOffWeaponButtonClickAdvice(i); }

                    else
                    {
                        string localizedWeaponName = Localization.Get(unit.equippedWeapon[i].itemNameLocalization);

                        if (text == localizedWeaponName)
                        {
                            if (unit.equippedWeapon[i].staminaCost > unit.stamina)
                            {
                                TurnOffWeaponButtonClickAdvice(i);
                            }
                            else
                            {
                                TurnOnWeaponButtonClickAdvice(i);
                            }
                        }
                    }
                }
            }
        }

        // Con questa funzione le abilità si sbloccano in base ai power up disponibili
        public void UnlockAbility(Unit unit)
        {
            foreach (Button button in abilityButtons)
            {
                string text = button.GetComponentInChildren<TMP_Text>().text;
                List<ScriptableAbility> abilities = unit.ability;

                for (int i = 0; i < unit.ability.Count; i++)
                {
                    if (abilities[i] == null) { button.interactable = false; }
                    else
                    {
                        if (text == abilities[i].abilityName)
                        {
                            if (abilities[i].APcost <= unit.abilityPoints)
                            {
                                button.interactable = true;
                                button.GetComponent<Image>().enabled = true;
                                TurnOnAbilityButtonClickAdvice(i);
                                button.GetComponentInChildren<Animator>().SetBool("isActive", true);

                            }
                            else
                            {
                                button.interactable = true;
                                button.GetComponent<Image>().enabled = false;
                                TurnOffAbilityButtonClickAdvice(i);
                                button.GetComponentInChildren<Animator>().SetBool("isActive", false);
                            }
                        }
                    }
                }
            }
        }

        public void PreparePlayerHUD(Unit unit)
        {
            PrepareAbilityHUD(unit);
            PrepareWeaponsHUD(unit);
            PrepareRestHUD(unit);
            TakePlayerEquippedDefences(unit);
        }

        void TakePlayerEquippedDefences(Unit unit)
        {
            LoadPlayerDefencesButtonsText(unit);
            defencePlayerBattleImage.sprite = IconsDatabase.Singleton.GetArmorSpriteByDefenceType(unit.DefenceType);
        }

        void LoadPlayerDefencesButtonsText(Unit unit)
        {
            int i = 0;
            int equippedDefenceTypeIndex = 0;

            switch (unit.DefenceType)
            {
                case TypeDatabase.DefenseType.Light:
                    equippedDefenceTypeIndex = 0;
                    break;
                case TypeDatabase.DefenseType.Avarage:
                    equippedDefenceTypeIndex = 1;
                    break;
                case TypeDatabase.DefenseType.Heavy:
                    equippedDefenceTypeIndex = 2;
                    break;
            }

            List<Equipment> playerEquippedShields = new List<Equipment>();
            // playerEquippedShields.Add(PlayerManager.Singleton.playerEquipment.equippedClassDefaultDefence);
            playerEquippedShields.Add(PlayerManager.Singleton.playerEquipment.equippedLightDefence);
            playerEquippedShields.Add(PlayerManager.Singleton.playerEquipment.equippedBalancedDefence);
            playerEquippedShields.Add(PlayerManager.Singleton.playerEquipment.equippedHeavyDefence);

            foreach (Button button in shieldsButtons)
            {
                if (playerEquippedShields[i] == null) { button.GetComponentInChildren<TMP_Text>().text = ""; }
                else
                {
                    button.GetComponentInChildren<TMP_Text>().text = Localization.Get(playerEquippedShields[i].itemNameLocalization);
                    if (playerEquippedShields[i].StaminaCost <= unit.stamina)
                    {
                        TurnOnDefencesButtonClickAdvice(i);
                    }
                    else
                    {
                        TurnOffDefencesButtonClickAdvice(i);
                    }
                }
                i++;
            }
            TurnOffDefencesButtonClickAdvice(equippedDefenceTypeIndex);
        }

        public void PrepareAbilityHUD(Unit unit)
        {
            currentTxtAPCount.text = unit.abilityPoints.ToString();
            maxTxtAPCount.text = unit.inteligence.ToString();
            battleController.abilityClicked = false;
            ChangeAbility(unit);
            ChangeAbilityCost(unit);
            ChangeAbilityText(unit);
            CheckAvaiableAbility(unit);
        }

        public void PrepareWeaponsHUD(Unit unit)
        {
            battleController.abilityClicked = false;
            SetUnitWeapons(unit);
            ShowWeaponsStaminaCost(unit);
            ShowDefencesStaminaCost();
            ChangeWeaponsBtnText(unit);
            CheckAvaiableWeapon(unit);
        }

        public void PrepareRestHUD(Unit unit)
        {
            float maxStamina = PlayerManager.Singleton.dexterity * 10;

            if (unit.stamina >= maxStamina)
            {
                TurnOffRestButtonClickAdvice();
            }
            else TurnOnRestButtonClickAdvice();
        }

        void ChangeWeaponsBtnText(Unit unit)
        {
            int i = 0;
            foreach (Button button in weaponsButtons)
            {
                if (unit.equippedWeapon[i] == null) { button.GetComponentInChildren<TMP_Text>().text = ""; }
                else
                {
                    button.GetComponentInChildren<TMP_Text>().text = Localization.Get(unit.equippedWeapon[i].itemNameLocalization);
                }
                i++;
            }
        }
        void ChangeAbilityText(Unit unit)
        {
            int i = 0;
            foreach (Button button in abilityButtons)
            {
                if (unit.ability[i] == null) { button.GetComponentInChildren<TMP_Text>().text = ""; }
                else
                {
                    button.GetComponentInChildren<TMP_Text>().text = unit.ability[i].abilityName;
                }
                i++;
            }
        }

        public void PreparePlayerFloatingPopup(Unit unit, float damageAmount, bool damageSourceIsStatus)
        {
            if (!damageSourceIsStatus)
            {
                if (enemyFailedToAttack)
                {
                    txtPopupDescription.text = Localization.Get(LocalizationIDDatabase.MISS_POPUP);
                    txtPopupDescription.gameObject.SetActive(true);
                    return;
                }

                if (damageIsCritical)
                {
                    txtPopupDescription.text = Localization.Get(LocalizationIDDatabase.CRITICAL_POPUP);
                    txtPopupDescription.gameObject.SetActive(true);
                }
            }

            txtPopupDamage.text = ((int)damageAmount).ToString();
            BattleEnemyManager.CheckDamage(damageAmount, txtPopupDamage);
            txtPopupDamage.gameObject.SetActive(true);
        }

        [SerializeField] TMP_Text txtFirstStaminaCost;
        [SerializeField] TMP_Text txtSecondStaminaCost;
        [SerializeField] TMP_Text txtThirdStaminaCost;
        [SerializeField] TMP_Text txtFourthStaminaCost;

        List<TMP_Text> weaponCost = new List<TMP_Text>();
        void ShowWeaponsStaminaCost(Unit unit)
        {
            weaponCost.Clear();
            weaponCost.Add(txtFirstStaminaCost);
            weaponCost.Add(txtSecondStaminaCost);
            weaponCost.Add(txtThirdStaminaCost);
            weaponCost.Add(txtFourthStaminaCost);

            foreach (TMP_Text txt in weaponCost)
            {
                txt.text = string.Empty;
            }
            int count = unit.equippedWeapon.Count;

            if (count == 0) { return; }

            for (int i = 0; i < count; i++)
            {
                if (unit.equippedWeapon[i] == null) { continue; }
                else
                {
                    int cost = (int)unit.equippedWeapon[i].staminaCost;
                    weaponCost[i].text = "st " + cost.ToString();
                }
            }
        }

        [SerializeField] TMP_Text txtFirstDefenceStaminaCost;
        [SerializeField] TMP_Text txtSecondDefenceStaminaCost;
        [SerializeField] TMP_Text txtThirdDefenceStaminaCost;

        List<TMP_Text> defenceCost = new List<TMP_Text>();
        void ShowDefencesStaminaCost()
        {
            defenceCost.Clear();
            defenceCost.Add(txtFirstDefenceStaminaCost);
            defenceCost.Add(txtSecondDefenceStaminaCost);
            defenceCost.Add(txtThirdDefenceStaminaCost);

            foreach (TMP_Text txt in defenceCost)
            {
                txt.text = string.Empty;
            }

            txtFirstDefenceStaminaCost.text = "ST" + PlayerManager.Singleton.playerEquipment.equippedLightDefence.StaminaCost.ToString();
            txtSecondDefenceStaminaCost.text = "ST" + PlayerManager.Singleton.playerEquipment.equippedBalancedDefence.StaminaCost.ToString();
            txtThirdDefenceStaminaCost.text = "ST" + PlayerManager.Singleton.playerEquipment.equippedHeavyDefence.StaminaCost.ToString();
        }

        [SerializeField] TMP_Text txtFirstAPCost;
        [SerializeField] TMP_Text txtSecondAPCost;
        [SerializeField] TMP_Text txtThirdAPCost;
        [SerializeField] TMP_Text txtFourthAPCost;

        [SerializeField] List<TMP_Text> abilitiesCost = new List<TMP_Text>();
        void ChangeAbilityCost(Unit unit)
        {
            abilitiesCost.Add(txtFirstAPCost);
            abilitiesCost.Add(txtSecondAPCost);
            abilitiesCost.Add(txtThirdAPCost);
            abilitiesCost.Add(txtFourthAPCost);

            foreach (TMP_Text txt in abilitiesCost)
            {
                txt.text = string.Empty;
            }

            int count = unit.ability.Count;

            if (count == 0) { return; }

            for (int i = 0; i < count; i++)
            {
                if (unit.ability[i] == null) { continue; }
                else
                {
                    int cost = unit.ability[i].APcost;
                    string zero = cost >= 10 ? "ap " + cost.ToString() : "ap 0" + cost.ToString();
                    abilitiesCost[i].text = zero;
                }
            }
        }

        void ChangeAbility(Unit unit)
        {
            abilities = unit.ability;
        }

        void SetUnitWeapons(Unit unit)
        {
            unitWeapons = unit.equippedWeapon;
        }

    }
}
