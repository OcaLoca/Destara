/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

//using Microsoft.Unity.VisualStudio.Editor;
using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Game
{

    public class InfoIconPanelManager : MonoBehaviour
    {
        public static InfoIconPanelManager Instance;
        [SerializeField] Image iconSprite;
        [SerializeField] TMP_Text iconDescription;
        [SerializeField] TMP_Text iconName;
        [SerializeField] GameObject panelInfoIcon, STIcon, APIcon, HPIcon;

        private void Awake()
        {
            Instance = this;
        }

        const string BTN = "Btn";
        public void SetPanelByButtonName(string btnIconName)
        {
            panelInfoIcon.SetActive(true);
            iconSprite.gameObject.SetActive(true);
            STIcon.SetActive(false);
            APIcon.SetActive(false);
            HPIcon.SetActive(false);

            iconSprite.transform.localScale = new Vector3(1, 1, 1);

            iconSprite.color = Color.white;

            string originalIconName = btnIconName.Replace(BTN, string.Empty);
            iconSprite.color = ColorDatabase.Singleton.GetColorByName("default");

            switch (originalIconName)
            {
                case IconsDatabase.LEVELICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LEVELICONID);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.LEVEL);
                    iconSprite.transform.localScale = new Vector3(1, -1, 1);
                    iconName.text = Localization.Get(LocalizationDatabase.LEVEL);
                    iconDescription.text = Localization.Get(LocalizationDatabase.LEVELINFO);
                    break;
                case IconsDatabase.HPICONID:
                    iconSprite.gameObject.SetActive(false);
                    HPIcon.SetActive(true);
                    iconName.text = Localization.Get(LocalizationDatabase.HP);
                    iconDescription.text = Localization.Get(LocalizationDatabase.HPINFO);
                    break;
                case IconsDatabase.SUPERSTITIONICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.SUPERSTITIONICONID);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.SUPERSTITION);
                    iconDescription.text = Localization.Get(LocalizationDatabase.SUPERSTITIONINFO);
                    iconName.text = Localization.Get(LocalizationDatabase.SUPERSTITION);
                    break;
                case IconsDatabase.COURAGEICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.COURAGEICONID);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.COURAGE);
                    iconDescription.text = Localization.Get(LocalizationDatabase.COURAGEINFO);
                    iconName.text = Localization.Get(LocalizationDatabase.COURAGE);
                    break;
                case IconsDatabase.LUCKYICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LUCKYICONID);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.LUCKY);
                    iconDescription.text = Localization.Get(LocalizationDatabase.LUCKYINFO);
                    iconName.text = Localization.Get(LocalizationDatabase.LUCKY);
                    break;
                case IconsDatabase.CONSTITUTIONICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.CONSTITUTIONICONID);
                    iconDescription.text = Localization.Get(LocalizationDatabase.COSTITUTIONINFO);
                    iconName.text = Localization.Get(LocalizationDatabase.COSTITUTION);
                    break;
                case IconsDatabase.DEXTERITYICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.DEXTERITYICONID);
                    iconDescription.text = Localization.Get(LocalizationDatabase.DEXTERITYINFO);
                    iconName.text = Localization.Get(LocalizationDatabase.DEXTERITY);
                    break;
                case IconsDatabase.STRENGHTICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.STRENGHTICONID);
                    iconDescription.text = Localization.Get(LocalizationDatabase.STRENGHTINFO);
                    iconName.text = Localization.Get(LocalizationDatabase.STRENGHT);
                    break;
                case IconsDatabase.INTELLIGENTICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.INTELLIGENTICONID);
                    iconDescription.text = Localization.Get(LocalizationDatabase.INTELLIGENCEINFO);
                    iconName.text = Localization.Get(LocalizationDatabase.INTELLIGENCE);
                    break;
                case IconsDatabase.LIGHTWEAPONICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LIGHTWEAPONICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.LIGHTWEAPON);
                    iconDescription.text = Localization.Get(LocalizationDatabase.LIGHTWEAPONINFO);
                    break;
                case IconsDatabase.HEAVYWEAPONICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.HEAVYWEAPONICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.HEAVYWEAPON);
                    iconDescription.text = Localization.Get(LocalizationDatabase.HEAVYWEAPONINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_WEAPON_COLOR);
                    break;
                case IconsDatabase.RANGEDWEAPONICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.RANGEDWEAPONICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.RANGEDWEAPON);
                    iconDescription.text = Localization.Get(LocalizationDatabase.RANGEDWEAPONINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_WEAPON_COLOR);
                    break;
                case IconsDatabase.SPECIALWEAPONICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.SPECIALWEAPONICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.SPECIALWEAPON);
                    iconDescription.text = Localization.Get(LocalizationDatabase.SPECIALWEAPONINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_WEAPON_COLOR);
                    break;
                case IconsDatabase.LIGHTSHIELDICON:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LIGHTSHIELDICON);
                    iconName.text = Localization.Get(LocalizationDatabase.LIGHTDEFENCE);
                    iconDescription.text = Localization.Get("InfoIcon" + LocalizationDatabase.LIGHTDEFENCE);
                    break;
                case IconsDatabase.BALANCEDSHIELDICON:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.BALANCEDSHIELDICON);
                    iconName.text = Localization.Get(LocalizationDatabase.BALANCEDDEFENCE);
                    iconDescription.text = Localization.Get("InfoIcon" + LocalizationDatabase.BALANCEDDEFENCE);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_DEFENCE_COLOR);
                    break;
                case IconsDatabase.HEAVYSHIELDICON:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.HEAVYSHIELDICON);
                    iconName.text = Localization.Get(LocalizationDatabase.HEAVYDEFENCE);
                    iconDescription.text = Localization.Get("InfoIcon" + LocalizationDatabase.HEAVYDEFENCE);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_DEFENCE_COLOR);
                    break;
                case IconsDatabase.EQUIPMENTICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.EQUIPMENTICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.EQUIPMENT);
                    iconDescription.text = Localization.Get(LocalizationDatabase.EQUIPMENTINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_DEFENCE_COLOR);
                    break;
                case IconsDatabase.SHIELDICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.SHIELDICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.SHIELD);
                    iconDescription.text = Localization.Get(LocalizationDatabase.SHIELDINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_DEFENCE_COLOR);
                    break;
                case IconsDatabase.ARMORICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.ARMORICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.ARMOR);
                    iconDescription.text = Localization.Get(LocalizationDatabase.ARMORINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_DEFENCE_COLOR);
                    break;
                case IconsDatabase.GEMSTONEICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.GEMSTONEICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.GEMSTONE);
                    iconDescription.text = Localization.Get(LocalizationDatabase.GEMSTONEINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_ACCESORIES_COLOR);
                    break;
                case IconsDatabase.RELICICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.RELICICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.RELIC);
                    iconDescription.text = Localization.Get(LocalizationDatabase.RELICINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_ACCESORIES_COLOR);
                    break;
                case IconsDatabase.TALISMANICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.TALISMANICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.TALISMAN);
                    iconDescription.text = Localization.Get(LocalizationDatabase.TALISMANINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_ACCESORIES_COLOR);
                    break;
                case IconsDatabase.ABILITYICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.ABILITYICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.ABILITY);
                    iconDescription.text = Localization.Get(LocalizationDatabase.ABILITYINFO);
                    iconSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_ABILITY_COLOR);
                    break;
                case IconsDatabase.CONSUMABLEICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.CONSUMABLEICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.CONSUMABLE);
                    iconDescription.text = Localization.Get(LocalizationDatabase.CONSUMABLEINFO);
                    break;
                case IconsDatabase.STORYICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.STORYICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.STORY);
                    iconDescription.text = Localization.Get(LocalizationDatabase.STORYINFO);
                    break;
                case IconsDatabase.COLLECTIBLEICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.COLLECTIBLEICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.COLLECTIBLE);
                    iconDescription.text = Localization.Get(LocalizationDatabase.COLLECTIBLEINFO);
                    break;
                case IconsDatabase.DEFENCEICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.DEFENCEICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.DEFENCE);
                    iconDescription.text = Localization.Get(LocalizationDatabase.DEFENCEINFO);
                    break;
                case IconsDatabase.WEIGHTICONID:
                    iconSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.WEIGHTICONID);
                    iconName.text = Localization.Get(LocalizationDatabase.WEIGHT);
                    iconDescription.text = Localization.Get(LocalizationDatabase.WEIGHTINFO);
                    break;
                case IconsDatabase.STICON:
                    iconSprite.gameObject.SetActive(false);
                    STIcon.SetActive(true);
                    iconName.text = Localization.Get(LocalizationDatabase.ST);
                    iconDescription.text = Localization.Get(LocalizationDatabase.STINFO);
                    break;
                case IconsDatabase.APICON:
                    iconSprite.gameObject.SetActive(false);
                    APIcon.SetActive(true);
                    iconName.text = Localization.Get(LocalizationDatabase.AP);
                    iconDescription.text = Localization.Get(LocalizationDatabase.APINFO);
                    break;
                default:
                    return;
            }
        }
    }

}
