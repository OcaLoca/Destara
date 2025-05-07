using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarworkGC.Localization;

namespace Game
{

    [CreateAssetMenu(fileName = "Abbot", menuName = "ScriptableClass/Abbot", order = 1)]

    public class Abbot : ScriptableClass
    {
        private void OnEnable()
        {
            savedName = "Abbot";
            className = Localization.Get("Abbot");
        }

        public void ComeBackToLifeEffect()
        {
            if (constitution == 0 && superstition > 59) 
            {
                int generator = Random.Range(0, 101);
                if (generator <= 5)
                {
                    PlayerManager.Singleton.constitution = this.constitution; 
                }
            }
        }

        public void IncreaseEquipementDefence()
        {
            if (PlayerManager.Singleton.constitution < 30 && PlayerManager.Singleton.superstition < 41) 
            {
                float finalArmor = PlayerManager.Singleton.GetDefenceArmor();
                finalArmor = (finalArmor / 100) * 130;
                //PlayerManager.Singleton.SetTotalArmor(finalArmor);
            }
        }
        
      //  public List<ScriptableAbility> ShowDifferentSkills()
      //  {
      //      List<ScriptableAbility> abilities = new List<ScriptableAbility>();
      //      if (superstition >= 50)
      //      {
      //          //usasolohighSuperstitionSkills;
      //          //i libri possono essere equipaggiati come armi
      //          foreach(ScriptableAbility ability in highSuperstitionSkills)
      //          {
      //              abilities.Add(ability);
      //          }
      //          abilities.Add(highSupersttionFinalSkills);
      //      }
      //      else
      //      {
      //          foreach (ScriptableAbility ability in lowSuperstitionSkills)
      //          {
      //              abilities.Add(ability);
      //          }
      //          abilities.Add(lowSupersttionFinalSkills);
      //      }
      //      //usasolo lowSuperstitionSkills;
      //      //i libri non possono essere equipaggiati come armi
      //      return abilities;
      //  }
      //

        public override bool CanEscape()
        {
            if (PlayerManager.Singleton.courage < 31)
            {
                return false;
            }
            return true;
        }

        public override bool CanEquipWeapon(Weapon weapon)
        {
            if (weapon.attackType == TypeDatabase.AttackType.Ranged)
            {
                return false;
            }
            return true;

        }
    }
}

