using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{

    [CreateAssetMenu(fileName = "NaturalEquipmentType", menuName = "Type/NaturalEquipmentType", order = 4)]
    public class NaturalEquipmentType : ScriptableObject
    {
        public NaturalEquipmentTypes naturalEquipmentTypes;

        public enum NaturalEquipmentTypes
        {
            Air,     
            Water,   
            Land,    
            Undefinied   
        }
       
    }

}