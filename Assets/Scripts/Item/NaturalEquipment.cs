using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableItem/NaturalEquipement", order = 0)]
    public class NaturalEquipment : ScriptableObject
    {

        public NaturalEquipmentType naturalEquipmentType;
        public ElementalBuff elementalBuff;
    }
}