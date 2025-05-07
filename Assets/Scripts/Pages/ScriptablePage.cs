using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Page", menuName = "ScriptableObjects/ScriptablePage", order = 0)]
    public class ScriptablePage : ScriptableObject
    {

        #region PageDecorationFeature
        [SerializeField] private bool decorationFadeIn;
        [SerializeField] private bool fadeInFromZero = false;
        [SerializeField] private bool haveCheckpoint;
        public bool HaveCheckpoint { get => haveCheckpoint; }
        public bool GetDecorationFadeIn { get => decorationFadeIn; }

        public bool GetFadeInFromZero { get => fadeInFromZero; }
        #endregion

        [Header("PageFeature")]
        public Section chapterSection;
        public ScriptableRiddle scriptableRiddle;
        public string pageID;
        public bool changeByTime;
        public bool beforeTitle;
        public bool writeThisTxtUnderButton;
        public bool pageOpenDemoView;
        public bool HaveAnalyzeButton;

        [Header("DeathController")]
        public bool playerDead;
        public bool pageBeforeDeath;

        [Header("TitlePage")]
        public string txtTitle;
        public string txtSubtitle;
        public Title title;
        public enum Title
        {
            AllaSalute,
            InVinoVeritas,
            Memorie,
            Scappa,
            DuraLex,
            Oscurità
        }


        [Header("PageUI")]
        public string subtitleID;
        public string chapterNumber;
        public string romanPageNumber;


        [Header("WriteElement")]
        public string randomString;
        public bool containPlayerName;

        [Header("DropInPage")]
        public bool clickAndDrop;
        public bool dropped = false;
        public bool containGenericDrop;
        public bool containSpecificDrop
        {
            get
            {

                //return drop.Count > 0; 

                if (dropped == true || searchDrop.Count == 0)
                {
                    return false;
                }
                else
                {
                    return searchDrop.Count > 0;
                }
            }
        }
        [Header("BattleRewards")]
        public ScriptableItem battleDropReward;
        public float battleXPGain;

        [Serializable]
        public struct StatsVariation
        {
            public Sprite iconSprite;
            public StatsVariatonLocalizedID iconName;
            public int buffDebuffAmount;
        }
        [Header("ChangeStats")]
        public StatsVariation [] statsVariations;

        public enum StatsVariatonLocalizedID
        {
            Lucky,
            Courage,
            Superstition,
            Life
        }

        [Header("SaveSystem")]
        [SerializeField] bool dontSave;
        public bool saveOnOpenPage;
        public bool GetNotSave { get => dontSave; }


        [Header("SoundElement")]
        [SerializeField] private string effectAudioID;
        [SerializeField] private bool haveEffectAudio;
        public bool HaveEffectAudio { get => haveEffectAudio; }
        public string GetEffectAudioID { get => effectAudioID; }
        public float effectAudioVolume;
        public bool effectWithLoop;
        public float effectStartDelay;
        public SoundBackground GetSoundtrackSection { get => soundBackground; }
        [SerializeField] private SoundBackground soundBackground;
        public enum SoundBackground
        {
            NoMusic,
            AllaSaluteSoundtrack,
            ScappaSoundtrack,
            InVinoVeritasSoundtrack,
            GenericFightSoundtrack,
            GenericTitle,
            GenericEscapeRoomSoundtrack,
            Death
        }

        [Serializable]
        public struct LocalizationValues
        {
            public bool containPlayerName;
            public bool containClass;
            public bool isAbbot;
            public bool isCrone;
            public bool isHunter;
            public bool isTrafficker;
            public bool containARandomString;
        }
        public LocalizationValues localizationValues;


        [Serializable]
        public struct PageBuff
        {
            public int RecoverAP;
            public int buffLucky;
            public int buffCourage;
            public int buffSuperstition;
            public float buffConstitution;
            public float buffDexterity;
            public float buffStrength;
            public float buffInteligence;
            public float buffLifepoint;
            public float buffLevel;
            public float defence;
            public BuffType buffType;
        }
        /// <summary>
        /// Il buff viene applicato al caricamento della pagina 
        /// </summary>
        public bool buffAppliedOnCurrentPage;

        public PageBuff[] pageBuffes;

        public enum BuffType
        {
            Lucky,
            Courage,
            Superstition,
            Constitution,
            Dexterity,
            Level,
            LifePoint,
            Strength,
            Inteligence,
            AP
        }

        [Serializable]
        public struct PageFeatures
        {
            public bool showAnHideChoice;
            public bool lockedChoice;
            public bool hideClickedButton;
            public bool containRiddle;
            public bool onlyImage;
            public bool unlockMuseumImage;
            public bool isEscapeRoom;
            public string unlockImageID;
            [Tooltip("Il tasto si sblocca se hai schiacciato un'altro tasto, 'Button to click'")]
            public bool unlockByClickedButton;
        }
        public PageFeatures pageFeatures;

        public LoadDifferentID loadDifferentID;
        public enum LoadDifferentID
        {
            None,
            Lucky,
            Superstition,
            LuckyAndSuperstition,
            Difficult,
            Casual
        }

        public enum ShowHideButtonType
        {
            enoughCourage,
            readedPages
        }
        public ShowHideButtonType showHideButtonRequirementsType;

        [Serializable]
        public struct ShowHideButtonConditions
        {
            public int courageToUnlock;
            public List<string> pageToUnlockShowBtn;
            public int numberPageToUnlock;
        }
        public ShowHideButtonConditions showHideButtonConditions;

        public enum LockedChoiceType
        {
            toLowCourage,
            noRightItem,
            noReadPage,
            workingInprogress,
            waitingForPlayerInput
        }
        public LockedChoiceType lockedChoiceType;

        [Serializable]
        public struct LockedChoiceConditions
        {
            public int courageToUnlock;
            public string pageToRead;
            public ScriptableItem itemToUnlock;
        }
        public LockedChoiceConditions lockedChoiceConditions;

        [Serializable]
        public struct ChoiceButtonFeatures
        {
            public string buttonText;
            public string pageID;
            public string highSuperstitionPageID, minSuperstitionPageID;
            public int minAmountSuperstition;
            public string LuckyPageID;
            public string CasualPageID;
            public int minAmountLucky;
            public string noCourageText;
            public string cowardDifficult;
            public string fearlessDifficult;
            public string insaneDifficult;
            public string[] buttonToClick;
            public string playerClickedTheButton;
            public string afterRepeatThisPageID;
            const string ANALYZEBUTTONID = "ANALYZECHARACTER";
            [Tooltip("ID UNIVOCO PER QUANDO HAI UN TASTO CHE ANALIZZA")]
            public string GetAnalyzeButtonID { get => ANALYZEBUTTONID; }
            [Tooltip("TESTO UNIVOCO PER QUANDO HAI UN TASTO CHE ANALIZZA")]
            const string ANALYZEBTNTEXT = "Analizza";
            public string GetAnalyzeButtonText { get => ANALYZEBTNTEXT; }
            [Tooltip("Se true carica la UI dei cambi delle stats del giocatore")]
            public bool loadStatsChangedPanel;
            public bool onlyOnce;
            public bool hideButton;
            public bool lockedButton;
            public bool attackButton, deathButton;
            public bool btnLoadDifferentPageForSex, btnLoadDifferentPageForClass;
            [Tooltip("Il tasto ti da reward se hai un oggetto KEYID nell'inventario o la pagina dona per la lettura")]
            public bool clickAndDrop;
            [Tooltip("L'oggetto che serve per sbloccare il tasto")]
            public string keyItemID;
            public bool changeIDByButtonClicked;
            [Tooltip("Se vero al click apre la schermata di analisi")]
            public bool analyzeButton;

            public float buffDebuffLifePointCurrentPage;
            public float buffDebuffExperienceCurrentPage;

            public AudioClip audio;
            public Color32 borderColor;
            public LoadDifferentID loadDifferentID;
        }
        public ChoiceButtonFeatures[] choicesButtons;

        [Serializable]
        public struct AnalyzeComponent
        {
            public string lblCharacterDescription;
            public int heightContentCharacterDescription;
            public string lblCharacterName;
            public Sprite sprCharacterSprite;
            public Sprite sprBackgroundSprite;
            public Sprite DefenceIcon;
            public Vector3 scaleSize;
            public bool isEnemyFight;

            public int lblCharacterLevel;
            public string lblCharacterLife;
            public string lblCharacterDefencesNumber;
            public string lblCharacterAbilityNumber;
            public string lblCharacterAttackValue;
            public string lblCharacterAttackType;
            public string lblCharacterDefenceValue;
            public string lblCharacterDefenceType;

            public Threat threat;
            public Difficult difficult;
        }
        public AnalyzeComponent analyzeComponentStruct;

        public enum Threat
        {
            friendly,
            threatening
        }
        public enum Difficult
        {
            easy,
            hard,
            impossible
        }

        [Serializable]
        public struct WinOrLoseFightID
        {
            public string winFightID;
            public string loseFightID;
            public string anotherWinFightID;
            public string anotherLoseFightID;
        }
        public WinOrLoseFightID winOrLoseFightID;

        public bool repeatSamePage
        {
            get { return repeatThisPage > 0; }
        }

        public int repeatThisPage;

        public bool removeClickedBtn
        {
            get { return btnsToRemove > 0; }
        }

        public int btnsToRemove;

        public bool containFight
        {
            get { return mobID.Count > 0; }
        }

        [Header("FightPage")]

        public string introLogTxtID;
        public List<ScriptableEnemy> easyDifficultEnemy;
        public List<ScriptableEnemy> mediumDifficultEnemy;
        public List<ScriptableEnemy> hardDifficultEnemy;
        public List<ScriptableEnemy> mobID;

        public bool haveAlly
        {
            get { return allyID.Count > 0; }
        }

        public List<ScriptableEnemy> allyID;


        public enum Section
        {
            Domande,
            Nulla,
            AllaSalute,
            Scappa,
            InVinoVeritas,
            DuraLex,
            Oscurità,
            Memorie,
            Catacombe,
            Monastero,
            Porto,
            Nave,
            Lebbrosario,
            NonUsare,
            Titolo,
            Fight,
            Death
        }
        public List<ScriptableItem> searchDrop;
        public List<ScriptableItem> readAndDrop;
        public List<int> readAndDropQuantity;
        public List<ScriptableItem> CowardItemList;
        public List<ScriptableItem> FearlessItemList;
        public List<ScriptableItem> InsaneItemList;

        internal List<ScriptableItem> GetItemListByDifficulty(PlayerManager.Difficulty difficulty)
        {
            switch (difficulty)
            {
                case PlayerManager.Difficulty.Coward:
                    return CowardItemList;

                case PlayerManager.Difficulty.Fearless:
                    return FearlessItemList;

                case PlayerManager.Difficulty.Insane:
                    return InsaneItemList;

                default:
                    Debug.LogWarning("Nessuna difficoltà selezionata o difficoltà non valida.");
                    return new List<ScriptableItem>(); // Restituisce una lista vuota se la difficoltà non è impostata
            }
        }

    }
}
