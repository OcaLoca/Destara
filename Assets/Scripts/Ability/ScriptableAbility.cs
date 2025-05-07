using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    public class ScriptableAbility : ScriptableObject
    {
        [Header("ID,Nome e Descrizione")]
        [SerializeField, Tooltip("L'ID che localizza il nome dell'abilità traducendolo nella lingua corretta")] public string localizedID;
        [Tooltip("Una voce per la descrizione dell'abilità")]
        public string abilityDescription;
        [Tooltip("Nome abilità prima della localizzazione")]
        public string abilityName;
        [SerializeField, Tooltip("ID che prende la descrizione per spammarla nella console dello scontro")]
        protected string abilityConsoleLog;
        /// <summary>
        /// Ability point richiesti 
        /// </summary>
        [Tooltip("Ability point richiesti")]
        public int APcost;
        [Header("Target dell'abilità, livello e rarità")]
        [Tooltip("Indica il target dell'abilità")]
        public TargetType[] moveType;
        [Tooltip("Livello dell'abilità, cioè da che livello viene appresa.")]
        public int abilityLevel;
        [Tooltip("Rarità dell'abilità, cioè quanta fortuna hai avuto nell'averla appresa ")]
        public ScriptableItem.Rarity rarity;

        [Header("Condizioni di stato, potenza delle condizioni")]
        [Tooltip("L'abilità applica condizioni di stato com Scottato, Congelato? Lasciare così se non applica nulla")]
        public PlayerManager.Stats applyStateCondition;
        [Tooltip("Corrisponde alla potenza del danno causato dall'elemento(veleno,bruciato). In caso dello stato confuso la potenza corrisponde alla possibilità di colpirsi da solo.")]
        [SerializeField] protected float[] elementalLevel;
        protected BattleController BattleController;
        [SerializeField, Tooltip("Se true l'abilità è usata solo dai nemici")] bool onlyEnemyAbility;
        /// <summary>
        /// Dato che abbiamo rimosso i buff cost non serve ma basta DefaultCost
        /// </summary>
        //[SerializeField,Tooltip("Costo attuale della mossa in base ai buff")] protected int cost;
        [Header("Costo, Tipo e Quantità di danno dell'abilità")]
        [SerializeField, Tooltip("E' il costo fisso di un'abilità che non ha PowerUp.")]
        protected int defaultCost;
        [SerializeField, Tooltip("Sono i danni di default di un abilità che non ha PowerUp")]
        public float defaultDamage;
        [SerializeField, Tooltip("Abilità influenzata dai PowerUp che farà i danni in base alla powerUpDamage")]
        protected bool havePowerUp;
        [SerializeField, Tooltip("L'abilità che scala sui PowerUp fanno danno in base al numero usato. " +
            "Esempio [1 = 20 dmg] [2 = 50 dmg]")]
        protected float[] powerUpDamage;
        [SerializeField, Tooltip("Abilità fa i danni in base arma, cioè il danno verrà scalatpo su una delle armi equipaggiate dal giocatore.")]
        protected bool useWeapon;
        [Tooltip("E' il tipo di danno che fa l'abilità.")]
        public TypeDatabase.AttackType attackType;

        public int NumberOfPowerup { get => powerUpDamage.Length; }
        public bool HavePowerup { get => havePowerUp; }
        public bool UseWeapon { get => useWeapon; }

        [SerializeField, Tooltip("Possibilità che l'abilità vada assegno")]
        int hitChance;
        [Tooltip("Indica se l'abilità provoca danni")]
        public bool dontGiveDamage = false;
        [SerializeField, Tooltip("Se l'abilità oltre ai suoi danni aggiunge i danni scalati. Questa è la % sul quale deve scalare sulle stats dell'unità")]
        protected int scaledAmount;
        [SerializeField] int criticalChance;


        [SerializeField, Tooltip("E' la % che l'abilità affligga uno status al nemico per ogni buff usato per la mossa.")]
        protected int[] elementalHitChance;
        [SerializeField, Tooltip("E' la % del buff che aumenta con il numero di pallini")]
        protected int[] buffPercentage;
        [SerializeField, Tooltip("E' la % del buff di default")]
        protected int defaultBuffPercentage;
        [SerializeField] bool upgradeWithPlayerLevel;
        public int GetAbilityCost { get => defaultCost; }
        public bool EnemyAbility { get => onlyEnemyAbility; }
        public bool CanBeBuffedWithNoLimit { get => havePowerUp; }
        [Tooltip("Se ha target multipli la mossa del nemico specificare se 2/3")]
        public int targetCount;
        [Tooltip("E' l'ID che serve per settare l'animazione dell'abilità con i tasti, ATTENZIONE sono collegate al nemico")]
        public AnimationTriggerID animationTriggerID;

        [Tooltip("E' l'ID che serve per richiamare l'animazione dell'abilità")]
        public string VFXID;
        [Tooltip("Durata dello slowMotion se lo ha")]
        public float slowMotionDuration;
        [Tooltip("Corrisponde alla decellerazione del tempo durante lo SlowMotion")]
        public float slowMotionTimeScale;
        [Tooltip("E' il numero di volte che richiama l'animazione")]
        public byte VFXloop = 0;
        [SerializeField, Tooltip("Se è attivo l'abilità fa lo shake.")]
        protected bool abilityShakeBoard;
        [SerializeField, Tooltip("E'l'intensità dello shake nelle abilità che non fanno danni.")]
        protected float abilityShakeBoardAmount;

        [Header("Sounds")]
        [SerializeField] public AudioClip abilitySounds;

        public enum TargetType
        {
            TargetEnemy,
            TargetMoreEnemies,
            TargetActor,
            TargetMoreActor
        }

        const string AP = "AP";
        public string GetAbilityCostToString()
        {
            string abilityCostToString = APcost < 10 ? AP + " 0" + APcost.ToString() : AP + " " + APcost.ToString();
            return abilityCostToString;
        }
        public enum AnimationTriggerID
        {
            FirstAbility,
            SecondAbility,
            ThirdAbility,
            FourthAbility
        }

        public enum Buff
        {
            strenght,
            dexterity,
            hp,
            defence,
            inteligence,
            costitution,
            baseAttack
        }
        public Buff abilityBuff;
        internal string txtBuffLocalized;

        public virtual string GetAbilityLocalizedName()
        {
            return Localization.Get(localizedID);
        }
        public virtual void LocalizeAbilityName()
        {
            abilityName = Localization.Get(localizedID);
        }
        public virtual string LocalizeBuffName()
        {
            return Localization.Get(abilityBuff.ToString());
        }
        public virtual string LocalizeIncrease()
        {
            return Localization.Get("increase");
        }
        public virtual string LocalizeApplied()
        {
            return Localization.Get("applied");
        }
        public virtual string LocalizeUnitName(string name)
        {
            return Localization.Get(name);
        }

        public virtual string LocalizeAmountOfBuffAmount(float value)
        {
            string debuffValue = string.Empty;
            switch (value)
            {
                case > 20:
                    return debuffValue = Localization.Get("drastically");
                case > 10:
                    return debuffValue = Localization.Get("lightly");
                default:
                    return debuffValue = Localization.Get("byALot"); 
            }
        }

        public virtual bool CanUseThis(Unit attacker, Unit unitTarget) { return true; }
        public virtual bool CanUseThisMultiple(Unit attacker, List<Unit> unitTargets) => true;

        public virtual Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            return Tuple.Create(unitTarget, abilityConsoleLog);
        }

        public virtual Tuple<Unit, string> TriggerFinalAbility(Unit attacker, Unit unitTarget, int weaponIndex, float powerUp)
        {
            return Tuple.Create(unitTarget, abilityConsoleLog);
        }

        public virtual Tuple<List<Unit>, List<string>> TriggerAreaFinalAbility(Unit attacker, List<Unit> multipleTargets, int weaponIndex, float powerUp)
        {
            List<string> consoleLog = new List<string>();
            return Tuple.Create(multipleTargets, consoleLog);
        }

        public virtual Tuple<List<Unit>, List<string>> TriggerAreaAbilityWithTuple(Unit attacker, List<Unit> multipleTargets, int weaponIndex, float powerUp)
        {
            List<string> consoleLog = new List<string>();
            return Tuple.Create(multipleTargets, consoleLog);
        }

        /// <summary>
        /// Returna il costo di default dell'abilità
        /// </summary>
        /// <returns></returns
        public virtual int ReturnAbilityCost()
        {
            return defaultCost;
        }
        public virtual Tuple<Unit, string, bool> BuffActor(Unit buffedUnit, bool scaleWithPowerUp = false, int powerUp = 0, bool recoverLife = false)
        {
            return Tuple.Create(buffedUnit, consoleLog, recoverLife);
        }

        public string ReturnAbilityNameLocalized()
        {
            return localizedID;
        }

        public virtual string GetLocalizedStatsName()
        {
            return Localization.Get(applyStateCondition.ToString());
        }

        public virtual void ActiveSlowMotion(float slowMotionTimeSìcale, float slowMotionDuration)
        {
            SlowMotionEffect.Instance.EnableSlowMotion(slowMotionTimeScale, slowMotionDuration);
        }

        public virtual void ActiveShakeBackground(float shakeAmount)
        {
            ShakeBattleground.Instance.ShakeObject(slowMotionTimeScale);
        }


        public virtual string ReturnDamageApplied(Unit unit)
        {
            string tmpConsole = string.Format(powerUpDamage.ToString());
            return tmpConsole;
        }

        protected float CalculateAbilityDamage(Unit target, float damage)
        {
            damage = TypeDatabase.IsEffectiveOrNot(attackType, target.DefenceType, damage, ref effectiveLog);
            damage = damage - target.defence;

            return damage;
        }


        public float ScaleEnemyAbilityDamage(float scaledAmount, Unit unit)
        {
            float damage = 0;

            switch (attackType)
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
    }
}

