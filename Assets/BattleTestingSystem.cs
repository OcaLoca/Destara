/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using SmartMVC;
using StarworkGC.Localization;
using StarworkGC.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;
using static Game.PlayerManager;

namespace Game
{
    public class BattleTestingSystem : MonoBehaviour
    {
        public static BattleTestingSystem instance { get; set; }

        [SerializeField] BattleEnemyManager BattleEnemyManager;
        [SerializeField] BattlePlayerManager BattlePlayerManager;
        public List<Unit> enemies = new List<Unit>();
        public List<Unit> allies = new List<Unit>();
        public List<Unit> allUnits = new List<Unit>();
        [SerializeField] CentralEnemyHealthbar centralEnemyhealthbar;
        [SerializeField] LeftEnemyHealthbar leftEnemyhealthbar;
        [SerializeField] RightEnemyHealthbar rightEnemyhealthbar;
        [SerializeField] BuffManager buffManager;

        [Header("UI")]
        [SerializeField] GameObject PanelShowTitle;
        [SerializeField] GameObject PanelInfoActor;
        [SerializeField] TMP_Text txtTurn;
        [SerializeField] Button btnSkipText;
        [SerializeField] Image enemyUIFace;
        [SerializeField] DialogConsoleManager dialogConsoleManager;

        [Header("VFX")]
        public GameObject vfxSlashContainer;
        public GameObject vfxAbilityContainer;

        const int NODELAY = 0;
        BattleTurn battleTurn;
        public static string consoleLog;
        public static string deathLog;
        public static string effectiveLog;
        public static string defenceLog;
        public static bool damageIsCritical = false;
        public static bool zeroDamage = false;
        public static bool enemyZeroDamage = false;
        public static bool failedToAttack = false;
        public static bool enemyFailedToAttack = false;
        ScriptableEnemy[] actorData;

        [Header("TESTBATTLE")]
        public ScriptableEnemy testEnemy;
        BattleView View => GameApplication.Singleton.view.BattleView;

        public static float StaminaRecoveredWithObj;
        //  [Header("UI")]
        // [SerializeField] TextMesh damaageTxt;
        // [SerializeField] Animator showDamage;

        public enum BattleTurn
        {
            Start,
            PlayerTurn,
            EnemyTurn,
        }

        private void Awake()
        {
            ScriptableEnemy[] test = new ScriptableEnemy[] { testEnemy };
            actorData = test;
            ResetBattleSettingsOnOpenBattleView();
        }

        private void OnEnable()
        {
            allUnits.Clear();
            enemies.Clear();
            allies.Clear();

            ResetBattleSettingsOnOpenBattleView();
            LoadActorFromPage();
            SortByDexterity();
            StartCoroutine(StartTestBattle());
        }

        public void UnlockPlayerAbility()
        {
            BattlePlayerManager.UnlockAbility(allUnits[lastIndex]);
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1 ||
                PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1)
            {
                ShowFirstTimeTutorial();
            }
        }

        void LoadActorFromPage()
        {
            CreateTestEnemyUnits(testEnemy);
            Debug.Log("DIO");
        }

        int turn;
        bool AlreadyShowTutorialOneTime = false;
        bool finished = false;
        IEnumerator StartTestBattle()
        {
            do
            {
                if (gameObject.activeSelf)
                {
                    StartCoroutine(StandardTurn());
                    yield return new WaitUntil(FinishTurn);
                    finished = false;
                }
            }
            while (!BattleIsFinish());

            UploadPlayerStats();
            View.LoadVictoryLosePanel();
            View.LoadIDOnBattleEnd();
        }

        void ResetBattleSettingsOnOpenBattleView()
        {
            turn = 0;

            PanelInfoActor.SetActive(false);
            BattleEnemyManager.DestroyAllEnemiesObject();
            BattleEnemyManager.CleanUIOnCloseBattleView();

            if (enemies.Count == 2)
            {
                rightEnemyhealthbar.PrepareLifeBar();
            }
            else if (enemies.Count == 3)
            {
                rightEnemyhealthbar.PrepareLifeBar();
                leftEnemyhealthbar.PrepareLifeBar();
            }
            // centralEnemyhealthbar.SetCentralEnemyLifeOnStartFight();

            foreach (Unit unit in allUnits)
            {
                SetEnemiesHealthbarOnStartBattle(unit);
            }
        }

        int lastIndex;
        public bool playerPlayed = false;
        public bool abilityClicked = false;
        public static bool PlayerRecoverLife = false;
        public static bool PlayerRecoverStamina = false;
        public bool PlayerAbilityNotUpdateLife = false;
        public bool PlayerAbilityNotGiveDamage = false;
        int wpnIndex = 0;


        string unitLocalizedName;
        IEnumerator StandardTurn()
        {
            battleTurn = BattleTurn.Start;

            SortByDexterity();
            Debug.Log("Controllare se va in conflitto con lastIndex che dopo Sort a volte inverte i numeri, per ora risolvo togliendolo");

            for (int i = 0; i < allUnits.Count; i++)
            {
                Unit unit = allUnits[i];
                lastIndex = i;

                if (unit.isDead)
                {
                    continue;
                }
                //Stato dell'unità
                CheckoutUnitStatus(unit);

                if (unit.mainCharacter)
                {
                    consoleLog = Localization.Get(LocalizationIDDatabase.WHAT_DO_BATTLE_LOG);
                    yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                }

                unitLocalizedName = allUnits[i].name;
                if (!unit.noStatus)
                {
                    CheckElementalStats(allUnits[i]);
                    if (GetIsParalyzed)
                    {
                        consoleLog = string.Format("{0} è paralizzato.", unitLocalizedName);
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();
                        continue;
                    }
                    if (GetIsBurned)
                    {
                        consoleLog = string.Format("{0} è scottato.", unitLocalizedName);
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();

                        consoleLog = string.Format("La scottatura fa male");
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();


                        unit.TakeDamage(unit.elementalLevel, unit);
                        UpdateEnemyUILifeBar(unit);
                        UpdateActorStats(unit);
                    }
                    if (GetIsConfused)
                    {
                        consoleLog = string.Format("{0} è confuso.", unitLocalizedName);
                        yield return dialogConsoleManager.TypeDialogTxt(consoleLog);

                        if (Random.Range(0, 101) < allUnits[i].elementalLevel)
                        {
                            consoleLog = string.Format("Cosi confuso da colpirsi da solo");
                            WriteLogByButton(consoleLog);
                            yield return new WaitUntil(PlayerClickOnLogTxt);
                            RefreshSkippedLog();

                            unit.TakeDamage(unit.strenght * 2, unit);
                            UpdateActorStats(unit);
                            yield return new WaitUntil(LifePointsUpdated);
                            yield return new WaitUntil(LifePointsTextUpdate);
                            UIEnemiesLifeAnimationFinish = false;
                            UIEnemiesLifeTextCountUpdateFinish = false;

                            //UpdateEnemyUILifeBar(unit);
                            //UpdateActorStats(unit);

                            continue;
                        }
                        else
                        {
                            consoleLog = "Ma rinsavisce";
                            WriteLogByButton(consoleLog);
                            yield return new WaitUntil(PlayerClickOnLogTxt);
                            RefreshSkippedLog();
                        }
                    }
                    if (GetIsInvulnerable)
                    {
                        unit.isInvurnerable = true;
                        UpdateActorStats(unit);
                    }
                    else
                    {
                        unit.isInvurnerable = false;
                        UpdateActorStats(unit);
                    }
                }

                //RefreshConsoleText();
                Debug.LogWarning("REFRESH");

                //StartCoroutine(OpenConsole(NODELAY));

                if (unit.mainCharacter)
                {
                    LocalizeAbilitiesNames(unit);

                    battleTurn = BattleTurn.PlayerTurn;
                    PanelInfoActor.SetActive(true);

                    // BuffOnePowerSlot(unit);
                    StartCoroutine(UIRecoverStaminaBar(unit, 30));
                    unit = RecoverUnitStamina(ref unit, 30);
                    UpdateActorStats(unit);
                    BattleEnemyManager.LastClickedEnemy();
                    BattlePlayerManager.ChangeUICharacterInBattle(unit);
                    BattlePlayerManager.PreparePlayerHUD(unit);
                    StartCoroutine(SetStatusHUD(unit, NODELAY));

                    yield return new WaitUntil(() => PlayerPlayed());
                    playerPlayed = false;

                    if (PlayerAttackWithWeapon)
                    {
                        string weaponLog = string.Empty;
                        weaponLog = string.Format(Localization.Get(LocalizationIDDatabase.USE_BATTLE_LOG), Localization.Get(unit.name), unit.equippedWeapon[wpnIndex].itemNameLocalized);
                        yield return dialogConsoleManager.TypeDialogTxt(weaponLog);
                        PlayerAttackWithWeapon = false;
                        if ((failedToAttack) || (zeroDamage))
                        {
                            UIEnemiesLifeAnimationFinish = true;
                            UIEnemiesLifeTextCountUpdateFinish = true;
                        }
                        yield return new WaitUntil(LifePointsUpdated);
                        yield return new WaitUntil(LifePointsTextUpdate);
                        UIEnemiesLifeAnimationFinish = false;
                        UIEnemiesLifeTextCountUpdateFinish = false;
                    }
                    else if (PlayerChangeDefence)
                    {
                        PlayerChangeDefence = false;
                    }
                    else if (PlayerUseObject)
                    {
                        PlayerUseObject = false;
                        if (PlayerRecoverLife)//se è un oggetto che ti fa recuperare vita
                        {
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeBarFinish);
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeTextBarFinish);
                            PlayerRecoverLife = false;
                        }
                        else if (PlayerRecoverStamina)
                        {
                            // yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                            StartCoroutine(UIRecoverStaminaBar(unit, StaminaRecoveredWithObj));
                            yield return new WaitUntil(GameApplication.Singleton.model.SingletonStaminaBarManager.PlayerStaminaBarAnimationFinish);
                            yield return new WaitUntil(GameApplication.Singleton.model.SingletonStaminaBarManager.PlayerStaminaBarTextFinish);
                            PlayerRecoverStamina = false;
                        }
                    }
                    else if (PlayerRest)
                    {

                        //Mostro il costo dell'arma che ha equipaggiato
                        float staminaRecoverAmount = (unit.dexterity + unit.inteligence) / 2;
                        StartCoroutine(UIRecoverStaminaBar(unit, staminaRecoverAmount));
                        unit = RecoverUnitStamina(ref unit, staminaRecoverAmount);
                        UpdateActorStats(unit);

                        string staminaToString = staminaRecoverAmount.ToString();
                        StartCoroutine(ClosePlayerCommandPanel());
                        consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.RECOVER_X_STAMINA), unit.name, staminaToString);

                        yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                        yield return new WaitUntil(GameApplication.Singleton.model.SingletonStaminaBarManager.PlayerStaminaBarAnimationFinish);
                        consoleLog = Localization.Get(LocalizationIDDatabase.RECOVER_STAMINA);
                        PlayerRest = false;
                    }
                    else
                    {
                        yield return dialogConsoleManager.TypeDialogTxt(abilityLog);
                        if (PlayerRecoverLife)//se è un abilità che ti fa recuperare vita
                        {
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeBarFinish);
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeTextBarFinish);
                            PlayerRecoverLife = false;
                        }

                        else if ((!PlayerRecoverLife) && (PlayerAbilityNotGiveDamage))
                        {
                            //usa abilità che fanno altro
                            Debug.LogWarning("Abilità che non fa ne danni ne recupera vita");
                            PlayerAbilityNotGiveDamage = false;
                        }
                        else
                        {
                            //usa abilità che non recuperano vita e che forse fanno danni
                            if (!PlayerAbilityNotGiveDamage) // se fa danni
                            {
                                yield return new WaitUntil(LifePointsUpdated);
                                yield return new WaitUntil(LifePointsTextUpdate);
                                UIEnemiesLifeAnimationFinish = false;
                                UIEnemiesLifeTextCountUpdateFinish = false;
                            }
                            else //se fa buff ad esempio
                            {
                                PlayerAbilityNotGiveDamage = false;
                            }
                        }
                    }
                    //WriteLogByButton(consoleLog);
                    yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                    // btnSkipText.gameObject.SetActive(true);
                    // yield return new WaitUntil(PlayerClickOnLogTxt);
                    // btnSkipText.gameObject.SetActive(false);

                    buffManager.TurnOffPowerupTxt();
                    //BattlePlayerManager.btnCancelBuff.gameObject.SetActive(false);
                }
                else
                {
                    battleTurn = BattleTurn.EnemyTurn;
                    //enemyUIFace.sprite = unit.enemyUISprite;
                    BattleEnemyManager.ChangeEnemyUI(unit);

                    //IA decisione
                    StartCoroutine(EnemyIAController(unit));

                    if (enemyUseAbility)
                    {
                        if (abilityLog == null)
                        {
                            abilityLog = "Ability Log null reference";
                        }
                        yield return dialogConsoleManager.TypeDialogTxt(abilityLog);
                    }
                    else
                    {
                        if (consoleLog == null)
                        {
                            consoleLog = "Null Reference";
                        }
                        yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                    }

                    //Scrivo se ha mancato o cosa usa
                    // yield return dialogConsoleManager.TypeDialogTxt(consoleLog);

                    if (Singleton.PlayerIsDead())
                    {
                        yield return dialogConsoleManager.TypeDialogTxt(deathLog);
                    }

                    // Scrivo se efficace o meno
                    //  yield return dialogConsoleManager.TypeDialogTxt(abilityLog);

                    //ANIMAZIONE 

                    if (!enemyFailedToAttack && !enemyZeroDamage)
                    {
                        yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeBarFinish);
                        yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeTextBarFinish);
                    }
                    else
                    {
                        enemyZeroDamage = false;
                        enemyFailedToAttack = false;
                    }

                    if (!Singleton.PlayerIsDead())
                    {
                        if (effectiveLog != null && effectiveLog != string.Empty)
                        {
                            consoleLog = effectiveLog;
                            yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                        }
                        if (defenceLog != string.Empty && defenceLog != null)
                        {
                            consoleLog = defenceLog;
                            yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                        }
                    }
                    RefreshSkippedLog();
                }
            }
            finished = true;
        }

        void CheckoutUnitStatus(Unit unit)
        {
            foreach (var value in unit.stats)
            {
                if (value.Value > 0)
                {
                    unit.noStatus = false;
                    break;
                }
                else
                {
                    unit.noStatus = true;
                }
            }
            UpdateActorStats(unit);
        }

        public static bool PlayerChangeDefence = false;
        internal void OnPlayerChangeDefence(int defenceIndex)
        {
            Unit unit = allUnits[GetMainPlayerIndex()];
            StartCoroutine(ClosePlayerCommandPanel());

            if (unit.DefenceType == (TypeDatabase.DefenseType)defenceIndex)
            {
                StartCoroutine(PlayerHaveAlreadyThisDefence());
                return;
            }

            string newDefenceToString = string.Empty;

            switch (defenceIndex)
            {
                case 0:
                    unit.defence = Singleton.playerEquipment.equippedLightDefence.defence;
                    unit.DefenceType = TypeDatabase.DefenseType.Light;
                    newDefenceToString = Localization.Get(Singleton.playerEquipment.equippedLightDefence.itemNameLocalization);
                    break;
                case 1:
                    unit.defence = Singleton.playerEquipment.equippedBalancedDefence.defence;
                    unit.DefenceType = TypeDatabase.DefenseType.Avarage;
                    newDefenceToString = Localization.Get(Singleton.playerEquipment.equippedBalancedDefence.itemNameLocalization);
                    break;
                case 2:
                    unit.defence = Singleton.playerEquipment.equippedHeavyDefence.defence;
                    unit.DefenceType = TypeDatabase.DefenseType.Heavy;
                    newDefenceToString = Localization.Get(Singleton.playerEquipment.equippedHeavyDefence.itemNameLocalization);
                    break;
            }

            string defenceTypeToString = Localization.Get(unit.DefenceType + "DfType");
            consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.CHANGE_DEFENCE_LOG), Localization.Get(unit.name), newDefenceToString, defenceTypeToString);
            BattlePlayerManager.defencePlayerBattleImage.sprite = IconsDatabase.Singleton.GetArmorSpriteByDefenceType(unit.DefenceType);
            UpdateActorStats(unit);
            playerPlayed = true;
            PlayerChangeDefence = true;
        }

        string abilityLog;
        public void UseAbility()
        {
            Unit unit = allUnits[lastIndex];
            ScriptableAbility ability = unit.ability[abilityIndex];
            if (ability.APcost > unit.abilityPoints)
            {
                StartCoroutine(LogAbilityIsToExpensive(unit.name));
                abilityClicked = false;
                return;
            }

            abilityLog = string.Format(Localization.Get(LocalizationIDDatabase.USE_BATTLE_LOG), unit.name, ability.abilityName);
            PlayerUsingAbility(ability);
        }

        IEnumerator LogAbilityIsToExpensive(string unitName)
        {
            StartCoroutine(ClosePlayerCommandPanel());
            consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.ABILITY_COST_TOMUCH), unitName);
            yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
            BattleView Battle = GameApplication.Singleton.view.BattleView;
            Battle.pnlButton.SetActive(true);
        }

        void LocalizeAbilitiesNames(Unit unit)
        {
            foreach (ScriptableAbility ability in unit.ability)
            {
                ability.LocalizeAbilityName();
            }
            if (unit.finalAbility != null)
            {
                unit.finalAbility.LocalizeAbilityName();
            }
        }

        int abilityIndex;
        bool isParalyzed;
        bool isConfused;
        bool isBurned;
        bool isPoisoned;
        bool isFreezed;
        bool isInvulnerable;
        bool isUnable;

        void CheckElementalStats(Unit unit)
        {
            ResetStats();

            if (unit.stats[Stats.burned] >= 1)
            {
                consoleLog = string.Format("{0} è scottato", unit.name);
                unit.stats[Stats.burned] = unit.stats[Stats.burned] - 1;
                isBurned = true;
            }
            else if (unit.stats[Stats.poisoned] >= 1)
            {
                consoleLog = string.Format("{0} è avvelenato", unit.name);
                unit.stats[Stats.poisoned] = unit.stats[Stats.poisoned] - 1;
                isPoisoned = true;
            }
            else if (unit.stats[Stats.paralyzed] >= 1)
            {
                Debug.LogFormat("Lo stato paralizzato di {1}  prima del conteggio dura ancora {0} turni", unit.stats[Stats.paralyzed], unit.placeID);
                consoleLog = string.Format("{0} è paralizzato", unit.name);
                unit.stats[Stats.paralyzed] = unit.stats[Stats.paralyzed] - 1;
                isParalyzed = true;
                Debug.LogFormat("Lo stato paralizzato di {1}  dopo il conteggio dura ancora {0} turni", unit.stats[Stats.paralyzed], unit.placeID);
            }
            else if (unit.stats[Stats.confused] >= 1)
            {
                Debug.LogFormat("Lo stato stordito di {1}  prima del conteggio dura ancora {0} turni", unit.stats[Stats.confused], unit.placeID);
                consoleLog = string.Format("{0} è confuso", unit.name);
                unit.stats[Stats.confused] = unit.stats[Stats.confused] - 1;
                isConfused = true;
                Debug.LogFormat("Lo stato stordito di {1}  dopo il conteggio dura ancora {0} turni", unit.stats[Stats.paralyzed], unit.placeID);
            }
            else if (unit.stats[Stats.freezed] >= 1)
            {
                consoleLog = string.Format("{0} è congelato", unit.name);
                unit.stats[Stats.freezed] = unit.stats[Stats.freezed] - 1;
                isFreezed = true;
            }
            else if (unit.stats[Stats.invurneable] >= 1)
            {
                consoleLog = string.Format("{0} è invulnerabile", unit.name);
                unit.stats[Stats.invurneable] = unit.stats[Stats.invurneable] - 1;
                isInvulnerable = true;
            }
            else if (unit.stats[Stats.unable] >= 1)
            {
                consoleLog = string.Format("{0} è accecato", unit.name);
                unit.stats[Stats.unable] = unit.stats[Stats.unable] - 1;
                isUnable = true;
            }
        }

        void ResetStats()
        {
            isParalyzed = false;
            isConfused = false;
            isBurned = false;
            isPoisoned = false;
            isFreezed = false;
            isInvulnerable = false;
            isUnable = false;
        }

        bool GetIsParalyzed { get => isParalyzed; }
        bool GetIsConfused { get => isConfused; }
        bool GetIsBurned { get => isBurned; }
        bool GetIsPoisoned { get => isPoisoned; }
        bool GetIsFreezed { get => isFreezed; }
        bool GetIsInvulnerable { get => isInvulnerable; }

        bool PlayerAttackWithWeapon = false;
        bool PlayerRest = false;
        public bool PlayerUseObject = false;
        public void OnPlayerClickAttack(bool isMainPlayer, int weaponIndex)
        {
            StartCoroutine(ClosePlayerCommandPanel());

            Unit target = BattleEnemyManager.ReturnClickedButton();
            Unit attacker = allUnits[GetMainPlayerIndex()];
            wpnIndex = weaponIndex;
            Weapon currentWeapon = attacker.equippedWeapon[wpnIndex];

            if (attacker.stamina < currentWeapon.staminaCost)
            {
                StartCoroutine(PlayerHaventEnoughStamina(attacker.name));
                return;
            }

            PlayerAttackWithWeapon = true;

            AlreadyPlayed(attacker);
            float tmpHp = target.hp;
            float damage = currentWeapon.CalculateWeaponDamage(target, attacker);

            if (!failedToAttack)
            {
                target.TakeDamage(damage, target);
            }

            attacker = PayStamina(ref attacker, currentWeapon);
            UpdateActorStats(attacker);

            //animazione&suono
            SetVFXPositionsOnPlayerTurn(false);
            CallSlashAnimation(VFXID);
            SoundEffectManager.Singleton.PlayAudioClip(attacker.equippedWeapon[wpnIndex].weaponAttackSound);

            if (failedToAttack) //se è schivato
            {
                BattleEnemyManager.EnemyAnimationController(target, null, false, false, true);
            }
            else if (zeroDamage) { } //Se non fai danni
            else
            {
                float shakeDuration = 0;
                BattleEnemyManager.EnemyAnimationController(target, null, false, true, false, shakeDuration, damage);
            }

            //AggiornoUI
            UpdateTargetStatsAndUI(target);

            //Scrivo se l'attacco è efficace e attendo l'input
            if(effectiveLog != null)
            {
                consoleLog = effectiveLog;
            }

            RefreshSkippedLog();
            //PreventNullReferenceFromLog();
            playerPlayed = true;
        }

        IEnumerator PlayerHaventEnoughStamina(string unitName)
        {
            consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.WEAPON_COST_TOOMUCH), unitName);
            yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
            BattleView Battle = GameApplication.Singleton.view.BattleView;
            Battle.pnlButton.SetActive(true);
        }

        IEnumerator PlayerHaveAlreadyThisDefence()
        {
            consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.DEFENCE_ALREADY_USED));
            yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
            BattleView Battle = GameApplication.Singleton.view.BattleView;
            Battle.pnlButton.SetActive(true);
        }

        void PreventNullReferenceFromLog()
        {
            if (effectiveLog == null)
            {
                effectiveLog = "NULLO";
                //if (failedToAttack)
                //{
                //    effectiveLog = Localization.Get(LocalizationIDDatabase.MISS_LOG);
                //}
                //else
                //{
                //    if (zeroDamage) { effectiveLog = Localization.Get(LocalizationIDDatabase.NOT_EFFECTIVE_LOG); }
                //    effectiveLog = Localization.Get(LocalizationIDDatabase.GENERIC_HIT_LOG);
                //}
            }
        }

        void UpdateTargetStatsAndUI(Unit target)
        {
            UpdateActorStats(target);
            UpdateEnemyUILifeBar(target);
        }

        void RefreshSkippedLog()
        {
            playerSkipText = false;
            btnSkipText.gameObject.SetActive(false);
        }

        public static string VFXID;
        public static float timeSlash;
        public static string targetID;
        public static void SetVFX(float time, string VFXID, string placeID)
        {
            BattleController.VFXID = VFXID;
            timeSlash = time;
            targetID = placeID;
        }

        void SetVFXPositionsOnPlayerTurn(bool targetActor)
        {
            if (!targetActor)
            {
                switch (targetID)
                {
                    case "CentralEnemy":
                        vfxSlashContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        vfxAbilityContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        break;
                    case "LeftEnemy":
                        vfxSlashContainer.transform.localPosition = BattleEnemyManager.leftEnemyTransform;
                        vfxAbilityContainer.transform.localPosition = BattleEnemyManager.leftEnemyTransform;
                        break;
                    case "RightEnemy":
                        vfxSlashContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        vfxAbilityContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        break;
                    default:
                        break;
                }
            }
        }

        void CallSlashAnimation(string slashID)
        {
            foreach (Transform child in vfxSlashContainer.transform)
            {
                if (child.name == slashID)
                {
                    child.GetComponent<ParticleSystem>().Play();
                    return;
                }
            }
        }

        void CallAbilityAnimation(string abilityAnimationID)
        {
            if (abilityAnimationID == string.Empty) { return; }

            foreach (Transform child in vfxAbilityContainer.transform)
            {
                if (child.name == abilityAnimationID)
                {
                    child.GetComponentInChildren<ParticleSystem>().Play();
                    if (child.Find("CornerEffect"))
                    {
                        GameObject obj = child.Find("CornerEffect").gameObject;
                        StartCoroutine(ActiveCornerEffect(obj));
                    }
                    return;
                }
            }
        }

        IEnumerator ActiveCornerEffect(GameObject obj)
        {
            obj.SetActive(true);
            //SlowMotionEffect.Instance.EnableSlowMotion(0.5f, 1);
            yield return CoroutinesHelper.PointSevenSeconds;
            obj.SetActive(false);
        }

        bool VFXAbilityPositionIsPlayer = false;
        void PlayerUsingAbility(ScriptableAbility ability)
        {
            StartCoroutine(ClosePlayerCommandPanel());
            Unit unit = allUnits[lastIndex];
            Unit target = BattleEnemyManager.ReturnClickedButton();
            AlreadyPlayed(unit);

            VFXAbilityPositionIsPlayer = false;

            if (ability.CanUseThis(unit, target))
            {
                //Scrivo x usa y
                StartCoroutine(ClosePlayerCommandPanel());

                if (ability.moveType.Contains(ScriptableAbility.TargetType.TargetEnemy))
                {
                    System.Tuple<Unit, string> tuple = ability.TriggerAbility(unit, target, ability.HavePowerup, ability.UseWeapon, ability.NumberOfPowerup, wpnIndex);
                    target = tuple.Item1;
                    //Scrivo cosa fa
                    consoleLog = tuple.Item2;
                    if (ability.dontGiveDamage)
                    {
                        PlayerAbilityNotGiveDamage = true;
                        UIEnemiesLifeAnimationFinish = true;
                        UIEnemiesLifeTextCountUpdateFinish = true;
                    }
                    UpdateTargetStatsAndUI(target);
                }
                else if (ability.moveType.Contains(ScriptableAbility.TargetType.TargetActor))
                {
                    PlayerAbilityNotGiveDamage = true;
                    VFXAbilityPositionIsPlayer = true;
                    System.Tuple<Unit, string, bool> tuple = ability.BuffActor(unit, false);
                    unit = tuple.Item1;
                    consoleLog = tuple.Item2;
                    if (tuple.Item3)
                    {
                        PlayerRecoverLife = true;
                        GameApplication.Singleton.model.playerHealthbarManager.UpdateCurrentLife(unit.hp);
                    }
                    UpdateActorStats(unit);
                }
            }

            if (!ability.moveType.Contains(ScriptableAbility.TargetType.TargetActor))
            {
                SetVFXPositionsOnPlayerTurn(VFXAbilityPositionIsPlayer);
            }

            int loop = ability.VFXloop;
            if (loop > 1)
            {
                StartCoroutine(CallMultipleVfx(ability.VFXID, ability.VFXloop));
            }
            else
            {
                CallAbilityAnimation(ability.VFXID);
            }

            if (!ability.CanBeBuffedWithNoLimit)
            {
                UpdatePlayerAP(unit, ability.APcost, true);
            }
            else
            {
                UpdatePlayerAP(unit, ability.APcost, false);
            }

            //ability.ResetDefaultCost();
            Debug.LogWarning("Se rimettiamo i buff qua resetta");
            Debug.Log(unit.powerUp);

            playerPlayed = true;
        }

        IEnumerator CallMultipleVfx(string VFXID, int loop)
        {
            for (int i = 0; i < loop; i++)
            {
                CallAbilityAnimation(VFXID);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public IEnumerator ChoiceAbility()
        {
            yield return new WaitUntil(IsClicked);
        }

        public IEnumerator OnClickDuringBattleRest()
        {
            //implementare meccanica rest
            if (PlayerCanRest())
            {
                PlayerRest = true;
                playerPlayed = true;
            }
            else
            {
                StartCoroutine(ClosePlayerCommandPanel(0));
                consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.STAMINA_IS_MAX), Singleton.playerName);
                yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                BattleView Battle = GameApplication.Singleton.view.BattleView;
                Battle.pnlButton.SetActive(true);
            }
        }

        bool PlayerCanRest()
        {
            Unit mainPlayer = allUnits[GetMainPlayerIndex()];
            return mainPlayer.stamina < mainPlayer.dexterity * 10;
        }

        void SetWeaponStarterAnimation(Unit unit)
        {
            SetAnimationForWeaponType(unit.equippedWeapon[wpnIndex].attackType);
            ChangeEquippedWeaponTxt(unit.equippedWeapon[wpnIndex].itemNameLocalization);
        }

        public static bool weapoUInAnimationFinish;
        IEnumerator ChangeWeaponAnimation(Unit unit)
        {
            yield return new WaitUntil(UIAnimationFinish);
            weapoUInAnimationFinish = false;
            SetAnimationForWeaponType(unit.equippedWeapon[wpnIndex].attackType);
            ChangeEquippedWeaponTxt(unit.equippedWeapon[wpnIndex].itemNameLocalization);
        }

        void ChangeEquippedWeaponTxt(string currentWeaponWxt)
        {
            GameApplication.Singleton.view.BattleView.currenrtWeaponTxt.text = Localization.Get(currentWeaponWxt);
        }

        void SetWeaponIconColor(Unit unit)
        {
            List<Color32> colors = new List<Color32>();

            foreach (Weapon weapon in unit.equippedWeapon)
            {
                Color32 tmpColor = new Color32();
                tmpColor = ColorDatabase.Singleton.GetRarityColor(weapon.rarity);
                colors.Add(tmpColor);
            }

            View.lightUIAnim.GetComponent<Image>().color = colors[0];
            View.heavyUIAnim.GetComponent<Image>().color = colors[1];
            View.rangedUIAnim.GetComponent<Image>().color = colors[2];
            View.specialUIAnim.GetComponent<Image>().color = colors[3];
        }

        bool UIAnimationFinish() { return weapoUInAnimationFinish; }
        /// <summary>
        /// In base al disegno dell'arma va a caricare un'animazione diversa
        /// </summary>
        /// <param name="renderType"></param>

        void SetAnimationForWeaponType(TypeDatabase.AttackType weaponType)
        {
            switch (weaponType)
            {
                case TypeDatabase.AttackType.Light:
                    GameApplication.Singleton.view.BattleView.lightUIAnim.SetActive(true);
                    break;
                case TypeDatabase.AttackType.Heavy:
                    GameApplication.Singleton.view.BattleView.heavyUIAnim.SetActive(true);
                    break;
                case TypeDatabase.AttackType.Ranged:
                    GameApplication.Singleton.view.BattleView.rangedUIAnim.SetActive(true);
                    break;
                case TypeDatabase.AttackType.Special:
                    GameApplication.Singleton.view.BattleView.specialUIAnim.SetActive(true);
                    break;
                default:
                    break;
            }

        }

        void CleanWeaponAnimationOnClose()
        {
            GameApplication.Singleton.view.BattleView.lightUIAnim.SetActive(false);
            GameApplication.Singleton.view.BattleView.heavyUIAnim.SetActive(false);
            GameApplication.Singleton.view.BattleView.rangedUIAnim.SetActive(false);
            GameApplication.Singleton.view.BattleView.specialUIAnim.SetActive(false);
        }


        List<float> moltiplicators = new List<float> { 1.2f, 1.5f, 2, 2.2f, 2.5f, 3 };
        public int buffedTime = 0;
        public int moltiplicator = 0;

        public static bool UIPlayerLifeAlreadyUpdate = false;
        public static bool UIPlayerStaminaAlreadyUpdate = false;

        public static bool UIPlayerLifeTextAlreadyUpdate = false;
        public static bool UIPlayerStaminaTextAlreadyUpdate = false;

        public static bool UIEnemiesLifeAnimationFinish = false;
        public static bool UIEnemiesLifeTextCountUpdateFinish = false;
        public void UpdateActorStats(Unit target)
        {
            if (target.isControllable)
            {
                if (target.hp <= 0) //se è morto
                {
                    target.isDead = true;
                    BattlePlayerManager.Destroy(target);
                    Singleton.PlayerDeathInBattle();
                    deathLog = string.Format(Localization.Get(LocalizationIDDatabase.DEATH_BATTLE_LOG), Localization.Get(target.name));
                }
                else
                {
                    allUnits[SearchIndexInAllUnits(target)] = target;
                }
            }

            else
            {
                if (target.hp <= 0) // se è morto
                {
                    DeathAnimationAndDestroy(target);
                }

                else
                {
                    //UpdateHealthbar(target);
                    allUnits[SearchIndexInAllUnits(target)] = target;
                    enemies[SearchIndexInEnemies(target)] = target;
                }
            }
        }

        void DeathAnimationAndDestroy(Unit target)
        {
            target.isDead = true;
            BattleEnemyManager.RemoveTargetFromDeathEnemy();
            BattleEnemyManager.EnemyAnimationController(target);

            Unit unit = allUnits[SearchIndexInAllUnits(target)];
            allUnits.Remove(unit);
            enemies.Remove(unit);
            string localizedName = Localization.Get(target.name);
            consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.DEATH_BATTLE_LOG), localizedName);
        }


        void UpdateHealthbar(Unit target, float newLifeAmount)
        {
            if (newLifeAmount <= 0) { newLifeAmount = 0; }

            switch (target.placeID)
            {
                case "CentralEnemy":
                    centralEnemyhealthbar.UpdateCentralEnemyCurrentLife(newLifeAmount);
                    break;
                case "LeftEnemy":
                    leftEnemyhealthbar.UpdateLeftEnemyLife();
                    break;
                case "RightEnemy":
                    rightEnemyhealthbar.UpdateRightEnemyLife();
                    break;

                default:
                    break;
            }
        }

        void SetEnemiesHealthbarOnStartBattle(Unit target)
        {
            switch (target.placeID)
            {
                case "CentralEnemy":
                    centralEnemyhealthbar.SetCentralEnemyLifeOnStartFight(target);
                    break;
                case "LeftEnemy":
                    // leftEnemyhealthbar.UpdateLeftEnemyLife();
                    break;
                case "RightEnemy":
                    // rightEnemyhealthbar.UpdateRightEnemyLife();
                    break;
                default:
                    break;
            }
        }

        bool PlayerPlayed()
        {
            return playerPlayed;
        }

        bool LifePointsUpdated()
        {
            return UIEnemiesLifeAnimationFinish;
        }
        bool LifePointsTextUpdate()
        {
            return UIEnemiesLifeTextCountUpdateFinish;
        }

        bool FinishTurn()
        {
            return finished;
        }

        public void UpdateAPText(int AP)
        {
            BattlePlayerManager.currentTxtAPCount.text = AP.ToString();
        }

        public void UpdatePlayerAP(Unit unit, int abilityPointCost, bool constCost)
        {
            if (constCost)
            {
                unit.abilityPoints = unit.abilityPoints - abilityPointCost;
            }
            else
            {
                unit.abilityPoints = unit.abilityPoints - abilityPointCost; //sostituire con buffedtime e mettere 0 come parametro passato se torna il buff
            }

            BattlePlayerManager.currentTxtAPCount.text = unit.abilityPoints.ToString();
            allUnits[SearchIndexInAllUnits(unit)] = unit;
            allies[SearchIndexInAllies(unit)] = unit;
            //BuffManager.powerupInUse = 0;
        }

        Unit PayStamina(ref Unit attacker, Weapon currentWeapon)
        {
            if (!attacker.mainCharacter) { return attacker; }

            float currentStamina = attacker.stamina - currentWeapon.staminaCost;
            if (currentStamina < 0) currentStamina = 0;
            GameApplication.Singleton.model.SingletonStaminaBarManager.UpdateCurrentStaminaBar(currentStamina);

            attacker.SetRemainingStamina(currentWeapon.staminaCost);

            return attacker;
        }

        public IEnumerator UIRecoverStaminaBar(Unit mainPlayerUnit, float recoveredStamina, bool isTurnRecover = true)
        {
            if (!mainPlayerUnit.mainCharacter) { yield return null; }

            float currentStamina = mainPlayerUnit.stamina + recoveredStamina;
            if (currentStamina > Singleton.GetStaminaLimit()) { currentStamina = Singleton.GetStaminaLimit(); }

            GameApplication.Singleton.model.SingletonStaminaBarManager.UpdateCurrentStaminaBar(currentStamina);
        }

        public Unit RecoverUnitStamina(ref Unit mainPlayerUnit, float recoveredStamina)
        {
            if (!mainPlayerUnit.mainCharacter) { return mainPlayerUnit; }
            float currentStamina = mainPlayerUnit.stamina + recoveredStamina;
            if (currentStamina > Singleton.GetStaminaLimit()) { currentStamina = Singleton.GetStaminaLimit(); }

            mainPlayerUnit.RecoverStamina(recoveredStamina);
            return mainPlayerUnit;
        }



        void AlreadyPlayed(Unit attacker)
        {
            attacker.attack = true;
            allUnits[SearchIndexInAllUnits(attacker)] = attacker;
        }

        void UpdateEnemyUILifeBar(Unit unit)
        {
            switch (unit.placeID)
            {
                case "CentralEnemy":
                    //GameApplication.Singleton.model.CentralEnemyHealthbar.UpdateCentralEnemyCurrentLife(unit.hp);
                    break;
                case "RightEnemy":
                    GameApplication.Singleton.model.RightEnemyHealthbar.SetLifeBarHUD(unit);
                    break;
                case "LeftEnemy":
                    GameApplication.Singleton.model.LeftEnemyHealthbar.SetLifeBarHUD(unit);
                    break;
                default:
                    break;
            }
        }

        bool enemyUseAbility;
        byte indestructibleCounter = 0;
        int abilityCounter = 0;
        IEnumerator EnemyIAController(Unit currentUnit)
        {
            if (currentUnit.isUnable)
            {
                if (Random.Range(0, 100) < 80)
                {
                    consoleLog = "Essendo accecato ha fallito l'attacco.";
                    WriteLogByButton(consoleLog);
                    yield return new WaitUntil(PlayerClickOnLogTxt);
                    RefreshSkippedLog();
                    yield break;
                }

                consoleLog = "E' accecato ma non ha fallito l'attacco.";
                WriteLogByButton(consoleLog);
                yield return new WaitUntil(PlayerClickOnLogTxt);
                RefreshSkippedLog();
            }

            if (currentUnit.indestructible)
            {
                indestructibleCounter++;
                if (indestructibleCounter == 2)
                {
                    enemyUseAbility = true;
                    EnemyChoiceAbility(currentUnit);
                    indestructibleCounter = 0;
                }
                else
                {
                    EnemyStartAttack(currentUnit);
                }
            }
            else
            {
                if (currentUnit.ability.Count == 0)
                {
                    EnemyStartAttack(currentUnit);
                }
                else
                {
                    abilityCounter += EnemyAbilityManager();

                    if (abilityCounter >= 5)
                    {
                        abilityCounter -= 5;
                        enemyUseAbility = true;
                        EnemyChoiceAbility(currentUnit);
                    }
                    else
                    {
                        EnemyStartAttack(currentUnit);
                    }
                }
            }
        }

        int EnemyAbilityManager()
        {
            switch (Singleton.selectedDifficulty)
            {
                case Difficulty.Coward:
                    return 2;
                case Difficulty.Fearless:
                    return 3;
                case Difficulty.Insane:
                    return 4;
                default:
                    break;
            }
            return 0;
        }


        void EnemyStartAttack(Unit currentUnit)
        {
            enemyUseAbility = false;
            StartCoroutine(EnemyAttack(currentUnit, EnemyChoiceSingleTarget()));
        }


        Unit EnemyChoiceSingleTarget()
        {
            return allUnits.Find(u => u.mainCharacter);
        }

        string tmpAbilityLog;
        void EnemyChoiceAbility(Unit attacker)
        {
            tmpAbilityLog = string.Empty;
            int random = Random.Range(0, attacker.ability.Count);
            EnemyUseAbility(attacker, attacker.ability[random]);
            ScriptableAbility ability = attacker.ability[random];

            abilityLog = string.Format(Localization.Get(LocalizationIDDatabase.USE_BATTLE_LOG), Localization.Get(attacker.name), Localization.Get(ability.localizedID));
        }

        IEnumerator EnemyAttack(Unit attacker, Unit target)
        {
            string targetLocalizedName = Localization.Get(target.name);
            string attackerLocalizedName = Localization.Get(attacker.name);

            if (target.isInvurnerable)
            {
                consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.IMMUNE_BATTLE_LOG), targetLocalizedName);
                WriteLogByButton(consoleLog);
                yield return new WaitUntil(PlayerClickOnLogTxt);
                RefreshSkippedLog();
            }
            else
            {
                consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.USE_BATTLE_LOG), attackerLocalizedName, Localization.Get(attacker.baseAttackName));
                float damage = 0;
                damage = GetEnemyAttackDamageAmount(target, attacker);

                if (!enemyFailedToAttack)
                {
                    target.TakeDamage(damage, target);
                }
                if (!enemyFailedToAttack && !enemyZeroDamage)
                {
                    HitEffect.Instance.Hit();
                    ShakeBattleground.Instance.ShakeObject(damage);
                }
                UpdateActorStats(target); // aggiorno personaggio principale
            }
            BattleEnemyManager.EnemyAnimationController(attacker, null, true, false, false, 1);
        }

        void EnemyUseAbility(Unit attacker, ScriptableAbility ability)
        {
            string attackerLocalizedName = Localization.Get(attacker.name);

            if (ability.moveType.Contains(ScriptableAbility.TargetType.TargetEnemy))
            {
                Unit target = EnemyChoiceSingleTarget();

                if (ability.CanUseThis(attacker, target))
                {
                    System.Tuple<Unit, string> tuple = ability.TriggerAbility(attacker, target, ability.HavePowerup, false, ability.NumberOfPowerup);
                    target = tuple.Item1;
                    effectiveLog = Localization.Get(tuple.Item2);
                    UpdateActorStats(target);
                    // SetVFXPositionsOnPlayerTurn(VFXAbilityPositionIsPlayer);
                    if (ability.VFXID != null)
                    {
                        CallAbilityAnimation(ability.VFXID);
                    }
                    BattleEnemyManager.EnemyAnimationController(attacker, ability.animationTriggerID.ToString());
                }
                else
                {
                    consoleLog = Localization.Get(LocalizationIDDatabase.DODGE_LOG);
                }
            }

            if (ability.moveType.Contains(ScriptableAbility.TargetType.TargetActor))
            {
                consoleLog = string.Format("{0} usa {1}.", attackerLocalizedName, ability.abilityName);

                System.Tuple<Unit, string, bool> tuple = ability.BuffActor(attacker, false);
                attacker = tuple.Item1;
                effectiveLog = tuple.Item2;
                UpdateActorStats(attacker);
                if (tuple.Item3)
                {
                    //Il nemico recupera vita
                    //Il nemico non fa danni
                }
                else
                {
                    //Il nemico non recupera vita e non fa danni
                }
                SetVFXPositionsOnPlayerTurn(false);
                CallAbilityAnimation(ability.VFXID);
                BattleEnemyManager.EnemyAnimationController(attacker, ability.animationTriggerID.ToString());
            }
        }

        public int GetMainPlayerIndex()
        {
            int targetIndex = allUnits.FindIndex(u => u.mainCharacter == true);
            return targetIndex;
        }

        public int SearchIndexInAllUnits(Unit myUnit)
        {
            int targetIndex = allUnits.FindIndex(u => u.name == myUnit.name);
            return targetIndex;
        }
        public int SearchIndexInAllies(Unit myUnit)
        {
            int targetIndex = allies.FindIndex(u => u.name == myUnit.name);
            return targetIndex;
        }

        public int SearchIndexInEnemies(Unit myUnit)
        {
            int targetIndex = enemies.FindIndex(u => u.name == myUnit.name);
            return targetIndex;
        }

        IEnumerator SetStatusHUD(Unit unit, float time)
        {
            yield return new WaitForSeconds(time);

            BattleView Battle = GameApplication.Singleton.view.BattleView;
            Battle.pnlButton.SetActive(true);
            Battle.SetName(unit);
            //BattlePlayerManager.SetAbilities(unit.ability);
        }

        /// <summary>
        /// Serve per aprire la console del testo prima di scrivere il log
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerator ClosePlayerCommandPanel(float time = 0)
        {
            yield return new WaitForSeconds(time);
            BattleView Battle = GameApplication.Singleton.view.BattleView;
            Battle.pnlButton.SetActive(false);
            BattlePlayerManager.abilityPanel.SetActive(false);
            BattlePlayerManager.weaponsPanel.SetActive(false);
            BattlePlayerManager.inventoryPanel.SetActive(false);
            BattlePlayerManager.shieldsPanel.SetActive(false);
        }

        public static bool playerSkipText = false;
        public static bool animationIsFinished = false;

        /// <summary>
        /// La console attende l'input del giocatore per andare avanti
        /// controllare che i rifrimwnti siano uguali a PlayerClickOnLogTxt
        /// </summary>
        /// <param name="txtLog"></param>
        public void WriteLogByButton(string txtLog)
        {
            btnSkipText.gameObject.SetActive(true);
            StartCoroutine(dialogConsoleManager.TypeDialogTxt(txtLog));
        }

        void ShowFirstTimeTutorial()
        {
            BattleView BattleView = GameApplication.Singleton.view.BattleView;
            BattleView.FirstPanelFightTutorial.SetActive(true);
        }
        public void RefreshConsoleText()
        {
            StartCoroutine(dialogConsoleManager.TypeDialogTxt(string.Empty));
        }
        bool PlayerClickOnLogTxt()
        {
            return playerSkipText;
        }
        public bool AbilityAnimationIsFinish()
        {
            return animationIsFinished;
        }

        bool BattleIsFinish()
        {
            if ((enemies.Count == 0) || (Singleton.PlayerIsDead()))
            {
                return true;
            }
            return false;
        }

        public void LoseBattle()
        {
            View.ShowLosePanel();
            UploadPlayerStats();
        }

        void UploadPlayerStats()
        {
            int index = allUnits.FindIndex(u => u.mainCharacter);
            Singleton.lifePoints = allUnits[index].hp; //ho aggiunto questa riga per aggiornare gli hp del giocatore a fine battaglia
            Singleton.currentAP = allUnits[index].abilityPoints; //ho aggiunto questa riga per aggiornare gli hp del giocatore a fine battaglia
                                                                 //Singleton.lifePoints = allUnits[index].hp;
        }

        void SortByDexterity()
        {
            allUnits = allUnits.OrderBy(u => u.dexterity).ToList();
            allUnits.Reverse();
        }

        public List<ScriptableEnemy> GetMobID()
        {
            List<ScriptableEnemy> unitsID = new List<ScriptableEnemy>();

            foreach (ScriptableEnemy unit in PlayerManager.Singleton.currentPage.mobID)
            {
                unitsID.Add(unit);
            }
            return unitsID;
        }

        string[] placeID = new string[3] { "CentralEnemy", "RightEnemy", "LeftEnemy" };
        int i = 0;
        public void CreateEnemyUnits()
        {
            foreach (ScriptableEnemy u in GetMobID())
            {
                if (actorData.Contains(u))
                {
                    Unit unit = new Unit(u.enemyName, u.constitution, u.dexterity, u.strength, u.inteligence, u.ability, u.canAttack, u.hp, u.isDead, false, 0, u.enemyImage,
                        u.normalMap, false, 0, placeID[i], u.equippedWeapon, u.itemInventory, u.myItems, u.healedUnit, u.elementalLevel,
                        u.animController, u.enemyPSB, u.scale, u.localPosition, u.defence, u.baseAttackType, u.defenseType, false, false,
                        u.finalAbility, u.baseAttack, u.level, u.noStatus, u.baseAttackName, 0, 0, u.baseAttackSound, u.dodgeSound, u.battleScreamSound, u.hitSound, u.enemyUISprite, u.indestructible, u.enemyShadePrefab);
                    enemies.Add(unit);
                    BattleEnemyManager.AssignUnitPlace(unit);
                    AddToAllUnitList(unit);
                    SetEnemiesHealthbarOnStartBattle(unit);
                    i++;
                }
                else
                {
                    Debug.LogError("Nessun nemico da inizializzare nella pagina, riguardare database in model e sotto fightenemies");
                }
            }
            //this.BattleEnemyManager.ShowEnemy();
            i = 0;
        }

        public void CreateTestEnemyUnits(ScriptableEnemy u)
        {
            Unit unit = new Unit(u.enemyName, u.constitution, u.dexterity, u.strength, u.inteligence, u.ability, u.canAttack, u.hp, u.isDead, false, 0, u.enemyImage,
                u.normalMap, false, 0, placeID[i], u.equippedWeapon, u.itemInventory, u.myItems, u.healedUnit, u.elementalLevel,
                u.animController, u.enemyPSB, u.scale, u.localPosition, u.defence, u.baseAttackType, u.defenseType, false, false,
                u.finalAbility, u.baseAttack, u.level, u.noStatus, u.baseAttackName, 0, 0, u.baseAttackSound, u.dodgeSound, u.battleScreamSound, u.hitSound, u.enemyUISprite, u.indestructible, u.enemyShadePrefab);
            enemies.Add(unit);
            BattleEnemyManager.AssignUnitPlace(unit);
            AddToAllUnitList(unit);
            SetEnemiesHealthbarOnStartBattle(unit);
            i++;
        }
        public void CreateTestPlayerUnit()
        {
            PrepareRightAbilities();

            Unit classPlayer = new Unit("TEST", Singleton.constitution, Singleton.dexterity,
                                       Singleton.strength, Singleton.inteligence, unitAbilities,
                                       false, Singleton.lifePoints, Singleton.PlayerIsDead(), true, -1, Singleton.classImage,
                                       null, true, 0, "Player", Singleton.EquippedWeaponToList(), Singleton.inventory,
                                       null, Singleton.healedUnit, Singleton.elementalLevel, null, null, null, null,
                                       Singleton.GetDefenceArmor(), TypeDatabase.AttackType.Heavy, Singleton.GetDefenceType(), false, false, finalAbility,
                                       0, Singleton.GetPlayerLevel, Singleton.noStatus, Singleton.baseAttackName, Singleton.GetPlayerAbilityPoints, Singleton.GetPlayerStamina,
                                       null, Singleton.playerEquipment.equippedBalancedDefence.hitSound);


            allies.Add(classPlayer);
            AddToAllUnitList(classPlayer);
            GameApplication.Singleton.model.playerHealthbarManager.SetLifeOnStartFight(classPlayer.hp);

            float staminaFirstWeaponCost = Singleton.equippedWeapon[0].staminaCost;

            GameApplication.Singleton.model.SingletonStaminaBarManager.SetStaminaOnStartFight(Singleton.GetPlayerStamina, staminaFirstWeaponCost);
        }




        public List<ScriptableEnemy> GetAllyID()
        {
            if (Singleton.currentPage.allyID.Count > 0)
            {
                List<ScriptableEnemy> unitsID = new List<ScriptableEnemy>();

                foreach (ScriptableEnemy ally in PlayerManager.Singleton.currentPage.allyID)
                {
                    unitsID.Add(ally);
                }
                return unitsID;
            }
            return null;

        }
        public void CreateAllyUnit()
        {
            foreach (ScriptableEnemy u in GetAllyID())
            {
                if (actorData.Contains(u))
                {
                    Unit unit = new Unit(u.name, u.constitution, u.dexterity, u.strength, u.inteligence, u.ability, u.canAttack, u.hp, u.isDead, true, 0,
                        u.enemyImage, null, false, 0, "SecondPlayer", u.equippedWeapon, u.itemInventory, u.myItems, u.healedUnit,
                        u.elementalLevel, u.animController, u.enemyPSB, u.scale, u.localPosition, u.defence, u.baseAttackType,
                        u.defenseType, false, false, u.finalAbility, u.baseAttack, u.level, u.noStatus, u.baseAttackName, 0, 0, u.baseAttackSound, u.dodgeSound, u.battleScreamSound, u.hitSound);
                    allies.Add(unit);
                    AddToAllUnitList(unit);
                }
            }
        }

        List<ScriptableAbility> unitAbilities;
        ScriptableAbility finalAbility;

        void PrepareRightAbilities()
        {
            if (Singleton.superstition > 50)
            {
                unitAbilities = Singleton.highPlayerAbility;
                finalAbility = Singleton.highFinalAbility;
            }
            else
            {
                unitAbilities = Singleton.playerAbility;
                finalAbility = Singleton.finalAbility;
            }

        }

        //Ci sarà da passare il tipo di difesa dell'armatura
        public void CreatePlayerUnit()
        {
            PrepareRightAbilities();

            Unit classPlayer = new Unit(Singleton.playerName, Singleton.constitution, Singleton.dexterity,
                                       Singleton.strength, Singleton.inteligence, unitAbilities,
                                       false, Singleton.lifePoints, Singleton.PlayerIsDead(), true, -1, Singleton.classImage,
                                       null, true, 0, "Player", Singleton.EquippedWeaponToList(), Singleton.inventory,
                                       null, Singleton.healedUnit, Singleton.elementalLevel, null, null, null, null,
                                       Singleton.GetDefenceArmor(), TypeDatabase.AttackType.Heavy, Singleton.GetDefenceType(), false, false, finalAbility,
                                       0, Singleton.GetPlayerLevel, Singleton.noStatus, Singleton.baseAttackName, Singleton.GetPlayerAbilityPoints, Singleton.GetPlayerStamina,
                                       null, Singleton.playerEquipment.equippedBalancedDefence.hitSound);


            allies.Add(classPlayer);
            AddToAllUnitList(classPlayer);
            GameApplication.Singleton.model.playerHealthbarManager.SetLifeOnStartFight(classPlayer.hp);

            float staminaFirstWeaponCost = Singleton.equippedWeapon[0].staminaCost;

            GameApplication.Singleton.model.SingletonStaminaBarManager.SetStaminaOnStartFight(Singleton.GetPlayerStamina, staminaFirstWeaponCost);
        }

        public void AddToAllUnitList(Unit unit)
        {
            allUnits.Add(unit);
        }

        List<bool> allAttack = new List<bool>();
        int unitsInScene;

        public bool IsTurnFinished() //ogni volta che attacca richiamo la funzione
        {
            unitsInScene = allUnits.Count;

            foreach (bool unit in allAttack)
            {
                Debug.Log(unit);
            }

            foreach (Unit unit in allUnits)
            {
                if (!unit.attack) { return false; }

                if (unit.attack)
                {
                    allAttack.Add(unit.attack);
                }
            }

            if (allAttack.Count == unitsInScene)
            {
                return true;
            }
            return false;
        }

        public void WaitInput(int index)
        {
            abilityIndex = index;
            abilityClicked = true;
        }

        public bool IsClicked()
        {
            return abilityClicked;
        }

        public float GetEnemyAttackDamageAmount(Unit target, Unit attacker)
        {
            float damage = attacker.baseAttack;
            if (damage == 0)
            {
                enemyZeroDamage = true;
                //effectiveLog = Localization.Get(LocalizationIDDatabase.ZERO_DAMAGE);
                return 1;
            }

            switch (attacker.baseAttackType)
            {
                case TypeDatabase.AttackType.Light:
                    damage = damage + (attacker.dexterity / 100 * damage);
                    break;
                case TypeDatabase.AttackType.Heavy:
                    damage = damage + (attacker.strenght / 100 * damage);
                    break;
                case TypeDatabase.AttackType.Ranged:
                    damage = ((attacker.dexterity / 2 + attacker.inteligence / 2) / 100) * damage;
                    break;
                default:
                    break;
            }

            int hit = Random.Range((int)attacker.dexterity, 100);
            damageIsCritical = false;

            if (hit > target.dexterity / 2)
            {
                int result = Random.Range(0, 100);
                if (result > 80)
                {
                    damage = damage * 2;
                    damageIsCritical = true;
                }
            }
            else if (hit < target.dexterity / 2)
            {
                effectiveLog = Localization.Get(LocalizationIDDatabase.MISS_LOG);
                enemyFailedToAttack = true;
                return 0;
            }

            if (damage == 0)
            {
                enemyZeroDamage = true;
                //effectiveLog = Localization.Get(LocalizationIDDatabase.ZERO_DAMAGE);
                return 1;
            }
            else
            {
                enemyZeroDamage = false;
                enemyFailedToAttack = false;
            }

            damage = TypeDatabase.IsEffectiveOrNot(attacker.baseAttackType, target.DefenceType, damage, ref effectiveLog);
            damage = damage - target.defence;
            defenceLog = GetDefenceResultBattleConsoleLog(damage, false);
            return damage;
        }

        public static string GetDefenceResultBattleConsoleLog(float damage, bool attackerIsPlayer = true)
        {
            switch (damage)
            {
                case <= 0:
                    if (attackerIsPlayer)
                    {
                        return Localization.Get(LocalizationIDDatabase.ENEMY_DEFENCE_STRONG);
                    }
                    return Localization.Get(LocalizationIDDatabase.PLAYER_DEFENCE_STRONG);

                default: return string.Empty;
            }
        }
    }
}
