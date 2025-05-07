/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using StarworkGC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;
using static Game.ConsumableItem;
using static Game.PlayerManager;
using static UnityEngine.UI.CanvasScaler;
using Unit = Game.BattleController.Unit;

namespace Game
{
    public class BattleControllerHelper : MonoBehaviour
    {
        public static BattleControllerHelper Instance { get; private set; }
        [SerializeField] BattleController bC;
        [SerializeField] BattlePlayerManager BattlePlayerManager;

        private void Awake()
        {
            Instance = this;
        }

        #region UIBattleHelpController

        internal void SetVFXPositionsOnPlayerTurn(bool targetActor, string target = "CentralEnemy")
        {
            if (!targetActor)
            {
                if(string.IsNullOrEmpty(targetID))
                {
                    targetID = target;
                }
                switch (targetID)
                {
                    case "CentralEnemy":
                        bC.vfxSlashContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        bC.vfxAbilityContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        break;
                    case "LeftEnemy":
                        bC.vfxSlashContainer.transform.localPosition = BattleEnemyManager.leftEnemyTransform;
                        bC.vfxAbilityContainer.transform.localPosition = BattleEnemyManager.leftEnemyTransform;
                        break;
                    case "RightEnemy":
                        bC.vfxSlashContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        bC.vfxAbilityContainer.transform.localPosition = BattleEnemyManager.centralEnemyTransform;
                        break;
                    default:
                        break;
                }
                return;
            }

            bC.vfxSlashContainer.transform.localPosition = BattlePlayerManager.PlayerTransformPosition;
            bC.vfxAbilityContainer.transform.localPosition = BattlePlayerManager.PlayerTransformPosition;
        }

        internal void CallSlashAnimation(string slashID)
        {
            foreach (Transform child in bC.vfxSlashContainer.transform)
            {
                if (child.name == slashID)
                {
                    child.GetComponent<ParticleSystem>().Play();
                    return;
                }
            }
        }

        internal void CallAbilityAnimation(string abilityAnimationID)
        {
            if (abilityAnimationID == string.Empty) { return; }

            foreach (Transform child in bC.vfxAbilityContainer.transform)
            {
                if (child.name == abilityAnimationID)
                {
                    child.GetComponentInChildren<ParticleSystem>().Play();
                    //if (child.Find("CornerEffect"))
                    //{
                    //    GameObject obj = child.Find("CornerEffect").gameObject;
                    //    StartCoroutine(ActiveCornerEffect(obj));
                    //}
                    return;
                }
            }
        }
        internal IEnumerator ActiveCornerEffect(GameObject obj)
        {
            obj.SetActive(true);
            //SlowMotionEffect.Instance.EnableSlowMotion(0.5f, 1);
            yield return CoroutinesHelper.PointSevenSeconds;
            obj.SetActive(false);
        }

        #endregion


        internal void CheckElementalStats(Unit unit)
        {
            ResetStats();

            if (unit.stats[Stats.burned] >= 1)
            {
                consoleLog = string.Format("{0} è scottato", unit.name);
                unit.stats[Stats.burned] = unit.stats[Stats.burned] - 1;
                bC.isBurned = true;
            }
            if (unit.stats[Stats.poisoned] >= 1)
            {
                consoleLog = string.Format("{0} è avvelenato", unit.name);
                unit.stats[Stats.poisoned] = unit.stats[Stats.poisoned] - 1;
                bC.isPoisoned = true;
            }
            if (unit.stats[Stats.paralyzed] >= 1)
            {
                Debug.LogFormat("Lo stato paralizzato di {1}  prima del conteggio dura ancora {0} turni", unit.stats[Stats.paralyzed], unit.placeID);
                consoleLog = string.Format("{0} è paralizzato", unit.name);
                unit.stats[Stats.paralyzed] = unit.stats[Stats.paralyzed] - 1;
                bC.isParalyzed = true;
                Debug.LogFormat("Lo stato paralizzato di {1}  dopo il conteggio dura ancora {0} turni", unit.stats[Stats.paralyzed], unit.placeID);
            }
            if (unit.stats[Stats.confused] >= 1)
            {
                Debug.LogFormat("Lo stato stordito di {1}  prima del conteggio dura ancora {0} turni", unit.stats[Stats.confused], unit.placeID);
                consoleLog = string.Format("{0} è confuso", unit.name);
                unit.stats[Stats.confused] = unit.stats[Stats.confused] - 1;
                bC.isConfused = true;
                Debug.LogFormat("Lo stato stordito di {1}  dopo il conteggio dura ancora {0} turni", unit.stats[Stats.paralyzed], unit.placeID);
            }
            if (unit.stats[Stats.freezed] >= 1)
            {
                consoleLog = string.Format("{0} è congelato", unit.name);
                unit.stats[Stats.freezed] = unit.stats[Stats.freezed] - 1;
                bC.isFreezed = true;
            }
            if (unit.stats[Stats.invurneable] >= 1)
            {
                consoleLog = string.Format("{0} è invulnerabile", unit.name);
                unit.stats[Stats.invurneable] = unit.stats[Stats.invurneable] - 1;
                bC.isInvulnerable = true;
            }
            if (unit.stats[Stats.unable] >= 1)
            {
                consoleLog = string.Format("{0} è accecato", unit.name);
                unit.stats[Stats.unable] = unit.stats[Stats.unable] - 1;
                bC.isUnable = true;
            }
        }

        public static string GetLocalizationStatusEffect(Stats stats)
        {
            return Localization.Get(stats.ToString() + "Effect");
        }

        public static string GetLocalizationStatusPain(Stats stats)
        {
            return Localization.Get(stats.ToString() + "Pain");
        }
        void ResetStats()
        {
            bC.isParalyzed = false;
            bC.isConfused = false;
            bC.isBurned = false;
            bC.isPoisoned = false;
            bC.isFreezed = false;
            bC.isInvulnerable = false;
            bC.isUnable = false;
        }


        internal float GetEnemyAttackDamageAmount(Unit target, Unit attacker)
        {
            float damage = attacker.baseAttack;
            if (damage <= 0)
            {
                enemyZeroDamage = true;
               // effectiveLog = Localization.Get(LocalizationIDDatabase.ZERO_DAMAGE);
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
                effectiveLog = Localization.Get(LocalizationIDDatabase.DODGE_LOG);
                SoundEffectManager.Singleton.PlayAudio(target.dodgeSound);
                enemyFailedToAttack = true;
                return 0;
            }

            if (damage <= 0)
            {
                enemyZeroDamage = true;
               // effectiveLog = Localization.Get(LocalizationIDDatabase.ZERO_DAMAGE);
                return 1;
            }
            else
            {
                enemyZeroDamage = false;
                enemyFailedToAttack = false;
            }

            damage = TypeDatabase.IsEffectiveOrNot(attacker.baseAttackType, target.DefenceType, damage, ref effectiveLog, attacker.isControllable);
            damage = damage - target.defence;
            defenceLog = GetDefenceResultBattleConsoleLog(damage, false);
            if (damage <= 0) damage = 1;
            return damage;
        }


        public bool BattleUselessItem(Unit unit, ConsumableItem item)
        {
            BuffType defaultBuff = item.defaultBuff;

            if (defaultBuff == BuffType.Health)
            {
                if (unit.hp >= unit.constitution * 10) return true;
            }
            else if (defaultBuff == BuffType.AP)
            {
               if (unit.abilityPoints >= Singleton.GetAbilityPointsLimit()) return true;
            }
            else if (defaultBuff == BuffType.Stamina)
            {
                if (unit.stamina >= Singleton.GetStaminaLimit()) return true;
            }
            return false;
        }

        public static string GetDefenceResultBattleConsoleLog(float damage, bool attackerIsPlayer = true)
        {
            switch (damage)
            {
                case <= 1:
                    if (attackerIsPlayer)
                    {
                        return Localization.Get(LocalizationIDDatabase.ENEMY_DEFENCE_STRONG);
                    }
                    return Localization.Get(LocalizationIDDatabase.PLAYER_DEFENCE_STRONG);
                case > 200:
                    if (attackerIsPlayer)
                    {
                        return string.Empty;
                    }
                    return Localization.Get(LocalizationIDDatabase.PLAYER_DEFENCE_WEAK);

                default: return string.Empty;
            }
        }
    }

}
