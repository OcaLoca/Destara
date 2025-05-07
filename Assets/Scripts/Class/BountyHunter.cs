using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BountyHunter", menuName = "ScriptableClass/BountyHunter", order = 3)]
    public class BountyHunter : ScriptableClass
    {
        private void OnEnable()
        {
            savedName = "BountyHunter";
            className = Localization.Get("BountyHunter");
        }
        public void BonusStats()
        {
            //può usare tutti i tipi di armi ed è l'unico che può usare le armi incendiarie
            //Possibilità del 30% che il nemico nel fight parte con il 10% in meno della sua costituzione
            if (courage < 30)
            {
                courage = 30;
            }

        }
    }


}

