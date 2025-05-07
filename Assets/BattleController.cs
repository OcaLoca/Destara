/* ----------------------------------------------
   * 
   * 				MobyBit
   * 
   * Creation Date: 01/09/2020 00:10:30
   * 
   * Copyright � MobyBit
   * ----------------------------------------------
   */

using StarworkGC.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using SmartMVC;
using static Game.PlayerManager;
using StarworkGC.Localization;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace Game
{
    public class BattleController : Controller<GameApplication>
    {
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
        [SerializeField] GameObject PanelInfoActor, PanelHideShake;
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
        public static string effectiveLog = string.Empty;
        public static string defenceLog;
        public static bool damageIsCritical = false;
        public static bool zeroDamage = false;
        public static bool enemyZeroDamage = false;
        public static bool failedToAttack = false;
        public static bool enemyFailedToAttack = false;
        public static bool enemyIsDeadInThisTurn = false;
        ScriptableEnemy[] actorData;
        BattleView View => app.view.BattleView;

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

        public struct Unit
        {
            public string name;
            public float constitution;
            public float dexterity;
            public float strenght;
            public float inteligence;
            public List<ScriptableAbility> ability;
            public bool attack;
            public float hp;
            public bool isDead;
            public bool isControllable;
            public float lastDamageTake;
            public bool mainCharacter;
            public Sprite sprite;
            public Material normalMap;
            [Range(0, 5)]
            public int powerUp;
            public string placeID;
            public List<Weapon> equippedWeapon;
            public Dictionary<ScriptableItem, int> bag;
            public List<ScriptableItem> equippedItem; //questo serve solo per alleato e nemici che non hanno un dizionario
            public Dictionary<Stats, int> stats;
            public float elementalLevel;
            public RuntimeAnimatorController runtimeAnimatorController;
            public GameObject PSB;
            public object scale;
            public object localPosition;
            public float defence;
            public TypeDatabase.AttackType baseAttackType;
            public TypeDatabase.DefenseType DefenceType;
            public bool isInvurnerable;
            public bool isUnable;
            public ScriptableAbility finalAbility;
            public float baseAttack;
            public int level;
            public bool noStatus;
            public string baseAttackName;
            public AudioClip baseAttackSound;
            public AudioClip dodgeSound;
            public AudioClip battleScreamSound;
            public AudioClip hitSound;
            public Sprite enemyUISprite;
            public bool indestructible;
            public GameObject enemyShade;
            public int abilityPoints;
            public float stamina;
            internal string IDToLoadEnemyPrefab;


            public Unit(string name, float constitution, float dexterity, float strenght, float inteligence, List<ScriptableAbility> ability, bool canAttack, float hp, bool isDead, bool isControllable, float lastDamageTake, Sprite sprite, Material normalMap, bool mainCharacter, int powerUp, string placeID, List<Weapon> equippedWeapon, Dictionary<ScriptableItem, int> bag, List<ScriptableItem> equippedItem, Dictionary<Stats, int> stats, float elementalLevel,
                RuntimeAnimatorController runtimeAnimatorController, GameObject PSB, object scale, object localPosition, float defence, TypeDatabase.AttackType baseAttackType, TypeDatabase.DefenseType baseDefenseType, bool isInvurnerable, bool isUnable, ScriptableAbility finalAbility, float baseAttack, int level, bool noStatus,
                string baseAttackName, int abilityPoints, float stamina, AudioClip baseAttackSound = null, AudioClip dodgeSound = null, AudioClip battleScreamSound = null, AudioClip hitSound = null, Sprite enemyUISprite = null,
                bool indestructible = false, GameObject enemyShade = null, string IDToLoadEnemyPrefab = null)
            {
                this.IDToLoadEnemyPrefab = IDToLoadEnemyPrefab;
                this.name = name;
                this.constitution = constitution;
                this.dexterity = dexterity;
                this.strenght = strenght;
                this.inteligence = inteligence;
                this.ability = ability;
                this.attack = canAttack;
                this.hp = hp;
                this.isDead = isDead;
                this.isControllable = isControllable;
                this.lastDamageTake = lastDamageTake;
                this.sprite = sprite;
                this.normalMap = normalMap;
                this.mainCharacter = mainCharacter;
                this.powerUp = powerUp;
                this.placeID = placeID;
                this.abilityPoints = (int)abilityPoints;
                this.stamina = stamina;
                this.equippedWeapon = equippedWeapon;
                this.bag = bag;
                this.equippedItem = equippedItem;
                this.stats = stats;
                this.elementalLevel = elementalLevel;
                this.runtimeAnimatorController = runtimeAnimatorController;
                this.PSB = PSB;
                this.scale = scale;
                this.localPosition = localPosition;
                this.defence = defence;
                this.baseAttackType = baseAttackType;
                this.DefenceType = baseDefenseType;
                this.isInvurnerable = isInvurnerable;
                this.isUnable = isUnable;
                this.finalAbility = finalAbility;
                this.baseAttack = baseAttack;
                this.level = level;
                this.noStatus = noStatus;
                this.baseAttackName = baseAttackName;
                this.baseAttackSound = baseAttackSound;
                this.dodgeSound = dodgeSound;
                this.battleScreamSound = battleScreamSound;
                this.hitSound = hitSound;
                this.enemyUISprite = enemyUISprite;
                this.indestructible = indestructible;
                this.enemyShade = enemyShade;
            }

            public void TakeDamage(float damage, Unit unit, bool damageSourceIsStatus = false)
            {
                if (damage <= 0)
                {
                    if (unit.mainCharacter)
                    {
                        enemyZeroDamage = true;
                    }
                    return;
                }

                hp = hp - damage;
                lastDamageTake = damage;

                if (unit.mainCharacter)
                {
                    GameApplication.Singleton.model.playerHealthbarManager.UpdateCurrentLife(hp);
                    GameApplication.Singleton.view.BattleView.battlePlayerManager.PreparePlayerFloatingPopup(unit, damage, damageSourceIsStatus);
                }
                else
                {
                    GameApplication.Singleton.view.BattleView.battleController.UpdateHealthbar(unit, hp);
                    GameApplication.Singleton.view.BattleView.battleEnemyManager.PrepareCentralEnemyFloatingPopup(unit, damage, damageSourceIsStatus);
                }
            }

            public void SetRemainingStamina(float remainingStamina)
            {
                stamina = stamina - remainingStamina;
                if (stamina <= 0) stamina = 0;
            }

            public void RecoverStamina(float recoveredStamina)
            {
                stamina = stamina + recoveredStamina;
                if (stamina > Singleton.GetStaminaLimit())
                { stamina = Singleton.GetStaminaLimit(); }
            }

            public void RecoverLife(float hpAmount)
            {
                hp = hp + hpAmount;

                float hpLimit = constitution * 10;

                if (hp >= hpLimit)
                {
                    hp = hpLimit;
                }
                else if (hp < 0)
                {
                    hp = 0;
                }
                Singleton.UpdateLifePoints(hpAmount);
                GameApplication.Singleton.model.playerHealthbarManager.UpdateCurrentLife(hp);
            }

            public void RecoverStamina(int staminaAmount)
            {
                stamina = stamina + staminaAmount;
                stamina = stamina > Singleton.GetStaminaLimit() ? stamina = Singleton.GetStaminaLimit() : stamina; //Qua prendo dalla classe e non dal valore dentro lp'unità che ppotrebbe baffarsi
                stamina = stamina < 0 ? 0 : stamina;

                Singleton.SetPlayermanagerStamina(stamina);
            }
            public void RecoverAbilityPointsInBattle(int abilityPointsAmount)
            {
                abilityPoints = abilityPoints + abilityPointsAmount;
                abilityPoints = abilityPoints > Singleton.GetAbilityPointsLimit() ? abilityPoints = Singleton.GetAbilityPointsLimit() : abilityPoints; //Qua prendo dalla classe e non dal valore dentro lp'unità che ppotrebbe baffarsi
                abilityPoints = abilityPoints < 0 ? 0 : abilityPoints;

                Singleton.SetPlayerAbilityPoints(abilityPoints);
                Singleton.UpdateBattleAPTxt(abilityPoints);
            }

            public void BuffDebuffStats(float buffPercentage, ScriptableAbility.Buff valueToBuff)
            {
                float buffValue = 0;

                switch (valueToBuff)
                {
                    case ScriptableAbility.Buff.strenght:
                        Debug.LogFormat("Sto baffando {0} che ha forza {1}", name, strenght);
                        buffValue = (strenght / 100) * buffPercentage;
                        strenght = strenght + buffValue;
                        Debug.LogFormat("Ora ha forza {0}", strenght);
                        break;
                    case ScriptableAbility.Buff.dexterity:
                        Debug.LogFormat("Sto baffando {0} che ha destrezza {1}", name, dexterity);
                        buffValue = (dexterity / 100) * buffPercentage;
                        dexterity = dexterity + buffValue;
                        Debug.LogFormat("Ora ha destrezza {0}", dexterity);
                        break;
                    case ScriptableAbility.Buff.hp:
                        //buffValue = (hp / 100) * buffPercentage;
                        hp = hp + buffPercentage;
                        if (hp >= constitution * 10)
                        {
                            hp = constitution * 10;
                        }
                        else if (hp < 0)
                        {
                            hp = 0;
                        }
                        Singleton.UpdateLifePoints(buffPercentage, 0);
                        // GameApplication.Singleton.model.playerHealthbarManager.UpdateCurrentLife(hp);
                        break;
                    case ScriptableAbility.Buff.defence:
                        Debug.LogFormat("Sto baffando {0} che ha difesa {1}", name, defence);
                        buffValue = (defence / 100) * buffPercentage;
                        defence = defence + buffValue;
                        Debug.LogFormat("Ora ha difesa {0}", defence);
                        break;
                    case ScriptableAbility.Buff.inteligence:
                        Debug.LogFormat("Sto baffando {0} che ha intelligenza {1}", name, inteligence);
                        buffValue = (inteligence / 100) * buffPercentage;
                        inteligence = inteligence + buffValue;
                        Debug.LogFormat("Ora ha intelligenza {0}", inteligence);
                        break;
                    case ScriptableAbility.Buff.costitution:
                        Debug.LogFormat("Sto baffando {0} che ha costituzione {1}", name, constitution);
                        buffValue = (constitution / 100) * buffPercentage;
                        constitution = constitution + buffValue;
                        Debug.LogFormat("Ora ha costituzione {0}", constitution);
                        break;
                    case ScriptableAbility.Buff.baseAttack:
                        Debug.LogFormat("Sto baffando {0} che ha attacco base {1}", name, baseAttack);
                        buffValue = (baseAttack / 100) * buffPercentage;
                        baseAttack = baseAttack + buffValue;
                        Debug.LogFormat("Ora ha attacco {0}", baseAttack);
                        break;
                    default:
                        break;
                }
            }

            public float ScaleUnitAttackDamage(int scaledAmount, Unit unit)
            {
                float damage = 0;

                switch (baseAttackType)
                {
                    case TypeDatabase.AttackType.Light:
                        damage = unit.dexterity * scaledAmount;
                        break;
                    case TypeDatabase.AttackType.Heavy:
                        damage = unit.strenght * scaledAmount;
                        break;
                    case TypeDatabase.AttackType.Ranged:
                        damage = (unit.dexterity / 2 + unit.inteligence / 2) * scaledAmount;
                        break;
                    default:
                        break;
                }
                return damage;
            }

            public void BecomeInvurnerable()
            {
                isInvurnerable = true;
            }
            public void BecomeUnable()
            {
                isUnable = true;
            }

            public void TakeElementalStats(Stats element, int turn, Unit unit, float elementalLevel)
            {
                switch (element)
                {
                    case Stats.burned:
                        Debug.LogFormat("Sto applicando scottato a {0} per {1} turni", unit.placeID, turn);
                        break;
                    case Stats.poisoned:
                        Debug.LogFormat("Sto applicando avvelenato a {0} per {1} turni", unit.placeID, turn);
                        break;
                    case Stats.confused:
                        Debug.LogFormat("Sto applicando confuso a {0} per {1} turni a {2}", unit.placeID, turn, name);
                        break;
                    case Stats.paralyzed:
                        Debug.LogFormat("Sto applicando paralizzato a {0} per {1} turni a {2}", unit.placeID, turn, name);
                        break;
                    case Stats.freezed:
                        Debug.LogFormat("Sto applicando congelato a {0} per {1} turni a {2}", unit.placeID, turn, name);
                        break;
                    default:
                        break;
                }

                if (elementalLevel != 0)
                {
                    this.elementalLevel = elementalLevel;
                }
                stats[element] = turn;
            }
        }

        private void Awake()
        {
            actorData = GameApplication.Singleton.model.ScriptableObjectsDatabase.enemiesDatabase;
            ResetBattleSettingsOnOpenBattleView();
        }

        private void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.OPEN_GAME_VIEW_AT, OpenGameAt);

            allUnits.Clear();
            enemies.Clear();
            allies.Clear();
            Singleton.UpdateDeathPlayerManagerCurrentState(); //Aggiorno perforza nel caso muore in un fight precedente ma ridò vita (ex tutorial)
            ResetBattleSettingsOnOpenBattleView();
            LoadActorFromPage();
            SortByMainCharacter();
            StartCoroutine(StartBattle());
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

        private void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.OPEN_GAME_VIEW_AT, OpenGameAt);
        }

        void LoadActorFromPage()
        {
            CreateEnemyUnits();
            CreatePlayerUnit();
        }

        int turn;

        void UpdateStats()
        {
            allUnits.Clear();
            allies.Clear();
            enemies.Clear();
            allAttack.Clear();
        }

        public static bool AlreadyShowTutorialOneTime = false;
        bool finished = false;
        IEnumerator StartBattle()
        {
            if (Singleton.currentPage.introLogTxtID == null) { consoleLog = string.Format(Localization.Get("introFightPuntini")); }
            else
            {
                consoleLog = string.Format(Localization.Get(Singleton.currentPage.introLogTxtID), Singleton.GetPlayerName());
            }

            abilityLog = string.Empty;
            effectiveLog = string.Empty;

            WriteLogByButton(consoleLog);
            yield return new WaitUntil(PlayerClickOnLogTxt);
            // PanelInfoActor.SetActive(true); // attivo UI stati player

            if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1 ||
                PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1)
            {
                if (TutorialSystemManager.FightTutorialPartNotShowed)
                {
                    TutorialSystemManager.FightTutorialPartNotShowed = false;
                    View.PanelFightTutorial.SetActive(true);
                }

            }
            RefreshSkippedLog();

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

            if (enemies.Count == 0)
            {
                // da mettere l'ultimo che hai sconfitto
                //BattleEnemyManager.RunEnemyDeathAnimation(); //qua prende la target sprite quindi va automatico sullultimo colpitpo
            }

            UploadPlayerStats();
            View.LoadVictoryLosePanel();
            View.LoadIDOnBattleEnd();
        }

        void ResetBattleSettingsOnOpenBattleView()
        {
            turn = 0;

            PanelInfoActor.SetActive(false);
            PanelHideShake.SetActive(false);
            BattleEnemyManager.DestroyAllEnemiesObject();
            BattleEnemyManager.CleanUIOnCloseBattleView();
            //Cancella le icone degli effetti di stato
            BattleEnemyManager.DeactivateEffectStatsIconContainerChild();
            BattlePlayerManager.DeactivateEffectStatsIconContainerChild();
            CleanWeaponAnimationOnClose();

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

            SortByMainCharacter();
            Debug.Log("Controllare se va in conflitto con lastIndex che dopo Sort a volte inverte i numeri, per ora risolvo togliendolo");

            for (int i = 0; i < allUnits.Count; i++)
            {
                Unit unit = allUnits[i];
                lastIndex = i;

                if (unit.isDead)
                {
                    continue;
                }
                unitLocalizedName = allUnits[i].name;

                if (!CheckoutUnitStatus(unit))
                {
                    BattleControllerHelper.Instance.CheckElementalStats(allUnits[i]);

                    if (unit.mainCharacter)
                    {
                        BattlePlayerManager.ActivatePlayerIconStatsEffect();
                    }
                    else
                    {
                        BattleEnemyManager.ActivateEnemyIconStatsEffect();
                    }

                    if (GetIsBurned)
                    {
                        string status = BattleControllerHelper.GetLocalizationStatusEffect(Stats.burned);
                        consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.TARGET_AFFECTED_BY_STATUS), unit.name, status);
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();

                        consoleLog = string.Format(BattleControllerHelper.GetLocalizationStatusPain(Stats.burned));
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();

                        unit.TakeDamage(unit.elementalLevel, unit, true);
                        UpdateEnemyUILifeBar(unit);
                        UpdateActorStats(unit); //aggiorno numero unita
                        if (allUnits.Count < 2)
                        {
                            //Mettere frase finale se si vuole
                            continue;
                        } //se quello del turno corrente è morto
                    }
                    if (GetIsPoisoned)
                    {
                        string status = BattleControllerHelper.GetLocalizationStatusEffect(Stats.poisoned);
                        consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.TARGET_AFFECTED_BY_STATUS), unit.name, status);
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();

                        consoleLog = string.Format(BattleControllerHelper.GetLocalizationStatusPain(Stats.poisoned));
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();


                        unit.TakeDamage(unit.elementalLevel, unit, true);
                        UpdateEnemyUILifeBar(unit);
                        UpdateActorStats(unit);
                        if (unit.mainCharacter)
                        {
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeBarFinish);
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeTextBarFinish);
                        }
                        else
                        {
                            yield return new WaitUntil(LifePointsUpdated);
                            yield return new WaitUntil(LifePointsTextUpdate);
                            UIEnemiesLifeAnimationFinish = false;
                            UIEnemiesLifeTextCountUpdateFinish = false;
                            if (allUnits.Count < 2)
                            {

                                //mettere frasi finale se si vuole
                                continue;
                            } //se è morto procedi 
                        }
                    }
                    if (GetIsParalyzed)
                    {
                        string status = BattleControllerHelper.GetLocalizationStatusEffect(Stats.paralyzed);
                        consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.TARGET_AFFECTED_BY_STATUS), unit.name, status);
                        WriteLogByButton(consoleLog);
                        yield return new WaitUntil(PlayerClickOnLogTxt);
                        RefreshSkippedLog();
                        continue;
                    }
                    if (GetIsConfused)
                    {
                        string status = BattleControllerHelper.GetLocalizationStatusEffect(Stats.confused);
                        consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.TARGET_AFFECTED_BY_STATUS), unit.name, status);
                        yield return dialogConsoleManager.TypeDialogTxt(consoleLog);

                        if (Random.Range(0, 101) > allUnits[i].elementalLevel)
                        {
                            consoleLog = string.Format(BattleControllerHelper.GetLocalizationStatusPain(Stats.confused));
                            WriteLogByButton(consoleLog);
                            yield return new WaitUntil(PlayerClickOnLogTxt);
                            RefreshSkippedLog();

                            unit.TakeDamage(unit.strenght * 2, unit, true);
                            UpdateActorStats(unit);

                            if (unit.mainCharacter)
                            {
                                yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeBarFinish);
                                yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeTextBarFinish);
                            }
                            else
                            {
                                yield return new WaitUntil(LifePointsUpdated);
                                yield return new WaitUntil(LifePointsTextUpdate);
                                UIEnemiesLifeAnimationFinish = false;
                                UIEnemiesLifeTextCountUpdateFinish = false;
                            }
                            continue;
                        }
                        else
                        {
                            consoleLog = Localization.Get("confusedFailed");
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
                else
                {
                    if (unit.mainCharacter)
                    {
                        BattlePlayerManager.DeactivateEffectStatsIconContainerChild();
                    }
                    else
                    {
                        BattleEnemyManager.pnlShowEnemyEffectsStats.SetActive(false);
                    }
                }

                if (unit.mainCharacter)
                {
                    consoleLog = Localization.Get(LocalizationIDDatabase.WHAT_DO_BATTLE_LOG);
                    yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                    LocalizeAbilitiesNames(unit);

                    battleTurn = BattleTurn.PlayerTurn;
                    PanelInfoActor.SetActive(true);
                    PanelHideShake.SetActive(true);

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
                        weaponLog = string.Format(Localization.Get(LocalizationIDDatabase.USE_BATTLE_LOG), Localization.Get(unit.name), unit.equippedWeapon[wpnIndex].GetLocalizedObjName());
                        yield return dialogConsoleManager.TypeDialogTxt(weaponLog);
                        PlayerAttackWithWeapon = false;
                        if (failedToAttack || zeroDamage || enemyIsDeadInThisTurn)
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
                            yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeBarFinish);
                            yield return new WaitUntil(GameApplication.Singleton.model.playerHealthbarManager.PlayerLifeTextBarFinish);
                            PlayerRecoverLife = false;
                            consoleLog = null; //se è null non parte una doppia scrittura a riga 784
                        }
                        else if (PlayerRecoverStamina)
                        {
                            StartCoroutine(UIRecoverStaminaBar(unit, StaminaRecoveredWithObj));
                            yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                            yield return new WaitUntil(GameApplication.Singleton.model.SingletonStaminaBarManager.PlayerStaminaBarAnimationFinish);
                            yield return new WaitUntil(GameApplication.Singleton.model.SingletonStaminaBarManager.PlayerStaminaBarTextFinish);
                            PlayerRecoverStamina = false;
                            consoleLog = null; //se è null non parte una doppia scrittura a riga 784
                        }
                    }
                    else if (PlayerRest)
                    {
                        float starterStamina = unit.stamina;
                        float staminaRecoverAmount = (unit.dexterity + unit.inteligence) * 1.5f;
                        StartCoroutine(UIRecoverStaminaBar(unit, staminaRecoverAmount));
                        unit = RecoverUnitStamina(ref unit, staminaRecoverAmount);
                        UpdateActorStats(unit);

                        string staminaToString = Mathf.RoundToInt(unit.stamina - starterStamina).ToString();
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

                        if (enemyIsDeadInThisTurn)
                        {
                            continue;
                        }

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
                    if (!string.IsNullOrEmpty(consoleLog))
                    {
                        yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                    }
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
                        //ogni tanto mando il reminder della difesa
                        int number = Random.Range(0, 5);
                        if (number >= 4)
                        {
                            if (defenceLog != string.Empty && defenceLog != null)
                            {
                                consoleLog = defenceLog;
                                yield return dialogConsoleManager.TypeDialogTxt(consoleLog);
                            }
                        }
                    }
                    RefreshSkippedLog();
                }
            }
            finished = true;
        }

        bool CheckoutUnitStatus(Unit unit)
        {
            foreach (var value in unit.stats)
            {
                if (value.Value > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool PlayerChangeDefence = false;
        internal void OnPlayerChangeDefence(int defenceIndex)
        {
            Unit unit = allUnits[GetMainPlayerIndex()];
            StartCoroutine(ClosePlayerCommandPanel());

            int tmpStaminaCost = 0;
            switch (defenceIndex)
            {
                case 0:
                    tmpStaminaCost = Singleton.playerEquipment.equippedLightDefence.StaminaCost;
                    break;
                case 1:
                    tmpStaminaCost = Singleton.playerEquipment.equippedBalancedDefence.StaminaCost;
                    break;
                case 2:
                    tmpStaminaCost = Singleton.playerEquipment.equippedHeavyDefence.StaminaCost;
                    break;
            }


            if (unit.DefenceType == (TypeDatabase.DefenseType)defenceIndex)
            {
                StartCoroutine(PlayerHaveAlreadyThisDefence());
                return;
            }

            if (unit.stamina < tmpStaminaCost)
            {
                StartCoroutine(PlayerHaventEnoughStamina(unit.name, false));
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

            unit = PayStamina(ref unit, tmpStaminaCost);
            UpdateActorStats(unit);

            playerPlayed = true;
            PlayerChangeDefence = true;
        }

        string abilityLog;
        public void UseAbility()
        {
            Unit unit = allUnits[lastIndex];
            Unit target = BattleEnemyManager.ReturnClickedButton();

            ScriptableAbility ability = unit.ability[abilityIndex];
            if (ability.APcost > unit.abilityPoints)
            {
                StartCoroutine(LogAbilityAlarm(unit.name, LocalizationIDDatabase.ABILITY_COST_TOMUCH));
                abilityClicked = false;
                return;
            }
            if (!ability.CanUseThis(unit, target))
            {
                StartCoroutine(LogAbilityAlarm(null, LocalizationIDDatabase.ABILITY_USELESS));
                abilityClicked = false;
                return;
            }

            abilityLog = string.Format(Localization.Get(LocalizationIDDatabase.USE_BATTLE_LOG), unit.name, ability.abilityName);
            PlayerUsingAbility(ability);
        }

        IEnumerator LogAbilityAlarm(string unitName, string newstring)
        {
            StartCoroutine(ClosePlayerCommandPanel());
            if (string.IsNullOrEmpty(unitName))
            {
                consoleLog = string.Format(Localization.Get(newstring));
            }
            else
            {
                consoleLog = string.Format(Localization.Get(newstring), unitName);
            }
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

        internal bool isParalyzed;
        internal bool isConfused;
        internal bool isBurned;
        internal bool isPoisoned;
        internal bool isFreezed;
        internal bool isInvulnerable;
        internal bool isUnable;

        internal bool GetIsParalyzed { get => isParalyzed; }
        internal bool GetIsConfused { get => isConfused; }
        internal bool GetIsBurned { get => isBurned; }
        internal bool GetIsPoisoned { get => isPoisoned; }
        internal bool GetIsFreezed { get => isFreezed; }
        internal bool GetIsInvulnerable { get => isInvulnerable; }

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

            attacker = PayStamina(ref attacker, currentWeapon.staminaCost);
            UpdateActorStats(attacker);

            //animazione&suono
            BattleControllerHelper.Instance.SetVFXPositionsOnPlayerTurn(false);
            BattleControllerHelper.Instance.CallSlashAnimation(VFXID);

            if (failedToAttack) //se è schivato
            {
                BattleEnemyManager.EnemyAnimationController(target, null, false, false, true);
                SoundEffectManager.Singleton.PlayAudio(target.dodgeSound);
            }
            else if (zeroDamage) { } //Se non fai danni
            else
            {
                float shakeDuration = 0;
                SoundEffectManager.Singleton.PlayAudio(attacker.equippedWeapon[wpnIndex].weaponAttackSound);
                BattleEnemyManager.EnemyAnimationController(target, null, false, true, false, shakeDuration, damage);
            }

            //AggiornoUI
            UpdateTargetStatsAndUI(target);

            //Scrivo se l'attacco è efficace e attendo l'input

            if (effectiveLog != null)
            {
                consoleLog = effectiveLog;
            }
            else
            {
                consoleLog = string.Empty;
            }

            RefreshSkippedLog();
            //PreventNullReferenceFromLog();
            playerPlayed = true;
        }


        IEnumerator PlayerHaventEnoughStamina(string unitName, bool isWeapon = true)
        {
            string localizeText = isWeapon ? LocalizationIDDatabase.WEAPON_COST_TOOMUCH : LocalizationIDDatabase.DEFENCE_COST_TOOMUCH;
            consoleLog = string.Format(Localization.Get(localizeText), unitName);
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
                if (failedToAttack)
                {
                    effectiveLog = Localization.Get(LocalizationIDDatabase.MISS_LOG);
                }
                else
                {
                    if (zeroDamage) { effectiveLog = Localization.Get(LocalizationIDDatabase.NOT_EFFECTIVE_LOG); }
                    effectiveLog = Localization.Get(LocalizationIDDatabase.EMPTY_HIT_LOG);
                }
            }
        }

        void UpdateTargetStatsAndUI(Unit target)
        {
            UpdateActorStats(target);
            UpdateEnemyUILifeBar(target);
        }

        void RefreshSkippedLog()
        {
            StopCoroutine(textLog);
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

        bool VFXAbilityPositionIsPlayer = false;
        void PlayerUsingAbility(ScriptableAbility ability)
        {
            StartCoroutine(ClosePlayerCommandPanel());
            Unit unit = allUnits[lastIndex];
            Unit target = BattleEnemyManager.ReturnClickedButton();
            AlreadyPlayed(unit);

            VFXAbilityPositionIsPlayer = false;

            //Scrivo x usa y
            StartCoroutine(ClosePlayerCommandPanel());

            SoundEffectManager.Singleton.PlayAudio(ability.abilitySounds);

            if (ability.moveType.Contains(ScriptableAbility.TargetType.TargetEnemy))
            {
                System.Tuple<Unit, string> tuple = ability.TriggerAbility(unit, target, ability.HavePowerup, ability.UseWeapon, ability.NumberOfPowerup, wpnIndex);
                VFXAbilityPositionIsPlayer = false;
                target = tuple.Item1;
                //Scrivo cosa fa
                consoleLog = tuple.Item2;

                Debug.LogWarning("Attenzione se l'abilità fa danni al di la del default damage");
                if ((ability.dontGiveDamage) || (ability.defaultDamage == 0))
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

            BattleControllerHelper.Instance.SetVFXPositionsOnPlayerTurn(VFXAbilityPositionIsPlayer);

            int loop = ability.VFXloop;
            if (loop > 1)
            {
                StartCoroutine(CallMultipleVfx(ability.VFXID, ability.VFXloop));
            }
            else
            {
                BattleControllerHelper.Instance.CallAbilityAnimation(ability.VFXID);
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
                BattleControllerHelper.Instance.CallAbilityAnimation(VFXID);
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
                SoundEffectManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.restAudio);
                PlayerRest = true;
                playerPlayed = true;
                yield break;
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
            float maxStamina = Singleton.dexterity * 10;
            return mainPlayer.stamina < maxStamina;
        }

        void CleanWeaponAnimationOnClose()
        {
            GameApplication.Singleton.view.BattleView.lightUIAnim.SetActive(false);
            GameApplication.Singleton.view.BattleView.heavyUIAnim.SetActive(false);
            GameApplication.Singleton.view.BattleView.rangedUIAnim.SetActive(false);
            GameApplication.Singleton.view.BattleView.specialUIAnim.SetActive(false);
        }

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
                    string description = TextTraductionUtility.GetCorrectSexText(LocalizationIDDatabase.DEATH_BATTLE_LOG, Localization.Get(target.name));
                    deathLog = description;
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
                    EnemyDeathAnimationAndDestroy(target);
                    BattleEnemyManager.RunEnemyDeathAnimation(); //qua prende la target sprite quindi va automatico sullultimo colpitpo
                    enemyIsDeadInThisTurn = true;
                    //SoundEffectManager.Singleton.PlayAudioClip(target.battleScreamSound);
                }

                else
                {
                    //UpdateHealthbar(target);
                    allUnits[SearchIndexInAllUnits(target)] = target;
                    enemies[SearchIndexInEnemies(target)] = target;
                }
            }
        }

        void EnemyDeathAnimationAndDestroy(Unit target)
        {
            target.isDead = true;
            BattleEnemyManager.RemoveTargetFromDeathEnemy();
            BattleEnemyManager.EnemyAnimationController(target);

            if (allUnits.Count > 1) //previeni errore quando il nemico muore durante il suo turno
            {
                Unit unit = allUnits[SearchIndexInAllUnits(target)];
                allUnits.Remove(unit);
                enemies.Remove(unit);
            }

            string localizedName = Localization.Get(target.name);
            consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.DEATH_BATTLE_LOG), localizedName);
        }

        public void UpdateHealthbar(Unit target, float newLifeAmount)
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

        Unit PayStamina(ref Unit attacker, float staminaCost)
        {
            if (!attacker.mainCharacter) { return attacker; }

            float currentStamina = attacker.stamina - staminaCost;
            if (currentStamina < 0) currentStamina = 0;
            GameApplication.Singleton.model.SingletonStaminaBarManager.UpdateCurrentStaminaBar(currentStamina);

            attacker.SetRemainingStamina(staminaCost);

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
            bool haveAllies = PlayerManager.Singleton.currentPage.haveAlly ? true : false;
            if (haveAllies)
            {
                int random = Random.Range(0, allies.Count);
                return allies[random];
            }
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
                damage = BattleControllerHelper.Instance.GetEnemyAttackDamageAmount(target, attacker);

                if (!enemyFailedToAttack)
                {
                    target.TakeDamage(damage, target);
                    SoundEffectManager.Singleton.PlayAudioClip(attacker.baseAttackSound);

                    if (!enemyZeroDamage)
                    {
                        HitEffect.Instance.Hit();
                        ShakeBattleground.Instance.ShakeObject(damage);
                    }
                }
                else
                {
                    View.GetComponent<Animator>().Play("DodgePlayerAnimation", 0, 0);
                    consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.DODGE_LOG));
                    SoundEffectManager.Singleton.PlayAudio(target.dodgeSound);
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
                    VFXAbilityPositionIsPlayer = true;
                    BattleControllerHelper.Instance.SetVFXPositionsOnPlayerTurn(VFXAbilityPositionIsPlayer);
                    if (ability.VFXID != null)
                    {
                        BattleControllerHelper.Instance.CallAbilityAnimation(ability.VFXID);
                    }
                    if (ability.abilitySounds != null)
                    {
                        SoundEffectManager.Singleton.PlayAudioClip(ability.abilitySounds);
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
                enemyZeroDamage = true;
                BattleControllerHelper.Instance.SetVFXPositionsOnPlayerTurn(false);
                BattleControllerHelper.Instance.CallAbilityAnimation(ability.VFXID);
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
        /// 
        Coroutine textLog;
        public void WriteLogByButton(string txtLog)
        {
            btnSkipText.gameObject.SetActive(true);
            textLog = StartCoroutine(dialogConsoleManager.TypeDialogTxt(txtLog));
        }

        void ShowFirstTimeTutorial()
        {
            BattleView BattleView = GameApplication.Singleton.view.BattleView;
            BattleView.FirstPanelFightTutorial.SetActive(true);
        }
        public void CleanConsoleText()
        {
            dialogConsoleManager.CleanText();
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

        void UploadPlayerStats()
        {
            int index = allUnits.FindIndex(u => u.mainCharacter);
            Singleton.lifePoints = allUnits[index].hp; //ho aggiunto questa riga per aggiornare gli hp del giocatore a fine battaglia
            Singleton.currentAP = allUnits[index].abilityPoints; //ho aggiunto questa riga per aggiornare gli hp del giocatore a fine battaglia
            //Singleton.lifePoints = allUnits[index].hp;
        }

        void SortByMainCharacter()
        {
            allUnits = allUnits.OrderBy(u => u.mainCharacter).ToList();
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
                    Unit unit = new Unit(Localization.Get(u.enemyName), u.constitution, u.dexterity, u.strength, u.inteligence, u.ability, u.canAttack, u.hp, u.isDead, false, 0, u.enemyImage,
                        u.normalMap, false, 0, placeID[i], u.equippedWeapon, u.itemInventory, u.myItems, u.healedUnit, u.elementalLevel,
                        u.animController, u.enemyPSB, u.scale, u.localPosition, u.defence, u.baseAttackType, u.defenseType, false, false,
                        u.finalAbility, u.baseAttack, u.level, u.noStatus, u.baseAttackName, 0, 0, u.baseAttackSound, u.dodgeSound, u.battleScreamSound, u.hitSound, u.enemyUISprite, u.indestructible,
                        u.enemyShadePrefab, u.enemyName);

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

        List<ScriptableAbility> unitAbilities;
        ScriptableAbility finalAbility;

        void PrepareRightAbilities()
        {
            unitAbilities = Singleton.playerAbility;
            finalAbility = Singleton.finalAbility;
        }

        //Ci sarà da passare il tipo di difesa dell'armatura
        public void CreatePlayerUnit()
        {
            PrepareRightAbilities();

            Unit classPlayer = new Unit(Singleton.playerName, Singleton.constitution, Singleton.dexterity,
                                       Singleton.strength, Singleton.inteligence, unitAbilities,
                                       false, Singleton.lifePoints, Singleton.PlayerIsDead(), true, 0, Singleton.classImage,
                                       null, true, 0, "Player", Singleton.EquippedWeaponToList(), Singleton.inventory,
                                       null, Singleton.healedUnit, Singleton.elementalLevel, null, null, null, null,
                                       Singleton.GetDefenceArmor(), TypeDatabase.AttackType.Heavy, Singleton.GetDefenceType(), false, false, finalAbility,
                                       0, Singleton.GetPlayerLevel, Singleton.noStatus, Singleton.baseAttackName, Singleton.GetPlayerAbilityPoints, Singleton.GetPlayerStamina,
                                       null, Singleton.dodgeSound, Singleton.classScreamSound,
                                       Singleton.playerEquipment.equippedBalancedDefence.hitSound);


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

        void OpenGameAt(params object[] data)
        {
            string pageID = (string)data[0];

            Singleton.pagesRead.Add(pageID);

            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.BookView;
            app.model.currentView.gameObject.SetActive(true);
        }
    }
}
