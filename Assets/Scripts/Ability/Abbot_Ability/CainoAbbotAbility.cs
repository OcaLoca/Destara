/* ----------------------------------------------
 * 
 * 				#PROJECTNAME#
 * 
 * Creation Date: #CREATIONDATE#
 * 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Caino ", menuName = "ScriptableAbility/Abate/Caino", order = 3)]
    public class CainoAbbotAbility : ScriptableAbility
    {
        private void OnEnable()
        {
            defaultCost = 2;
        }

        public override System.Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            switch (defaultCost)
            {
                case 2:
                    if (Random.Range(0, 101) < elementalHitChance[0])
                    {
                        unitTarget.TakeElementalStats(PlayerManager.Stats.confused, 1, unitTarget, elementalLevel[0]);
                    }
                    defaultCost = 1;
                    break;
                case 3:
                    if (Random.Range(0, 101) < elementalHitChance[1])
                    {
                        unitTarget.TakeElementalStats(PlayerManager.Stats.confused, 2, unitTarget, elementalLevel[1]);
                    }
                    defaultCost = 2;
                    break;
                case 4:
                    if (Random.Range(0, 101) < elementalHitChance[2])
                    {
                        unitTarget.TakeElementalStats(PlayerManager.Stats.confused, 3, unitTarget, elementalLevel[2]);
                    }
                    defaultCost = 3;
                    break;
                default:
                    if (Random.Range(0, 101) < elementalHitChance[2])
                    {
                        unitTarget.TakeElementalStats(PlayerManager.Stats.confused, 3, unitTarget, elementalLevel[2]);
                    }
                    defaultCost = 3;
                    break;
            }

            return System.Tuple.Create(unitTarget, abilityConsoleLog);
        }

        public override int ReturnAbilityCost()
        {
            return defaultCost;
        }

    }

}
