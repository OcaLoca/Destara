using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CreateAssetMenu(fileName = "Death", menuName = "ScriptableDeath", order = 0)]
    public class ScriptableDeath : ScriptableObject
    {
        public string deathID;
        public string txtDeathDescription;
        public Texture2D deathImage;
    }
}
