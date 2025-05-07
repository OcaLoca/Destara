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
    [CreateAssetMenu(fileName = "Abilità: Danno diretto target singolo ", menuName = "ScriptableAbility/Generiche/DannoDirettoSingoloTarget", order = 0)]
    public class GiveDamageGenericAbility : ScriptableAbility
    {
        [SerializeField] bool useBaseAttackDamageScale;
        [SerializeField] int baseAttackDamageMoltiplicator;
        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int NumberOfPowerUp = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            float realDamage;

            if (useBaseAttackDamageScale)
            {
                realDamage = CalculateAbilityDamage(unitTarget, attacker.baseAttack * baseAttackDamageMoltiplicator);
            }
            else
            {
                realDamage = CalculateAbilityDamage(unitTarget, defaultDamage);
            }
            unitTarget.TakeDamage(realDamage, unitTarget);

            
            ShakeBattleground.Instance.ShakeObject(realDamage);
            SlowMotionEffect.Instance.EnableSlowMotion(slowMotionTimeScale, slowMotionDuration);

            return Tuple.Create(unitTarget, effectiveLog);
        }
    }

}
