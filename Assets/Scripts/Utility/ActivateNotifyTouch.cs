/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ActivateNotifyTouch : MonoBehaviour
    {
        [SerializeField] GameObject NotifyPanel;
        [SerializeField] float time;
        private void OnEnable()
        {
            NotifyPanel.SetActive(false);
            //StartCoroutine(Activate(time));
        }

        IEnumerator Activate(float time)
        {
            yield return new WaitForSeconds(time);
            NotifyPanel.SetActive(true);
        }
    }

}
