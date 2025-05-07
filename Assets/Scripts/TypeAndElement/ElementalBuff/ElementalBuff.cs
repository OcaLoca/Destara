using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(fileName = "ElementalBuff", menuName = "Element/ElementBuff", order = 0)]
    public class ElementalBuff : ScriptableObject
    {
        public ElementalBuff elementalBuff;
        public ElementalBuff[] effectiveAgainstElements;
        public enum ElementalBuffes
        {
            Fire,  //infleggi tot danni a tutti                  
            Ice,  //rallenta
            Poisonous, //avvelena
            NoElement
        }

    }
}
