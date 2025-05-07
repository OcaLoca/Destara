/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GenericGoal", menuName = "ScriptableGoal/ReadOneOfThis", order = 0)]

    public class UnlockByReadOneOfThisPage : ScriptableGoal
    {
        [SerializeField] string[] pagesToRead;

        public override bool UnlockTheGoal()
        {

            foreach (string pg in pagesToRead)
            {
                if (PlayerManager.Singleton.currentPage.pageID == pg)
                {
                    return true;
                }
            }
            return false;
        }

        public override float GetExperienceFromTrophy()
        {
            return base.GetExperienceFromTrophy();
        }


    }

}
