using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(fileName = "LionHearthMedal", menuName = "ScriptableGoal/Medal/LionHearthMedal", order = 0)]
    public class LionHearthGoal : ScriptableGoal
    {
        public override bool UnlockTheGoal()
        {
            if (PlayerManager.Singleton.courage == 100)
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
