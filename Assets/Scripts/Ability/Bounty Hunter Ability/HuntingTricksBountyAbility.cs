using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BattleController;
using Random = UnityEngine.Random;

namespace Game
{
    [CreateAssetMenu(fileName = "Abilità: Trucchi_Caccia", menuName = "ScriptableAbility/Cacciatore/TrucchiCaccia", order = 0)]
    public class HuntingTricksBountyAbility : ScriptableAbility
    {
        private void OnEnable()
        {
            defaultCost = 2;
        }

        public override Tuple<Unit, string> TriggerAbility(Unit attacker, Unit unitTarget, bool havePowerup = true, bool haveWeapon = true, int numberOfPowerup = 0, int weaponIndex = 0, Buff buff = Buff.strenght)
        {
            float debuffDexterity;
            int random = Random.Range(0, 3);
            {
                switch (random)
                {
                    case 2:
                        debuffDexterity = ((unitTarget.dexterity / 100) * 15);
                        break;
                    case 1:
                        debuffDexterity = ((unitTarget.dexterity / 100) * 10);
                        break;
                    default:
                        debuffDexterity = ((unitTarget.dexterity / 100) * 20);
                        break;
                }

                abilityConsoleLog = LocalizeText(LocalizationDatabase.DEXTERITY,unitTarget.name); 
                unitTarget.BuffDebuffStats(-debuffDexterity, Buff.dexterity);
                return Tuple.Create(unitTarget, abilityConsoleLog);
            }
        }

        string LocalizeText(string buff, string name)
        {
            return string.Format(Localization.Get(LocalizationIDDatabase.DEBUFF_BATTLE_LOG), Localization.Get(buff), name);
        }
    }
}
