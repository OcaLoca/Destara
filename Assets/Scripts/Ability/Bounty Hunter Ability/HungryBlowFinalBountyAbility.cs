using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità_Finale:Colpo_Famelico ", menuName = "ScriptableAbility/Cacciatore/ColpoFamelicoMossaFinale", order = 7)]

    public class HungryBlowFinalBountyAbility : ScriptableAbility
    {
        public override Tuple<Unit,string> TriggerFinalAbility(Unit attacker, Unit unitTarget, int weaponIndex, float powerUp)
        {
            if (unitTarget.constitution * 30 <= unitTarget.hp)
            {
                unitTarget.TakeDamage((attacker.equippedWeapon[weaponIndex].damage / 100) * 300, unitTarget);
            }
            else
            {
                unitTarget.TakeDamage(attacker.equippedWeapon[weaponIndex].damage, unitTarget);
            }
            return Tuple.Create(unitTarget,consoleLog);
        }
    }
}
