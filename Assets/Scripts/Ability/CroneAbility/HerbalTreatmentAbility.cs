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
    [CreateAssetMenu(fileName = "Abilità:Cura_Erboritica ", menuName = "ScriptableAbility/Megera/CuraErboristica", order = 1)]

    public class HerbalTreatmentAbility : ScriptableAbility
    {
        public override System.Tuple<Unit, string, bool> BuffActor(Unit buffedUnit, bool scaleWithPowerUp, int powerUp, bool recoveryLife)
        {
            float buffAmount = 10;
            int random = Random.Range(0, 3);
            string buffLocalized = string.Empty;

            switch (random)
            {
                case 0:
                    buffedUnit.BuffDebuffStats(buffAmount, Buff.strenght);
                    buffLocalized = LocalizationDatabase.STRENGHT;
                    break;
                case 1:
                    buffedUnit.BuffDebuffStats(buffAmount, Buff.dexterity);
                    buffLocalized = LocalizationDatabase.DEXTERITY;
                    break;
                case 3:
                    buffedUnit.BuffDebuffStats(buffAmount, Buff.inteligence);
                    buffLocalized = LocalizationDatabase.INTELLIGENCE;
                    break;
                default:
                    buffedUnit.BuffDebuffStats(buffAmount, Buff.costitution);
                    buffLocalized = LocalizationDatabase.COSTITUTION;
                    break;
            }

            consoleLog = LocalizeText(buffLocalized, buffedUnit.name);
            return System.Tuple.Create(buffedUnit, consoleLog, recoveryLife);
        }

        string LocalizeText(string buff, string name)
        {
            return string.Format(Localization.Get(LocalizationIDDatabase.BUFF_BATTLE_LOG), Localization.Get(buff), name);
        }
    }

}
