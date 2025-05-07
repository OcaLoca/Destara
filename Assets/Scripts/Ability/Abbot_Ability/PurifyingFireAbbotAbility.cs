using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Fuoco_Purificatore", menuName = "ScriptableAbility/Abate/FuocoPurificatore", order = 3)]

    public class PurifyingFireAbbotAbility : ScriptableAbility
    {
        //public override System.Tuple<List<Unit>, List<string>> TriggerAreaAbilityWithTuple(Unit attacker, List<Unit> multipleTargets, int weaponIndex, float powerUp)
        //{
        //    List<Unit> updatedTargets = new List<Unit>();
        //    List<string> consoleLog = new List<string>();

        //    foreach (Unit enemy in multipleTargets)
        //    {
        //        enemy.TakeDamage(powerUpDamage[0], enemy);

        //        if (Random.Range(0, 101) <= elementalHitChance[0])
        //        {
        //            consoleLog.Add(string.Format("{0} si è bruciato.", enemy.name));
        //            enemy.TakeElementalStats(applyStateCondition, 3, enemy, elementalLevel[0]);
        //        }
        //        updatedTargets.Add(enemy);
        //    }

        //    return System.Tuple.Create(updatedTargets, consoleLog);
        //}
    }
}
