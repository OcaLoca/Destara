using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BattleGroundManager : MonoBehaviour
    {
        [SerializeField] string[] bGroundID = new string[2];
        public string [] GetGroundID { get => bGroundID; }
    }
}

