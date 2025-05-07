/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: ", menuName = "ScriptableAbility/Generiche/ApplicaStatoAlNemico", order = 5)]
    public class ApplyStatsToEnemyAbility : ScriptableAbility
    {
        [SerializeField] int stateConditionTurnDuration;
        [SerializeField, Tooltip("Se lo stato fa danni mettere true")] bool stateAfflictDamage;
        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            float stateDamage = stateAfflictDamage ? elementalLevel[0] : 100;
            Debug.LogWarningFormat("STATE DAMAGE {0}", stateDamage.ToString());
            unitTarget.TakeElementalStats(applyStateCondition, stateConditionTurnDuration, unitTarget, stateDamage);

            string localizeTargetName = LocalizeUnitName(unitTarget.name);
            string targetAffectedByStats = string.Format(Localization.Get(LocalizationIDDatabase.TARGET_AFFECTED_BY_STATUS), localizeTargetName, BattleControllerHelper.GetLocalizationStatusEffect(PlayerManager.Stats.confused));

            abilityConsoleLog = targetAffectedByStats;
            return Tuple.Create(unitTarget, abilityConsoleLog);
        }

       
    }

}
