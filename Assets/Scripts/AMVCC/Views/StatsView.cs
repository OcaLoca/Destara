using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SmartMVC;
using TMPro;
using StarworkGC.Localization;

namespace Game
{
    public class StatsView : View<GameApplication>
    {
        [Header("UI")]
        public GameObject PanelTutorial;
        public ScrollRect statsScrollRect;
        public Button btnBack;
        public Button btnEquip;
        public Button btnInventory;
        public List<ScriptableItem> specialWeaponList;
        [SerializeField] List<Sprite> classSprites = new List<Sprite>();
        [SerializeField] List<Sprite> classImagesBackground = new List<Sprite>();
        public GameObject PanelLegend;
        public Button btnOpenLegendPanel;


        [Header("Name&Life&Level")]
        [SerializeField] TMP_Text playerName;
        [SerializeField] TMP_Text currentHp;
        [SerializeField] TMP_Text level;
        [SerializeField] TMP_Text currentAP;
        [SerializeField] TMP_Text stamina;


        [Header("BookStats")]
        [SerializeField] TMP_Text braveValue;
        [SerializeField] TMP_Text luckyValue;
        [SerializeField] TMP_Text superstitionValue;

        [Header("PlayerStats")]
        [SerializeField] TMP_Text constitutionValue;
        [SerializeField] TMP_Text strenghtValue;
        [SerializeField] TMP_Text dexterityValue;
        [SerializeField] TMP_Text intelligenceValue;

        [Header("Weapon")]
        [SerializeField] TMP_Text lightWeaponName;
        [SerializeField] TMP_Text lightWeaponValue;
        [SerializeField] TMP_Text heavyWeaponName;
        [SerializeField] TMP_Text heavyWeaponValue;
        [SerializeField] TMP_Text RangedWeaponName;
        [SerializeField] TMP_Text RangedWeaponValue;
        [SerializeField] TMP_Text SpecialWeaponName;
        [SerializeField] TMP_Text SpecialWeaponValue;
        [SerializeField] Image LightWeaponIcon;
        [SerializeField] Image HeavyWeaponIcon;
        [SerializeField] Image RangedWeaponIcon;
        [SerializeField] Image SpecialWeaponIcon;

        [Header("Armor")]
        //[SerializeField] TMP_Text defenceType;
        //[SerializeField] TMP_Text defenceValue;
        [SerializeField] TMP_Text classDefenceName;
        [SerializeField] Image classDefenceImgType;
        [SerializeField] TMP_Text lightDefenceName;
        [SerializeField] TMP_Text balancedDefenceName;
        [SerializeField] TMP_Text heavyDefenceName;
        [SerializeField] TMP_Text classDefenceValue;
        [SerializeField] TMP_Text lightDefenceValue;
        [SerializeField] TMP_Text balancedDefenceValue;
        [SerializeField] TMP_Text heavyDefenceValue;

        [Header("Ability")]
        [SerializeField] TMP_Text firstAbilityName;
        [SerializeField] TMP_Text secondAbilityName;
        [SerializeField] TMP_Text thirdAbilityName;
        [SerializeField] TMP_Text fourthAbilityName;
        // [SerializeField] TMP_Text finalAbilityName;
        // [SerializeField] Button finalAbilityInfoBtn;

        [SerializeField] GameObject infoPanel;
        [SerializeField] TMP_Text txtInfo;
        [SerializeField] Button btnCloseInfoPanel;

        public static bool notShowed;

        void Start()
        {
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnReturnInGameSession);
            btnEquip.onClick.RemoveAllListeners();
            btnEquip.onClick.AddListener(delegate { OnClickEquipButton(); });
            btnInventory.onClick.RemoveAllListeners();
            btnInventory.onClick.AddListener(OnClickInventory);
        }

        private void OnEnable()
        {
            PanelTutorial.SetActive(false);

            if (notShowed)
            {
                if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1 ||
               PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1)
                {
                    notShowed = false;
                    PanelTutorial.SetActive(true);
                }
            }

            btnOpenLegendPanel.onClick.RemoveAllListeners();
            btnOpenLegendPanel.onClick.AddListener(OpenLegendPanel);

            //Reset scroll vertical position
            statsScrollRect.verticalNormalizedPosition = 1f;

            playerName.text = PlayerManager.Singleton.GetPlayerName();

            currentHp.text = ((int)PlayerManager.Singleton.lifePoints + "/" + (int)PlayerManager.Singleton.constitution * 10).ToString();
            currentAP.text = (PlayerManager.Singleton.GetPlayerAbilityPoints + "/" + PlayerManager.Singleton.GetAbilityPointsLimit()).ToString();
            stamina.text = ((int)PlayerManager.Singleton.GetPlayerStamina).ToString();

            int levelPlayer = PlayerManager.Singleton.GetPlayerLevel;
            level.text = UIUtility.GetLevelForUI(levelPlayer);

            braveValue.text = PlayerManager.Singleton.courage.ToString() + "%";
            superstitionValue.text = PlayerManager.Singleton.superstition.ToString() + "%";
            luckyValue.text = PlayerManager.Singleton.lucky.ToString() + "%";

            constitutionValue.text = PlayerManager.Singleton.constitution.ToString();
            strenghtValue.text = PlayerManager.Singleton.strength.ToString();
            dexterityValue.text = PlayerManager.Singleton.dexterity.ToString();
            intelligenceValue.text = PlayerManager.Singleton.inteligence.ToString();

            lightWeaponName.text = PlayerManager.Singleton.playerWeapon.lightWeapon.GetLocalizedObjName();
            lightWeaponValue.text = PlayerManager.Singleton.playerWeapon.lightWeapon.weaponConstDamage.ToString();
            heavyWeaponName.text = PlayerManager.Singleton.playerWeapon.heavyWeapon.GetLocalizedObjName();
            heavyWeaponValue.text = PlayerManager.Singleton.playerWeapon.heavyWeapon.weaponConstDamage.ToString();
            RangedWeaponName.text = PlayerManager.Singleton.playerWeapon.rangeWeapon.GetLocalizedObjName();
            RangedWeaponValue.text = PlayerManager.Singleton.playerWeapon.rangeWeapon.weaponConstDamage.ToString();
            SpecialWeaponName.text = PlayerManager.Singleton.playerWeapon.specialWeapon.GetLocalizedObjName();
            SpecialWeaponValue.text = PlayerManager.Singleton.playerWeapon.specialWeapon.weaponConstDamage.ToString();

            LightWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LIGHTWEAPONICONID);
            HeavyWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.HEAVYWEAPONICONID);
            RangedWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.RANGEDWEAPONICONID);
            SpecialWeaponIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.SPECIALWEAPONICONID);

            PlayerManager.PlayerEquipment equip = PlayerManager.Singleton.playerEquipment;

            classDefenceName.text = PlayerManager.Singleton.GetDefaultClassDefence().GetLocalizedObjName();
            classDefenceName.text = PlayerManager.Singleton.GetDefaultClassDefence().defence.ToString();
            classDefenceImgType.sprite = IconsDatabase.Singleton.GetArmorSpriteByDefenceType(PlayerManager.Singleton.GetDefaultClassDefence().defenseType);
            lightDefenceName.text = equip.equippedLightDefence.GetLocalizedObjName();
            lightDefenceValue.text = equip.equippedLightDefence.defence.ToString();
            balancedDefenceName.text = equip.equippedBalancedDefence.GetLocalizedObjName();
            balancedDefenceValue.text = equip.equippedBalancedDefence.defence.ToString();
            heavyDefenceName.text = equip.equippedHeavyDefence.GetLocalizedObjName();
            heavyDefenceValue.text = equip.equippedHeavyDefence.defence.ToString();

            firstAbilityName.text = PlayerManager.Singleton.GetPlayerAbilities()[0].GetAbilityLocalizedName();
            secondAbilityName.text = PlayerManager.Singleton.GetPlayerAbilities()[1].GetAbilityLocalizedName();
            thirdAbilityName.text = PlayerManager.Singleton.GetPlayerAbilities()[2].GetAbilityLocalizedName();
            fourthAbilityName.text = PlayerManager.Singleton.GetPlayerAbilities()[3].GetAbilityLocalizedName();

            btnCloseInfoPanel.onClick.RemoveAllListeners();
            btnCloseInfoPanel.onClick.AddListener(CloseInfoPanel);

            //Cambio immagine classe

            if (string.IsNullOrEmpty(PlayerManager.Singleton.selectedClass.name))
            {
                //caricato continua
                PlayerManager.Singleton.selectedClass.name = PlayerManager.Singleton.classPlayerTypeToString;
            }
            loadCharacterClassImages(PlayerManager.Singleton.selectedClass.name);
        }

        void OpenLegendPanel()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            PanelLegend.SetActive(true);
        }

        void CloseInfoPanel()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            infoPanel.gameObject.SetActive(false);
        }

        void loadCharacterClassImages(string className)
        {
            GameObject spriteImage = null;
            GameObject objBackgroundImage = null;
            Sprite classSprite = null;
            Sprite classBackground = null;

            switch (className)
            {
                case "Abbot":
                    break;
                case "BountyHunter":
                    classSprite = classSprites[1];
                    classBackground = classImagesBackground[1];
                    break;
                case "Crone":
                    classSprite = classSprites[2];
                    classBackground = classImagesBackground[2];
                    break;
                case "Trafficker":
                    break;
            }

            objBackgroundImage = GameObject.Find("ClassImageBackground");
            objBackgroundImage.GetComponent<RawImage>().texture = classBackground.texture;
            spriteImage = GameObject.Find("CharacterImage");
            spriteImage.GetComponent<Image>().sprite = classSprite;
        }

        void OnReturnInGameSession()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            string ID = PlayerManager.Singleton.currentPage.pageID;
            if (TutorialSystemManager.bringPlayerToChangeDefense)
            {
                TutorialSystemManager.bringPlayerToChangeDefense = false;
                LoadCorrectViewInTutorial(ID);
                return;
            }
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
            Notify(MVCEvents.LOAD_PAGE, ID);
        }
        public void OnClickEquipButton(bool openForTutorial = false)
        {
            Notify(MVCEvents.OPEN_EQUIP_VIEW);
            if (openForTutorial) return;
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
        }
        void OnClickInventory()
        {
            Notify(MVCEvents.OPEN_INVENTORY_VIEW);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
        }

        void LoadCorrectViewInTutorial(string pageID)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }
            app.model.currentView = app.view.BookView;
            app.model.currentView.gameObject.SetActive(true);
            GameApplication.Singleton.view.BookView.LoadPage(pageID);
            UIStatsManager.Singleton.UpdateGems();
        }
    }
}