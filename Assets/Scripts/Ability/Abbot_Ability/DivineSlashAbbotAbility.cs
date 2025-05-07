using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Fendente_Divino", menuName = "ScriptableAbility/Abate/FendenteDivino", order = 2)]
    public class DivineSlashAbbotAbility : ScriptableAbility
    {
        //public override bool CanUseThis(Unit attacker, Unit unitTarget)
        //{
        //    return true;//se è più smart returna true
        //}

        //public override System.Tuple<Unit,string> TriggerAbility(Unit attacker, Unit unitTarget, int weaponIndex, float powerUp, Buff buff)
        //{
        //    unitTarget.TakeDamage(CalculateAbilityDamage(unitTarget, powerUpDamage[PlayerManager.Singleton.GetPlayerLevel]), unitTarget);
        //    return System.Tuple.Create(unitTarget, abilityConsoleLog);
        //}

        //public override int ReturnAbilityCost()
        //{
        //    return defaultCost;
        //}


    }

}
