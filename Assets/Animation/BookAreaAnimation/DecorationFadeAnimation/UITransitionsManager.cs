/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    
    public class UITransitionsManager : View<GameApplication>
    {
        public Animator transition;
        public void PlayFightAnimationTransition()
        {
            transition.SetTrigger("Start");
        }
    }

}
