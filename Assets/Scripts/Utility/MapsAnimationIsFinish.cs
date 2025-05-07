/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using UnityEngine;

namespace Game
{
    public class MapsAnimationIsFinish : MonoBehaviour
    {
        public void SetAnimationFinish()
        {
            MapAnimationManager.mapsAnimationIsFinish = true;
        }
    }
}
