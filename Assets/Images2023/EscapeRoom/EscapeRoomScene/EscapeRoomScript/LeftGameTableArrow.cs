/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LeftGameTableArrow : MonoBehaviour
    {
        [SerializeField] GameObject[] panelWood = new GameObject[4];
        int temp = 0;

        public void OnMouseDown()
        {
            panelWood[temp].SetActive(false);
            if(temp == 3) { temp = -1; }
            temp++;
            panelWood[temp].SetActive(true);
        }
    }
}
