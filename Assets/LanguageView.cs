/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;
using UnityEngine.Rendering;

namespace Game
{
    public class LanguageView : View<GameApplication>
    {
        public Button btnBack, btnConfirmEngLanguage, btnConfirmItalian, btnConfirmFrench, btnConfirmSpanish, btnConfirmPortoghês,
              btnConfirmGerman, btnConfirmJapanese;
        public Button btnShowEnglish, btnShowItalian, btnShowFrench, btnShowSpanish, btnShowPortoghês, btnShowGerman, btnShowJapanese;
        public TMP_Text txtEnglish, txtItalian, txtFrench, txtSpanish, txtPortogh, txtGerman, txtJapanese;
        public GameObject UISelectedItalian, UISelectedEnglish, UISelectedFrench, UISelectedSpanish, UISelectedPortoghês, UISelectedGerman, UISelectedJapanese;

        public bool alreadyChoice;
        public bool openFromGame;

        [Header("Sounds")]
        [SerializeField] AudioClip audioConfirmLanguage;

        [SerializeField] Color colorLessAlpha, colorActive; 

        void Start()
        {
            CheckFirstGame();
            colorLessAlpha = txtEnglish.color;
            colorActive = txtItalian.color;
        }

        private void OnEnable()
        {
            btnBack.gameObject.SetActive(false);

            string deviceLanguage = Application.systemLanguage.ToString();
            CheckCurrentLanguage(deviceLanguage);

            // Rimuovere gli ascoltatori precedenti
            btnShowEnglish.onClick.RemoveAllListeners();
            btnShowItalian.onClick.RemoveAllListeners();
            btnShowFrench.onClick.RemoveAllListeners();
            btnShowSpanish.onClick.RemoveAllListeners();
            btnShowPortoghês.onClick.RemoveAllListeners();
            btnShowGerman.onClick.RemoveAllListeners();
            btnShowJapanese.onClick.RemoveAllListeners();
            btnConfirmEngLanguage.onClick.RemoveAllListeners();
            btnConfirmItalian.onClick.RemoveAllListeners();
            btnConfirmFrench.onClick.RemoveAllListeners();
            btnConfirmSpanish.onClick.RemoveAllListeners();
            btnConfirmPortoghês.onClick.RemoveAllListeners();
            btnConfirmGerman.onClick.RemoveAllListeners();
            btnConfirmJapanese.onClick.RemoveAllListeners();
            btnBack.onClick.RemoveAllListeners();

            // Aggiungere nuovi ascoltatori per i pulsanti
            btnBack.onClick.AddListener(OnClickBack);

            // Creare una lista di tutte le lingue con i rispettivi metodi
            var languages = new (Button button, string language)[]
            {
                (btnShowEnglish, "English"),
                (btnShowItalian, "Italiano"),
                (btnShowFrench, "Français"),
                (btnShowSpanish, "Español"),
                (btnShowPortoghês, "Portoghês"),
                (btnShowGerman, "Deutsch"),
                (btnShowJapanese, "日本語")
            };

            // Aggiungi gli ascoltatori per tutti i pulsanti
            foreach (var lang in languages)
            {
                lang.button.onClick.AddListener(() => OnClickShowLanguage(lang.language));
            }

            // Aggiungi gli ascoltatori per i pulsanti di conferma
            btnConfirmEngLanguage.onClick.AddListener(delegate { OnConfirm("English"); });
            btnConfirmItalian.onClick.AddListener(delegate { OnConfirm("Italiano"); });
            btnConfirmFrench.onClick.AddListener(delegate { OnConfirm("Français"); });
            btnConfirmSpanish.onClick.AddListener(delegate { OnConfirm("Español"); });
            btnConfirmPortoghês.onClick.AddListener(delegate { OnConfirm("Portoghês"); });
            btnConfirmGerman.onClick.AddListener(delegate { OnConfirm("Deutsch"); });
            btnConfirmJapanese.onClick.AddListener(delegate { OnConfirm("日本語"); });

            if (openFromGame)
            {
                btnBack.gameObject.SetActive(true);
            }
        }


        void CheckCurrentLanguage(string currentLanguage)
        {
            ColorWithNoEvidenceColor();
            ShowButton(currentLanguage);

            switch (currentLanguage)
            {
                case LocalizationIDDatabase.ENGLISH:
                    txtEnglish.color = colorActive;
                    UISelectedEnglish.gameObject.SetActive(true);
                    break;
                case LocalizationIDDatabase.ITALIAN:
                    txtItalian.color = colorActive;
                    UISelectedItalian.gameObject.SetActive(true);
                    break;
                case LocalizationIDDatabase.FRENCH:
                    txtFrench.color = colorActive;
                    UISelectedFrench.gameObject.SetActive(true);
                    break;
                case LocalizationIDDatabase.SPANISH:
                    txtSpanish.color = colorActive;
                    UISelectedSpanish.gameObject.SetActive(true);
                    break;
                case LocalizationIDDatabase.PORTOGUESE:
                    txtPortogh.color = colorActive;
                    UISelectedPortoghês.gameObject.SetActive(true);
                    break;
                case LocalizationIDDatabase.GERMAN:
                    txtGerman.color = colorActive;
                    UISelectedGerman.gameObject.SetActive(true);
                    break;
                case LocalizationIDDatabase.JAPANESE:
                    txtJapanese.color = colorActive;
                    UISelectedJapanese.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        void OnClickShowLanguage(string language)
        {
            ShowButton(language);
            OnValueChanged(language);

            // Reset color per tutte le lingue
            ColorWithNoEvidenceColor();

            // Imposta il colore attivo per la lingua selezionata
            switch (language)
            {
                case "English":
                    txtEnglish.color = colorActive;
                    break;
                case "Italiano":
                    txtItalian.color = colorActive;
                    break;
                case "Français":
                    txtFrench.color = colorActive;
                    break;
                case "Español":
                    txtSpanish.color = colorActive;
                    break;
                case "Portoghês":
                    txtPortogh.color = colorActive;
                    break;
                case "Deutsch":
                    txtGerman.color = colorActive;
                    break;
                case "日本語":
                    txtJapanese.color = colorActive;
                    break;
            }

            UISoundManager.Singleton.PlayAudioClip(app.Sounds.ConfirmDifficultButton);
        }

        void ColorWithNoEvidenceColor()
        {
            txtEnglish.color = colorLessAlpha;
            txtItalian.color = colorLessAlpha;
            txtFrench.color = colorLessAlpha;
            txtSpanish.color = colorLessAlpha;
            txtPortogh.color = colorLessAlpha;
            txtGerman.color = colorLessAlpha;
            txtJapanese.color = colorLessAlpha;
        }

        void CheckFirstGame()
        {
            openFromGame = false;

            if (alreadyChoice)
            {
                CheckClick();
                gameObject.SetActive(false);
            }
        }

        void OnValueChanged(string languageIndex)
        {
            Localization.language = languageIndex;
        }

        void OnConfirm(string languageIndex)
        {
            switch (languageIndex)
            {
                case "Italiano":
                    PlayerManager.Singleton.selectedLanguage = PlayerManager.SelectedLanguage.Italiano;
                    break;
                case "English":
                    PlayerManager.Singleton.selectedLanguage = PlayerManager.SelectedLanguage.Inglese;
                    break;
                case "Français":
                    PlayerManager.Singleton.selectedLanguage = PlayerManager.SelectedLanguage.Francese;
                    break;
                case "Español":
                    PlayerManager.Singleton.selectedLanguage = PlayerManager.SelectedLanguage.Spagnolo;
                    break;
                case "Portoghês":
                    PlayerManager.Singleton.selectedLanguage = PlayerManager.SelectedLanguage.Portoghese;
                    break;
                case "Deutsch":
                    PlayerManager.Singleton.selectedLanguage = PlayerManager.SelectedLanguage.Tedesco;
                    break;
                case "日本語":
                    PlayerManager.Singleton.selectedLanguage = PlayerManager.SelectedLanguage.Giapponese;
                    break;
            }

            Localization.language = languageIndex;
            CheckClick();
            UISoundManager.Singleton.PlayAudioClip(audioConfirmLanguage);
        }

        void ShowButton(string language)
        {
            UISelectedItalian.gameObject.SetActive(language == "Italiano");
            UISelectedEnglish.gameObject.SetActive(language == "English");
            UISelectedFrench.gameObject.SetActive(language == "Français");
            UISelectedSpanish.gameObject.SetActive(language == "Español");
            UISelectedPortoghês.gameObject.SetActive(language == "Portoghês");
            UISelectedGerman.gameObject.SetActive(language == "Deutsch");
            UISelectedJapanese.gameObject.SetActive(language == "日本語");
        }

        void OnClickBack()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

        void CheckClick()
        {
            Notify(MVCEvents.OPEN_MAINMENU_VIEW);
        }


    }
}
