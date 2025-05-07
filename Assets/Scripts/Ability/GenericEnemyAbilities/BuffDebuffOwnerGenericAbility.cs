/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;
using static Game.ScriptableAbility;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Buff/Debuff proprietario", menuName = "ScriptableAbility/Generiche/BuffDebuffProprietario", order = 3)]
    public class BuffDebuffOwnerGenericAbility : ScriptableAbility
    {
        public override System.Tuple<Unit, string, bool> BuffActor(Unit buffedUnit, bool scaleWithPowerUp, int powerUp, bool recoveryLife = false)
        {
            string buffAmountImpact = string.Empty;
            if (scaleWithPowerUp)
            {
                buffedUnit.BuffDebuffStats(buffPercentage[powerUp], abilityBuff);
                buffAmountImpact = LocalizeAmountOfBuffAmount(defaultBuffPercentage);
            }
            else
            {
                buffedUnit.BuffDebuffStats(defaultBuffPercentage, abilityBuff);
                buffAmountImpact = LocalizeAmountOfBuffAmount(defaultBuffPercentage);
            }

            if (abilityShakeBoard)
            {
                ShakeBattleground.Instance.ShakeObject(abilityShakeBoardAmount);
            }

            string abilityName = GetAbilityLocalizedName();
            string buffName = LocalizeBuffName();
            string increaseLog = LocalizeIncrease();

            abilityConsoleLog = string.Format(increaseLog, abilityName, buffName, buffAmountImpact);
            return System.Tuple.Create(buffedUnit, abilityConsoleLog, recoveryLife);
        }
    }
}
