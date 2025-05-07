/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    
    public class IconsDatabase : MonoBehaviour
    {
        public const string LUCKYICONID = "LuckyIcon";
        public const string COURAGEICONID = "CourageIcon";
        public const string SUPERSTITIONICONID = "SuperstitionIcon";
        public const string CONSTITUTIONICONID = "ConstitutionIcon";
        public const string DEXTERITYICONID = "DexterityIcon";
        public const string STRENGHTICONID = "StrenghtIcon";
        public const string INTELLIGENTICONID = "IntelligenceIcon";
        public const string LIGHTWEAPONICONID = "LightWeaponIcon";
        public const string HEAVYWEAPONICONID = "HeavyWeaponIcon";
        public const string RANGEDWEAPONICONID = "RangedWeaponIcon";
        public const string SPECIALWEAPONICONID = "SpecialWeaponIcon";
        public const string ABILITYICONID = "AbilityIcon";
        public const string SHIELDICONID = "ShieldIcon";
        public const string ARMORICONID = "ArmorIcon";
        public const string HELMETICONID = "HelmetIcon";
        public const string EQUIPMENTICONID = "EquipmentIcon";
        public const string ACCESSORYICONID = "AccessoryIcon";
        public const string TALISMANICONID = "TalismanIcon";
        public const string RELICICONID = "RelicIcon";
        public const string GEMSTONEICONID = "GemStoneIcon";
        public const string LEVELICONID = "LevelIcon";
        public const string HPICONID = "HPIcon";
        public const string CONSUMABLEICONID = "ConsumableIcon";
        public const string COLLECTIBLEICONID = "CollectibleIcon";
        public const string STORYICONID = "StoryIcon";
        public const string DEFENCEICONID = "DefenceIcon";
        public const string WEIGHTICONID = "WeightIcon";
        public const string LIGHTSHIELDICON = "LightShieldIcon";
        public const string QUESTIONMARKICON = "QuestionMarkIcon";
        public const string BALANCEDSHIELDICON = "BalancedShieldIcon";
        public const string HEAVYSHIELDICON = "HeavyShieldIcon";
        public const string ABBOTICON = "AbbotIcon";
        public const string BOUNTYICON = "BountyIcon";
        public const string CRONEICON = "CroneIcon";
        public const string TRAFFICKERICON = "TraffickerIcon";
        public const string HPICON = "HPIcon";
        public const string STICON = "STIcon";
        public const string APICON = "APIcon";
        public const string PARALYZEDICON = "ParalyzedIcon";
        public const string BURNEDICON = "BurnedIcon";
        public const string POISONEDICON = "PoisonedIcon";
        public const string CONFUSEDICON = "ConfusedIcon";
        public const string FROZENICON = "FrozenIcon";
        public const string INVULNERABLEICON = "InvulnerableIcon";


        [SerializeField] List<Sprite> iconChapterDatabase;

        public static IconsDatabase Singleton { get; set; }

        void Awake()
        {
            Singleton = this;
        }

        public Sprite GetImageByName()
        {
            foreach(Sprite icon in iconChapterDatabase)
            {
                if (icon.name == PlayerManager.Singleton.currentPage.chapterSection.ToString() + "Icon")
                {
                    return icon;
                }
            }

            return null;
        }

        public Sprite GetSpriteIcon(string iconName)
        {
            foreach (Sprite icon in iconChapterDatabase)
            {
                if (icon.name == iconName)
                {
                    return icon;
                }
            }
            return null;
        }

        public Sprite GetEffectStatSpriteByStatusType(PlayerManager.Stats stats)
        {
            switch (stats)
            {
                case PlayerManager.Stats.paralyzed:
                    return Singleton.GetSpriteIcon(PARALYZEDICON);
                case PlayerManager.Stats.burned:
                    return Singleton.GetSpriteIcon(BURNEDICON);
                case PlayerManager.Stats.confused:
                    return Singleton.GetSpriteIcon(CONFUSEDICON);
                case PlayerManager.Stats.poisoned:
                    return Singleton.GetSpriteIcon(POISONEDICON);
                case PlayerManager.Stats.freezed:
                    return Singleton.GetSpriteIcon(FROZENICON);
                default:
                case PlayerManager.Stats.invurneable:
                    return Singleton.GetSpriteIcon(INVULNERABLEICON);
            }
        }



        public Sprite GetWeaponSpriteByAttackType(TypeDatabase.AttackType attackType)
        {
            switch (attackType)
            {
                case TypeDatabase.AttackType.Light:
                    return Singleton.GetSpriteIcon(LIGHTWEAPONICONID);
                case TypeDatabase.AttackType.Ranged:
                    return Singleton.GetSpriteIcon(RANGEDWEAPONICONID);
                case TypeDatabase.AttackType.Heavy:
                    return Singleton.GetSpriteIcon(HEAVYWEAPONICONID);
                case TypeDatabase.AttackType.Special:
                    return Singleton.GetSpriteIcon(SPECIALWEAPONICONID);
                default:
                    return Singleton.GetSpriteIcon(SPECIALWEAPONICONID);
            }
        }


        public Sprite GetArmorSpriteByDefenceType(TypeDatabase.DefenseType unitDefence)
        {
            switch (unitDefence)
            {
                case TypeDatabase.DefenseType.Light:
                    return Singleton.GetSpriteIcon(LIGHTSHIELDICON);
                case TypeDatabase.DefenseType.Avarage:
                    return Singleton.GetSpriteIcon(BALANCEDSHIELDICON);
                case TypeDatabase.DefenseType.Heavy:
                    return Singleton.GetSpriteIcon(HEAVYSHIELDICON);
                case TypeDatabase.DefenseType.Special:
                    return Singleton.GetSpriteIcon(QUESTIONMARKICON);
                default:
                    return Singleton.GetSpriteIcon(QUESTIONMARKICON);
            }
        }

        public Sprite GetSpriteByObjectType(ScriptableItem.ItemType objType)
        {
            switch (objType)
            {
                case ScriptableItem.ItemType.Consumable:
                    return Singleton.GetSpriteIcon(CONSUMABLEICONID);
                case ScriptableItem.ItemType.Story:
                    return Singleton.GetSpriteIcon(STORYICONID);
                case ScriptableItem.ItemType.Collectible:
                    return Singleton.GetSpriteIcon(COLLECTIBLEICONID);
                case ScriptableItem.ItemType.Relic:
                    return Singleton.GetSpriteIcon(RELICICONID);
                case ScriptableItem.ItemType.Talisman:
                    return Singleton.GetSpriteIcon(TALISMANICONID);
                case ScriptableItem.ItemType.GemStone:
                    return Singleton.GetSpriteIcon(GEMSTONEICONID);
                default:
                    return Singleton.GetSpriteIcon(GEMSTONEICONID);
            }
        }
    }
}
