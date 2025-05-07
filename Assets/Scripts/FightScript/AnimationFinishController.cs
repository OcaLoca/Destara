/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //Serve per dare tempistiche alla console
    public class AnimationFinishController : MonoBehaviour
    {
        public void AnimationIsFinish()
        {
            BattleController.animationIsFinished = true;
        }
    }

}
