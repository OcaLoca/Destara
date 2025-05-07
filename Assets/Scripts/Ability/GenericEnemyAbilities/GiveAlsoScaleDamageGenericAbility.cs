/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Danno diretto target singolo e scalato ", menuName = "ScriptableAbility/Generiche/DannoDirettoEScalatoSingoloTarget", order = 1)]

    public class GiveAlsoScaleDamageGenericAbility : ScriptableAbility
    {
        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            float damage;
            
            if(havePowerup)
            {
                damage = CalculateAbilityDamage(unitTarget, powerUpDamage[numberOfPowerup]) + ScaleEnemyAbilityDamage(scaledAmount, attacker);
            }
            else
            {
                damage = CalculateAbilityDamage(unitTarget, defaultDamage) + ScaleEnemyAbilityDamage(scaledAmount, attacker);
            }

            SlowMotionEffect.Instance.EnableSlowMotion(slowMotionTimeScale, slowMotionDuration);
            ShakeBattleground.Instance.ShakeObject(damage);
            unitTarget.TakeDamage(damage, unitTarget);
            return Tuple.Create(unitTarget, abilityConsoleLog);
        }
    }

}
