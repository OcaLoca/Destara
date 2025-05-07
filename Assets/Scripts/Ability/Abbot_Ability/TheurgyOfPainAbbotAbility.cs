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
    [CreateAssetMenu(fileName = "Abilità:Teurgia_Del_Dolore ", menuName = "ScriptableAbility/Abate/TeurgiaDelDolore", order = 6)]
    public class TheurgyOfPainAbbotAbility : ScriptableAbility
    {
        //public override Tuple<List<Unit>, List<string>> TriggerAreaAbilityWithTuple(Unit attacker, List<Unit> multipleTargets, int weaponIndex, float powerUp)
        //{
        //    List<Unit> updatedTargets = new List<Unit>();
        //    List<string> consoleLog = new List<string>();

        //    foreach (Unit enemy in multipleTargets)
        //    {
        //        enemy.TakeDamage(CalculateAbilityDamage(enemy,powerUpDamage[0]), enemy);

        //        if (UnityEngine.Random.Range(0, 101) <= elementalHitChance[0])
        //        {
        //            consoleLog.Add(string.Format("{0} è stordito.", enemy.name));
        //            enemy.TakeElementalStats(applyStateCondition, 1, enemy, elementalLevel[0]);
        //        }
        //        updatedTargets.Add(enemy);
        //    }

        //    return Tuple.Create(updatedTargets, consoleLog);
        //}
    }

}
