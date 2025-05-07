/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    public class TurnOffAnimation : MonoBehaviour
    {
        public void TurnOffFromAnim()
        {
            gameObject.SetActive(false);
        }
    }

}
