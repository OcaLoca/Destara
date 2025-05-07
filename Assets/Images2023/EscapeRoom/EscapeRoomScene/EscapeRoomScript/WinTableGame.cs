/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WinTableGame : MonoBehaviour
    {
        [SerializeField] GameObject[] itemTurnOff;
        [SerializeField] GameObject solution;
        private void OnTriggerEnter2D(Collider2D someObject)
        {
            if (someObject.transform.gameObject.name == "PlayerSphere")
            {
                foreach(GameObject obj in itemTurnOff)
                {
                    obj.SetActive(false);
                }
                solution.gameObject.SetActive(true);
            }
        }

    }

}
