/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EscapeRoomManager : MonoBehaviour
    {
        [SerializeField] string[] bRoomID = new string[2];
        public string[] GetRoomID { get => bRoomID; }
    }

}
