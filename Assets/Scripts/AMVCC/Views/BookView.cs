using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;
using StarworkGC.Localization;
using System;
using StarworkGC.Utils;
using static Game.ScriptablePage;
using System.Runtime.InteropServices.WindowsRuntime;
using Random = UnityEngine.Random;
using static Game.ScriptableRiddle;

namespace Game
{
    public class BookView : View<GameApplication>
    {
        [Header("CURRENTPAGE")]
        public string showCurrentPage;
        [Header("Reference")]
        public BookController BookController;

        [Header("Button")]
        public ChoiceButton btnOptionPrefab;
        public Button btnRiddlePrefab;
        public Button openSettingsView;
        public Button btnOpenPlayerStats;
        public Image imgPlayerClassIcon;

        public GameObject expAndLifeUIContainer;
        public GameObject[] lifeUIContainer; //Gestisce i riempimenti 
        public GameObject[] lifeUISection; // Gestisce il container
        public GameObject[] expUIContainer; // Gestisce i riempimenti
        public GameObject[] expUISection; // Gestisce il container
        public Button btnHideoPopUp;
        public Button btnSearch;
        public Button btnEmptySearch;
        public Button btnEmptyStats;
        public Button btnYesTutorial;
        public Button btnNoTutorial;
        public SkipElementButtonManager btnChangeByTouch;
        public GameObject inputFieldPrefabContainer;

        public Button btnDesignerBalanceView;

        [Header("Image")]
        public RawImage resultBackGround;
        public RawImage mapsVideo;
        public RawImage mainBackgroundImage;
        public RawImage shadowImage;

        public Image imgDesignerPanel;

        [Header("Text")]
        public TMP_Text textPrefab;
        public TMP_Text result;
        public TMP_Text txtChapter;
        public TMP_Text txtChapterSubtitle;
        public TMP_Text txtTourchCount;
        public TMP_Text itemUsedToUnlock;
        public TMP_Text alarmFinishObject;
        public TMP_Text titleYouTake, pageReadInCurrentRun;

        [Header("ElementForUI")]
        public RectTransform chapterContainer;
        public ScrollRect scrollRect;
        public GameObject horizontalBtnGroup;
        public GameObject imgHideFrameDelay;
        public GameObject levelUpUI;
        public GameObject objGems;
        public GameObject objBookmark;

        [Header("ExternalPanel")]
        [SerializeField] GameObject layoutComponent;
        [SerializeField] DroppedItem tmpDroppedItem;
        [SerializeField] GameObject layoutComponentChangedStats;
        [SerializeField] ChangedStats tmpChangedStats;
        public GameObject panelKeyItemUsed;
        public GameObject panelFindObject;
        /// <summary>
        /// Pannello che mostra i cambi di stats del pg
        /// </summary>
        public GameObject panelStatsAreChanged;
        public GameObject panelTutorialRequest;
        public GameObject panelTutorialContainer;
        public GameObject panelAnalyzeCharachter;
        public GameObject panelTutorialAnalyzePanel;
        public GameObject panelTutorialButtonAnalyze;

        [SerializeField] GameObject activeTorch;
        [SerializeField] GameObject popup;


        [Header("Animator")]
        public Animator animator;
        public Animator decorationFadeIn;


        // Start is called before the first frame update

        public List<ScriptablePage> pageWithToggleOn = new List<ScriptablePage>();
        public void ShowBuffes()
        {
            animator.SetTrigger("PopUpShowed");
        }

        public GameObject GetTorchAnimation { get => activeTorch; }
        public GameObject GetPopup { get => popup; }
        // public RawImage GetBubbleImagePopup { get => bubbleImage; }
        BookModel Model { get { return app.model.BookModel; } }

        #region UIManager
        public void RefreshUIOnStartNewRun(bool newRun)
        {
            btnSearch.gameObject.SetActive(false);
            btnSearch.enabled = false;
            btnEmptySearch.gameObject.SetActive(true);
            btnEmptyStats.gameObject.SetActive(true);
            txtTourchCount.gameObject.SetActive(false);
            objGems.gameObject.SetActive(false);
            expAndLifeUIContainer.gameObject.SetActive(false);

            btnOpenPlayerStats.gameObject.SetActive(false);

            if (newRun) //Se nuova partita lo vede morto prima della selezione classe
            {
                if (PlayerManager.Singleton.PlayerIsDead())
                {
                    objBookmark.gameObject.SetActive(false);
                }
            }
        }

        public int classButtonIndex;
        public void UpdateClassUIElements()
        {
            expAndLifeUIContainer.gameObject.SetActive(true);
            app.view.Settings.btnOpenVisualElementPanel.gameObject.SetActive(true);
            ClassButtonManage();
            UpdateRemainingTorchNumbersText();
        }

        public void ShowTorchTxt()
        {
            txtTourchCount.gameObject.SetActive(true);
            UpdateRemainingTorchNumbersText();
        }

        void ClassButtonManage()  //Abbot,Bounty,Crone,Trafficker
        {
            btnOpenPlayerStats.gameObject.SetActive(true);
            btnEmptyStats.gameObject.SetActive(false);
            SetClassSpecificAsset();
        }

        void SetClassSpecificAsset()
        {

            foreach (GameObject lifeUI in lifeUISection)
            {
                lifeUI.SetActive(true);
                UILifecounter = 12;
            }
            foreach (GameObject expUI in expUISection)
            {
                expUI.SetActive(true);
                UIExpcounter = 6;
            }
            expAndLifeUIContainer.gameObject.SetActive(true);

            switch (PlayerManager.Singleton.selectedClass)
            {
                case BountyHunter:
                    imgPlayerClassIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.BOUNTYICON);
                    break;
                case Abbot:
                case Trafficker:
                    break;
                default:
                    imgPlayerClassIcon.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.CRONEICON);
                    break;
            }
        }


        #endregion

        public static string CurrentPage;
        public static bool titleIsClosed;

        void OnEnable()
        {
            openSettingsView.onClick.RemoveAllListeners();
            openSettingsView.onClick.AddListener(OpenSettingMenu);
            panelFindObject.SetActive(false);
            panelStatsAreChanged.SetActive(false);
            expAndLifeUIContainer.gameObject.SetActive(false);
            btnNoTutorial.onClick.RemoveAllListeners();
            btnNoTutorial.onClick.AddListener(OnClickNoTutorial);
            btnYesTutorial.onClick.RemoveAllListeners();
            btnYesTutorial.onClick.AddListener(ConfirmTutorial);

            btnSearch.onClick.RemoveAllListeners();
            btnSearch.onClick.AddListener(OnClickSearch);
            //carico le preferenze dei settaggi del giocatore
            app.view.Settings.SetSavedSettings();

            scrollRect.onValueChanged.RemoveAllListeners();
            Debug.LogWarning("Prima aspettavo che la scroll arrivasse ad un certo punto");
            //scrollRect.onValueChanged.AddListener(delegate { StartCoroutine(UnlockTheButton()); });

            if (gameOpenedFromSettingsInGame)
            {
                scrollRect.verticalNormalizedPosition = saveScrollValue;
            }

            btnOpenPlayerStats.gameObject.SetActive(false);

            if (PlayerManager.Singleton.selectedClass)
            {
                LoadSelectedClassTutorial();
                btnOpenPlayerStats.onClick.RemoveAllListeners();
                btnOpenPlayerStats.onClick.AddListener(delegate { OpenCharacterMenu(); });
            }

            int pageRead = PlayerManager.Singleton.pagesRead.Count;
            app.model.currentView = this;

            if (pageRead <= 1)
            {
                LoadPage(PageIDDatabase.ALLASALUTECOPERTINAID);
                RefreshUIOnStartNewRun(true);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void ManageTheSearchButton(ScriptableItem torch)
        {
            if (PlayerManager.Singleton.TakeItemNumberFromInventory(torch) > 0)
            {
                btnSearch.interactable = true;
                ShowTorchTxt();
            }
            else btnSearch.interactable = false;
        }

        IEnumerator UnlockTheButton()
        {
            yield return new WaitForSeconds(1);
            if (AllButtonsAreVisibles() || PlayerManager.Singleton.currentPage.writeThisTxtUnderButton)
            {
                UnlockAllClickedButton();
            }
        }

        public void LoadSelectedClassTutorial()
        {
            if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1 ||
                PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1)
            {
                string currentPageID = PlayerManager.Singleton.currentPage.pageID;
                if (currentPageID == PageIDDatabase.ALLASALUTEFIRSTPAGE)
                {
                    panelTutorialContainer.SetActive(true);
                }
            }
            else
            {
                panelTutorialContainer.SetActive(false);
            }

        }
        public void LoadPage(string pageID)
        {
            Notify(MVCEvents.LOAD_PAGE, pageID);
        }

        public float saveScrollValue;

        /// <summary>
        /// Questo booleano chiede se il gioco è stato aperto dal Menu del player o delle impostazioni
        /// </summary>
        public static bool gameOpenedFromSettingsInGame;
        public void OpenCharacterMenu(bool openForTutorial = false)
        {
            saveScrollValue = scrollRect.verticalScrollbar.value;
            gameOpenedFromSettingsInGame = true;
            Notify(MVCEvents.OPEN_CHARACTER_MENU);
            if (openForTutorial) return;
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
        }
        void OpenSettingMenu()
        {
            saveScrollValue = scrollRect.verticalScrollbar.value;
            gameOpenedFromSettingsInGame = true;
            Notify(MVCEvents.OPEN_GAMESETTING_VIEW);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
        }

        void OnClickNoTutorial()
        {
            PlayerPrefs.SetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL, 0);
            panelTutorialRequest.gameObject.SetActive(false);
        }

        [SerializeField] TutorialSystemManager tutorialSystemManager;

        void ConfirmTutorial()
        {
            tutorialSystemManager.RefreshDataForTheTutorial();
            PlayerPrefs.SetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL, 1);
            objBookmark.SetActive(true);
            panelTutorialRequest.gameObject.SetActive(false);
            panelTutorialContainer.gameObject.SetActive(true);
            tutorialSystemManager.ShowFirstPartOfTutorial();
        }

        string tmpID;
        int tmpCount = 0;
        public void OnClickSearch()
        {
            SearchAnimationIsFinish.searchAnimIsFinish = false;
            DropObjPanelIsActive.dropObjPanelIsActive = false;

            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            ScriptablePage scriptablePage = PlayerManager.Singleton.currentPage;

            if (tmpID != scriptablePage.pageID)
            {
                tmpID = scriptablePage.pageID;
                tmpCount = 0;
            }

            btnSearch.interactable = false;
            GetTorchAnimation.SetActive(true);
            SoundEffectManager.Singleton.PlayAudioClip(app.Sounds.torchAudio);
            ScriptableItem torch = ScriptableItemsDatabase.Singleton.itemsDatabase.Find(s => s.ID == "Torcia");
            PlayerManager.Singleton.RemoveItemFromPlayerInventory(torch, 1);
            UpdateRemainingTorchNumbersText();
            tmpCount++;

            if (tmpCount < 3)
            {
                titleYouTake.gameObject.SetActive(true);

                if (scriptablePage.containGenericDrop)
                {
                    TakeObjectFromPool();
                    StartCoroutine(ShowFindObjWhenSearch());
                    return;
                }
                else if (scriptablePage.containSpecificDrop)
                {
                    TakeSpecificObject();
                    StartCoroutine(ShowFindObjWhenSearch());
                    return;
                }
            }

            ShowNoObjectFind();
        }

        void ShowNoObjectFind()
        {
            DestroyFindedObj();
            titleYouTake.gameObject.SetActive(false);
            alarmFinishObject.text = Localization.Get(LocalizationIDDatabase.NO_OBJECT_IN_PAGE);
            StartCoroutine(ShowFindObjWhenSearch(false));
            Debug.LogWarning("Non c'è più nulla...");
        }


        public IEnumerator ShowFindObjWhenSearch(bool cleanAlarmText = true)
        {
            yield return new WaitUntil(SearchAnimationIsFinish.GetSearchAnimationIsFinish);

            if (cleanAlarmText)
            {
                alarmFinishObject.text = string.Empty;
            }

            panelFindObject.SetActive(true);
            PlayerCanUseTorch();

            yield return new WaitUntil(DropObjPanelIsActive.GetDropPanelIsActive);
            GetTorchAnimation.SetActive(false);
        }

        public const string TORCHOBJID = "Torcia";
        void UpdateRemainingTorchNumbersText()
        {
            txtTourchCount.text = PlayerManager.Singleton.TakeItemNumberFromInventory(ScriptableItemsDatabase.Singleton.GetItemById(TORCHOBJID)).ToString();
        }

        [SerializeField] TMP_Text objName;
        [SerializeField] TMP_Text objDescription;
        void TakeObjectFromPool()
        {
            Section currentSection = PlayerManager.Singleton.currentPage.chapterSection;
            Tuple<List<ScriptableItem>, List<int>> tuple = ScriptableItemsDatabase.Singleton.GetObjsFromPool(currentSection);
            Dictionary<ScriptableItem, int> itemsNumber = new Dictionary<ScriptableItem, int>();

            int i = 0;
            foreach (ScriptableItem obj in tuple.Item1)
            {
                if (tuple.Item2 == null)
                {
                    itemsNumber.Add(obj, 1);
                }
                else
                {
                    if (tuple.Item2[i] == 0)
                    {
                        itemsNumber.Add(obj, 1);
                    }
                    else
                    {
                        itemsNumber.Add(obj, tuple.Item2[i]);
                    }
                }
                i++;
            }

            int randomNumber = Random.Range(0, tuple.Item1.Count);
            ScriptableItem item = GetObjByLuckyAndDifficult(tuple.Item1, PlayerManager.Singleton.selectedDifficulty, PlayerManager.Singleton.lucky, randomNumber);
            DestroyFindedObj();
            ShowSingleFindObj(item, itemsNumber[tuple.Item1[randomNumber]]);
            PlayerManager.Singleton.AddDroppedPage(PlayerManager.Singleton.currentPage.pageID);
            PlayerManager.Singleton.AddItemDrop(tuple.Item1[randomNumber], itemsNumber[tuple.Item1[randomNumber]]);
            ScriptableItemsDatabase.Singleton.RemoveObjsFromPool(currentSection, randomNumber);
        }
        void TakeSpecificObject()
        {
            List<ScriptableItem> items = PlayerManager.Singleton.currentPage.readAndDrop;
            List<int> number = PlayerManager.Singleton.currentPage.readAndDropQuantity;
            Dictionary<ScriptableItem, int> itemsNumber = new Dictionary<ScriptableItem, int>();
            int i = 0;
            foreach (ScriptableItem obj in items)
            {
                itemsNumber.Add(obj, number[i]);
                i++;
            }
            int randomNumber = UnityEngine.Random.Range(0, items.Count);
            DestroyFindedObj(); //distruggo prima quelli che erano presenti
            ShowMoreFindedObjs(items[randomNumber], itemsNumber[items[randomNumber]]);
            PlayerManager.Singleton.AddDroppedPage(PlayerManager.Singleton.currentPage.pageID);
            PlayerManager.Singleton.AddItemDrop(items[randomNumber], itemsNumber[items[randomNumber]]);
            items.RemoveAt(randomNumber);
        }

        ScriptableItem GetObjByLuckyAndDifficult(List<ScriptableItem> items, PlayerManager.Difficulty difficulty, int lucky, int randomNumber)
        {
            ScriptableItem giftItem = items[randomNumber];
            if (lucky > 90)
            {
                foreach (ScriptableItem obj in items)
                {
                    if (obj.rarity == ScriptableItem.Rarity.Legendary || obj.rarity == ScriptableItem.Rarity.SuperRare)
                    {
                        return obj;
                    }
                }
            }
            if (difficulty == PlayerManager.Difficulty.Insane)
            {
                Debug.LogError("Non hai trovato nulla");
                return null;
            }
            return giftItem;
        }


        public void ShowMoreFindedObjs(ScriptableItem item, int quantity = 1)
        {
            DroppedItem DroppedItem = Instantiate(tmpDroppedItem);
            DroppedItem.SetupItem(item, quantity);
            DroppedItem.transform.SetParent(layoutComponent.transform);
            DroppedItem.transform.localPosition = new Vector3(0, 0, 0);
            DroppedItem.transform.localScale = new Vector3(1, 1, 1);
        }

        public void ShowSingleFindObj(ScriptableItem item, int quantity = 1)
        {
            DroppedItem DroppedItem = Instantiate(tmpDroppedItem);
            DroppedItem.SetupItem(item, quantity);
            DroppedItem.transform.SetParent(layoutComponent.transform);
            DroppedItem.transform.localPosition = new Vector3(0, 0, 0);
            DroppedItem.transform.localScale = new Vector3(1, 1, 1);
        }

        public void DestroyFindedObj()
        {
            foreach (Transform child in layoutComponent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void ShowChangedStats(int buffDebuffAmount, string statsName)
        {
            ChangedStats ChangedStats = Instantiate(tmpChangedStats);
            ChangedStats.SetupChangedStats(IconsDatabase.Singleton.GetSpriteIcon(statsName + "Icon"), buffDebuffAmount, statsName);
            ChangedStats.transform.SetParent(layoutComponentChangedStats.transform);
            ChangedStats.transform.localPosition = new Vector3(0, 0, 0);
            ChangedStats.transform.localScale = new Vector3(1, 1, 1);
        }

        public void DestroyChangedStats()
        {
            foreach (Transform child in layoutComponentChangedStats.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void PlayerCanUseTorch()
        {
            ScriptableItem torch = ScriptableItemsDatabase.Singleton.itemsDatabase.Find(s => s.ID == "Torcia");
            if (PlayerManager.Singleton.TakeItemNumberFromInventory(torch) > 0 && PlayerManager.Singleton.pagesRead.Contains("fineTutorial"))
            {
                btnSearch.interactable = true;
                ShowTorchTxt();
            }
        }

        ScriptablePage previowsPage;
        public void ShowPage(ScriptablePage page)
        {
            if (page == null) { Debug.LogWarning("La pagina è null"); return; }

            if (previowsPage == null)
            {
                previowsPage = page;
            }

            UIPageElementManager(page);
            PreparePageAndCheck(page);

            if (!gameOpenedFromSettingsInGame)
            {
                if (PlayerManager.Singleton.selectedClass == null &&
              page.pageID == PageIDDatabase.ALLASALUTEFIRSTPAGE && !ContinueView.StartGameFromContinue)
                {
                    LaunchTheTutorial();
                }

                GetTutorial(page);

                if (!ContinueView.StartGameFromContinue)
                {
                    GoalsDatabase.Singleton.GoalsUnlocked();
                }
            }

            if (page.writeThisTxtUnderButton)
            {
                if (pageWithToggleOn.Contains(page)) { return; }
                pageWithToggleOn.Add(page);
            }

            LoadTextChangingPage(page);

            //check if the character is woman and add the prefix
            string ID = TextTraductionUtility.AddFemalePrefixToPageID(page.pageID);
            string localizedText = GetPageText(page, ID);

            //if in the database there isn't the female page load the default male page 
            if (CheckString(ID, localizedText))
            {
                localizedText = string.Empty;
                localizedText = GetPageText(page, page.pageID);
            }

            TMP_Text characterPage = Instantiate(textPrefab, chapterContainer);
            characterPage.text = localizedText;
            AnalyzePage(page);

            if (PlayerManager.Singleton.selectedClass)
            {
                //Aggiorno icone stats
                OnChangeLife(PlayerManager.Singleton.lifePoints, PlayerManager.Singleton.constitution);
                UpdateLevelUI();
            }
        }

        public IEnumerator CheckIfEndOfPageReached(string pageID)
        {
            // Forza l'aggiornamento del layout prima di controllare
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            yield return new WaitForSeconds(0.3f); // Aspetta per dare tempo al layout di aggiornarsi

            // Controlla se i pulsanti sono visibili
            if (IsChoiceButtonVisibiles())
            {
                NotifyPageEndReached(pageID); // Notifica la fine della pagina
                yield break; // Esci dalla coroutine
            }

            // Se i pulsanti non sono visibili, continua a controllare (se necessario)
            while (!IsChoiceButtonVisibiles())
            {
                yield return null; // Aspetta il prossimo frame
            }

            // Se i pulsanti diventano visibili, notifica la fine della pagina
            NotifyPageEndReached(pageID);
        }

        internal void NotifyPageEndReached(string page, bool writeTextUnderButton = false)
        {
            Debug.Log("FINE RAGGIUNTA" + page);
            if (writeTextUnderButton)
            {
                ShowTheButtons(true);
                return;
            }
            ShowTheButtons();
        }
        private bool IsChoiceButtonVisibiles()
        {
            // Ottieni i rettangoli della viewport dello scroll
            Rect viewportRect = scrollRect.viewport.rect;

            // Ottieni il ChoiceButton
            ChoiceButton btn = chapterContainer.GetComponentInChildren<ChoiceButton>();
            if (btn == null)
            {
                Debug.LogWarning("Nessun ChoiceButton trovato.");
                return false;
            }

            // Ottieni i confini del pulsante in coordinate mondiali
            Vector3[] markerCorners = new Vector3[4];
            btn.rectTranform.GetWorldCorners(markerCorners);

            // Converti le coordinate mondiali del pulsante in coordinate locali della viewport
            Vector3 bottomLeft = scrollRect.viewport.InverseTransformPoint(markerCorners[0]);
            Vector3 topRight = scrollRect.viewport.InverseTransformPoint(markerCorners[2]);

            // Controlla se il pulsante è completamente visibile all'interno della viewport
            bool isVisibleVertically = topRight.y <= viewportRect.yMax && bottomLeft.y >= viewportRect.yMin;

            return isVisibleVertically;
        }

        private void ShowTheButtons(bool writeTextUnderBtn = false)
        {
            List<ChoiceButton> buttons = new List<ChoiceButton>();

            foreach (Transform child in chapterContainer)
            {
                ChoiceButton button = child.GetComponent<ChoiceButton>();

                if (button != null && !button.buttonIsAlreadyFadeIn)
                {
                    buttons.Add(button);
                }
            }

            StartCoroutine(ShowButtonsWithFade(buttons, writeTextUnderBtn));
        }

        private IEnumerator ShowButtonsWithFade(List<ChoiceButton> buttons, bool writeTextUnderBtn = false)
        {
            foreach (ChoiceButton button in buttons)
            {
                if (writeTextUnderBtn)
                {
                    yield return StartCoroutine(button.ShowButtonWithoutFade());
                }
                else
                {
                    yield return StartCoroutine(button.ShowTheButton());
                }
            }

            ActivateRaycast(buttons);
        }


        void ActivateRaycast(List<ChoiceButton> buttons)
        {
            foreach (ChoiceButton button in buttons)
            {
                CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.blocksRaycasts = true; // Riattiva i raycast
                }
            }
        }

        void ActivateAnalizeTutorialPanel(ScriptablePage page)
        {
            if ((PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1 ||
                PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1)
                  && page.pageID == PageIDDatabase.ANALIZZAMENESTRELLOID
                  && !gameOpenedFromSettingsInGame)
            {
                panelTutorialAnalyzePanel.SetActive(true);
                panelTutorialButtonAnalyze.SetActive(true);
            }
        }

        public void OnAnalyzeTutorialPanelTurnOff()
        {
            tutorialSystemManager.ShowSuperstitionButtonTutorial();
        }
        void LaunchTheTutorial()
        {
            if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKFIRSTGAMELAUNCH, 1) == 1)
            {
                PlayerPrefs.SetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH, 1);
                PlayerPrefs.SetInt(LocalizationIDDatabase.CHECKFIRSTGAMELAUNCH, 0); //Set to false
                LaunchFirstTimeTutorial();
                return;
            }

            panelTutorialContainer.gameObject.SetActive(false);
            PlayerPrefs.SetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH, 0);

            if (SettingsMenuView.ShowTutorialRequest)
            {
                panelTutorialRequest.SetActive(true);
                return;
            }
            panelTutorialRequest.SetActive(false);
        }

        void GetTutorial(ScriptablePage page)
        {
            int activeTutorial = PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL);
            int firstLaunchTutorial = PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH);

            if (activeTutorial == 1 || firstLaunchTutorial == 1)
            {
                switch (page.pageID)
                {
                    case PageIDDatabase.ALLASALUTEFIRSTPAGE:
                        if (PlayerManager.Singleton.selectedClass == null
                            && firstLaunchTutorial == 1) //Quando carico passa la pagina "intro"
                        {
                            if (TutorialSystemManager.FirstTutorialPartNotShowed)
                            {
                                tutorialSystemManager.ShowFirstPartOfTutorial();
                                TutorialSystemManager.FirstTutorialPartNotShowed = false;
                            }
                        }
                        break;
                    case PageIDDatabase.ANALIZZAMENESTRELLOID:
                        if (TutorialSystemManager.AnalyzeTutorialPartNotShowed)
                        {
                            ActivateAnalizeTutorialPanel(page);
                            TutorialSystemManager.AnalyzeTutorialPartNotShowed = false;
                        }
                        break;
                    case PageIDDatabase.TUTORIALCOURAGEID:
                        tutorialSystemManager.ShowCourageButtonTutorial();
                        break;
                    case PageIDDatabase.TUTORIALLUCKYID:
                        tutorialSystemManager.ShowLuckyButtonTutorial();
                        break;
                    case PageIDDatabase.BEFOREFIRSTFIGHT:
                    case PageIDDatabase.BEFOREFIRSTFIGHTHIGH:
                        if (TutorialSystemManager.BeforeFightTutorialPartNotShowed)
                        {
                            tutorialSystemManager.ShowBeforeFightTutorial();
                            TutorialSystemManager.BeforeFightTutorialPartNotShowed = false;
                        }
                        break;
                    case PageIDDatabase.TORCHPAGEID:
                        if (TutorialSystemManager.TorchTutorialPartNotShowed)
                        {
                            tutorialSystemManager.ShowTorchTutorial();
                            tutorialSystemManager.SetTutorialFinish();
                            TutorialSystemManager.TorchTutorialPartNotShowed = false;
                        }
                        break;
                    case PageIDDatabase.TUTORIALFIRSTPAGE:
                        if (TutorialSystemManager.SecondTutorialPartNotShowed)
                        {
                            TutorialSystemManager.SecondTutorialPartNotShowed = false;
                            tutorialSystemManager.ShowSecondPartOfTutorial();
                        }
                        break;
                    default:
                        tutorialSystemManager.gameObject.SetActive(false);
                        break;  // No tutorial for this page
                }
            }

        }

        void PreparePageAndCheck(ScriptablePage page)
        {
            if (!page.writeThisTxtUnderButton && !gameOpenedFromSettingsInGame)
            {
                scrollRect.verticalNormalizedPosition = 1f;
                gameOpenedFromSettingsInGame = false;
            }

            if (page.HaveEffectAudio || SoundEffectManager.soundEffectIsPlaying)
            {
                Model.soundEffectManager.LoadSoundEffectDuringReadingPhase(page);
            }

        }

        void LaunchFirstTimeTutorial()
        {
            tutorialSystemManager.RefreshDataForTheTutorial();
            panelTutorialRequest.gameObject.SetActive(false);
            panelTutorialContainer.gameObject.SetActive(true);
        }
        void AnalyzePage(ScriptablePage page)
        {
            Notify(MVCEvents.ANALYZE_PAGE, page);
        }

        public void OnlyImage(ScriptablePage page)
        {
            foreach (ChoiceButtonFeatures choiceButtonFeatures in page.choicesButtons)
            {
                SkipElementButtonManager changeByTouch = Instantiate(btnChangeByTouch, chapterContainer);
                changeByTouch.Initialize(choiceButtonFeatures.pageID);
            }
        }

        public void LoadAnalyzeButton(ChoiceButtonFeatures choiceButtonFeatures)
        {
            if (PlayerManager.Singleton.currentPage.chapterSection == Section.Titolo) { return; }

            ChoiceButton analyzeButton = Instantiate(btnOptionPrefab, chapterContainer);
            analyzeButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(LocalizationIDDatabase.BTN_ANALYZE);
            analyzeButton.Initialize(choiceButtonFeatures.GetAnalyzeButtonID, choiceButtonFeatures.clickAndDrop, choiceButtonFeatures.loadStatsChangedPanel, false, choiceButtonFeatures.audio, choiceButtonFeatures.borderColor);
        }

        public void LoadAButton(ChoiceButtonFeatures choiceButtonFeatures, bool isCourageButton = false)
        {
            if (PlayerManager.Singleton.currentPage.chapterSection == Section.Titolo) { return; }

            Color32 btnColor = choiceButtonFeatures.borderColor;

            if (isCourageButton)
            {
                btnColor = ColorDatabase.Singleton.courageColor;
            }

            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);
            tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, choiceButtonFeatures.loadStatsChangedPanel, false, choiceButtonFeatures.audio, btnColor, choiceButtonFeatures.attackButton, choiceButtonFeatures.deathButton);
        }

        public void LoadDifferentAfterRepeat(ScriptablePage.ChoiceButtonFeatures choice)
        {
            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choice.buttonText);
            tmpButton.Initialize(choice.afterRepeatThisPageID, choice.clickAndDrop, false);
        }

        public bool isLock;
        public void TryUnlockButton(ScriptablePage.ChoiceButtonFeatures choiceButtonFeatures, ScriptablePage page)
        {
            switch (page.lockedChoiceType)
            {
                case LockedChoiceType.noReadPage:
                    break;
                case ScriptablePage.LockedChoiceType.noRightItem:

                    if (PlayerManager.Singleton.GetInventoryItemById(choiceButtonFeatures.keyItemID))
                    {
                        ScriptableItem tmpItem = PlayerManager.Singleton.GetInventoryItemById(choiceButtonFeatures.keyItemID);
                        ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                        tmpButton.btnImage.enabled = true;
                        isLock = true;
                        tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, true);
                        tmpButton.button.onClick.AddListener(delegate { ShowItemKeyMessage(tmpItem); });
                        tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);
                    }
                    else
                    {
                        ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                        tmpButton.btnShowNoItemMessage.enabled = true;
                        tmpButton.btnImage.enabled = true;
                        isLock = false;
                        //tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, true);
                        tmpButton.btnShowNoItemMessage.onClick.AddListener(delegate { ShowItemKeyMessage(null); });
                        tmpButton.button.interactable = true;
                        tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);
                    }
                    break;
                case LockedChoiceType.toLowCourage:

                    if (PlayerManager.Singleton.courage < page.lockedChoiceConditions.courageToUnlock)
                    {
                        ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                        tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.noCourageText);
                        tmpButton.DeactivateButton(LockedChoiceType.toLowCourage, true);
                        tmpButton.SetOnlyColor(ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.COURAGE));
                    }
                    else
                    {
                        if (choiceButtonFeatures.loadDifferentID != ScriptablePage.LoadDifferentID.None)
                        {
                            LoadDifferentID(choiceButtonFeatures, ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.COURAGE));
                        }
                        else
                        {
                            LoadAButton(choiceButtonFeatures, true);
                        }
                    }
                    break;
                case LockedChoiceType.workingInprogress:
                    ChoiceButton buttonWorkInProgress = Instantiate(btnOptionPrefab, chapterContainer);
                    buttonWorkInProgress.GetComponentInChildren<TMP_Text>().text = Localization.Get("workingInProgress");
                    buttonWorkInProgress.DeactivateButton(LockedChoiceType.workingInprogress, true);
                    buttonWorkInProgress.SetOnlyColor(Color.white, Color.white, Color.black, true);
                    break;
                default:
                    Debug.LogWarning("Probabilmente non hai selezionato il tipo di LockedChoiceType");
                    break;
            }
        }

        public IEnumerator WaitTwelveSecond()
        {
            yield return new WaitForSeconds(10f);
        }


        void ShowItemKeyMessage(ScriptableItem tmpItem)
        {
            Notify(MVCEvents.SHOW_UNLOCK_BTN_MESSAGE, tmpItem);
        }

        public void LoadDifferentID(ChoiceButtonFeatures optionsFeature, Color32? originalColor = null)
        {
            switch (optionsFeature.loadDifferentID)
            {
                case ScriptablePage.LoadDifferentID.Lucky:
                    LoadIDByLucky(optionsFeature, originalColor);
                    break;
                case ScriptablePage.LoadDifferentID.Superstition:
                    LoadNewIDBySuperstition(optionsFeature);
                    break;
                case ScriptablePage.LoadDifferentID.LuckyAndSuperstition:
                    LoadIDByLuckAndSuperstition(optionsFeature);
                    break;
                case ScriptablePage.LoadDifferentID.Difficult:
                    LoadIDByDifficult(optionsFeature);
                    break;
                case ScriptablePage.LoadDifferentID.Casual:
                    LoadCasualID(optionsFeature);
                    break;
            }
        }

        public void LoadIDByLuckAndSuperstition(ChoiceButtonFeatures choiceButtonFeatures)
        {
            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);

            int luckyDivider = choiceButtonFeatures.minAmountLucky;
            int luckyAmount = GetRandomNumber(luckyDivider, 60);

            if (PlayerManager.Singleton.lucky < luckyAmount)
            {
                tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, false, false, null, ColorDatabase.Singleton.luckyColor);
            }
            else
            {
                // Tengo colore della fortuna che è il primo riferimento
                if (PlayerManager.Singleton.superstition > 50)
                {
                    tmpButton.Initialize(choiceButtonFeatures.highSuperstitionPageID, choiceButtonFeatures.clickAndDrop, false, false, null, ColorDatabase.Singleton.luckyColor, choiceButtonFeatures.attackButton);
                }
                else
                {
                    tmpButton.Initialize(choiceButtonFeatures.minSuperstitionPageID, choiceButtonFeatures.clickAndDrop, false, false, null, ColorDatabase.Singleton.luckyColor, choiceButtonFeatures.attackButton);
                }
            }
        }

        public void LoadNewIDBySuperstition(ChoiceButtonFeatures choiceButtonFeatures)
        {
            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);

            if (PlayerManager.Singleton.superstition > 50)  //se sup alta carica altro ID
            {
                tmpButton.Initialize(choiceButtonFeatures.highSuperstitionPageID, choiceButtonFeatures.clickAndDrop, false, false, null, ColorDatabase.Singleton.supertitionColor, choiceButtonFeatures.attackButton);
            }
            else
            {
                tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, false, false, null, ColorDatabase.Singleton.supertitionColor, choiceButtonFeatures.attackButton);
            }
        }

        public void LoadIDByLucky(ChoiceButtonFeatures choiceButtonFeatures, Color32? originalColor = null)
        {
            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);
            int luckyDivider = choiceButtonFeatures.minAmountLucky;

            int luckyAmount = GetRandomNumber(luckyDivider, 60); //30%di variazione

            Color32 color = ColorDatabase.Singleton.luckyColor;
            if (originalColor != null)
            {
                color = (Color32)originalColor;
            }

            if (PlayerManager.Singleton.lucky > luckyAmount)  //se sup alta carica altro ID
            {
                tmpButton.Initialize(choiceButtonFeatures.LuckyPageID, choiceButtonFeatures.clickAndDrop, false, false, null, color);
            }
            else
            {
                tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, false, false, null, color);
            }
        }

        public int GetRandomNumber(float x, float variationPercentage)
        {
            float minValue = x - (x * variationPercentage / 100f);  // Calcola il valore minimo
            float maxValue = x + (x * variationPercentage / 100f);  // Calcola il valore massimo

            float randomValue = Random.Range(minValue, maxValue);

            return (int)Mathf.Clamp(randomValue, 0f, 100f);
        }

        public void LoadIDByDifficult(ScriptablePage.ChoiceButtonFeatures choiceButtonFeatures)
        {
            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);

            switch (PlayerManager.Singleton.selectedDifficulty)
            {
                case PlayerManager.Difficulty.Coward:
                    tmpButton.Initialize(choiceButtonFeatures.cowardDifficult, choiceButtonFeatures.clickAndDrop, false, false, null, choiceButtonFeatures.borderColor);
                    break;
                case PlayerManager.Difficulty.Fearless:
                    tmpButton.Initialize(choiceButtonFeatures.fearlessDifficult, choiceButtonFeatures.clickAndDrop, false, false, null, choiceButtonFeatures.borderColor);
                    break;
                case PlayerManager.Difficulty.Insane:
                    tmpButton.Initialize(choiceButtonFeatures.insaneDifficult, choiceButtonFeatures.clickAndDrop, false, false, null, choiceButtonFeatures.borderColor);
                    break;
                default:
                    Debug.Log("Difficoltà non selezionata");
                    tmpButton.Initialize(choiceButtonFeatures.fearlessDifficult, choiceButtonFeatures.clickAndDrop, false);
                    break;
            }
        }


        public void LoadCasualID(ScriptablePage.ChoiceButtonFeatures choiceButtonFeatures)
        {
            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);

            int casualValue = UnityEngine.Random.Range(0, 101);

            if (casualValue > 50)  //se sup alta carica altro ID
            {
                tmpButton.Initialize(choiceButtonFeatures.CasualPageID, choiceButtonFeatures.clickAndDrop, choiceButtonFeatures.loadStatsChangedPanel, false, null, choiceButtonFeatures.borderColor);
            }
            else
            {
                tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, choiceButtonFeatures.loadStatsChangedPanel, false, null, choiceButtonFeatures.borderColor);
            }
        }

        public void HideButtonAlreadyClicked(ScriptablePage.ChoiceButtonFeatures choiceButton, ScriptablePage page)
        {
            if (PlayerManager.Singleton.pagesRead.Contains(choiceButton.pageID) || PlayerManager.Singleton.pagesRead.Contains(LocalizationIDDatabase.FEM_PREFIX + choiceButton.pageID))
            {
                Debug.LogFormat("Hai già letto {0}", choiceButton.pageID);
                return;
            }

            if (choiceButton.loadDifferentID != ScriptablePage.LoadDifferentID.None)
            {
                LoadDifferentID(choiceButton);
            }
            else
            {
                LoadAButton(choiceButton);
            }
        }

        public void Intro(ScriptableRiddle riddle, ScriptablePage page)
        {
            foreach (MultipleChoice choice in riddle.multipleChoice)
            {
                ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choice.buttonText);
                tmpButton.Initialize(null, false, false, false);

                Debug.Log("PASSARE NULL in INITIALIZE PER FAR PARTIRE LA FUNZIONE DOPO");
                //tmpButton.Initialize(choice.pageID, choice.clickAndDrop, false);

                //AVVIA
                string buttonID = choice.buttonText;
                string pageID = choice.pageID;
                tmpButton.SetFunctionForTheButton(() => IntroAnswer(buttonID, pageID));
                //AddPointToTheAnswer(tmpButton, choice.pageID, choice.buttonText);
            }
        }

        void IntroAnswer(string buttonID, string pageID)
        {
            app.model.BookModel.longQuestionDatabase.IntroQuestionRiddle(buttonID); //attacco a component nuova classe 
            LoadPage(pageID);
        }

        //nuovo

        int attempsNumber = 0; //number of attemp in choice

        public void ShowRiddle(ScriptableRiddle riddle, ScriptablePage page)
        {
            if (riddle.riddleType == ScriptableRiddle.RiddleType.SceltaMultipla)
            {
                switch (riddle.multipleChoiceQuestions)
                {
                    case ScriptableRiddle.MultipleChoiceQuestion.introQuestion:
                        Intro(riddle, page);
                        break;
                    default:

                        break;
                }

            }

            else if (riddle.riddleType == ScriptableRiddle.RiddleType.Fortuna)
            {
                foreach (ScriptableRiddle.LuckyButton luckyButton in riddle.luckyButton)
                {
                    ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                    tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(luckyButton.btnLuckyText);
                    tmpButton.Initialize(null, false, false, false, luckyButton.audioClip, ColorDatabase.Singleton.luckyColor);
                    LuckyGames(page, tmpButton.button, luckyButton);
                }
                //controllo quali enigma c'è , se nn è un gioco si va di fortuna
            }

            else if (riddle.riddleType == ScriptableRiddle.RiddleType.InserimentoTestuale)
            {
                GameObject inputFieldContainer = Instantiate(inputFieldPrefabContainer, chapterContainer);
                ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                TMP_InputField inputField = inputFieldContainer.GetComponentInChildren<TMP_InputField>();
                tmpButton.button.interactable = false;
                tmpButton.GetComponentInChildren<TMP_Text>().text = riddle.GetLocalizedText();
                inputField.onValueChanged.AddListener(delegate { CheckIfTheAnswerIsRight(inputField, tmpButton); });
                tmpButton.button.onClick.AddListener(delegate { CheckTheSolution(riddle, inputField); });
            }

            else if (riddle.riddleType == ScriptableRiddle.RiddleType.InserimentoNumerico)
            {
                GameObject inputFieldContainer = Instantiate(inputFieldPrefabContainer, chapterContainer);
                TMP_InputField inputField = inputFieldContainer.GetComponentInChildren<TMP_InputField>();
                ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                StartCoroutine(CheckIfEndOfPageReached(PlayerManager.Singleton.currentPage.pageID));
                tmpButton.DeactivateButton(LockedChoiceType.waitingForPlayerInput, true);
                tmpButton.SetOnlyColor(Color.white, Color.white, Color.black, true);
                tmpButton.GetComponentInChildren<TMP_Text>().text = riddle.GetLocalizedText();
                //tmpInputField.text = "Scrivi ehm...qua";
                inputField.onValueChanged.AddListener(delegate { CheckIfTheNumberIsRight(inputField, tmpButton, riddle.GetLocalizedText()); });
                tmpButton.button.onClick.AddListener(delegate { CheckTheSolution(riddle, inputField); });
            }

            else if (riddle.riddleType == ScriptableRiddle.RiddleType.SerieDiScelteCorrette)
            {
                attempsNumber++;

                foreach (ScriptableRiddle.RightPreviousChoice choice in riddle.rightPreviousChoice)
                {
                    ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                    tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choice.btnText);
                    if (attempsNumber > riddle.MinNumberOfAttemps)
                    {
                        tmpButton.Initialize(choice.finishAttempsID, false, false);
                        continue;
                    }
                    if (choice.normalButton)
                    {
                        tmpButton.Initialize(choice.repeatID, false, false);
                        Debug.Log("Tasto normale");
                    }
                    else
                    {
                        if (CheckRightChoices(choice, riddle))
                        {
                            tmpButton.Initialize(choice.winID, false, false);
                        }
                        else
                        {
                            if (loseClicked == choice.buttonToClick)
                            {
                                tmpButton.Initialize(choice.loseID, false, false);
                            }
                            else
                            {
                                tmpButton.Initialize(choice.repeatID, false, false);
                            }
                        }
                    }

                }
                PlayerManager.Singleton.buttonClicked.RemoveRange(PlayerManager.Singleton.buttonClicked.Count - riddle.btnToDelete, riddle.btnToDelete);
            }
        }


        void LuckyGames(ScriptablePage page, Button tmpButton, ScriptableRiddle.LuckyButton choice)
        {
            switch (page.pageID)
            {
                case PageIDDatabase.TUTORIALLUCKYID:
                    tmpButton.onClick.AddListener(delegate { StartCoroutine(PlayRolls(page, choice)); });
                    break;
                case "giochiCarte":
                    tmpButton.onClick.AddListener(delegate { PlayCard(choice); });
                    break;
                case "giochiCartept2":
                    tmpButton.onClick.AddListener(delegate { PlayCard(choice); });
                    break;

                default:
                    if (PlayerManager.Singleton.lucky <= 50)
                    {
                        LoadPage(choice.pageLoseID);
                    }
                    else
                    {
                        LoadPage(choice.pageWinID);
                    }
                    break;
            }

        }

        int rightClicked;
        int loseClicked;
        bool CheckRightChoices(ScriptableRiddle.RightPreviousChoice choice, ScriptableRiddle riddle)
        {
            rightClicked = 0;
            loseClicked = 0;

            riddle.buttonLoseIDList.Clear();
            riddle.buttonWinIDList.Clear();

            RiddleBtnInList(riddle, choice);

            if (choice.rightBtnID.Length > 0)
            {
                RiddleBtnClicked(riddle.buttonWinIDList, true);
            }
            if (choice.loseBtnID.Length > 0)
            {
                RiddleBtnClicked(riddle.buttonLoseIDList, false);
            }

            if (rightClicked >= choice.buttonToClick)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        void RiddleBtnInList(ScriptableRiddle riddle, ScriptableRiddle.RightPreviousChoice choice)
        {
            if (Localization.language == "Italiano")
            {
                foreach (string ID in choice.rightBtnID)
                {
                    riddle.buttonWinIDList.Add(ID);
                }
                foreach (string ID in choice.loseBtnID)
                {
                    riddle.buttonLoseIDList.Add(ID);
                }
            }
            else
            {
                foreach (string ID in choice.rightBtnIDEnglish)
                {
                    riddle.buttonWinIDList.Add(ID);
                }
                foreach (string ID in choice.loseBtnIDEnglish)
                {
                    riddle.buttonLoseIDList.Add(ID);
                }
            }
        }

        //test
        void RiddleBtnClicked(List<string> btnList, bool winList)
        {
            foreach (string buttonToClick in btnList)
            {
                if (PlayerManager.Singleton.buttonClicked.Contains(buttonToClick)) //errore qua alla ripetiuzione sicuramente ci sarà
                {
                    if (winList)
                    {
                        rightClicked++;
                    }
                    else
                    {
                        loseClicked++;
                    }
                }
            }
        }

        public void CheckButtonClicked(ScriptablePage page)
        {
            foreach (ChoiceButtonFeatures optionsFeature in page.choicesButtons)
            {
                if (optionsFeature.changeIDByButtonClicked)
                {
                    foreach (string btnToClickID in optionsFeature.buttonToClick)
                    {
                        if (PlayerManager.Singleton.buttonClicked.Contains(btnToClickID))
                        {
                            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(optionsFeature.buttonText);
                            tmpButton.Initialize(optionsFeature.playerClickedTheButton, optionsFeature.clickAndDrop, false);
                        }
                    }
                }
            }
        }

        IEnumerator PlayRolls(ScriptablePage page, ScriptableRiddle.LuckyButton choice)
        {
            result.gameObject.SetActive(false);
            popupOrVideoIsClosed = false;
            int firstRollDices = GetRandomIntBetween(1, 12);
            result.text = firstRollDices.ToString();
            resultBackGround.gameObject.SetActive(true);
            //diceResultPopUp.gameObject.SetActive(true);
            DicesResultManager();

            yield return new WaitUntil(GetPopupOrVideoIsClosed);

            //yield return new WaitUntil(VideoPlayerFinish);

            if ((firstRollDices > 9) || (firstRollDices < 4))
            {
                foreach (StatsVariation variation in page.statsVariations)
                {
                    ShowChangedStats(-variation.buffDebuffAmount, variation.iconName.ToString());
                }
                panelStatsAreChanged.gameObject.SetActive(true);
                yield return new WaitUntil(PlayerClosedStatsPanel);
                DestroyChangedStats();
                LoadPage(choice.pageLoseID);
                GameApplication.Singleton.app.Notify(MVCEvents.LOAD_STATUS, choice.pageLoseID);
            }
            else
            {
                foreach (StatsVariation variation in page.statsVariations)
                {
                    ShowChangedStats(variation.buffDebuffAmount, variation.iconName.ToString());
                }
                panelStatsAreChanged.gameObject.SetActive(true);
                yield return new WaitUntil(PlayerClosedStatsPanel);
                DestroyChangedStats();
                LoadPage(choice.pageWinID);
                GameApplication.Singleton.app.Notify(MVCEvents.LOAD_STATUS, choice.pageWinID);
            }
        }

        bool PlayerClosedStatsPanel() { return !panelStatsAreChanged.gameObject.activeSelf; }

        int myTotalCardValue = 0;
        void PlayCard(ScriptableRiddle.LuckyButton choice)
        {
            string draft = Localization.Get("giochiCartetst");
            string draftAgain = Localization.Get("giochiCartept2tst");
            string stopHere = Localization.Get("giochiCartept3tst");

            if ((choice.btnLuckyText == "giochiCartetst") || (choice.btnLuckyText == "giochiCartept2tst"))
            {
                int playerCard = GetRandomIntBetween(1, 9);

                if (myTotalCardValue == 0)
                {
                    myTotalCardValue = playerCard;
                }
                else
                {
                    myTotalCardValue = myTotalCardValue + playerCard;
                }

                resultBackGround.gameObject.SetActive(true);
                btnHideoPopUp.gameObject.SetActive(true);
                StartCoroutine(GeneralRiddleResultsManager());
                result.text = myTotalCardValue.ToString();
                result.text = string.Format(Localization.Get("resultCard"), myTotalCardValue);

                if (myTotalCardValue >= 9)
                {
                    result.text = string.Format(Localization.Get("cardLoseOver9"), myTotalCardValue);
                    Debug.LogFormat("Hai perso. Hai fatto {0}", result); LoadPage(choice.pageLoseID);
                }
                else
                {
                    LoadPage(choice.pageWinID);
                }
            }

            else
            {
                int moribondCard;
                if (PlayerManager.Singleton.lucky > 80)
                {
                    moribondCard = GetRandomIntBetween(1, 3);
                }
                else if ((PlayerManager.Singleton.lucky < 80) && (PlayerManager.Singleton.lucky > 30))
                {
                    moribondCard = GetRandomIntBetween(1, 9);
                }
                else
                {
                    moribondCard = GetRandomIntBetween(6, 9);
                }

                result.text = myTotalCardValue.ToString();

                if (myTotalCardValue <= moribondCard)
                {
                    if (myTotalCardValue == moribondCard)
                    {
                        result.text = string.Format(Localization.Get("cardPair"), myTotalCardValue);
                    }
                    else
                    {
                        result.text = string.Format(Localization.Get("cardLose"), myTotalCardValue, moribondCard);
                    }
                    LoadPage(choice.pageLoseID);
                }
                else
                {
                    result.text = string.Format(Localization.Get("cardWin"), myTotalCardValue, moribondCard);
                    LoadPage(choice.pageWinID);
                }

                resultBackGround.gameObject.SetActive(true);
                btnHideoPopUp.gameObject.SetActive(true);
                StartCoroutine(GeneralRiddleResultsManager());
            }
        }

        bool popupOrVideoIsClosed;
        bool GetPopupOrVideoIsClosed() { return popupOrVideoIsClosed; }
        void DicesResultManager()
        {
            result.gameObject.SetActive(true);

            btnHideoPopUp.gameObject.SetActive(true);
            btnHideoPopUp.onClick.RemoveAllListeners();
            btnHideoPopUp.onClick.AddListener(delegate
            {
                resultBackGround.gameObject.SetActive(false);
                //resultPopUp.gameObject.SetActive(false);
                btnHideoPopUp.gameObject.SetActive(false);
                popupOrVideoIsClosed = true;
            }
            );
        }

        IEnumerator GeneralRiddleResultsManager()
        {
            yield return new WaitForSeconds(2);
            btnHideoPopUp.gameObject.SetActive(true);
            btnHideoPopUp.onClick.RemoveAllListeners();
            btnHideoPopUp.onClick.AddListener(delegate
            {
                resultBackGround.gameObject.SetActive(false);
                btnHideoPopUp.gameObject.SetActive(false);
            }
            );
        }

        public void CheckIfTheNumberIsRight(TMP_InputField inputField, ChoiceButton tmpButton, string localizedButtontext)
        {
            if (inputField.text.Length > 0)
            {
                tmpButton.button.interactable = true;
                tmpButton.SetDefaultColor();
                tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get("correct");
            }
            else
            {
                tmpButton.DeactivateButton(LockedChoiceType.waitingForPlayerInput, true);
                tmpButton.SetOnlyColor(Color.white, Color.white, Color.black, true);
                tmpButton.GetComponentInChildren<TMP_Text>().text = localizedButtontext;
            }
        }

        int playerAttempt = 0;

        void CheckTheSolution(ScriptableRiddle riddle, TMP_InputField inputField)
        {
            playerAttempt++;

            foreach (string playerSolution in riddle.writeSolution.rightNumberStringSolution)
            {
                if (inputField.text == playerSolution.ToString())
                {
                    playerAttempt = 0;
                    if (PlayerManager.Singleton.superstition < 50)
                    {
                        LoadPage(riddle.writeSolution.winPageID);
                        return;
                    }
                    else
                    {
                        LoadPage(riddle.writeSolution.winPageID);
                        return;
                    }
                }
                else if ((playerAttempt >= riddle.writeSolution.numberOfAttempt) && (riddle.writeSolution.numberOfAttempt != 0))
                {
                    playerAttempt = 0;
                    if (PlayerManager.Singleton.superstition < 50)
                    {
                        LoadPage(riddle.writeSolution.losePageID);
                    }
                    else
                    {
                        LoadPage(riddle.writeSolution.highSuperstitionLosePageID);
                    }
                }
                else
                {
                    if (riddle.writeSolution.numberOfAttempt == 0)
                    {
                        if (PlayerManager.Singleton.superstition < 50)
                        {
                            LoadPage(riddle.writeSolution.losePageID);
                        }
                        else
                        {
                            LoadPage(riddle.writeSolution.highSuperstitionLosePageID);
                        }
                    }
                    else
                    {
                        LoadPage(riddle.writeSolution.repeatID);
                    }
                }
            }

        }

        public void CheckIfTheAnswerIsRight(TMP_InputField inputField, ChoiceButton choiceButton)
        {
            if (inputField.text.Length > 0)
            {
                choiceButton.button.interactable = true;
                choiceButton.GetComponentInChildren<TMP_Text>().text = Localization.Get("correct");
            }
        }

        int enoughPageReaded = 1;
        public void UnlockHideButton(ScriptablePage page, ScriptablePage.ChoiceButtonFeatures choiceButtonFeatures)
        {
            if (page.showHideButtonRequirementsType == ScriptablePage.ShowHideButtonType.readedPages)
            {
                foreach (string pageID in page.showHideButtonConditions.pageToUnlockShowBtn)
                {
                    if (PlayerManager.Singleton.pagesRead.Contains(pageID) || PlayerManager.Singleton.pagesRead.Contains(LocalizationIDDatabase.FEM_PREFIX + pageID))
                    {
                        enoughPageReaded++; //col foreach faccio (primo cliclo non entra/+1+1/+1+1 quindi minimo è 3 max 4
                        int result = enoughPageReaded;
                        if (result == page.showHideButtonConditions.numberPageToUnlock) //lascio uguale e nn maggiore perchè tanto passa comunque 
                        {
                            ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                            tmpButton.GetComponentInChildren<TMP_Text>().text = Localization.Get(choiceButtonFeatures.buttonText);

                            if (choiceButtonFeatures.loadDifferentID != ScriptablePage.LoadDifferentID.None)
                            {
                                LoadDifferentID(choiceButtonFeatures);
                            }
                            else
                            {
                                tmpButton.Initialize(choiceButtonFeatures.pageID, choiceButtonFeatures.clickAndDrop, false);
                            }
                            enoughPageReaded = 1;
                        }
                    }
                }
                //Correzione bug in combinazione con stats / equip view
                enoughPageReaded = 1;
            }

            else if (page.showHideButtonRequirementsType == ScriptablePage.ShowHideButtonType.enoughCourage)
            {
                if (PlayerManager.Singleton.courage > page.showHideButtonConditions.courageToUnlock)
                {
                    ChoiceButton tmpButton = Instantiate(btnOptionPrefab, chapterContainer);
                }
            }
        }

        public int GetRandomIntBetween(int min, int max)
        {
            int randomNumber = Random.Range(min, max);
            return randomNumber;
        }
        public void CheckLongQuestion(string selectedBtnID)
        {
        }

        public string GetPageText(ScriptablePage page, string ID)
        {
            if (page.localizationValues.containPlayerName)
            {
                return string.Format(Localization.Get(ID), PlayerManager.Singleton.GetPlayerName());
            }
            else if (page.localizationValues.containClass)
            {
                return string.Format(Localization.Get(ID), PlayerManager.Singleton.GetPlayerClass());
            }
            else if (page.localizationValues.containARandomString)
            {
                return string.Format(Localization.Get(ID), page.randomString);
            }
            else if (page.localizationValues.isCrone)
            {
                string cronePageID = string.Format(ID + "_C");
                return Localization.Get(cronePageID);
            }
            else if (page.localizationValues.isAbbot)
            {
                string abbotPageID = string.Format(ID + "_A");
                return Localization.Get(abbotPageID);
            }
            else if (page.localizationValues.isHunter)
            {
                string hunterPageID = string.Format(ID + "_H");
                return Localization.Get(hunterPageID);
            }
            else if (page.localizationValues.isTrafficker)
            {
                string traffickerPageID = string.Format(ID + "_T");
                return Localization.Get(traffickerPageID);
            }
            return Localization.Get(ID);
        }

        bool CheckString(string pageID, string ID)
        {
            if (ID == pageID)
            {
                return true;
            }
            return false;
        }

        void UIPageElementManager(ScriptablePage page)
        {
            if ((!page) || (page.chapterSection == Section.Titolo)
               || (page.chapterSection == Section.Fight))
            { return; }

            if (PlayerManager.Singleton.selectedClass)
            {
                UpdateClassUIElements();
            }

            ManageBookmark(page);
            ManageGems(page);
            ManageTorchUI(page);
            Model.subtitleManager.LoadSubtitle(page);
            Model.loadSpriteAsset.LoadCurrentChapterSpriteAtlas();
        }

        void ManageBookmark(ScriptablePage page)
        {
            if (page.chapterSection == Section.Titolo || page.chapterSection == Section.Fight)
            {
                objBookmark.SetActive(false);
                return;
            }
            objBookmark.SetActive(true);



            if (!PlayerManager.Singleton.pagesRead.Contains(PageIDDatabase.TUTORIALFIRSTPAGE))
            {
                if (previowsPage == null) return;
                if (previowsPage.chapterSection != page.chapterSection)
                {
                    BookmarkManager.Singleton.SetBookmarkIcon();
                }
                //se lo rimuovi il giocatore può rimuovere a piacimento prima della fine della scelta classe il segnalibro
                return;
            }
            var bookmarkActive = app.view.Settings.toggleShowBookmark.isOn ? 0 : 1;

            if (bookmarkActive == 0)
            {
                objBookmark.SetActive(true);
                if (ContinueView.StartGameFromContinue)
                {
                    BookmarkManager.Singleton.SetBookmarkIcon();
                    return;
                }

                if (previowsPage == null) return;
                if (previowsPage.chapterSection != page.chapterSection)
                {
                    BookmarkManager.Singleton.SetBookmarkIcon();
                }
            }
            else objBookmark.SetActive(false);
        }

        void ManageGems(ScriptablePage page)
        {
            if (!PlayerManager.Singleton.pagesRead.Contains(PageIDDatabase.TUTORIALFIRSTPAGE))
            {
                objGems.SetActive(false);
                return;
            }

            if (PlayerPrefs.GetInt(SettingsMenuView.SHOWGEMS) == 0)
            {
                objGems.SetActive(true);
                app.view.Settings.toggleShowGems.isOn = true;

                if (SettingsMenuView.UPDATEGEMS)
                {
                    SettingsMenuView.UPDATEGEMS = false;
                    StartCoroutine(WaitUIStatsManagerNotNull());
                }
            }
            else
            {
                objGems.SetActive(false);
                app.view.Settings.toggleShowGems.isOn = false;
            }
        }

        IEnumerator WaitUIStatsManagerNotNull()
        {
            yield return new WaitUntil(BookController.UIStatsManagerIsActived);
            UIStatsManager.Singleton.UpdateGems();
        }

        void ManageTorchUI(ScriptablePage page)
        {
            if (!PlayerManager.Singleton.pagesRead.Contains(PageIDDatabase.TORCHPAGEID))
            {
                return;
            }
            if (!PlayerManager.Singleton.PlayerIsDead())
            {
                ShowTorchTxt();
            }

            btnEmptySearch.gameObject.SetActive(false);
            btnSearch.gameObject.SetActive(true);
            btnSearch.enabled = true;
        }


        public IEnumerator ShowDivider(float time)
        {
            yield return new WaitForSeconds(time);
            mapsVideo.CrossFadeAlpha(1, 2, false);
            txtChapter.gameObject.SetActive(false);
            txtChapterSubtitle.gameObject.SetActive(false);
            StartCoroutine(ActivateText(0));
        }

        public IEnumerator ActivateText(float time)
        {
            yield return new WaitForSeconds(time);
            txtChapter.gameObject.SetActive(true);
            txtChapterSubtitle.gameObject.SetActive(true);
        }

        public void SavePopUp()
        {
            //Automatic hard save
            if (PlayerManager.Singleton.currentPage.title != ScriptablePage.Title.AllaSalute)
            {
                GameApplication.Singleton.model.SaveNotificationManager.ShowSavePopUp();
                Debug.Log("HARD SAVE " + Application.persistentDataPath);
                // SaveSystem.SavePlayer(SaveType.Hard);
            }
        }

        static int UILifecounter;
        static int minLifeCounter;
        static int UIExpcounter;
        static int minExpCounter;
        public void OnChangeLife(float currentLifePoint, float constitution)
        {
            float maxHp = constitution * 10;
            float singleValue = maxHp / UILifecounter;

            float UICount = currentLifePoint / singleValue;

            foreach (var item in lifeUIContainer)
            {
                item.gameObject.SetActive(false);
            }
            float count = UICount + minLifeCounter - 1;

            for (int i = minLifeCounter; i <= count; i++)
            {
                lifeUIContainer[i].SetActive(true);
            }

            if (currentLifePoint > 0) { lifeUIContainer[minLifeCounter].SetActive(true); }
        }

        bool ConstitutionDataLoaded()
        {
            return PlayerManager.Singleton.constitution > 0;
        }

        public void UpdateLevelUI()
        {
            float playerExperience = PlayerManager.Singleton.GetPlayerExp();
            int playerLevel = PlayerManager.Singleton.GetPlayerLevel;
            int containerToFill = LevelManagerUtilities.ConvertExperienceForUI(playerExperience, playerLevel);

            foreach (var item in expUIContainer)
            {
                item.gameObject.SetActive(false);
            }

            for (int i = 0; i <= containerToFill; i++)
            {
                expUIContainer[i].SetActive(true);
            }
        }

        void LoadTextChangingPage(ScriptablePage page)
        {
            if ((!page.writeThisTxtUnderButton) || (page.chapterSection == ScriptablePage.Section.Titolo))
            {
                DestroyAllPageGameObj();
            }
            else if (page.writeThisTxtUnderButton)
            {
                BlockAllClickedButton();
            }
        }

        public void DestroyAllPageGameObj()
        {
            foreach (Transform child in chapterContainer)
            {
                Destroy(child.gameObject);
            }
        }
        public void BlockAllClickedButton()
        {
            foreach (Transform child in chapterContainer)
            {
                if (child.name == "ChoiceButtonPrefab(Clone)")
                {
                    child.GetComponent<ChoiceButton>().alreadyPressed = true;
                }
            }
        }


        public Camera mainCamera;
        public float yOffesetButtonBorderValue;
        public bool AllButtonsAreVisibles()
        {
            //chapterContainer.transform.("ChoiceButtonPrefab(Clone)");
            uint count = 0;
            uint visibleButtons = 0;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            float fUiUnitToWorldUnit = Screen.height / (mainCamera.orthographicSize * 2);
            foreach (Transform child in chapterContainer)
            {
                if (child.name == "ChoiceButtonPrefab(Clone)")
                {
                    count++;
                    Bounds buttonBounds = new(new Vector3(child.position.x, child.position.y + yOffesetButtonBorderValue, child.position.z),
                                           new Vector3(child.GetComponent<RectTransform>().sizeDelta.x / fUiUnitToWorldUnit, child.GetComponent<RectTransform>().sizeDelta.y / fUiUnitToWorldUnit, 1));
                    if (!GeometryUtility.TestPlanesAABB(planes, buttonBounds))
                    {
                        Debug.Log("NotVisible");
                        return false;
                    }
                    visibleButtons++;
                    Debug.Log("Visible");
                }
            }
            if (count == 0)
            {
                return false;
            }
            if (count == visibleButtons)
            {
                return true;
            }
            return false;
        }


        public void UnlockAllClickedButton()
        {
            foreach (Transform child in chapterContainer)
            {
                if (child.name == "ChoiceButtonPrefab(Clone)")
                {
                    ChoiceButton btn = child.GetComponent<ChoiceButton>();
                    btn.buttonBlockIsActived = false;
                    btn.imgBlock.enabled = false;
                }
            }
        }


        #region PopUpManager

        [SerializeField] List<Texture2D> popupImage = new List<Texture2D>();
        public List<Texture2D> GetPopupImage { get => popupImage; }
        public bool imageShowed = false;
        public bool animatorReady = false;
        void PopupManager(ScriptablePage page)
        {
            Notify(MVCEvents.OPEN_POPUP_MANAGER, page);
        }

        #endregion
    }
}


