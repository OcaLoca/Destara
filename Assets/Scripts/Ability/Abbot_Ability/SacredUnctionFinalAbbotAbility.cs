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
    [CreateAssetMenu(fileName = "Abilità:Sacra_Unzione ", menuName = "ScriptableAbility/Abate/SacraUnzioneMossaFinale", order = 4)]
    public class SacredUnctionFinalAbbotAbility : ScriptableAbility
    {
        public override Tuple<Unit, string> TriggerFinalAbility(Unit attacker, Unit unitTarget, int weaponIndex, float powerUp)
        {
            attacker.TakeElementalStats(PlayerManager.Stats.invurneable, 2, attacker, elementalLevel[0]);
            attacker.BecomeInvurnerable(); //primo turno dato da questo
            return Tuple.Create(attacker, consoleLog);
        }
    }
}
