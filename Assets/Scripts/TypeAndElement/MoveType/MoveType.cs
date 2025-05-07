using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{

    [CreateAssetMenu(fileName = "MoveType", menuName = "Type/MoveType", order = 0)]
    public class MoveType : ScriptableObject
    {
        public MoveTypes moveTypes;
        public EquipmentType[] effectiveAgainstEquipments;
        public EquipmentType[] ineffectiveAgainstEquipments;
        public EquipmentType[] cantAttackEquipments;



        public enum MoveTypes
        {
            Light,
            Heavy,
            Ranged,
            Shield,
            Jolly
        }
       
    }
}
