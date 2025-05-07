/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */

using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class MainMenuView : View<GameApplication>
    {
        public Button btnContinue;
        public Button btnNewReading;
        //[SerializeField, Autohook(Context.Child, Visibility.Visible)]
        public Button btnCuriosity;
        public Button btnLibrary;
        public Button btnOptions;
        public TMP_Text lblContinue;
        public TMP_Text lblNewGame;
        public TMP_Text lblCuriosity;
        public TMP_Text lblOption;


        [Header("Sounds")]
        [SerializeField] AudioClip newGameAudio;
        [SerializeField] AudioClip continueGameAudio;
        [SerializeField] AudioClip curiosityGameAudio;
        [SerializeField] AudioClip optionGameAudio;


        public bool noSavedSlot;

        private void Start()
        {
            app.view.GoalsView.SetupGoalView();
        }

        void OnEnable()
        {
            app.view.Settings.btnOpenVisualElementPanel.gameObject.SetActive(false);

            if (FirstView.comeFromFirstView)
            {
                FirstView.comeFromFirstView = false;
            }

            if (!SaveSystemUtilities.AlreadySave() || PlayerPrefs.GetInt(DemoView.RUN_FINISHED) == 1)
            {
                btnContinue.interactable = false;
                lblContinue.CrossFadeAlpha(0.5f, 0f, false);
            }
            else
            {
                lblContinue.CrossFadeAlpha(1f, 0f, false);
                btnContinue.interactable = true;
            }

            Debug.LogWarning("RICORDARSI DI ELIMINARE LA RIGA 77 QUANDO TORNA DISPONIBILE IL TASTO CONTINUA");
            PlayerManager.Singleton.RefreshPlayerManager();

            btnContinue.onClick.RemoveAllListeners();
            btnContinue.onClick.AddListener(OnClickContinue);

            btnNewReading.onClick.RemoveAllListeners();
            btnNewReading.onClick.AddListener(OnClickNewGame);

            btnCuriosity.onClick.RemoveAllListeners();
            btnCuriosity.onClick.AddListener(OnClickCuriosity);

            btnOptions.onClick.RemoveAllListeners();
            btnOptions.onClick.AddListener(OnClickSettings);

            btnLibrary.onClick.RemoveAllListeners();
            btnLibrary.onClick.AddListener(OnClickLibrary);
        }

        void OnClickContinue()
        {
            Notify(MVCEvents.MAIN_START_BUTTON_CLICKED);
            UISoundManager.Singleton.PlayAudioClip(continueGameAudio);
        }

        void OnClickNewGame()
        {
            Notify(MVCEvents.OPEN_NEW_GAME_VIEW);
            UISoundManager.Singleton.PlayAudioClip(newGameAudio);
        }

        void OnClickCuriosity()
        {
            Notify(MVCEvents.OPEN_CURIOSITY_VIEW);
            UISoundManager.Singleton.PlayAudioClip(curiosityGameAudio);
        }

        void OnClickSettings()
        {
            SettingsMenuView.TurnOnLanguageButton = true;
            Notify(MVCEvents.OPEN_SETTING);
            UISoundManager.Singleton.PlayAudioClip(optionGameAudio);
        }

        void OnClickLibrary()
        {
            Notify(MVCEvents.OPEN_LIBRARY);
            UISoundManager.Singleton.PlayAudioClip(optionGameAudio);
        }

        const string URL = "https://www.mobybit.it/";
        void OnClickMobyBit()
        {
            Application.OpenURL(URL);
            UISoundManager.Singleton.PlayAudioClip(optionGameAudio);
        }
    }
}