/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ColorDatabase : MonoBehaviour
    {
        public static ColorDatabase Singleton { get; set; }

        private void Awake()
        {
            Singleton = this;
        }
        //Rarity color
        [SerializeField] public Color32 commonColor;
        [SerializeField] public Color32 uncommonColor;
        [SerializeField] public Color32 rareColor;
        [SerializeField] public Color32 superRareColor;
        [SerializeField] public Color32 legendaryColor;
        [SerializeField] public Color32 buttonDefaultColor;
        [SerializeField] public Color32 courageColor;
        [SerializeField] public Color32 luckyColor;
        [SerializeField] public Color32 supertitionColor;
        [SerializeField] public Color32 hpColor;
        [SerializeField] public Color32 levelColor;
        [SerializeField] public Color32 damagedColor;
        [SerializeField] public Color32 genericIconColor;
        [SerializeField] public Color32 genericWeaponColor;
        [SerializeField] public Color32 genericDefenceColor;
        [SerializeField] public Color32 genericAccessoriesColor;
        [SerializeField] public Color32 staminaColor;
        [SerializeField] public Color32 APColor;
        [SerializeField] public Color32 genericAbilityColor;
        [SerializeField] public Color32 underlineTextColor;
        [SerializeField] public Color32 textImportantColor;
        [SerializeField] public Color32 textPainColor;
        [SerializeField] public Color32 textSearchLocationColor;
        [SerializeField] public Color32 whiteCream;

        public Color32 GetRarityColor(ScriptableItem.Rarity rarity)
        {
            switch (rarity)
            {
                case ScriptableItem.Rarity.Common:
                    return commonColor;
                case ScriptableItem.Rarity.Usual:
                    return uncommonColor;
                case ScriptableItem.Rarity.Rare:
                    return rareColor;
                case ScriptableItem.Rarity.SuperRare:
                    return superRareColor;
                case ScriptableItem.Rarity.Legendary:
                    return legendaryColor;
            }
            return commonColor;
        }

        public Color32 GetColorByName(string statName)
        {
            switch (statName)
            {
                case LocalizationDatabase.COURAGE:
                    return courageColor;
                case LocalizationDatabase.LUCKY:
                    return luckyColor;
                case LocalizationDatabase.SUPERSTITION:
                    return supertitionColor;
                case LocalizationDatabase.HP:
                    return hpColor;
                case LocalizationDatabase.LEVEL:
                    return levelColor;
                case LocalizationDatabase.DAMAGECOLOR:
                    return damagedColor;
                case LocalizationDatabase.GENERIC_WEAPON_COLOR:
                    return genericWeaponColor;
                case LocalizationDatabase.GENERIC_DEFENCE_COLOR:
                    return genericDefenceColor;
                case LocalizationDatabase.GENERIC_ACCESORIES_COLOR:
                    return genericAccessoriesColor;
                case LocalizationDatabase.GENERIC_ABILITY_COLOR:
                    return genericAbilityColor;
                case LocalizationDatabase.STAMINA_COLOR:
                    return staminaColor;
                case LocalizationDatabase.AP_COLOR:
                    return APColor;
                    case LocalizationDatabase.WHITE_COLOR:
                        return whiteCream;
                default:
                    return genericIconColor;
            }
        }
    }

}
