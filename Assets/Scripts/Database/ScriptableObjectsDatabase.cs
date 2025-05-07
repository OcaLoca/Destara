using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScriptableObjectsDatabase : MonoBehaviour
    {
        public static ScriptableObjectsDatabase Singleton;
        
        public WeaponType rangedType;
        public WeaponType heavyType;
        public ScriptableEnemy[] enemiesDatabase;
        public Abbot abbot;
        public Crone crone;
        public BountyHunter bountyHunter;
        public Trafficker trafficker;
        public Equipment[] equipmentDatabase;
        public EquipmentType jollyEquipmentType;
         
        private void Awake() //prima che parta il gioco
        {
            Singleton = this;
        }

        public Equipment GetEquipmentById(string ID)
        {
            foreach (Equipment equip in equipmentDatabase)
            {
                if (equip.ID == ID)
                {
                    return equip;
                }
            }
            return null;
        }
    }
}


