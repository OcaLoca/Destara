/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità:Maledizione ", menuName = "ScriptableAbility/Megera/Maledizione", order = 3)]

    public class CurseAbility : ScriptableAbility
    {
        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            int randomNumber = UnityEngine.Random.Range(1, 3);

            float finalDamage = (defaultDamage * randomNumber);
            float realDamage = CalculateAbilityDamage(unitTarget, finalDamage);
            unitTarget.TakeDamage(realDamage, unitTarget);
            unitTarget.TakeElementalStats(PlayerManager.Stats.poisoned, 2, unitTarget, 50f);

            //if(realDamage == 0) { return null; }

            SlowMotionEffect.Instance.EnableSlowMotion(slowMotionTimeScale, slowMotionDuration);
            ShakeBattleground.Instance.ShakeObject(finalDamage);

            string targetName = Localization.Get(unitTarget.name);
            string abilityGiveDamageTo = string.Format(Localization.Get(LocalizationIDDatabase.ABILITY_GIVE_DAMAGE), abilityName, finalDamage);
            string targetAffectedByStats = string.Format(Localization.Get(LocalizationIDDatabase.TARGET_AFFECTED_BY_STATUS), targetName, BattleControllerHelper.GetLocalizationStatusEffect(PlayerManager.Stats.poisoned));

            abilityConsoleLog = abilityGiveDamageTo + "\n" + targetAffectedByStats;
            return Tuple.Create(unitTarget, abilityConsoleLog);
        }
    }

}
