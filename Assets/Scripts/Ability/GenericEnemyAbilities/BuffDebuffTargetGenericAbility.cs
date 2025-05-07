/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Buff/Debuff nemico", menuName = "ScriptableAbility/Generiche/BuffDebuffNemico", order = 2)]

    public class BuffDebuffTargetGenericAbility : ScriptableAbility
    {
        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            string buffAmountImpact = string.Empty;

            if (havePowerup)
            {
                unitTarget.BuffDebuffStats(buffPercentage[numberOfPowerup], buff);
                buffAmountImpact = LocalizeAmountOfBuffAmount(defaultBuffPercentage);
            }
            else
            {
                unitTarget.BuffDebuffStats(defaultBuffPercentage, buff);
                buffAmountImpact = LocalizeAmountOfBuffAmount(defaultBuffPercentage);
            }
            if (abilityShakeBoard)
            {
                ShakeBattleground.Instance.ShakeObject(abilityShakeBoardAmount);
            }

            string buffName = LocalizeBuffName();
            abilityConsoleLog = string.Format("{0} ha abbassato {1} {2}", abilityName, buffName, buffAmountImpact);
            return System.Tuple.Create(unitTarget, abilityConsoleLog);
        }
    }

}
