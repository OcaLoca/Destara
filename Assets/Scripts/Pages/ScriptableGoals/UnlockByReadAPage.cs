/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GenericGoal", menuName = "ScriptableGoal/ReadOnePage", order = 1)]

    public class UnlockByReadAPage : ScriptableGoal
    {
        [SerializeField] string pageToRead;

        public override bool UnlockTheGoal()
        {
            //controllo se pagina è stata letta
            if (PlayerManager.Singleton.pagesRead.Contains(pageToRead) && (PlayerManager.Singleton.currentPage.pageID == pageToRead))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override float GetExperienceFromTrophy()
        {
            return base.GetExperienceFromTrophy();
        }
    }

}
