/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //Serve per dare tempistiche ai feedback di danno
    public class PlayerAnimationFinish : MonoBehaviour
    {
        public static bool AttackAnimationFinished = false;

        public void AttackIsFinished()
        {
            AttackAnimationFinished = true;
        }

    }

}
