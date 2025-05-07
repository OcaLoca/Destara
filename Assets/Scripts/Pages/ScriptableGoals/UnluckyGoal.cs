using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "UnluckyMedal", menuName = "ScriptableGoal/Medal/UnluckyMedal", order = 0)]

    public class UnluckyGoal : ScriptableGoal
    {

        public override bool UnlockTheGoal()
        {
            if ((PlayerManager.Singleton.lucky < 10) && (PlayerManager.Singleton.pagesRead.Contains("inizioTutorial")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
