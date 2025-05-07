using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "WeaponType", menuName = "Type/WeaponType", order = 0)]
    public class WeaponType : ScriptableObject
    {
        /// non so qua se quando creo il tipo mi setta per quello scelto nell'enum, nel caso faccio una classe fissa per tipo.
        /// </summary>
        /// 
        public TypeDatabase.AttackType type;
        public EquipmentType[] effectiveAgainstEquipment; 
        public EquipmentType[] ineffectiveAgainstEquipment;
        public EquipmentType[] cantAttackEquipment;
        public NaturalEquipmentType[] effectiveAgainstNaturalEquipment;
        public NaturalEquipmentType[] ineffectiveAgainstNaturalEquipment;
        public NaturalEquipmentType[] cantAttackNaturalEquipment;
    }

}

