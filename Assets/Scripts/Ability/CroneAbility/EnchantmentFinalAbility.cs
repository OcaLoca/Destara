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
    [CreateAssetMenu(fileName = "Abilità:Incanto ", menuName = "ScriptableAbility/Megera/Incanto", order = 4)]

    public class EnchantmentFinalAbility : ScriptableAbility
    {
        public override Tuple<Unit, string> TriggerFinalAbility(Unit attacker, Unit unitTarget, int weaponIndex, float powerUp)
        {
            unitTarget.TakeElementalStats(PlayerManager.Stats.unable, 2, unitTarget, elementalLevel[0]);
            unitTarget.BecomeUnable(); //primo turno dato da questo
            return Tuple.Create(attacker, consoleLog);
        }

    }

}
