using StarworkGC.Localization;
using StarworkGC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.TypeDatabase;
using static UnityEngine.Rendering.DebugUI;
//using static UnityEditor.Progress;

namespace Game
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Singleton { get; set; }
        public DeathController deathController;
        public BattleController battleController;

        [Header("GameFunction")]
        public float playerChoice;
        public static string COWARD_DESCRIPTION = "coward_description";
        public static string FEARLESS_DESCRIPTION = "fearless_description";
        public static string INSANE_DESCRIPTION = "insane_description";
        public SelectedLanguage selectedLanguage;
        public enum SelectedLanguage
        {
            None,
            Italiano,
            Inglese,
            Francese,
            Spagnolo,
            Portoghese,
            Tedesco,
            Giapponese
        }

        public List<string> pagesRead;
        public List<string> pagesDropped;
        public List<string> goalAlreadyUnlocked;
        public ScriptablePage currentPage => GetCurrentPage();
        public string lastPage;

        [Header("SelectedClassModel")]
        public Sprite classImage;
        public List<ScriptableAbility> playerAbility = new List<ScriptableAbility>();
        public List<ScriptableAbility> highPlayerAbility = new List<ScriptableAbility>();
        public ScriptableAbility finalAbility;
        public ScriptableAbility highFinalAbility;
        public List<ScriptableAbility> GetPlayerAbilities() { return playerAbility; }

        [SerializeField] PagesMaleDatabase pagesMaleDatabase;
        [SerializeField] PagesFemaleDatabase pagesFemaleDatabase;

        public enum Difficulty
        {
            Coward,
            Fearless,
            Insane,
            Null
        }

        [Header("PlayerStats")]
        public string className;
        [Range(0, 100)]
        public int courage;
        [Range(0, 100)]
        public int lucky;
        [Range(0, 100)]
        public int superstition;
        public bool isMale;
        public string playerName;
        public int GetPlayerLevel { get => playerLevel; }
        public float GetPlayerExp() { return playerExperience; }
        public float GetPlayerMaxHP() { return constitution * 10; }
        public void SetPlayerExp(float newExp) { playerExperience = newExp; }
        public void SetPlayerLvl(int newLvl) { playerLevel = newLvl; }

        [SerializeField] int playerLevel;
        [SerializeField] float playerExperience;

        public int maxTorchCount;
        public int currentTorch;
        public int maxPaperCount;
        public int currentPaper;
        public string classPlayerTypeToString;
        public List<string> buttonClicked = new List<string>();
        [SerializeField] bool isDead;

        internal Difficulty lastDifficultyChoice;

        private Difficulty _selectedDifficulty;
        public Difficulty selectedDifficulty
        {
            get
            {
                // Verifica se il valore è "Null" e, in tal caso, assegna un valore di fallback
                if (_selectedDifficulty == Difficulty.Null)
                {
                    // Assegna lastDifficultyChoice se esiste, altrimenti fallback a "Coward"
                    _selectedDifficulty = (lastDifficultyChoice != Difficulty.Null) ? lastDifficultyChoice : Difficulty.Coward;
                }
                return _selectedDifficulty;
            }
            set
            {
                // Consente di impostare un nuovo valore
                _selectedDifficulty = value;
            }
        }


        public ScriptableClass selectedClass;
        public ScriptablePage.Section lastChapter;
        public bool CheckPlayerIsDead { get => isDead; }

        [Header("PlayerBattleStats")]
        public float constitution;
        public float dexterity;
        public float strength;
        public float inteligence;
        public float lifePoints;
        public int currentAP;

        [SerializeField] internal AudioClip dodgeSound, classScreamSound;


        //int abilityPoints;
        public int GetPlayerAbilityPoints { get => currentAP; }
        public int GetAbilityPointsLimit() { return (int)inteligence; }
        public void SetPlayerAbilityPoints(int newAbilityPoints) { currentAP = newAbilityPoints; }

        float stamina;
        public float GetPlayerStamina { get => stamina; }
        public int GetStaminaLimit() { return (int)dexterity * 10; }
        public void SetPlayermanagerStamina(float newStamina) { stamina = newStamina; }

        public float defence;
        public float elementalLevel = 65; //da implementare 
        public ElementalBuff[] elementalBuffs;
        public static int powerUpAvaiable;
        public bool noStatus;
        public string baseAttackName;

        public Dictionary<Stats, int> healedUnit = new Dictionary<Stats, int>();
        public enum Stats
        {
            none,
            burned,
            poisoned,
            confused,
            paralyzed,
            freezed,
            invurneable,
            unable
        }

        void CreateStats()
        {
            healedUnit.Add(Stats.burned, 0);
            healedUnit.Add(Stats.confused, 0);
            healedUnit.Add(Stats.paralyzed, 0);
            healedUnit.Add(Stats.poisoned, 0);
            healedUnit.Add(Stats.freezed, 0);
            healedUnit.Add(Stats.invurneable, 0);
            healedUnit.Add(Stats.unable, 0);
        }

        public ScriptablePage GetCurrentPage()
        {
            if (pagesRead.Count == 0) return null;

            return pagesMaleDatabase.GetPageByID(pagesRead[pagesRead.Count - 1]);

        }

        public Bag bag;

        public struct EquippedWeapon
        {
            public Weapon lightWeapon;
            public Weapon rangeWeapon;
            public Weapon heavyWeapon;
            public Weapon specialWeapon;
        }
        public EquippedWeapon playerWeapon;

        public List<Weapon> equippedWeapon;
        public List<Weapon> EquippedWeaponToList()
        {
            equippedWeapon = new List<Weapon>() { playerWeapon.lightWeapon, playerWeapon.rangeWeapon, playerWeapon.heavyWeapon, playerWeapon.specialWeapon };

            return equippedWeapon;
        }

        //Array contenenti tutte le armi del giocatore
        public List<ScriptableItem> lightWeaponList;
        public List<ScriptableItem> rangeWeaponList;
        public List<ScriptableItem> heavyWeaponList;
        public List<ScriptableItem> specialWeaponList;

        public struct PlayerEquipment
        {
            public Equipment equippedClassDefaultDefence;
            public Equipment equippedLightDefence;
            public Equipment equippedBalancedDefence;
            public Equipment equippedHeavyDefence;
            public Equipment equippedGemstone;
            public Equipment equippedTalisman;
            public Equipment equippedRelic;
        }
        public PlayerEquipment playerEquipment;


        public DefenseType initialEquipmentDefenseType;

        public Equipment GetDefaultClassDefence()
        {
            return playerEquipment.equippedClassDefaultDefence;
        }

        public Equipment GetDefaultClassDefense()
        {
            switch (initialEquipmentDefenseType)
            {
                case DefenseType.Light:
                    return playerEquipment.equippedLightDefence;
                case DefenseType.Avarage:
                    return playerEquipment.equippedBalancedDefence;
                case DefenseType.Heavy:
                    return playerEquipment.equippedHeavyDefence;
                default:
                    return null;
            }
        }

        //Array contenenti tutti gli equip del giocatore
        public List<ScriptableItem> lightDefenceList;
        public List<ScriptableItem> balancedDefenceList;
        public List<ScriptableItem> heavyDefenceList;
        /// <summary>
        /// Talisman list
        /// </summary>
        public List<ScriptableItem> talismansList;
        /// <summary>
        /// Relic list
        /// </summary>
        public List<ScriptableItem> relicsList;
        /// <summary>
        /// Gemstone list
        /// </summary>
        public List<ScriptableItem> gemstonesList;

        float totalWeightArmor;


        [Header("Inventory")]
        public List<ScriptableItem> ObjInventoryInTheInspector = new List<ScriptableItem>();
        public List<int> QuantityInventoryInTheInspector = new List<int>();
        public Dictionary<ScriptableItem, int> inventory = new Dictionary<ScriptableItem, int>();

        void UpdateInventoryInspector(ScriptableItem item = null)
        {
            if (item == null)
            {
                return;
            }
            Singleton.ObjInventoryInTheInspector.Add(item);
            Singleton.QuantityInventoryInTheInspector.Add(item.GetObjectQuantity);
            if (item.ID == "Torcia")
            {
                GameApplication.Singleton.view.BookView.ManageTheSearchButton(item);
            }
        }


        public void CleanInventory()
        {
            inventory.Clear();
            inventory = null;
            QuantityInventoryInTheInspector?.Clear();
            ObjInventoryInTheInspector?.Clear();
            inventory = new Dictionary<ScriptableItem, int>();
            heavyWeaponList.Clear();
            lightWeaponList.Clear();
            rangeWeaponList.Clear();
            specialWeaponList.Clear();
            lightDefenceList.Clear();
            balancedDefenceList.Clear();
            heavyDefenceList.Clear();
            talismansList.Clear();
            relicsList.Clear();
        }

        public int TakeItemNumberFromInventory(ScriptableItem item)
        {
            if (inventory.ContainsKey(item))
            {
                int value = inventory[item];
                return value;
            }
            else return 0;
        }

        public bool LevelChange(int tmpLevel)
        {
            return tmpLevel < playerLevel;
        }
        public void CleanEquipment()
        {
            // playerEquipment.equippedAccessory1 = null;
            // playerEquipment.equippedAccessory2 = null;
            // playerEquipment.equippedHeadEquipment = null;
            // playerEquipment.equippedShieldEquipment = null;
            // playerEquipment.equippedTorsoEquipment = null;
            // playerWeapon.heavyWeapon = null;
            // playerWeapon.lightWeapon = null;
            // playerWeapon.rangeWeapon = null;
            // playerWeapon.specialWeapon = null; commento o appena rifaccio mi da null reference 
        }

        void Awake()
        {
            UpdateInventoryInspector();

            pagesRead = new List<string>();
            DontDestroyOnLoad(gameObject);
            Singleton = this;
            UpdateLevel(true);
        }

        void Start()
        {
            CreateStats();
        }

        public void PlayerDeathInBattle()
        {
            lifePoints = 0;
            UpdateDeathPlayerManagerCurrentState();
        }

        public void UpdateDeathPlayerManagerCurrentState()
        {
            isDead = lifePoints > 0 ? false : true;
        }

        public void LoadLastRunSave(SaveType saveType, out string pageToLoad, bool notChoiceClass = false, ScriptableClass classToLoad = null, string slotID = null)
        {
            PlayerData data = SaveSystem.LoadLastRun();
            pageToLoad = data.currentPageID;

            if (data == null) { Debug.LogWarning("Caricamento fallito"); }

            if (data.classIDToString == string.Empty) return;

            if (data.lifePoints <= 0 || data.strength <= 0)
            {
                Debug.LogWarning("Dati caricati non validi, stats sono a 0!");
            }

            if (Singleton == null)
            {
                Debug.LogError("PlayerManager non inizializzato correttamente.");
                return;
            }

            // Aggiornamento delle proprietà del giocatore
            UpdatePlayerStats(data);

            // Ripristino dell'inventario
            Dictionary<ScriptableItem, int> loadedTempInventory = LoadInventory(data);

            // Aggiungi gli oggetti all'inventario
            AddItemsToInventory(loadedTempInventory);

            // Ripristino delle abilità del giocatore
            RestorePlayerAbilities(data);

            // Ripristino degli oggetti equipaggiati
            RestoreEquippedItems(data);

            pagesRead = data.passedPages;
            LoadClassInGame();
        }


        private void UpdatePlayerStats(PlayerData data)
        {
            playerName = data.playerName;
            lifePoints = data.lifePoints;
            currentAP = data.currentAP;
            playerLevel = data.level;
            playerExperience = data.exp;
            courage = data.courage;
            lucky = data.lucky;
            superstition = data.superstition;
            constitution = data.constitution;
            inteligence = data.intelligence;
            dexterity = data.dexterity;
            SetPlayermanagerStamina((int)(data.dexterity * 10));
            strength = data.strength;
            classPlayerTypeToString = data.classIDToString;
            lastPage = data.currentPageID;
            currentTorch = data.currentTorch;
            maxTorchCount = data.maxTorch;
            currentPaper = data.currentPaper;
        }

        private Dictionary<ScriptableItem, int> LoadInventory(PlayerData data)
        {
            Dictionary<ScriptableItem, int> loadedTempInventory = new Dictionary<ScriptableItem, int>();
            for (int i = 0; i < data.playerInventoryItemsID.Count; i++)
            {
                int itemQuantityCount = i < data.playerInventoryItemsNumber.Count ? data.playerInventoryItemsNumber[i] : 1;
                ScriptableItem item = ScriptableItemsDatabase.Singleton.GetItemById(data.playerInventoryItemsID[i]);
                if (item != null && !loadedTempInventory.ContainsKey(item))
                {
                    loadedTempInventory.Add(item, itemQuantityCount);
                }
            }
            return loadedTempInventory;
        }

        private void AddItemsToInventory(Dictionary<ScriptableItem, int> loadedTempInventory)
        {
            CleanInventory();
            foreach (KeyValuePair<ScriptableItem, int> items in loadedTempInventory)
            {
                ScriptableItem item = items.Key;
                if (item is Weapon weapon)
                {
                    AddWeaponToInventory(weapon);
                }
                else if (item is Equipment equipment)
                {
                    AddEquipmentToInventory(equipment);
                }
                else
                {
                    // Aggiungi gli altri tipi di oggetti (consumabili, collezionabili, ecc.) direttamente all'inventario
                    inventory.Add(item, items.Value);
                }
            }
        }

        private void AddWeaponToInventory(Weapon weapon)
        {
            switch (weapon.attackType)
            {
                case TypeDatabase.AttackType.Heavy:
                    heavyWeaponList.Add(weapon);
                    break;
                case TypeDatabase.AttackType.Ranged:
                    rangeWeaponList.Add(weapon);
                    break;
                case TypeDatabase.AttackType.Light:
                    lightWeaponList.Add(weapon);
                    break;
                case TypeDatabase.AttackType.Special:
                    specialWeaponList.Add(weapon);
                    break;
            }
        }

        private void AddEquipmentToInventory(Equipment equipment)
        {
            switch (equipment.equipPlaceType)
            {
                case Equipment.EquipPlaceType.LightDefence:
                    lightDefenceList.Add(equipment);
                    break;
                case Equipment.EquipPlaceType.BalancedDefence:
                    balancedDefenceList.Add(equipment);
                    break;
                case Equipment.EquipPlaceType.HeavyDefence:
                    heavyDefenceList.Add(equipment);
                    break;
                case Equipment.EquipPlaceType.Talisman:
                    talismansList.Add(equipment);
                    break;
                case Equipment.EquipPlaceType.Relic:
                    relicsList.Add(equipment);
                    break;
                case Equipment.EquipPlaceType.GemStone:
                    gemstonesList.Add(equipment);
                    break;
            }
        }

        private void RestorePlayerAbilities(PlayerData data)
        {
            Singleton.GetPlayerAbilities().Clear();
            foreach (string abilityID in data.abilities)
            {
                Singleton.GetPlayerAbilities().Add(ScriptableItemsDatabase.Singleton.GetAbilityFromDatabaseWithID(abilityID));
            }
        }

        private void RestoreEquippedItems(PlayerData data)
        {
            Singleton.playerWeapon.heavyWeapon = (Weapon)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[0]);
            Singleton.playerWeapon.lightWeapon = (Weapon)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[1]);
            Singleton.playerWeapon.rangeWeapon = (Weapon)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[2]);
            Singleton.playerWeapon.specialWeapon = (Weapon)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[3]);

            Singleton.playerEquipment.equippedLightDefence = (Equipment)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[4]);
            Singleton.playerEquipment.equippedHeavyDefence = (Equipment)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[5]);
            Singleton.playerEquipment.equippedBalancedDefence = (Equipment)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[6]);
            Singleton.playerEquipment.equippedClassDefaultDefence = (Equipment)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[7]);
            Singleton.initialEquipmentDefenseType = Singleton.playerEquipment.equippedClassDefaultDefence.defenseType;
            Singleton.playerEquipment.equippedGemstone = (Equipment)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[8]);
            Singleton.playerEquipment.equippedTalisman = (Equipment)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[9]);
            Singleton.playerEquipment.equippedRelic = (Equipment)ScriptableItemsDatabase.Singleton.GetItemById(data.equippedItemsList[10]);
        }

        public void LoadClassInGame()
        {
            if (classPlayerTypeToString == null) { return; }

            switch (classPlayerTypeToString)
            {
                case "Abbot":
                    Abbot abt = new Abbot();
                    selectedClass = abt;
                    isMale = true;
                    break;
                case "BountyHunter":
                    BountyHunter bh = new BountyHunter();
                    selectedClass = bh;
                    isMale = true;
                    break;
                case "Crone":
                    Crone crn = new Crone();
                    isMale = (false);
                    selectedClass = crn;
                    break;
                case "Trafficker":
                    Trafficker trf = new Trafficker();
                    isMale = (false);
                    selectedClass = trf;
                    break;
                default:
                    return;
            }
        }

        public void RefreshPlayerManager()
        {
            pagesRead = new List<string>();
            pagesDropped = new List<string>();
            playerName = string.Empty;
            lifePoints = 0;
            playerExperience = 0;
            courage = 0;
            lucky = 0;
            superstition = 0;
            classPlayerTypeToString = string.Empty;
            lastPage = string.Empty;
            selectedClass = null;
            currentAP = 0;
            stamina = 0;
        }

        public bool playerLevelUp = false;

        public IEnumerator WaitUntillTurnOff()
        {
            yield return new WaitUntil(ReturnObject);
        }

        bool ReturnObject()
        {
            return !GameApplication.Singleton.view.BookView.levelUpUI.gameObject.activeSelf;
        }

        public float GetDefenceArmor()
        {
            return playerEquipment.equippedClassDefaultDefence.defence;
        }

        public TypeDatabase.DefenseType GetDefenceType()
        {
            return playerEquipment.equippedClassDefaultDefence.defenseType;
        }

        public void UpdateEquipStats()
        {
            defence = 0;
            defence = GetDefenceArmor();
        }

        public const string TORCHOBJID = "Torcia";
        public const string PAPEROBJID = "Pergamena";
        public void AddItemToInventory(ScriptableItem item, int quantity)
        {
            if (inventory.ContainsKey(item))
            {
                inventory[item] += quantity;
                if (item.ID == TORCHOBJID)
                {
                    if (inventory[item] >= maxTorchCount)
                    {
                        inventory[item] = maxTorchCount;
                    }
                }
                if (item.ID == PAPEROBJID)
                {
                    if (inventory[item] >= maxPaperCount)
                    {
                        inventory[item] = maxPaperCount;
                    }
                }
            }
            else
            {
                Debug.Log("Nuovo oggetto");
                if (quantity == 0)
                {
                    quantity = 1;
                    Debug.Log("Modifico quantity da 0 a 1");
                }
                inventory.Add(item, quantity);
            }

            UpdateInventoryInspector(item);
        }

        public void RemoveItemFromPlayerInventory(ScriptableItem item, int quantity)
        {
            if (inventory.ContainsKey(item))
            {
                if (inventory[item] <= quantity)
                {
                    inventory.Remove(item);
                    return;
                }
                inventory[item] -= quantity;
                return;
            }
            Debug.LogError("Tryed to remove not owned item from inventory!");
        }

        public Dictionary<ScriptableItem, int> GetInventoryItems()
        {
            return inventory;
        }

        public Dictionary<string, int> GetAndCovertItemsInventoryForJSON()
        {
            Dictionary<string, int> itemsID = new Dictionary<string, int>();

            foreach (KeyValuePair<ScriptableItem, int> key in inventory)
            {
                itemsID.Add(key.Key.ID, key.Value);
            }
            return itemsID;
        }

        public void UpdateLifePoints(float lifePointsAmount, float time = 0)
        {
            float maxLifePoint = constitution * 10;

            if (lifePoints >= maxLifePoint && lifePointsAmount > 0)
            {
                ConsumableItem.playerCanTriggerObject = false;
                return;
            }

            lifePoints = lifePoints + lifePointsAmount;

            if (lifePoints > maxLifePoint)
            {
                lifePoints = maxLifePoint;
            }

            if (lifePoints <= 0)
            {
                Singleton.PlayerIsDead();
            }

        }

        public void UpdatePlayerManagerAbilityPoints(int newAbilityPointsAmount)
        {
            if (currentAP >= GetAbilityPointsLimit()) { ConsumableItem.playerCanTriggerObject = false; return; }

            currentAP = currentAP + newAbilityPointsAmount;
            if (currentAP > GetAbilityPointsLimit())
            {
                currentAP = GetAbilityPointsLimit();
            }
        }

        public void UpdatePlayerManagerStamina(int staminaBuffDebuffAmount)
        {
            if (stamina >= GetStaminaLimit()) { Debug.Log("FULLSTAMINA"); return; }

            stamina = stamina + staminaBuffDebuffAmount;
            if (stamina > GetStaminaLimit())
            {
                stamina = GetAbilityPointsLimit();
            }
        }



        internal int previewLevel = 0;
        public void UpdateExperience(float experienceAmount, float time = 0)
        {
            StartCoroutine(Wait(time));
            playerExperience += experienceAmount;
            previewLevel = playerLevel;
            UpdateLevel();
        }

        public void UpdateLucky(int luckyAmount, float time = 0)
        {
            lucky += luckyAmount;
            lucky = Mathf.Clamp(lucky, 0, 100);
        }

        public void UpdateCourage(int courageAmount, float time = 0)
        {
            courage += courageAmount;
            courage = Mathf.Clamp(courage, 0, 100);
        }
        public void UpdateSuperstition(int superstitionAmount, float time = 0)
        {
            superstition += superstitionAmount;
            superstition = Mathf.Clamp(superstition, 0, 100);
        }
        public void UpdateCostitution(float constitutionProgress, float time = 0, bool levelUp = false)
        {
            StartCoroutine(Wait(time));

            if (levelUp)
            {
                constitution += constitutionProgress;
            }
        }

        public void UpdateDexterity(float dexterityProgress, float time = 0, bool levelUp = false)
        {
            StartCoroutine(Wait(time));

            if (levelUp)
            {
                dexterity += dexterityProgress;
            }
        }

        public void UpdateIntelligence(float intelligenceProgress, float time = 0, bool levelUp = false)
        {
            StartCoroutine(Wait(time));
            if (levelUp)
            {
                inteligence += intelligenceProgress;
            }
        }

        public void UpdateStrenght(float strenghtProgress, float time = 0, bool levelUp = false)
        {
            StartCoroutine(Wait(time));

            if (levelUp)
            {
                strength += strenghtProgress;
            }
        }

        IEnumerator Wait(float time)
        {
            yield return CoroutinesHelper.Wait(new WaitForSeconds(time));
        }

        internal static string answerClassSolution;

        public string GetPlayerClass()
        {
            if (selectedClass == null)
            {
                return Localization.Get(answerClassSolution);  // se la classe non è selezionata mando il consiglio
            }
            return selectedClass.GetClassName;
        }

        public string GetPlayerName()
        {
            return playerName;
        }

        public bool PlayerIsDead()
        {
            return CheckPlayerIsDead;
        }

        public void ResetOnDeath()
        {
            className = string.Empty;
            playerLevel = 1;
            playerName = string.Empty;
            playerExperience = 0;
            playerChoice = 0;

            superstition = 0;
            courage = 0;
            lucky = 0;

            dexterity = 0;
            inteligence = 0;
            strength = 0;
            constitution = 0;

            defence = 0;
            classImage = null;
            maxTorchCount = 0;
            currentTorch = 0;
            maxPaperCount = 0;
            currentPaper = 0;
            classPlayerTypeToString = string.Empty;

            pagesRead.Clear();
            isDead = false;
            lastPage = string.Empty;

            selectedDifficulty = Difficulty.Null;
            selectedClass = null;

            playerEquipment.equippedGemstone = null;
            playerEquipment.equippedTalisman = null;
            playerEquipment.equippedRelic = null;
            lightWeaponList.Clear();

            CleanInventory();
            CleanEquipment();
        }


        public void UpdateLevel(bool firstAwake = false)
        {
            if (HasPlayerLeveledUp(playerLevel, playerExperience))
            {
                SetPlayerLvl(playerLevel + 1);
            }

            if (firstAwake)
            {
                return;
            }

            if (LevelChange(previewLevel))
            {
                LevelUpController.LevelUpSingleton.UpdateStatsOnPlayerProgression((short)playerLevel - 1, selectedClass);
                GameApplication.Singleton.view.BookView.levelUpUI.gameObject.SetActive(true);

                RefreshExpContainers();
            }

        }

        public bool HasPlayerLeveledUp(int currentLevel, float currentPoints)
        {
            float nextLevelThreshold = LevelManagerUtilities.GetPlayerCurrentThreshold(currentLevel);

            float previousThresholds = 0;
            for (int i = 1; i < currentLevel; i++)
            {
                previousThresholds += LevelManagerUtilities.GetPlayerCurrentThreshold(i); //tutte le soglie fino a questo livello 
            }

            nextLevelThreshold += previousThresholds;
            return currentPoints >= nextLevelThreshold;
        }

        internal void RefreshExpContainers()
        {
            foreach (var item in GameApplication.Singleton.view.BookView.expUIContainer)
            {
                item.gameObject.SetActive(false);
            }
        }

        public ScriptableItem GetInventoryItemById(string ID)
        {
            if (ID != "" && ID != null)
            {
                foreach (ScriptableItem item in inventory.Keys)
                {
                    if (item)
                    {
                        if (item.ID == ID)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public void AddItemDrop(ScriptableItem item, int quantity = 1)
        {
            if (item)
            {
                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    switch (weapon.attackType)
                    {
                        case TypeDatabase.AttackType.Heavy:
                            Debug.Log("Aggiunto arma pesante" + weapon.itemNameLocalization);
                            Singleton.heavyWeaponList.Add(weapon);
                            break;
                        case TypeDatabase.AttackType.Ranged:
                            Debug.Log("Aggiunto arma distanza" + weapon.itemNameLocalization);
                            Singleton.rangeWeaponList.Add(weapon);
                            break;
                        case TypeDatabase.AttackType.Light:
                            Debug.Log("Aggiunto arma leggera" + weapon.itemNameLocalization);
                            Singleton.lightWeaponList.Add(weapon);
                            break;
                        case TypeDatabase.AttackType.Special:
                            Debug.Log("Aggiunto arma speciale" + weapon.itemNameLocalization);
                            Singleton.specialWeaponList.Add(weapon);
                            break;
                        default:
                            break;
                    }
                    return;
                }
                if (item is Equipment)
                {
                    Equipment equip = (Equipment)item;

                    switch (equip.equipPlaceType)
                    {
                        case Equipment.EquipPlaceType.LightDefence:
                            Singleton.lightDefenceList.Add(equip);
                            break;
                        case Equipment.EquipPlaceType.BalancedDefence:
                            Singleton.balancedDefenceList.Add(equip);
                            break;
                        case Equipment.EquipPlaceType.HeavyDefence:
                            Singleton.heavyDefenceList.Add(equip);
                            break;
                        case Equipment.EquipPlaceType.Talisman:
                            break;
                        case Equipment.EquipPlaceType.Relic:
                            break;
                        case Equipment.EquipPlaceType.GemStone:
                            break;
                    }

                    switch (equip.itemType)
                    {
                        case ScriptableItem.ItemType.Relic:
                            Singleton.relicsList.Add(equip);
                            break;
                        case ScriptableItem.ItemType.Talisman:
                            Singleton.talismansList.Add(equip);
                            break;
                        case ScriptableItem.ItemType.GemStone:
                            Singleton.gemstonesList.Add(equip);
                            break;
                        default:
                            break;
                    }
                    return;
                }

                if (item is ScriptableItem)
                {
                    Debug.Log("Aggiungo ITEM : " + item.itemNameLocalization);
                    Singleton.AddItemToInventory(item, quantity);
                    return;
                }
            }
            else
            {
                Debug.Log("L'oggetto inserito non esiste");
            }
        }

        public bool IsPageDropped(string checkId)
        {
            foreach (string pageID in pagesDropped)
            {
                if (pageID == checkId)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddDroppedPage(string pageID)
        {
            pagesDropped.Add(pageID);
        }

        public void UpdateBattleAPTxt(int AP)
        {
            battleController.UpdateAPText(AP);
        }

    }
}
