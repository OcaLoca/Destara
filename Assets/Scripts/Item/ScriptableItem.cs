using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;

namespace Game
{
    public class ScriptableItem : ScriptableObject
    {
        public string ID;
        public string itemNameLocalization;
        [HideInInspector]
        public string itemNameLocalized;

        public string bioDescriptionLocalized;
        public string infoDescriptionLocalized;
        public Sprite icon;
        /// <summary>
        /// Il numero di oggetti che ha standard quell'oggetto esempio 
        /// Cestino di mele = x2 mele
        /// Mela = 1 mela
        /// </summary>
        [Tooltip("Numero di oggetti nell'oggetto. Ex: Cestino di mele = x2 / Mela = 1 mela")]
        public int objectQuantity = 1;
        public int GetObjectQuantity { get => objectQuantity; }
        public Rarity rarity;
        public bool consumableInFight;
        public int level;
        public ItemType itemType;

        public enum Rarity
        {
            Common,
            Usual,
            Rare,
            SuperRare,
            Legendary
        }

       public enum ItemType
        {
            Consumable,
            Story,
            Collectible,
            Weapon,
            Armor,
            Relic,
            Talisman,
            GemStone
        }

        public string GetItemType()
        {
            return itemType.ToString();
        }

        internal string GetLocalizedRarity(Rarity rarity)
        {
            return Localization.Get(rarity.ToString());
        }


        private void OnEnable()
        {
            SetLocalizedText();
        }

        public void SetLocalizedText()
        {
            SetLocalizeBio();
            SetLocalizeInfo();
            SetLocalizeTitle();
        }

        public void SetLocalizeBio()
        {
            bioDescriptionLocalized = Localization.Get(itemNameLocalization + "Bio");
        }
        public void SetLocalizeInfo()
        {
            infoDescriptionLocalized = Localization.Get(itemNameLocalization + "Info");
        }

        internal string GetLocalizedBio()
        {
            SetLocalizeBio();
            return bioDescriptionLocalized;
        }
        public void SetLocalizeTitle()
        {
            itemNameLocalized = Localization.Get(itemNameLocalization);
        }

        internal string GetLocalizedObjName()
        {
            SetLocalizeTitle();
            return itemNameLocalized;
        }
        public string GetLocalizedInfo()
        {
            SetLocalizeInfo();
            return infoDescriptionLocalized;
        }

        //Utilizzare item consumabili
        public virtual void TriggerItem() { return; }
        public virtual Unit TriggerItemInBattle(ref Unit unit, ConsumableItem item)
        {
            return unit;
        }

        //Utilizzare oggetti collezionabili
        public virtual bool UnlockAllCollectible()
        {
            return false;
        }

        public virtual string UseStoryItem()
        {
            return null;
        }



    }
}

