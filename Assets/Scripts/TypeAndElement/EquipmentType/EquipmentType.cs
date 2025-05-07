using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "EquipmentType", menuName = "Type/EquipmentType", order = 1)]
    public class EquipmentType : ScriptableObject
    {
        public TypeDatabase.DefenseType type;
        public EquipmentPositionType equipmentPosition;
        public ElementalBuff elementalDefence;

        public enum EquipmentPositionType
        {
            Headgear,
            Armor,
            Shield,
            Special,
            Accessory
        }
    }
}


