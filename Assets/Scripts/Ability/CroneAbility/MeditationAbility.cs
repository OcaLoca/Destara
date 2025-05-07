/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità:Meditazione ", menuName = "ScriptableAbility/Megera/Meditazione", order = 0)]
    public class MeditationAbility : ScriptableAbility
    {
        public override bool CanUseThis(Unit attacker, Unit unitTarget)
        {
            return attacker.hp < PlayerManager.Singleton.GetPlayerMaxHP();
        }


        public override System.Tuple<Unit, string, bool> BuffActor(Unit buffedUnit, bool scaleWithPoerUp = false, int powerUp = 0, bool recoveryLife = true)
        {
            float startHpAmount = buffedUnit.hp;
            float buffedHp;
            recoveryLife = true;

            int random = Random.Range(0, 3);

            switch (random)
            {
                case 0:
                case 1:
                    buffedHp = (buffedUnit.constitution * 100) /20;
                    buffedUnit.BuffDebuffStats(buffedHp, Buff.hp);
                    break;
                case 2:
                    buffedHp = (buffedUnit.constitution * 100) / 25;
                    buffedUnit.BuffDebuffStats(buffedHp, Buff.hp);
                    break;
                case 3:
                    buffedHp = (buffedUnit.constitution * 100) / 35;
                    buffedUnit.BuffDebuffStats(elementalLevel[1], Buff.hp);
                    break;
                default:
                    buffedHp = (buffedUnit.constitution * 100) / 20;
                    buffedUnit.BuffDebuffStats(buffedHp, Buff.hp);
                    break;
            }

            string targetName = Localization.Get(buffedUnit.name);
            float hpRecovered = buffedUnit.hp - startHpAmount;
            consoleLog = string.Format(Localization.Get(LocalizationIDDatabase.PLAYERRECOVERXHP), targetName, hpRecovered);

            return System.Tuple.Create(buffedUnit, consoleLog, recoveryLife);
        }
    }
}
