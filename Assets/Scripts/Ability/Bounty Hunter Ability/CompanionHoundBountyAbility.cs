using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Compagno_Segugio", menuName = "ScriptableAbility/Cacciatore/Compagno_Segugio", order = 1)]

    public class CompanionHoundBountyAbility : ScriptableAbility
    {
        [SerializeField] float damageWolf;
        public override int ReturnAbilityCost()
        {
            return base.ReturnAbilityCost();
        }

        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            float damage = ((defaultDamage + damageWolf) * 4);
            float realDamage = CalculateAbilityDamage(unitTarget, damage);

            unitTarget.TakeDamage(realDamage, unitTarget);
            string logXTimes = string.Format(Localization.Get(LocalizationIDDatabase.HIT_X_TIMES), 4);
            string abilityGiveDamageTo = string.Format(Localization.Get(LocalizationIDDatabase.ABILITY_GIVE_DAMAGE), abilityName, realDamage);

            SlowMotionEffect.Instance.EnableSlowMotion(slowMotionTimeScale, slowMotionDuration);
            ShakeBattleground.Instance.ShakeObject(damage);

            abilityConsoleLog = logXTimes + "\n" + abilityGiveDamageTo;
            return Tuple.Create(unitTarget, abilityConsoleLog);
        }

        public override bool CanUseThis(Unit attacker, Unit unitTarget)
        {
            return base.CanUseThis(attacker, unitTarget);
        }


    }
}

