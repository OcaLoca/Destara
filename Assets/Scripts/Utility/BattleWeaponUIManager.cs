/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BattleWeaponUIManager : MonoBehaviour
    {
        public static BattleWeaponUIManager Singleton { get; set; }
        public Animator animator;

        private void OnEnable()
        {
            Singleton = this;
        }

        public void WeaponExitFromScene()
        {
            gameObject.SetActive(false);
        }
    }

}
