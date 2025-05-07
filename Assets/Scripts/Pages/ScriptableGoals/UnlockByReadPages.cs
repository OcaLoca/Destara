/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GenericGoal", menuName = "ScriptableGoal/ReadPages", order = 0)]
    public class UnlockByReadPages : ScriptableGoal
    {
        [SerializeField] string [] pagesToRead;

        public override bool UnlockTheGoal()
        {
            int countToUnlock = pagesToRead.Length;
            int count = 0;
            foreach (string pg in pagesToRead)
            {
                if (PlayerManager.Singleton.currentPage.pageID != pg)
                {
                    return false;
                }
            }
            foreach(string pg in pagesToRead)
            {
                if (PlayerManager.Singleton.pagesRead.Contains(pg))
                {
                    count += 1;
                }
            }

            if(count == countToUnlock) { return true; }
            else { return false; }
        }

        public override float GetExperienceFromTrophy()
        {
            return base.GetExperienceFromTrophy();
        }

    }

}
