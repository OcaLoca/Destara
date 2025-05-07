/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RightGameTableArrow : MonoBehaviour
    {
        [SerializeField] GameObject[] panelWood = new GameObject[4];
        int temp = 3;

        public void OnMouseDown()
        {
            panelWood[temp].SetActive(false);
            if (temp == 0) { temp = 4; }
            temp--;
            panelWood[temp].SetActive(true);
        }
    }
}
