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
    [CreateAssetMenu(fileName = "Abilità_Finale:Pioggia_Divina ", menuName = "ScriptableAbility/Abate/PioggiaDivinaMossaFinale", order = 7)]
    public class DivineRainFinalAbbotAbility : ScriptableAbility
    {
        //float totalDamage;

        //public override Tuple<List<Unit>, List<string>> TriggerAreaFinalAbility(Unit attacker, List<Unit> multipleTargets, int weaponIndex, float powerUp)
        //{
        //    List<Unit> updatedTargets = new List<Unit>();
        //    List<string> consoleLog = new List<string>();

        //    foreach (Unit enemy in multipleTargets)
        //    {
        //        enemy.TakeDamage(CalculateAbilityDamage(enemy, powerUpDamage[0]), enemy);
        //        totalDamage += CalculateAbilityDamage(enemy, powerUpDamage[0]);

        //        if (UnityEngine.Random.Range(0, 101) <= elementalHitChance[0])
        //        {
        //            consoleLog.Add(string.Format("{0} è stordito.", enemy.name));
        //            enemy.TakeElementalStats(applyStateCondition, 1, enemy, elementalLevel[0]);
        //        }
        //        updatedTargets.Add(enemy);
        //    }
        //    return Tuple.Create(updatedTargets, consoleLog);
        //}


        //ricordare di fare i buff nella final
        //public override Tuple<Unit, string, bool> BuffActor(Unit buffedUnit, int powerUp, bool recoveryLife = true)
        //{
        //    buffedUnit.BuffDebuffStats(totalDamage / 2, Buff.hp);
        //    return Tuple.Create(buffedUnit, consoleLog, recoveryLife); ;
        //}
    }

}
