/* ----------------------------------------------
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Fiuto_Da_Caccia", menuName = "ScriptableAbility/Cacciatore/FiutoDaCaccia", order = 3)]

    public class HuntingFlairBountyAbility : ScriptableAbility
    {
        public override int ReturnAbilityCost()
        {
            return 1;
        }
        public override Tuple<Unit, string, bool> BuffActor(Unit buffedUnit,bool scaleWithPowerUp, int powerUp, bool recoveryLife = false)
        {
            buffedUnit.BuffDebuffStats(15, Buff.strenght);
            buffedUnit.BuffDebuffStats(15, Buff.dexterity);
            string strenght = Localization.Get(LocalizationDatabase.STRENGHT);
            string dexterity = Localization.Get(LocalizationDatabase.DEXTERITY);
            abilityConsoleLog = LocalizeText(strenght, dexterity, buffedUnit.name);
            return Tuple.Create(buffedUnit, abilityConsoleLog, recoveryLife);
        }

        string LocalizeText(string buff, string secondBuff, string name)
        {
            return string.Format(Localization.Get(LocalizationIDDatabase.BUFF_TWO_BATTLE_LOG), Localization.Get(buff), Localization.Get(secondBuff), name);
        }
    }
}
