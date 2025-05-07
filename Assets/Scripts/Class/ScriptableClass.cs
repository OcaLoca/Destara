using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

   
    public abstract class ScriptableClass : ScriptableObject
    {
        #region GeneralStats
        [SerializeField] int maxTorchCount;

        [SerializeField] int maxPaperCount;
        public int GetMaxPaperCount { get => maxPaperCount; }
        public int GetMaxTorchCount { get => maxTorchCount; }
        #endregion

        #region BookStats

        [Range(0, 100)]
        public int courage;
        [Range(0, 100)]
        public int luck;
        [Range(0, 100)]
        public int superstition;

        #endregion
        #region FighStats

        public string GetClassName { get => className; }
        protected string className; 
        protected string savedName;
        public string GetSavedName { get => savedName; }
        public string classDescriptionKey;
        public float constitution;
        internal float constitutionLimit;
        public float dexterity;
        internal float dexterityLimit;
        public float strength;
        internal float strengthLimit;
        public float inteligence;
        internal float intelligenceLimit;
       
        public List<ScriptableAbility> abilities = new List<ScriptableAbility>();
        public ScriptableAbility finalAbility;
        public List<ScriptableAbility> highAbilities = new List<ScriptableAbility>();
        public ScriptableAbility highFinalAbility;

        public RuntimeAnimatorController classAnimation;

        #endregion

        public Equipment equippedClassDefence;
        public Equipment equippedLightDefence;
        public Equipment equippedBalancedDefence;
        public Equipment equippedHeavyDefence;
        public Equipment equippedGemStone;
        public Equipment equippedTalisman;
        public Equipment equippedRelic;
        public Weapon lightWeapon;
        public Weapon heavyWeapon;
        public Weapon rangeWeapon;
        public Weapon specialWeapon;
        public Weapon equippedWeapon;
        public ScriptableItem[] itemInventory; //Oggetti da dare al player in base alla classe scelta

        public virtual bool CanEscape()
        {
            return true;
        }

        public virtual bool CanEquipWeapon(Weapon weapon)
        {
            return true;
        }

        public virtual bool CanEquip(Equipment equip)
        {
            return true;
        }

        // public virtual void ReviewStats(){}
    }
}