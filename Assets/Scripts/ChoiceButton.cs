using SmartMVC;
using StarworkGC.Localization;
using StarworkGC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace Game
{
    public class ChoiceButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static ChoiceButton Singleton;
        protected void Awake()
        {
            Singleton = this;
        }

        [Header("UI Elements")]
        public Button button;
        public Button btnShowNoItemMessage;
        public RawImage btnImage;
        public Image imgStatsColor, imgDefaultColor, imgAttack, imgBorder, imgBlock, fullPressedImage;
        internal RectTransform rectTranform;
        [SerializeField] Color defaultStatsColor, defaultDefaultColor, defaultBorderColor, defaultTextColor;

        [Tooltip("E' l'ID della pagina che verrà caricata")]
        public string nextPageID;

        [Header("Button Behavior")]
        public bool buttonBlockIsActived, buttonIsAlreadyFadeIn;
        private bool unlockObject, unlockChoice, choiceChangeStats;

        [SerializeField] private TMP_Text myText;

        private AudioClip audioClip;
        private Coroutine onPressingButton = null;
        private Animator animator = null;
        ScrollRect scrollRect; // Riferimento allo ScrollRect

        CanvasGroup canvasGroup;   // Il CanvasGroup da gestire
        public float fadeDuration = 1f;


        private const string ANALYZEBUTTONID = "ANALYZECHARACTER";
        private bool isPressed = false; //per la pressione del tasto
        internal bool alreadyPressed = false; //per capire se è già stato cliccato
        private int pressCountFailed = 0;

        public bool PlayerCloseFindObjPanel { get; private set; } = false;
        public bool PlayerCloseChangedStatsPanel { get; private set; } = false;
        public List<string> clickedButton = new List<string>();

        protected void Start()
        {
            scrollRect = FindFirstObjectByType<ScrollRect>();
            animator = GetComponent<Animator>();
            rectTranform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            SetAlreadyPressedButtonState(false);
            buttonIsAlreadyFadeIn = false;
        }

        ScriptablePage.LockedChoiceType lockedReason;
        internal void DeactivateButton(ScriptablePage.LockedChoiceType lockedReason, bool lockedReasonExist = false)
        {
            this.lockedReason = lockedReason;
            button.interactable = false;
        }

        internal void SetFont(TMP_FontAsset newFont)
        {
            myText.font = newFont;
        }

        internal void SetNewFontSize(float newFontSize)
        {
            myText.fontSize = newFontSize;
        }

        internal void SetDefaultColor()
        {
            imgStatsColor.color = defaultStatsColor;
            imgDefaultColor.color = defaultDefaultColor;
            imgBorder.color = defaultBorderColor;
            myText.color = defaultTextColor;
            Canvas.ForceUpdateCanvases();
        }
        internal void SetUIAttackButton()
        {
            imgBorder.color = Color.black;
            imgDefaultColor.gameObject.SetActive(false);
            imgStatsColor.gameObject.SetActive(false);
            imgAttack.gameObject.SetActive(true);
            myText.text = Localization.Get(LocalizationIDDatabase.BTN_ATTACK);
        }

        internal void SetUIDeathButton()
        {
            nextPageID = PageIDDatabase.DEATHPAGEID;
            myText.text = TextTraductionUtility.GetCorrectSexText(LocalizationIDDatabase.DEATH_BUTTON_TEXT);
        }

        protected void OnEnable()
        {
            PlayerCloseFindObjPanel = false;
            btnImage.enabled = false;
            btnShowNoItemMessage.enabled = false;

            if (onPressingButton != null) { return; }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ContinueView.StartGameFromContinue = false;
            if (buttonBlockIsActived)
            {
                SetAlreadyPressedButtonState(false);
                return;
            }

            if (alreadyPressed && (nextPageID != ANALYZEBUTTONID)) { return; }

            if (PlayerPrefs.GetInt(SettingsMenuView.ACTIVEPRESSEDCLICK) == 1) //se non è attiva la pressione
            {
                if (!button.interactable)
                {
                    if (lockedReason == ScriptablePage.LockedChoiceType.workingInprogress ||
                        lockedReason == ScriptablePage.LockedChoiceType.waitingForPlayerInput)
                    {
                        //se vorremmo fare qualcosa di diverso in base al locked type
                        fullPressedImage.gameObject.SetActive(false);
                        return;
                    }
                    if (lockedReason == ScriptablePage.LockedChoiceType.toLowCourage && myText.text == Localization.Get("notEnoughBrave"))
                    {
                        fullPressedImage.gameObject.SetActive(false);
                        return;
                    }
                }

                OnClick();
                return;
            }
            isPressed = true;
            onPressingButton = StartCoroutine(RunAnimationOnPress());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!button.interactable)
            {
                return;
            }

            isPressed = false;
            //Blocco quando alzo
            animator.SetTrigger("StopLoadingAnim"); //blocco l'animazione se rilascio
            CoroutinesHelper.StopAndNullifyRoutine(ref onPressingButton, this);

            pressCountFailed++;
            if (PlayerPrefs.GetInt(SettingsMenuView.ACTIVEPRESSEDCLICK) == 0) //se è attiva la pressopne dei tasto
            {
                if (pressCountFailed > 2)
                {
                    AttentionPanel.Singleton.ActiveAttentionPanel();
                    Debug.Log("DEVI TENERE PREMUTO, SE PREFERISCI IL CLICK VAI NELLE OPZIONI E ATTIVA IL CLICK");
                }
                return;
            }

            pressCountFailed = 0;
        }

        IEnumerator RunAnimationOnPress()
        {
            if (!button.interactable)
            {
                if (lockedReason == ScriptablePage.LockedChoiceType.workingInprogress ||
                    lockedReason == ScriptablePage.LockedChoiceType.waitingForPlayerInput)
                {
                    //se vorremmo fare qualcosa di diverso in base al locked type
                    fullPressedImage.gameObject.SetActive(false);
                    yield break;
                }
                if (lockedReason == ScriptablePage.LockedChoiceType.toLowCourage && myText.text == Localization.Get("notEnoughBrave"))
                {
                    fullPressedImage.gameObject.SetActive(false);
                    yield break;
                }
            }

            animationLoadingIsFinish = false;
            animator.SetTrigger("StartPressAnim");

            while (isPressed)
            {
                yield return new WaitUntil(AnimationLoadIsFinish);

                if (!button.interactable)
                {
                    //carico animazione ma poi mostro che è bloccato cambiando testo
                    if (lockedReason == ScriptablePage.LockedChoiceType.toLowCourage)
                    {
                        myText.text = Localization.Get("notEnoughBrave");
                        SetOnlyColor(Color.white, Color.white, Color.black, true);

                        Canvas.ForceUpdateCanvases();
                        // Forza l'aggiornamento del layout del pulsante
                        LayoutRebuilder.ForceRebuildLayoutImmediate(button.GetComponent<RectTransform>());
                        // Se il pulsante è in un layout dinamico, forza il layout del contenitore
                        LayoutRebuilder.ForceRebuildLayoutImmediate(button.transform.parent.GetComponent<RectTransform>());
                        fullPressedImage.gameObject.SetActive(false);
                    }

                    yield break;
                }

                pressCountFailed = 0;
                animationLoadingIsFinish = false;
                SetAlreadyPressedButtonState(true);

                BookView.gameOpenedFromSettingsInGame = false;
                UISoundManager.Singleton.PlayAudioClip(audioClip);
                SaveButtonClicked();
                DividerFadeIn();
                StartCoroutine(LoadSpecificEventAfterClick());
            }

            if (!isPressed)
            {
                animator.SetTrigger("StopLoadingAnim");
                yield break;
            }
        }

        void OnClick()
        {
            if (!button.interactable)
            {
                //carico animazione ma poi mostro che è bloccato cambiando testo
                if (lockedReason == ScriptablePage.LockedChoiceType.toLowCourage)
                {
                    myText.text = Localization.Get("notEnoughBrave");
                    SetOnlyColor(Color.white, Color.white, Color.black, true);

                    Canvas.ForceUpdateCanvases();
                    // Forza l'aggiornamento del layout del pulsante
                    LayoutRebuilder.ForceRebuildLayoutImmediate(button.GetComponent<RectTransform>());
                    // Se il pulsante è in un layout dinamico, forza il layout del contenitore
                    LayoutRebuilder.ForceRebuildLayoutImmediate(button.transform.parent.GetComponent<RectTransform>());
                }

                return;
            }

            SetAlreadyPressedButtonState(true);
            BookView.gameOpenedFromSettingsInGame = false;
            UISoundManager.Singleton.PlayAudioClip(audioClip);
            SaveButtonClicked();
            //DividerFadeIn();
            StartCoroutine(LoadSpecificEventAfterClick());
        }

        private void SetAlreadyPressedButtonState(bool alreadyPressed)
        {
            this.alreadyPressed = alreadyPressed;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="statsColor">Se vuoi colorare il tasto nel lato delle stats</param>
        /// <param name="defaultColor">Se vuoi colorare il tasto nella parte che è sempre marrone</param>
        /// <param name="textColor">Se vuoi fare il testo di un altro colore</param>
        /// <param name="allowBlackAndWhite">Se permetti colori bianchi o neri</param>
        internal void SetOnlyColor(Color32? statsColor = null, Color32? defaultColor = null, Color32? textColor = null, bool allowBlackAndWhite = false)
        {
            SetColor(statsColor, defaultColor, textColor, allowBlackAndWhite);
        }

        public void Initialize(string nextPageID, bool unlockObject = false, bool choiceChangeStats = false, bool unlockChoice = false, AudioClip audioClip = null, Color32? color = null, bool attackButton = false, bool deathButton = false)
        {
            SetColor(color);

            if (attackButton)
            {
                SetUIAttackButton();
            }
            if (deathButton)
            {
                SetUIDeathButton();
            }

            if (audioClip == null)
            {
                this.audioClip = GameApplication.Singleton.Sounds.changeGenericPage;
            }
            else
            {
                this.audioClip = audioClip;
            }

            if (PlayerManager.Singleton.currentPage.writeThisTxtUnderButton)
            {
                GameApplication.Singleton.view.BookView.NotifyPageEndReached(PlayerManager.Singleton.currentPage.pageID, true);
            }
            else
            {
                StartCoroutine(GameApplication.Singleton.view.BookView.CheckIfEndOfPageReached(PlayerManager.Singleton.currentPage.pageID));
            }

            if (string.IsNullOrEmpty(nextPageID)) { return; }

            if (deathButton)
            {
                this.nextPageID = PageIDDatabase.DEATHPAGEID;
                return;
            }

            this.nextPageID = nextPageID;
            this.unlockObject = unlockObject;
            this.unlockChoice = unlockChoice;
            this.choiceChangeStats = choiceChangeStats;
        }

        void SetColor(Color32? statsColor = null, Color32? defaultColor = null, Color32? textColor = null, bool allowBlackOrWhiteColor = false)
        {
            if (statsColor != null)
            {
                SetColorToObject(statsColor, imgStatsColor, allowBlackOrWhiteColor);
            }

            if (defaultColor != null)
            {
                SetColorToObject(statsColor, imgDefaultColor, allowBlackOrWhiteColor);
            }

            if (textColor != null)
            {
                SetColorToObject(textColor);
            }
        }

        void SetColorToObject(Color32? newColor, Image imgToColor = null, bool allowBlackOrWhiteColor = false)
        {
            Color32 white = new Color32(255, 255, 255, 0);
            Color32 black = new Color32(0, 0, 0, 0);

            if (imgToColor == null)
            {
                myText.color = (UnityEngine.Color)newColor;
                return;
            }

            if (newColor == null || (!allowBlackOrWhiteColor && (Equals(newColor, white) || Equals(newColor, black))))
            {
                imgToColor.color = ColorDatabase.Singleton.buttonDefaultColor;
            }
            else
            {
                Color32 tmpColor = (Color32)newColor;
                tmpColor.a = 255;
                imgToColor.color = tmpColor;
            }
        }

        public void LoadPage()
        {
            PlayerManager.Singleton.lastPage = nextPageID;
            SaveOnOpenPage(nextPageID);
            GameApplication.Singleton.app.Notify(MVCEvents.LOAD_PAGE, nextPageID);
        }

        void SaveOnOpenPage(string lastPageID)
        {
            if (PlayerManager.Singleton.currentPage.GetNotSave) return;
            if (PlayerManager.Singleton == null) return;
            if (ContinueView.StartGameFromContinue) return;
            SaveSystem.SavePlayerLastRun(SaveType.Soft);
            Debug.LogFormat("Sto salvando a pagina {0}", lastPageID);
        }

        public void LoadStatus()
        {
            GameApplication.Singleton.app.Notify(MVCEvents.LOAD_STATUS, nextPageID);
        }

        Action functionDelegate = null;

        IEnumerator LoadSpecificEventAfterClick()
        {
            BookView view = GameApplication.Singleton.view.BookView;

            CallEventOnClickBeforeChangePage();

            yield return new WaitUntil(EventIsClosed);

            GameObject obj = view.panelFindObject.gameObject;
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }

            if(functionDelegate != null)
            {
                functionDelegate();
                functionDelegate = null;
            }


            if (string.IsNullOrEmpty(nextPageID))
            {
                Debug.LogWarning("ID del tasto è nullo");
                yield break;
            }
            else
            {
                switch (nextPageID)
                {
                    case (PageIDDatabase.SCELTACLASSEID):
                        switch (PlayerManager.answerClassSolution)
                        {
                            case LocalizationIDDatabase.ABBOTADVICE:
                                GameApplication.Singleton.view.StatsChoicePanel.TurnOnSelectedPanel("Abbot");
                                break;
                            case LocalizationIDDatabase.BOUNTYADVICE:
                                GameApplication.Singleton.view.StatsChoicePanel.TurnOnSelectedPanel("BountyHunter");
                                break;
                            case LocalizationIDDatabase.CRONEADVICE:
                                GameApplication.Singleton.view.StatsChoicePanel.TurnOnSelectedPanel("Crone");
                                break;
                            case LocalizationIDDatabase.TRAFFICKERADVICE:
                                GameApplication.Singleton.view.StatsChoicePanel.TurnOnSelectedPanel("Trafficker");
                                break;
                            default:
                                break;
                        }
                        GameApplication.Singleton.app.Notify(MVCEvents.OPEN_CHARACTER_CREATION_VIEW);
                        break;
                    case (ANALYZEBUTTONID):
                        view.panelAnalyzeCharachter.SetActive(true);
                        break;
                    default:
                        BlockChoice();
                        LoadPage();
                        LoadStatus();
                        break;
                }
            }
        }


        internal void SetFunctionForTheButton(Action functionDelegate)
        {
            this.functionDelegate = functionDelegate;
        }

        /// <summary>
        /// Serve a controllare se il tasto prima di cambiare pagina deve aprire qualche pannello
        /// </summary>
        void CallEventOnClickBeforeChangePage()
        {
            if (choiceChangeStats)
            {
                ShowChangedStatsOnClick();
            }
        }

        /// <summary>
        /// Serve a dare il tempo di attesa al cambio pagina se il tasto ha aperto un altro evento prima
        /// </summary>
        /// <returns></returns>
        public bool EventIsClosed()
        {
            if (PlayerManager.Singleton.currentPage.chapterSection == ScriptablePage.Section.Titolo ||
        PlayerManager.Singleton.currentPage.beforeTitle)
            {
                return true;
            }

            // Check if chapter change is triggered
            if (OnChangeChapter())
            {
                StartCoroutine(WaitFor(6f));
                return PlayerCloseFindObjPanel;
            }

            // Handle object unlocking logic
            if (unlockObject)
            {
                if (!PlayerManager.Singleton.currentPage.clickAndDrop)
                {
                    Debug.Log("Current page doesn't have click and drop enabled.");
                    return true;
                }

                UnlockOnClick();
                return PlayerCloseFindObjPanel;
            }

            // Handle stats change panel
            if (choiceChangeStats)
            {
                return PlayerCloseChangedStatsPanel;
            }

            return true;
        }

        public int objNumbers;
        void UnlockOnClick()
        {
            var view = GameApplication.Singleton.view.BookView;
            var page = PlayerManager.Singleton.currentPage;

            // Set item list based on selected difficulty level
            page.readAndDrop = page.GetItemListByDifficulty(PlayerManager.Singleton.selectedDifficulty);

            // Check if page has items to unlock and hasn’t been previously dropped
            if (page.readAndDrop != null && page.readAndDrop.Count > 0 && !PlayerManager.Singleton.IsPageDropped(page.pageID))
            {
                PopulateItemsOnPage(view, page);
            }
        }

        private void PopulateItemsOnPage(BookView view, ScriptablePage page)
        {
            int i = 0;
            var quantities = page.readAndDropQuantity;
            var itemsNumber = new Dictionary<ScriptableItem, int>();

            view.DestroyFindedObj();

            foreach (var item in page.readAndDrop)
            {
                int quantity = quantities.Count > i ? quantities[i] : 1;
                itemsNumber[item] = quantity;

                view.ShowSingleFindObj(item, quantity);
                StartCoroutine(OpenPanelFindObjects());
                PlayerManager.Singleton.AddDroppedPage(page.pageID);
                PlayerManager.Singleton.AddItemDrop(item, quantity);

                i++;
            }
        }

        void ShowChangedStatsOnClick()
        {
            var page = PagesMaleDatabase.Singleton.GetPageByID(nextPageID);
            var view = GameApplication.Singleton.view.BookView;

            foreach (var variation in page.statsVariations)
            {
                view.ShowChangedStats(variation.buffDebuffAmount, variation.iconName.ToString());
            }

            StartCoroutine(OpenChangedStatsPanel());
        }

        public IEnumerator OpenPanelFindObjects()
        {
            var view = GameApplication.Singleton.view.BookView;
            view.titleYouTake.gameObject.SetActive(true);
            view.alarmFinishObject.text = string.Empty;
            view.panelFindObject.gameObject.SetActive(true);

            yield return new WaitUntil(() => PanelFindedObjIsClosed());

            view.DestroyFindedObj();
            PlayerCloseFindObjPanel = true;
        }

        public IEnumerator OpenChangedStatsPanel()
        {
            var view = GameApplication.Singleton.view.BookView;
            view.panelStatsAreChanged.gameObject.SetActive(true);

            yield return new WaitUntil(() => PanelChangedStatsIsClosed());

            view.DestroyChangedStats();
            PlayerCloseChangedStatsPanel = true;
        }

        public bool PanelFindedObjIsClosed() =>
            !GameApplication.Singleton.view.BookView.panelFindObject.gameObject.activeSelf;

        public bool PanelChangedStatsIsClosed() =>
            !GameApplication.Singleton.view.BookView.panelStatsAreChanged.gameObject.activeSelf;

        IEnumerator WaitFor(float time)
        {
            yield return new WaitForSeconds(time);
            PlayerCloseFindObjPanel = true;
        }

        void BlockChoice()
        {
            foreach (Transform child in GameApplication.Singleton.view.BookView.chapterContainer)
            {
                if (child.name == "ChoiceButtonPrefab(Clone)")
                {
                    child.GetComponent<Button>().interactable = false;
                }
            }
        }

        void SaveButtonClicked()
        {
            PlayerManager.Singleton.buttonClicked.Add(myText.text);
        }

        public void DividerFadeIn()
        {
            if (OnChangeChapter())
            {
                GameApplication.Singleton.view.BookView.txtChapter.gameObject.SetActive(false);
                GameApplication.Singleton.view.BookView.txtChapterSubtitle.gameObject.SetActive(false);
                StartCoroutine(GameApplication.Singleton.view.BookView.ShowDivider(1.5f));
                StartCoroutine(GameApplication.Singleton.view.BookView.ActivateText(2.5f));
            }
        }

        public bool OnChangeChapter()
        {
            var chapterIDs = new HashSet<string> { "scappa", "inVinoVeritas", "duraLex" };
            if (chapterIDs.Contains(nextPageID))
            {
                StartFadeOut();
                return true;
            }
            return false;
        }

        public void StartFadeOut()
        {
            StartCoroutine(MusicFadeManager.MusicFadeOut(GameApplication.Singleton.model.BookModel.soundManager.audioSource, 50f, 1f, 14f, 0));
        }

        bool animationLoadingIsFinish = false;

        public void LoadingOnPointerDown()
        {
            animationLoadingIsFinish = true;
        }

        internal bool AnimationLoadIsFinish() => animationLoadingIsFinish;

        internal IEnumerator ShowTheButton()
        {
            float elapsedTime = 0f;

            // Ciclo per aumentare l'alpha progressivamente
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // Interpola da 0 a 1
                yield return null; // Attendi il prossimo frame
            }

            // Una volta completato il fade, rendi il canvas interattivo
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            buttonIsAlreadyFadeIn = true;
        }

        internal IEnumerator ShowTheButtonUnderText()
        {
            float elapsedTime = 0f;

            yield return new WaitUntil(delegate { return canvasGroup != null; });

            // Ciclo per aumentare l'alpha progressivamente
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // Interpola da 0 a 1
                yield return null; // Attendi il prossimo frame
            }

            // Una volta completato il fade, rendi il canvas interattivo
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            buttonIsAlreadyFadeIn = true;
        }

        internal IEnumerator ShowButtonWithoutFade()
        {
            yield return new WaitUntil(delegate { return canvasGroup != null; });
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
        }

    }

}