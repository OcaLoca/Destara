/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TriggerableTable : MonoBehaviour
    {
        [SerializeField] GameObject [] arrowBtns;
        [SerializeField] GameObject [] otherArrowBtns;
        [SerializeField] string console;
        private void OnTriggerEnter2D(Collider2D someObject)
        {
            if (someObject.transform.gameObject.name == "PlayerSphere")
            {
                Debug.Log(console);
                foreach(GameObject obj in arrowBtns) { obj.gameObject.SetActive(false); }
                foreach(GameObject obj in otherArrowBtns) { obj.gameObject.SetActive(true); }
            }
        }
    }

}
