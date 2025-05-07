using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Crone", menuName = "ScriptableClass/Crone", order = 0)]
    public class Crone : ScriptableClass
    {
        private void OnEnable()
        {
            savedName = "Crone";
            className = Localization.Get("Crone");
        }

    }
}

