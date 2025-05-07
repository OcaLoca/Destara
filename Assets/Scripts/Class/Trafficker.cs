using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Trafficker", menuName = "ScriptableClass/Trafficker", order = 2)]

    public class Trafficker : ScriptableClass
    {
        private void OnEnable()
        {
            savedName = "Smuggler";
            className = Localization.Get("Smuggler");
        }
    }

}


