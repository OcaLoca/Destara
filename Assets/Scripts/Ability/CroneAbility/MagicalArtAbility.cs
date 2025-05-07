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
    [CreateAssetMenu(fileName = "Abilità:Arte_Magica ", menuName = "ScriptableAbility/Megera/ArteMagica", order = 2)]

    public class MagicalArtAbility : ScriptableAbility
    {
        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            unitTarget.TakeDamage(defaultDamage, unitTarget);
            unitTarget.TakeElementalStats(PlayerManager.Stats.paralyzed, 2, unitTarget,0); 
            SlowMotionEffect.Instance.EnableSlowMotion(slowMotionTimeScale, slowMotionDuration);
            ShakeBattleground.Instance.ShakeObject(defaultDamage);

            string targetName = Localization.Get(unitTarget.name);
            string abilityGiveDamageTo = string.Format(Localization.Get(LocalizationIDDatabase.ABILITY_GIVE_DAMAGE), abilityName, defaultDamage);
            string targetAffectedByStats = string.Format(Localization.Get(LocalizationIDDatabase.TARGET_AFFECTED_BY_STATUS), targetName, BattleControllerHelper.GetLocalizationStatusEffect(PlayerManager.Stats.paralyzed));

            abilityConsoleLog = abilityGiveDamageTo + "\n" + targetAffectedByStats;
            return Tuple.Create(unitTarget, abilityConsoleLog);
        }
    }

}
