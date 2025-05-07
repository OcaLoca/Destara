/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    public class MoveObjAnimationEscape : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] float alarmTime;
        [SerializeField] string storyID;

        private void OnMouseDown()
        {
            GameApplication.Singleton.view.EscapeRoomView.TurnOnPanelAlarm(storyID, alarmTime);
            animator.SetTrigger("Move");
        }

    }

}
