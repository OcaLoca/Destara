/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using DG.Tweening;
using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class AnalyzeCharacterManager : MonoBehaviour
    {
        public static AnalyzeCharacterManager Singleton { get; set; }
        /// <summary>
        /// Nome del personaggio analizzato
        /// </summary>
        [SerializeField] TMP_Text lblCharacterName;
        /// <summary>
        /// Storia del personaggio analizzato
        /// </summary>
        [SerializeField] TMP_Text lblCharacterDescription;
        /// <summary>
        /// Valori del personaggio analizzato
        /// </summary>
        [SerializeField] TMP_Text lblCharacterLevel;
        [SerializeField] TMP_Text lblCharacterLife;
        [SerializeField] TMP_Text lblCharacterDefencesNumber;
        [SerializeField] TMP_Text lblCharacterAbilitiesNumber;
        [SerializeField] TMP_Text lblCharacterAttackValue;
        [SerializeField] TMP_Text lblCharacterAttackType;
        [SerializeField] TMP_Text lblCharacterDefenceValue;
        [SerializeField] TMP_Text lblCharacterDefenceType;
        [SerializeField] TMP_Text lblCharacterThreat;
        [SerializeField] TMP_Text lblCharacterDangerous;
        //[SerializeField] Image sprCharacterDefenceIcon;

        public const int LBLCHARACHTERVALUESCOUNT = 9;
        /// <summary>
        /// Immagine del personaggio analizzato
        /// </summary>
        [SerializeField] SpriteRenderer sprCharacterSprite;
        /// <summary>
        /// Immagine di sfondo del personaggio analizzato
        /// </summary>
        [SerializeField] RawImage sprBackgroundSprite;
        /// <summary>
        /// Animazione Idle del pg
        /// </summary>
        [SerializeField] Animator animCharacter;
        [SerializeField] Button btnClosePanel;

        //[SerializeField] GameObject storyContainer;
        //[SerializeField] GameObject valueContainer;
        [SerializeField] Button btnChangeContainer;
        [SerializeField] ScrollRect verticalScroolbar;


        private void Awake()
        {
            Singleton = this;
        }

        private void OnEnable()
        {
            btnClosePanel.onClick.RemoveAllListeners();
            btnClosePanel.onClick.AddListener(delegate
            {
                verticalScroolbar.verticalNormalizedPosition = 1f;
                GameApplication.Singleton.view.BookView.panelAnalyzeCharachter.SetActive(false);
            });
        }

        Vector3 defaultScale = new Vector3(210, 210, 1);
        public void SetAnalyzePage(string lblCharacterName, string lblCharacterDescription, Sprite sprCharacterSprite, Sprite sprBackgroundSprite, int storyContentHeight = 1600, Vector3? scaleSize = null, bool isEnemyFight = false, RuntimeAnimatorController animCharacter = null)
        {
            if (scaleSize == null)
            {
                scaleSize = defaultScale;
            }

            if (lblCharacterDescription == null)
            {
                lblCharacterDescription = "???";
            }

            this.lblCharacterName.text = Localization.Get(lblCharacterName);
            this.lblCharacterDescription.text = Localization.Get(lblCharacterDescription);
            RectTransform heightSize = this.lblCharacterDescription.GetComponent<RectTransform>();
            heightSize.sizeDelta = new Vector2(heightSize.sizeDelta.x, storyContentHeight);
            this.sprCharacterSprite.sprite = sprCharacterSprite;
            this.sprCharacterSprite.GetComponent<RectTransform>().localScale = (Vector3)scaleSize;
            this.sprBackgroundSprite.texture = sprBackgroundSprite.texture;

            if (animCharacter != null)
            {
                this.animCharacter.runtimeAnimatorController = animCharacter;
            }
        }

        public const string EASY = "easy";
        public const string HARD = "hard";
        public const string FRIENDLY = "friendly";
        public const string THREATENING = "threatening";

        public void SetBattleValue(string lblCharacterThreat = "??", string lblCharacterLife = "???", int lblCharacterLevel = 1,
                string lblCharacterDefencesNumber = "???", string lblCharacterAttackValue = "???", string lblCharacterAbilitiesNumber = "???",
                string lblCharacterAttackType = "???", string lblCharacterDefenceType = "???", string lblCharacterDefenceValue = "???",
                string lblCharacterDangerous = "???", Sprite sprCharacterDefenceIcon = null)
        {

            this.lblCharacterLevel.text = UIUtility.GetLevelForUI(lblCharacterLevel);
            //this.sprCharacterDefenceIcon.sprite = sprCharacterDefenceIcon == null ? IconsDatabase.Singleton.GetArmorSpriteByDefenceType(TypeDatabase.DefenseType.Light) : sprCharacterDefenceIcon;
            this.lblCharacterThreat.text = Localization.Get(lblCharacterThreat);
            this.lblCharacterDangerous.text = Localization.Get(lblCharacterDangerous);
            this.lblCharacterLife.text = lblCharacterLife;
            this.lblCharacterAttackType.text = Localization.Get(lblCharacterAttackType);
            this.lblCharacterDefenceType.text = Localization.Get(lblCharacterDefenceType);
            this.lblCharacterDefenceValue.text = lblCharacterDefenceValue;
            this.lblCharacterAttackValue.text = lblCharacterAttackValue;
            this.lblCharacterDefencesNumber.text = lblCharacterDefencesNumber;
            this.lblCharacterAbilitiesNumber.text = lblCharacterAbilitiesNumber;

            if (lblCharacterThreat == FRIENDLY)
            {
                this.lblCharacterThreat.color = ColorDatabase.Singleton.luckyColor; //green
            }
            else
                this.lblCharacterThreat.color = ColorDatabase.Singleton.supertitionColor;

            if (lblCharacterDangerous == EASY)
            {
                this.lblCharacterDangerous.color = ColorDatabase.Singleton.luckyColor;
            }
            else if (lblCharacterDangerous == HARD)
            {
                this.lblCharacterDangerous.color = ColorDatabase.Singleton.rareColor;
            }
            else
                this.lblCharacterDangerous.color = ColorDatabase.Singleton.supertitionColor;

        }

        private void OnDisable()
        {
            RefreshMyUIElement();
        }

        public void RefreshMyUIElement()
        {
            lblCharacterDescription.text = string.Empty;
            lblCharacterName.text = string.Empty;
        }
    }

}
