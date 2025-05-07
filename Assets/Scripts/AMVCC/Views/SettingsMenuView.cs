using SmartMVC;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SettingsMenuView : View<GameApplication>
    {
        [SerializeField] Slider sliderMusicVolume;
        bool musicIsSilenced;
        [SerializeField] Slider sliderEffectVolume;
        bool effectIsSilenced;

        [SerializeField] Slider sliderUIVolume;

        [SerializeField] TMP_Dropdown dropdownFontChoice, dropdownFontForrButtonChoice;

        Slider GetMusicVolumeFromSettings { get => sliderMusicVolume; }
        Slider GetEffectVolumeFromSettings { get => sliderEffectVolume; }
        Slider GetUIVolumeFromSettings { get => sliderUIVolume; }

        public bool GetMusicIsSilenced { get => musicIsSilenced; }
        public bool GetEffectIsSilenced { get => effectIsSilenced; }


        [Header("UI")]
        [SerializeField] GameObject volumePanel;
        [SerializeField] GameObject preferencePanel;
        public TMP_Text txtPreview, txtButtonPreviewText;
        public TMP_Text txtBackgroundPreview;
        public Toggle toggleShowGems;
        public Toggle toggleShowBookmark;
        public Toggle toggleShowTutorialAnswer;
        public Toggle toggleActivePressedButton;
        public static bool TurnOnLanguageButton = true;

        [Header("Button")]
        public Button btnOpenVisualElementPanel;
        public Button btnOpenChangeFontPanel;
        //public Button btnOpenChangeBackgroundMode;
        [SerializeField] Button btnClosePanel;
        [SerializeField] Button btnClosePreferencesPanel;
        [SerializeField] Button btnCloseVisualPanel;
        [SerializeField] Button btnBack;
        [SerializeField] Button btnLanguage;
        [SerializeField] Button btnVolume;
        //[SerializeField] Button btnPreferences, btnHelp;


        [Header("FontMenu")]
        [SerializeField] GameObject fontMenuPanel;
        [SerializeField] Slider sliderFontSize, sliderButtonFontSize;
        [SerializeField] Button btnChangeFont;
        [SerializeField] Button btnGaramoundFont;
        [SerializeField] Button btnFontConfirm;

        [Header("BackgroundMenu")]
        [SerializeField] GameObject imgMenuPanel;
        [SerializeField] GameObject VisualElementPanel;
        [SerializeField] GameObject ChangeFontPanel;
        [SerializeField] RawImage imgTest;
        [SerializeField] RawImage imgTestBackground;
        [SerializeField] Slider sliderChangeDark;
        [SerializeField] Slider sliderColorBackground;
        [SerializeField] Button btnBackgroundConfirm;
        [SerializeField] Button btnFirstBackground;
        [SerializeField] Button btnSecondBackground;

        [Header("Sounds")]
        [SerializeField] AudioClip audioOpenLanguageMenu;
        [SerializeField] AudioClip audioVolumeMenu;
        [SerializeField] AudioClip audioBack;

        //PLAYERPREFSID
        const string SHOWTUTORIALREQUEST = "ShowTutorialRequest";
        public const string SHOWGEMS = "ShowGems";
        /// <summary>
        /// serve per aggiornare le gemme solo alla prima volta che il giocatore le riattiva
        /// </summary>
        public static bool UPDATEGEMS = false;
        const string SHOWBOOKMARK = "ShowBookmark";
        internal const string ACTIVEPRESSEDCLICK = "PressClick";
        public const string FONTSIZESCROLLVALUE = "FontScrollSize";
        public const string SELECTEDFONT = "SelectedFont";

        public const string FONTBUTTONSIZESCROLLVALUE = "FontButtonScrollSize";
        public const string SELECTEDBUTTONFONT = "SelectedButtonFont";
        public const string UIVOLUMEVALUE = "UIVolumeValue";
        public const string EFFECTVOLUMEVALUE = "EffectVolumeValue";
        public const string MUSICVOLUMEVAULE = "MusicVolumeValue";

        const float MINSCROLLFONTBTNSIZE = 0.45f;
        const float STARTERSCROLLFONTBTNSIZE = 0.55f;
        const float MAXSCROLLFONTBTNSIZE = 0.65f;
        const float MINSCROLLFONTSIZE = 0.55f;
        const float STARTERSCROLLFONTSIZE = 0.65f;
        const float MAXSCROLLFONTSIZE = 0.80f;

        const float STARTERMUSICVOLUMEVALUE = 0.10f;
        const float MAXMUSICVOLUMEVALUE = 0.25f;

        const float STARTEREFFECTVOLUMVALUE = 0.13f;
        const float MAXEFFECTVOLUMVALUE = 0.20f;

        const float STARTERUIVOLUMEVALUE = 0.20f;
        const float MAXUIVOLUMEVALUE = 0.30f;

        private void Start()
        {
            sliderMusicVolume.maxValue = MAXMUSICVOLUMEVALUE;
            sliderEffectVolume.maxValue = MAXEFFECTVOLUMVALUE;
            sliderUIVolume.maxValue = MAXUIVOLUMEVALUE;

            sliderButtonFontSize.minValue = MINSCROLLFONTBTNSIZE;
            sliderButtonFontSize.maxValue = MAXSCROLLFONTBTNSIZE;
            sliderFontSize.minValue = MINSCROLLFONTSIZE;
            sliderFontSize.maxValue = MAXSCROLLFONTSIZE;
        }

        void OnEnable()
        {
            if (TurnOnLanguageButton) { btnLanguage.gameObject.SetActive(true); }
            else { btnLanguage.gameObject.SetActive(false); }

            SetSavedSettings();

            fontMenuPanel.gameObject.SetActive(false);

            GetMusicVolumeFromSettings.onValueChanged.RemoveAllListeners();
            GetMusicVolumeFromSettings.onValueChanged.AddListener(OnClickMusicVolume);

            GetEffectVolumeFromSettings.onValueChanged.RemoveAllListeners();
            GetEffectVolumeFromSettings.onValueChanged.AddListener(OnClickEffectVolume);

            GetUIVolumeFromSettings.onValueChanged.RemoveAllListeners();
            GetUIVolumeFromSettings.onValueChanged.AddListener(OnClickUIVolume);

            btnLanguage.onClick.RemoveAllListeners();
            btnLanguage.onClick.AddListener(OnClickLanguage);

            sliderFontSize.onValueChanged.RemoveAllListeners();
            sliderFontSize.onValueChanged.AddListener(OnSliderFontSizeChanged);

            sliderButtonFontSize.onValueChanged.RemoveAllListeners();
            sliderButtonFontSize.onValueChanged.AddListener(OnSliderButtonFontSizeChanged);

            btnVolume.onClick.RemoveAllListeners();
            btnVolume.onClick.AddListener(OnClickVolume);

            btnClosePanel.onClick.RemoveAllListeners();
            btnClosePanel.onClick.AddListener(OnCloseVolumePanel);

            //btnPreferences.onClick.RemoveAllListeners();
            //btnPreferences.onClick.AddListener(OnClickPreferences);

            btnOpenVisualElementPanel.onClick.RemoveAllListeners();
            btnOpenVisualElementPanel.onClick.AddListener(OnClickOpenVisualPanel);

            btnCloseVisualPanel.onClick.RemoveAllListeners();
            btnCloseVisualPanel.onClick.AddListener(OnCloseVisualPanel);

            btnOpenChangeFontPanel.onClick.RemoveAllListeners();
            btnOpenChangeFontPanel.onClick.AddListener(OnClickChangeFontPanel);

            btnClosePreferencesPanel.onClick.RemoveAllListeners();
            btnClosePreferencesPanel.onClick.AddListener(OnClosePreferencePanel);

            if (PlayerManager.Singleton.selectedClass == null)
            { toggleShowGems.interactable = false; }
            else { toggleShowGems.interactable = true; }

            toggleShowGems.onValueChanged.RemoveAllListeners();
            toggleShowGems.onValueChanged.AddListener(delegate { OnGemsToggleValuedChanged(); });

            toggleShowBookmark.onValueChanged.RemoveAllListeners();
            toggleShowBookmark.onValueChanged.AddListener(delegate { OnBookmarkToggleValuedChanged(); });

            if (BookView.gameOpenedFromSettingsInGame)
            {
                toggleShowTutorialAnswer.gameObject.SetActive(false);
                //btnHelp.gameObject.SetActive(false);
            }
            else
            {
                //btnHelp.gameObject.SetActive(true);
                toggleShowTutorialAnswer.gameObject.SetActive(true);
                toggleShowTutorialAnswer.onValueChanged.RemoveAllListeners();
                toggleShowTutorialAnswer.onValueChanged.AddListener(delegate { OnTutorialAnswerToggleValuedChanged(); });
            }

            toggleActivePressedButton.onValueChanged.RemoveAllListeners();
            toggleActivePressedButton.onValueChanged.AddListener(delegate { OnTutorialInstanteClickToggleValuedChanged(); });

            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);

            //btnHelp.onClick.RemoveAllListeners();
            //btnHelp.onClick.AddListener(OnClickHelp);

            dropdownFontChoice.onValueChanged.RemoveAllListeners();
            dropdownFontChoice.onValueChanged.AddListener(OnSelectedNewFontFromDropdown);

            dropdownFontForrButtonChoice.onValueChanged.RemoveAllListeners();
            dropdownFontForrButtonChoice.onValueChanged.AddListener(OnSelectedNewButtonFontFromDropdown);
        }

        public void SetSavedSettings()
        {
            SetPreferenceToggles();
            SetDeafultFont();
            SetDeafultFontSize();
            SetDeafultButtonFont();
            SetDeafultButtonFontSize();
            SetMusicVolumeValue();
            SetEffectVolumeValue();
            SetUIVolumeValue();
        }

        public void SetPreferenceToggles()
        {
            if (PlayerPrefs.GetInt(SHOWTUTORIALREQUEST, 0) == 0)
            {
                toggleShowTutorialAnswer.GetComponent<Toggle>().isOn = true;
                ShowTutorialRequest = true;
            }
            else
            {
                toggleShowTutorialAnswer.GetComponent<Toggle>().isOn = false;
                ShowTutorialRequest = false;
            }

            if (PlayerPrefs.GetInt(ACTIVEPRESSEDCLICK, 0) == 0)
            {
                toggleActivePressedButton.GetComponent<Toggle>().isOn = true;
                ActivePressedClick = true;
            }
            else
            {
                toggleActivePressedButton.GetComponent<Toggle>().isOn = false;
                ActivePressedClick = false;
            }
        }


        /// <summary>
        /// La prima volta viene settata la grandezza dello 0.6 se non trova valori salvati
        /// </summary>
        void SetDeafultFontSize()
        {
            float fontValue = PlayerPrefs.GetFloat(FONTSIZESCROLLVALUE, STARTERSCROLLFONTSIZE);
            float sizeValue = fontValue * 100;
            sliderFontSize.value = fontValue;
            app.view.BookView.textPrefab.fontSize = sizeValue;
            app.view.Settings.txtPreview.fontSize = sizeValue;
        }

        void SetDeafultButtonFontSize()
        {
            float fontValue = PlayerPrefs.GetFloat(FONTBUTTONSIZESCROLLVALUE, STARTERSCROLLFONTBTNSIZE);
            float sizeValue = fontValue * 100;
            sliderButtonFontSize.value = fontValue;
            txtButtonPreviewText.fontSize = sizeValue;
            app.view.BookView.btnOptionPrefab.SetNewFontSize(sizeValue);
        }

        /// <summary>
        /// La prima volta viene settato il font di default se non trova valori salvati
        /// </summary>
        void SetDeafultFont()
        {
            int selectedFont = PlayerPrefs.GetInt(SELECTEDFONT, 0);
            dropdownFontChoice.value = selectedFont;
            TMP_FontAsset newFont = FontDatabase.Singleton.GetFontByIndex(selectedFont);
            app.view.BookView.textPrefab.font = newFont;
            txtPreview.font = newFont;
        }

        void SetDeafultButtonFont()
        {
            int selectedFont = PlayerPrefs.GetInt(SELECTEDBUTTONFONT, 0);
            dropdownFontForrButtonChoice.value = selectedFont;
            TMP_FontAsset newFont = FontDatabase.Singleton.GetButtonFontByIndex(selectedFont);
            app.view.BookView.btnOptionPrefab.SetFont(newFont);
            txtButtonPreviewText.font = newFont;
        }

        void OnClickMusicVolume(float newVolume)
        {
            SoundManager soundManager = app.model.BookModel.soundManager;
            PlayerPrefs.SetFloat(MUSICVOLUMEVAULE, newVolume);
            soundManager.audioSource.volume = newVolume; // cambio volume
            soundManager.maxVolume = newVolume; // cambio volume

            if (newVolume == 0)
            {
                musicIsSilenced = true;
            }
            else { musicIsSilenced = false; }
        }
        void SetMusicVolumeValue()
        {
            float volumeValue = PlayerPrefs.GetFloat(MUSICVOLUMEVAULE, STARTERMUSICVOLUMEVALUE);
            SoundManager.Singleton.ChangeVolume(volumeValue);
            sliderMusicVolume.value = volumeValue;
        }

        void OnClickEffectVolume(float newVolume)
        {
            SoundEffectManager soundEffectManager = app.model.BookModel.soundEffectManager;
            PlayerPrefs.SetFloat(EFFECTVOLUMEVALUE, newVolume);
            soundEffectManager.audioSource.volume = newVolume;
            soundEffectManager.maxVolume = newVolume;
            if (newVolume == 0)
            {
                effectIsSilenced = true;
            }
            else { effectIsSilenced = false; }
        }

        void SetEffectVolumeValue()
        {
            SoundEffectManager soundEffectManager = app.model.BookModel.soundEffectManager;
            float volumeValue = PlayerPrefs.GetFloat(EFFECTVOLUMEVALUE, STARTEREFFECTVOLUMVALUE);
            soundEffectManager.audioSource.volume = volumeValue;
            soundEffectManager.maxVolume = volumeValue;
            sliderEffectVolume.value = volumeValue;
        }

        void OnClickUIVolume(float newVolume)
        {
            PlayerPrefs.SetFloat(UIVOLUMEVALUE, newVolume);
            UISoundManager.Singleton.maxVolume = newVolume;
            UISoundManager.Singleton.audioSource.volume = newVolume;
            sliderUIVolume.value = newVolume;
        }

        void SetUIVolumeValue()
        {
            UISoundManager uISoundManager = app.model.BookModel.UISoundManager;
            float volumeValue = PlayerPrefs.GetFloat(UIVOLUMEVALUE, STARTERUIVOLUMEVALUE);
            uISoundManager.audioSource.volume = volumeValue;
            uISoundManager.maxVolume = volumeValue;
            sliderUIVolume.value = volumeValue;
        }

        void OnClickVolume()
        {
            volumePanel.gameObject.SetActive(true);
            UISoundManager.Singleton.PlayAudioClip(audioVolumeMenu);
        }

        void OnCloseVolumePanel()
        {
            volumePanel.gameObject.SetActive(false);
            UISoundManager.Singleton.PlayAudioClip(audioBack);
        }
        void OnClickPreferences()
        {
            preferencePanel.gameObject.SetActive(true);
            UISoundManager.Singleton.PlayAudioClip(audioVolumeMenu);
        }

        void OnClickOpenVisualPanel()
        {
            VisualElementPanel.gameObject.SetActive(true);
            UISoundManager.Singleton.PlayAudioClip(audioVolumeMenu);
        }

        void OnClickChangeFontPanel()
        {
            ChangeFontPanel.gameObject.SetActive(true);
            UISoundManager.Singleton.PlayAudioClip(audioVolumeMenu);
        }

        void OnClosePreferencePanel()
        {
            preferencePanel.gameObject.SetActive(false);
            UISoundManager.Singleton.PlayAudioClip(audioBack);
        }

        void OnCloseVisualPanel()
        {
            VisualElementPanel.gameObject.SetActive(false);
            UISoundManager.Singleton.PlayAudioClip(audioBack);
        }

        /// <summary>
        /// Causa bug devo fare con un numero dato che il toogle non funziona
        /// </summary>
        public static bool ShowTutorialRequest = true;
        void OnGemsToggleValuedChanged()
        {
            if (PlayerManager.Singleton.selectedClass == null) { return; }

            int showGems = toggleShowGems.isOn ? 0 : 1;

            PlayerPrefs.SetInt(SHOWGEMS, showGems);

            if (PlayerPrefs.GetInt(SHOWGEMS) == 0)
            {
                PlayerPrefs.SetInt(SHOWGEMS, showGems);
                UPDATEGEMS = true;
                app.view.BookView.objGems.gameObject.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt(SHOWGEMS, showGems);
                app.view.BookView.objGems.gameObject.SetActive(false);
            }
        }
        void OnBookmarkToggleValuedChanged()
        {
            int showBookmark = toggleShowBookmark.isOn ? 0 : 1;

            PlayerPrefs.SetInt(SHOWBOOKMARK, showBookmark);

            if (PlayerPrefs.GetInt(SHOWBOOKMARK) == 0)
            {
                PlayerPrefs.SetInt(SHOWBOOKMARK, showBookmark);
                app.view.BookView.objBookmark.gameObject.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt(SHOWBOOKMARK, showBookmark);
                app.view.BookView.objBookmark.gameObject.SetActive(false);
            }
        }
        void OnTutorialAnswerToggleValuedChanged()
        {
            int tutorialIsOn = toggleShowTutorialAnswer.isOn ? 0 : 1;

            PlayerPrefs.SetInt(SHOWTUTORIALREQUEST, tutorialIsOn);

            if (PlayerPrefs.GetInt(SHOWTUTORIALREQUEST) == 0)
            {
                ShowTutorialRequest = true;
                PlayerPrefs.SetInt(SHOWTUTORIALREQUEST, tutorialIsOn);
            }
            else
            {
                ShowTutorialRequest = false;
                PlayerPrefs.SetInt(SHOWTUTORIALREQUEST, tutorialIsOn);
            }
        }
        /// <summary>
        /// Se vera la pressione dei tasti è attiva
        /// </summary>
        public static bool ActivePressedClick;
        void OnTutorialInstanteClickToggleValuedChanged()
        {
            int activePressedButton = toggleActivePressedButton.isOn ? 0 : 1;

            PlayerPrefs.SetInt(ACTIVEPRESSEDCLICK, activePressedButton);

            if (PlayerPrefs.GetInt(ACTIVEPRESSEDCLICK) == 0)
            {
                ActivePressedClick = true;
                PlayerPrefs.SetInt(ACTIVEPRESSEDCLICK, activePressedButton);
            }
            else
            {
                ActivePressedClick = false;
                PlayerPrefs.SetInt(ACTIVEPRESSEDCLICK, activePressedButton);
            }
        }

        void OnSliderFontSizeChanged(float newSize)
        {
            Notify(MVCEvents.NEW_FONT_SIZE, newSize);
        }

        void OnSliderButtonFontSizeChanged(float newSize)
        {
            Notify(MVCEvents.NEW_FONT_BUTTON_SIZE, newSize);
        }

        void OnClickBack()
        {
            Notify(MVCEvents.SAVE_SETTINGS);
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
            UISoundManager.Singleton.PlayAudioClip(audioBack);

            if (AttentionPanel.openFromAttentionPanel) //Torna in gioco
            {
                AttentionPanel.openFromAttentionPanel = false;
                Notify(MVCEvents.LOAD_PAGE, PlayerManager.Singleton.currentPage.pageID);
            }
        }

        void OnClickHelp()
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.HelpView;
            app.model.currentView.gameObject.SetActive(true);
        }
        void OnClickLanguage()
        {
            app.view.LanguageView.openFromGame = true;
            Notify(MVCEvents.OPEN_LANGUAGE_VIEW);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
        }

        void OnSelectedNewFontFromDropdown(int selectedFontIndex)
        {
            Notify(MVCEvents.CHANGE_FONT, selectedFontIndex);
        }

        void OnSelectedNewButtonFontFromDropdown(int selectedFontButtonIndex)
        {
            Notify(MVCEvents.NEW_BUTTON_FONT, selectedFontButtonIndex);
        }

        void OnDarkAlphaChanged(float newSize)
        {
            Color tmpColor = imgTest.color;
            tmpColor.a = newSize;
            imgTest.color = tmpColor;
            app.model.Settings.imgShadow.color = tmpColor;
        }

        void OnColorBackgroundChanged(float newSize)
        {
            Color tmpColor = imgTestBackground.color;
            tmpColor.r = newSize;
            tmpColor.g = newSize;
            tmpColor.b = newSize;
            imgTestBackground.color = tmpColor;
            app.model.Settings.imgBackground.color = tmpColor;
        }

        void OnClickFirstBackground()
        {
            Color white = new Color(255, 255, 255, 255);
            app.view.BookView.textPrefab.color = white;
            txtBackgroundPreview.color = white;
            txtPreview.color = white;
        }
        void OnClickSecondBackground()
        {
            Color dark = new Color(0, 0, 0, 255);
            app.view.BookView.textPrefab.color = dark;
            txtBackgroundPreview.color = dark;
            txtPreview.color = dark;
        }
    }
}