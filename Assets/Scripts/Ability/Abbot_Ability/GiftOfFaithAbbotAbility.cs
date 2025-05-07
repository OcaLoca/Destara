/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità:Dono_Della_Fede ", menuName = "ScriptableAbility/Abate/DonoDellaFede", order = 5)]
    public class GiftOfFaithAbbotAbility : ScriptableAbility
    {
        public override System.Tuple<Unit, string, bool> BuffActor(Unit buffedUnit, bool scaleWithPowerUp, int powerUp, bool recoveryLife = true)
        {
            float buffedHp;

            switch (defaultCost)
            {
                case 2:
                    buffedHp = (buffedUnit.lastDamageTake / 100) * elementalLevel[0];
                    buffedUnit.BuffDebuffStats(buffedHp, Buff.hp);
                    defaultCost = 2;
                    break;
                case 3:
                    buffedHp = (buffedUnit.lastDamageTake / 100) * elementalLevel[1];
                    buffedUnit.BuffDebuffStats(buffedHp, Buff.hp);
                    defaultCost = 3;
                    break;
                case 4:
                    buffedHp = (buffedUnit.lastDamageTake / 100) * elementalLevel[2];
                    buffedUnit.BuffDebuffStats(buffedHp, Buff.hp);
                    defaultCost = 4;
                    break;

                default:
                    buffedHp = (buffedUnit.lastDamageTake / 100) * elementalLevel[2];
                    buffedUnit.BuffDebuffStats(buffedHp, Buff.hp);
                    defaultCost = 4;
                    break;
            }

            return System.Tuple.Create(buffedUnit, consoleLog, recoveryLife);
        }


    }

}
