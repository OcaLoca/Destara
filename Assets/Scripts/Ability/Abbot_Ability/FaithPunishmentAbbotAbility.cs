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
    [CreateAssetMenu(fileName = "Abilità: Castigo_Della_Fede", menuName = "ScriptableAbility/Abate/CastigoDellaFede", order = 3)]
    public class FaithPunishmentAbbotAbility : ScriptableAbility
    {
        private void OnEnable()
        {
            defaultCost = 1;
        }

        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            float debuffDefence = 0;
            float debuffDexterity = 0;
            switch (defaultCost)
            {
                case 1:
                    debuffDefence = ((unitTarget.defence / 100) * 5);
                    debuffDexterity = ((unitTarget.dexterity / 100) * 2.5f);
                    defaultCost = 1;
                    break;
                case 2:
                    debuffDefence = ((unitTarget.defence / 100) * 10);
                    debuffDexterity = ((unitTarget.dexterity / 100) * 5);
                    defaultCost = 2;
                    break;
                case 3:
                    debuffDefence = ((unitTarget.defence / 100) * 15);
                    debuffDexterity = ((unitTarget.dexterity / 100) * 7.5f);
                    defaultCost = 3;
                    break;
                default:
                    debuffDefence = ((unitTarget.defence / 100) * 15);
                    debuffDexterity = ((unitTarget.dexterity / 100) * 7.5f);
                    defaultCost = 3;
                    break;
            }

            unitTarget.BuffDebuffStats(-debuffDefence, Buff.defence);
            unitTarget.BuffDebuffStats(-debuffDexterity, Buff.dexterity);

            return Tuple.Create(unitTarget, abilityConsoleLog);
        }

    }

}
