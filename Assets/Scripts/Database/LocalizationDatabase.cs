using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class LocalizationDatabase : MonoBehaviour
    {
        //Class stories

        public static string ABBOT_STORY = "AbbotStory";
        public static string BOUNTY_STORY = "BountyStory";
        public static string CRONE_STORY = "CroneStory";
        public static string TRAFFICKER_STORY = "TraffickerStory";

        //BonusMalus

        public static string ABBOT_PECULIARITY = "AbbotPeculiarity";
        public static string BOUNTY_PECULIARITY = "BountyPeculiarity";
        public static string CRONE_PECULIARITY = "CronePeculiarity";
        public static string TRAFFICKER_PECULIARITY = "TraffickerPeculiarity";

        //Fight ability

        public static string ABBOT_ABILITY_DESCRIPTION = "AbbotAbilityDescription";
        public static string BOUNTY_ABILITY_DESCRIPTION = "BountyAbilityDescription";
        public static string CRONE_ABILITY_DESCRIPTION = "CroneAbilityDescription";
        public static string TRAFFICKER_ABILITY_DESCRIPTION = "TraffickerAbilityDescription";

        //Fight and Story Stats 
        public const string LEVEL = "level";
        public const string HP = "hp";
        public static string ST = "Stamina";
        public static string AP = "AbilityPoints";

        //color
        public const string DAMAGECOLOR = "damageColor";
        public const string GENERIC_WEAPON_COLOR = "genericWeaponColor";
        public const string GENERIC_DEFENCE_COLOR = "genericDefenceColor";
        public const string GENERIC_ACCESORIES_COLOR = "genericAccesoriesColor";
        public const string GENERIC_ABILITY_COLOR = "genericAbilityColor";
        public const string STAMINA_COLOR = "staminaColor";
        public const string AP_COLOR = "APColor";
        public const string WHITE_COLOR = "White";

        public static string LEVELINFO = "infoIconLivello";
        public static string HPINFO = "infoHP";
        public static string STINFO = "infoStamina";
        public static string APINFO = "infoAbilityPoints";

        public const string SUPERSTITION = "Superstition";
        public const string COURAGE = "Courage";
        public const string LUCKY = "Lucky";

        public const string SUPERSTITIONINFO = "infoIconSuperstition";
        public const string COURAGEINFO = "infoIconCourage";
        public const string LUCKYINFO = "infoIconLuck";

        public static string COSTITUTION = "Costitution";
        public static string STRENGHT = "Strenght";
        public static string DEXTERITY = "Dexterity";
        public static string INTELLIGENCE = "Intelligence";

        public static string COSTITUTIONINFO = "infoIconCostituzione";
        public static string STRENGHTINFO = "infoIconForza";
        public static string DEXTERITYINFO = "infoIconDestrezza";
        public static string INTELLIGENCEINFO = "infoIconIntelligenza";

        //UI

        public static string CONTINUE = "Continue";
        public static string NEWGAME = "NewGame";
        public static string CURIOSITY = "Curiosity";
        public static string OPTION = "Option";

        //EQUIPMENT
        public static string LIGHTWEAPON = "LightWeapon";
        public static string HEAVYWEAPON = "HeavyWeapon";
        public static string RANGEDWEAPON = "RangedWeapon";
        public static string SPECIALWEAPON = "SpecialWeapon";

        public static string LIGHTWEAPONINFO = "InfoIconArmaleggera";
        public static string HEAVYWEAPONINFO = "InfoIconArmapesante";
        public static string RANGEDWEAPONINFO = "InfoIconArmaDistanza";
        public static string SPECIALWEAPONINFO = "SpecialWeaponInfo";

        public static string EQUIPMENT = "Armor2";
        public static string ARMOR = "Armor"; //"Intesa come corazza"
        public static string HELMET = "Helmet";
        public static string SHIELD = "Shield";
        public static string ACCESSORY = "Accessory";
        public static string TALISMAN = "Talisman";
        public static string RELIC = "Relic";       
        public static string GEMSTONE = "Gemstone";
        public static string ABILITY = "Ability";
        public static string LIGHTDEFENCE = "LightDefence";
        public static string BALANCEDDEFENCE = "BalancedDefence";
        public static string HEAVYDEFENCE = "HeavyDefence";

        public static string EQUIPMENTINFO = "InfoIconArmatura";
        public static string ARMORINFO = "InfoIconCorazza";
        public static string HELMETINFO = "InfoIconElmo";
        public static string SHIELDINFO = "InfoIconScudo";
        public static string ACCESSORYINFO = "InfoIconAccessorio";
        public static string TALISMANINFO = "InfoIconTalismano";
        public static string RELICINFO = "InfoIconRelic";
        public static string GEMSTONEINFO = "InfoIconGemStone";
        public static string ABILITYINFO = "InfoIconAbilità";
        public static string LIGHTDEFENCEINFO = "InfoIconLightDefence";
        public static string BALANCEDDEFENCEINFO = "InfoIconBalancedDefence";
        public static string HEAVYDEFENCEINFO = "InfoIconHeavyDefence";
        public static string DEFENECE = "InfoIconAbilità";

        public static string CONSUMABLE = "Consumable";
        public static string COLLECTIBLE = "Collectible";
        public static string STORY = "Story";

        public static string CONSUMABLEINFO = "InfoIconOggettoConsumabile";
        public static string COLLECTIBLEINFO = "InfoIconOggettoCollezionabile";
        public static string STORYINFO = "InfoIconOggettoStoria";

        public static string DEFENCE = "defence";
        public static string WEIGHT = "weight";

        public static string DEFENCEINFO = "InfoIconDefence";
        public static string WEIGHTINFO = "InfoIconWeight";
    }
}