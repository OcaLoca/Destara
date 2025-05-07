/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;


namespace Game
{
    public class NewGameView : View<GameApplication>
    {
        public Button btnCowardDifficulty;
        public Button btnFearlessDifficulty;
        public Button btnInsaneDifficulty;

        public TMP_Text txtCoward;
        public TMP_Text txtFearless;
        public TMP_Text txtInsane;

        public Button btnConfirmSelection;
        public Button btnBack;
        public TMP_Text lblDifficultyDescription;
        public GameObject pnlOverwrite;
       
        public Button btnYesOverwrite;
        public Button btnNoOverwrite;
        public RawImage crossFadePanel;

        [Header("UI")]
        [SerializeField] GameObject skullContainer;
        [SerializeField] Image[] skullImages = new Image[3];
        [SerializeField] GameObject[] highLineAnimators = new GameObject[3];
        public GameObject GetSkullContainer { get => skullContainer; }
        public GameObject[] GetHighLineAnimators { get => highLineAnimators; }
        public Image[] GetBlackSkulls { get => skullImages; }


        const string TUTORIALURL = "https://www.youtube.com/watch?v=qn713CwrkRc&list=RDMMqn713CwrkRc&start_radio=1";

        [Header("Sounds")]
        [SerializeField] AudioClip audioSelectCoward;
        [SerializeField] AudioClip audioSelectFearless;
        [SerializeField] AudioClip audioSelectInsane;
        [SerializeField] AudioClip audioConfirm;
        [SerializeField] AudioClip audioTurnBack;
        [SerializeField] AudioClip audioStartTutorial;
        [SerializeField] AudioClip audioNoTutorial;

        void OnEnable()
        {
            //if (SaveSystem.LoadPlayer(SaveType.Soft, null, null) != null) {  }

            RefreshUI();

            app.model.currentView = this;

            btnCowardDifficulty.onClick.RemoveAllListeners();
            btnCowardDifficulty.onClick.AddListener(OpenCowardInfoDifficulty);

            btnFearlessDifficulty.onClick.RemoveAllListeners();
            btnFearlessDifficulty.onClick.AddListener(OpenFearlessInfoDifficulty);

            btnInsaneDifficulty.onClick.RemoveAllListeners();
            btnInsaneDifficulty.onClick.AddListener(OpenInsaneInfoDifficulty);

            btnConfirmSelection.onClick.RemoveAllListeners();
            btnConfirmSelection.onClick.AddListener(ConfirmSelection);

            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnBackClick);
        }

        Color colorAlphaMore = new Color();
        Color colorAlphaLess = new Color(255, 255, 255, 0.5f);

        void RefreshUI()
        {
            foreach (Image skullImage in GetBlackSkulls) { skullImage.enabled = false; }
            foreach (GameObject lineAnimator in GetHighLineAnimators) { lineAnimator.gameObject.SetActive(false); }

            colorAlphaMore = lblDifficultyDescription.color;

            txtCoward.color = colorAlphaLess;
            txtFearless.color = colorAlphaLess;
            txtInsane.color = colorAlphaLess;

            lblDifficultyDescription.text = string.Empty;

            crossFadePanel.canvasRenderer.SetAlpha(0.0f);
            btnConfirmSelection.interactable = false;
            btnConfirmSelection.GetComponent<Animator>().enabled = false;
        }

        void OpenCowardInfoDifficulty()
        {
            txtCoward.color = colorAlphaMore;
            txtFearless.color = colorAlphaLess;
            txtInsane.color = colorAlphaLess;
            Notify(MVCEvents.CHANGE_LAST_SELECTED_DIFFICULTY, PlayerManager.Difficulty.Coward);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.CowardButton);
        }

        void OpenFearlessInfoDifficulty()
        {
            txtCoward.color = colorAlphaLess;
            txtFearless.color = colorAlphaMore;
            txtInsane.color = colorAlphaLess;
            Notify(MVCEvents.CHANGE_LAST_SELECTED_DIFFICULTY, PlayerManager.Difficulty.Fearless);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.FearlessButton);
        }

        void OpenInsaneInfoDifficulty()
        {
            txtCoward.color = colorAlphaLess;
            txtFearless.color = colorAlphaLess;
            txtInsane.color = colorAlphaMore;
            Notify(MVCEvents.CHANGE_LAST_SELECTED_DIFFICULTY, PlayerManager.Difficulty.Insane);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.InsaneButton);
        }

        void ConfirmSelection()
        {
            //RefreshUI();
            Notify(MVCEvents.CONFIRM_DIFFICULTY_SELECTION);
            SettingsMenuView.UPDATEGEMS = true;
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.ConfirmDifficultButton);
        }
        
        public void OnClickContinue()
        {
            PlayerPrefs.SetInt(DemoView.RUN_FINISHED, 0);
            app.view.BookView.pageWithToggleOn.Clear();
            crossFadePanel.CrossFadeAlpha(120f, 390f, false);
            ContinueView.StartGameFromContinue = false;
            StartFadeOut(30f, 1f, 14f); //lento/veloce/medio
            StartCoroutine(OpenGame(0f)); //delay tra schermata continua/noTutorial e inizio gioco
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
        }

        IEnumerator OpenGame(float time)
        {
            yield return new WaitForSeconds(time);
            PlayerManager.Singleton.selectedClass = null;

            //const string firstRun = "firstRun";
            //if (PlayerPrefs.GetInt(firstRun, 1) == 1)
            //{
            //    Notify(MVCEvents.SKIP_TUTORIAL_REQUEST);
            //    PlayerPrefs.SetInt(firstRun, 0);
            //}
            Notify(MVCEvents.OPEN_GAME_VIEW);
        }

        public void StartFadeOut(float slowFade, float fastFade, float FadeMedium)
        {
            StartCoroutine(MusicFadeManager.MusicFadeOut(GameApplication.Singleton.model.BookModel.soundManager.audioSource, slowFade, fastFade, FadeMedium, 0));
        }

        void OnBackClick()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
        }
    }
}