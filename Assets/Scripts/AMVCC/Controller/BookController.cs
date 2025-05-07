using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using StarworkGC.Localization;
using System.Runtime.InteropServices.WindowsRuntime;
using AV;
using static Game.ScriptablePage;

namespace Game
{
    public class BookController : Controller<GameApplication>
    {
        BookModel Model { get { return app.model.BookModel; } }
        BookView View { get { return app.view.BookView; } }
        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.LOAD_PAGE, OnLoadPage);
            AddEventListenerToApp(MVCEvents.OPEN_CHARACTER_MENU, OnClickCharacterMenuButton);
            AddEventListenerToApp(MVCEvents.OPEN_EQUIP_VIEW, OnClickEquipButton);
            AddEventListenerToApp(MVCEvents.OPEN_STATS_VIEW, OnClickStatsButton);
            AddEventListenerToApp(MVCEvents.OPEN_INVENTORY_VIEW, OnClickInventoryButton);
            AddEventListenerToApp(MVCEvents.OPEN_DROP_VIEW, OnClickDropButton);
            AddEventListenerToApp(MVCEvents.LOAD_STATUS, UpdatePlayerGeneralStatsOnClick);
            AddEventListenerToApp(MVCEvents.ANALYZE_PAGE, CheckPageFeature);
            AddEventListenerToApp(MVCEvents.OPEN_GAMESETTING_VIEW, OnClickSettingsMenuButton);
            AddEventListenerToApp(MVCEvents.OPEN_POPUP_MANAGER, StartPopupManager);
            AddEventListenerToApp(MVCEvents.SHOW_UNLOCK_BTN_MESSAGE, ShowItemKeyMessage);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.LOAD_PAGE, OnLoadPage);
            RemoveEventListenerFromApp(MVCEvents.OPEN_CHARACTER_MENU, OnClickCharacterMenuButton);
            RemoveEventListenerFromApp(MVCEvents.OPEN_EQUIP_VIEW, OnClickEquipButton);
            RemoveEventListenerFromApp(MVCEvents.OPEN_STATS_VIEW, OnClickStatsButton);
            RemoveEventListenerFromApp(MVCEvents.OPEN_INVENTORY_VIEW, OnClickInventoryButton);
            RemoveEventListenerFromApp(MVCEvents.OPEN_DROP_VIEW, OnClickDropButton);
            RemoveEventListenerFromApp(MVCEvents.LOAD_STATUS, UpdatePlayerGeneralStatsOnClick);
            RemoveEventListenerFromApp(MVCEvents.ANALYZE_PAGE, CheckPageFeature);
            RemoveEventListenerFromApp(MVCEvents.OPEN_GAMESETTING_VIEW, OnClickSettingsMenuButton);
            RemoveEventListenerFromApp(MVCEvents.OPEN_POPUP_MANAGER, StartPopupManager);
            RemoveEventListenerFromApp(MVCEvents.SHOW_UNLOCK_BTN_MESSAGE, ShowItemKeyMessage);
            StopAllCoroutines();
        }

        void SetDifficult(ScriptablePage page)
        {
            switch (PlayerManager.Singleton.selectedDifficulty)
            {
                case PlayerManager.Difficulty.Coward:
                    page.mobID = page.easyDifficultEnemy;
                    break;
                case PlayerManager.Difficulty.Fearless:
                    page.mobID = page.mediumDifficultEnemy;
                    break;
                case PlayerManager.Difficulty.Insane:
                    page.mobID = page.hardDifficultEnemy;
                    break;
                default:
                    break;
            }
        }

        void OnLoadPage(params object[] data)  /// devo fare un Ienumerator che blocco 
        {
            string pageID = (string)data[0];

            ScriptablePage pageToShow = PagesMaleDatabase.Singleton.GetPageByID(pageID);

            if (pageToShow.chapterSection == Section.Titolo || pageToShow.chapterSection == Section.Fight)
            {
                View.DestroyAllPageGameObj();
            }
            else
            {
                View.imgHideFrameDelay.gameObject.SetActive(true);
                StartCoroutine(HideBlockFrameDelayImg());
            }

            if (pageToShow.saveOnOpenPage)
            {
                PlayerManager.Singleton.lastPage = pageToShow.pageID;
                SaveSystem.SavePlayerLastRun(SaveType.Soft);
            }

            int lastPageReadedByPlayerIndex = PlayerManager.Singleton.pagesRead.Count - 1;
            //Correzione bug Pages Read duplicata dopo uscita character menu
            if (pageID != PlayerManager.Singleton.pagesRead[lastPageReadedByPlayerIndex])
            {
                PlayerManager.Singleton.pagesRead.Add(pageID);
                View.pageReadInCurrentRun.text = UIUtility.GetNumberForUI(lastPageReadedByPlayerIndex + 1); //ricordarsi di non aggiungerla se fight o titolo ecc
            }

            View.ShowPage(pageToShow);
        }

        IEnumerator HideBlockFrameDelayImg()
        {
            yield return new WaitForEndOfFrame();
            View.imgHideFrameDelay.gameObject.SetActive(false);
        }

        public void OnClickCharacterMenuButton(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.StatsView;
            app.model.currentView.gameObject.SetActive(true);

        }

        public void OnClickSettingsMenuButton(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.GameSettingsView;
            app.model.currentView.gameObject.SetActive(true);

        }

        //Da stats, equip e drop rimuovo il settaggio della previews view, altrimenti non torna più indietro
        public void OnClickInventoryButton(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                //app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.InventoryView;
            app.model.currentView.gameObject.SetActive(true);

        }

        public void OnClickStatsButton(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                //app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.StatsView;
            app.model.currentView.gameObject.SetActive(true);

        }

        public void OnClickDropButton(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                //app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.DropView;
            app.model.currentView.gameObject.SetActive(true);

        }

        public void OnClickEquipButton(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
            }

            app.model.currentView = app.view.EquipmentView;
            app.model.currentView.gameObject.SetActive(true);
        }

        public void UpdatePlayerGeneralStatsOnClick(params object[] data)
        {
            string pageID = (string)data[0];
            ScriptablePage page = PagesMaleDatabase.Singleton.GetPageByID(pageID);
            SaveModelInPlayerSettings(page);

            if (page.buffAppliedOnCurrentPage)
            {
                //La parte di UI viene fatta al click
                UpdatePlayerGeneralStats(page);
            }
        }

        public void UpdatePlayerGeneralStats(ScriptablePage page)
        {
            foreach (ScriptablePage.PageBuff pageBuff in page.pageBuffes)
            {
                PlayerManager.Singleton.UpdateCostitution(pageBuff.buffConstitution);
                PlayerManager.Singleton.UpdateDexterity(pageBuff.buffDexterity);
                PlayerManager.Singleton.UpdateExperience(pageBuff.buffLevel);
                PlayerManager.Singleton.UpdateLifePoints(pageBuff.buffLifepoint);
                PlayerManager.Singleton.UpdateStrenght(pageBuff.buffStrength);
                PlayerManager.Singleton.UpdateIntelligence(pageBuff.buffInteligence);
                PlayerManager.Singleton.UpdatePlayerManagerAbilityPoints(pageBuff.RecoverAP);

                bool gemsCondition = pageBuff.buffType == ScriptablePage.BuffType.Lucky ||
                                     pageBuff.buffType == ScriptablePage.BuffType.Superstition ||
                                     pageBuff.buffType == ScriptablePage.BuffType.Courage;
                if (gemsCondition)
                {
                    PlayerManager.Singleton.UpdateLucky(pageBuff.buffLucky);
                    PlayerManager.Singleton.UpdateCourage(pageBuff.buffCourage);
                    PlayerManager.Singleton.UpdateSuperstition(pageBuff.buffSuperstition);

                    if (View.objGems.activeSelf)
                    {
                        if (page.chapterSection == ScriptablePage.Section.Death ||
                                page.chapterSection == ScriptablePage.Section.Fight ||
                                page.chapterSection == ScriptablePage.Section.Titolo) return;
                        UIStatsManager.Singleton.UpdateGems();
                        // UIStatsManager.Singleton.ChangeGemsUI(pageBuff.buffType);
                    }
                }
            }

            if (PlayerManager.Singleton.constitution == 0) return;
            View.OnChangeLife(PlayerManager.Singleton.lifePoints, PlayerManager.Singleton.constitution);
        }

        void SaveModelInPlayerSettings(ScriptablePage page)
        {
            PlayerManager.Singleton.lastChapter = page.chapterSection;
        }


        Coroutine waitUIStatsManager;
        Coroutine mapSystemAnimationControllerCoroutine;
        public void CheckPageFeature(params object[] data)
        {
            ScriptablePage page = (ScriptablePage)data[0];
            BookView.CurrentPage = page.pageID;
            View.showCurrentPage = BookView.CurrentPage;
            SetDifficult(page);
            CheckChapterNumber();

            if (page.chapterSection == ScriptablePage.Section.Titolo)
            {
                Model.soundManager.audioSource.Stop(); //stoppo durante il video
                mapSystemAnimationControllerCoroutine = StartCoroutine(ActivateMapAnimationAndChapterCover(page));
            }
            else
            {
                //l'animazione si spegne da sola 
                Model.coverOfTheChapter.TurnOffChapterCover();
            }

            if (gameObject.activeSelf && SettingsMenuView.UPDATEGEMS)
            {
                if(page.chapterSection == Section.Fight || page.chapterSection == Section.Titolo) { }
                else
                {
                    waitUIStatsManager = StartCoroutine(WaitUntilUIStatsmanagerIsNotNull());
                }
            }

            if (page.pageOpenDemoView)
            {
                Notify(MVCEvents.OPEN_DEMO_VIEW);
            }

            if (page.pageFeatures.isEscapeRoom)
            {
                Notify(MVCEvents.OPEN_ESCAPE_ROOM_VIEW);
            }

            if (page.pageBeforeDeath)
            {
                View.RefreshUIOnStartNewRun(false);
                SoundManager.Singleton.LoadMusicAudioClip(page);
                foreach (ChoiceButtonFeatures choiceButtonFeatures in page.choicesButtons)
                {
                    View.LoadAButton(choiceButtonFeatures);
                }
                return;
            }

            if (page.playerDead)
            {
                Notify(MVCEvents.OPEN_DEATH_VIEW);
            }

            if (page.repeatSamePage)
            {
                RepeatSamePage(page);
            }
            if (page.removeClickedBtn)
            {
                RemoveButtonClicked(page.btnsToRemove);
            }

            if (page.containFight)
            {
                int lastPageReadedByPlayerIndex = PlayerManager.Singleton.pagesRead.Count - 1;
                View.pageReadInCurrentRun.text = UIUtility.GetNumberForUI(lastPageReadedByPlayerIndex + 1);
                LoadBattleView();
                SoundManager.Singleton.LoadMusicAudioClip(page);
                return;
            }

            if (page.pageFeatures.showAnHideChoice)
            {
                foreach (ChoiceButtonFeatures optionsFeature in page.choicesButtons) //per ogni tasto se è nascosto apri
                {
                    if (optionsFeature.hideButton)
                    {
                        View.UnlockHideButton(page, optionsFeature);
                    }
                }
            }
            if (page.pageFeatures.hideClickedButton)
            {
                foreach (ChoiceButtonFeatures optionsFeature in page.choicesButtons)
                {
                    if (optionsFeature.onlyOnce)
                    {
                        View.HideButtonAlreadyClicked(optionsFeature, page);   //se è solo una volta passa
                    }
                }
            }
            if (page.pageFeatures.lockedChoice)
            {
                foreach (ChoiceButtonFeatures optionsFeature in page.choicesButtons)
                {
                    if (optionsFeature.lockedButton)
                    {
                        View.TryUnlockButton(optionsFeature, page);   //se è solo una volta passa
                    }
                }
            }
            ////la pagina contiene un enigma tra cui scelta multipla lunga
            if (page.pageFeatures.containRiddle)
            {
                View.ShowRiddle(page.scriptableRiddle, page);
            }

            if (page.pageFeatures.unlockByClickedButton)
            {
                View.CheckButtonClicked(page);
            }

            if (page.pageFeatures.onlyImage)
            {
                View.OnlyImage(page);
            }

            if (page.choicesButtons.Length == 0 || page.choicesButtons == null) return;


            foreach (ChoiceButtonFeatures choiceButtonFeatures in page.choicesButtons)
            {
                if (choiceButtonFeatures.analyzeButton)
                {
                    if (!page.HaveAnalyzeButton)
                    {
                        Debug.LogWarning("La pagina non è settata per il tasto Analizza");
                        continue;
                    }
                    else if (AnalyzeCharacterManager.Singleton != null)
                    {
                        AnalyzeComponent cmp = page.analyzeComponentStruct;
                        AnalyzeCharacterManager.Singleton.SetAnalyzePage(cmp.lblCharacterName, cmp.lblCharacterDescription, cmp.sprCharacterSprite, cmp.sprBackgroundSprite, cmp.heightContentCharacterDescription, cmp.scaleSize, cmp.isEnemyFight);
                        AnalyzeCharacterManager.Singleton.SetBattleValue(cmp.threat.ToString(), cmp.lblCharacterLife, cmp.lblCharacterLevel, cmp.lblCharacterDefencesNumber, cmp.lblCharacterAttackValue, cmp.lblCharacterAbilityNumber,
                                                                         cmp.lblCharacterAttackType, cmp.lblCharacterDefenceType, cmp.lblCharacterDefenceValue, cmp.difficult.ToString());
                        View.LoadAnalyzeButton(choiceButtonFeatures);
                    }
                }
                else if (choiceButtonFeatures.hideButton || choiceButtonFeatures.onlyOnce || choiceButtonFeatures.lockedButton
                     || choiceButtonFeatures.changeIDByButtonClicked || page.pageFeatures.onlyImage || page.repeatSamePage) {  }

                else if (choiceButtonFeatures.loadDifferentID != LoadDifferentID.None)
                {
                    View.LoadDifferentID(choiceButtonFeatures);
                }
                else
                {
                    View.LoadAButton(choiceButtonFeatures);
                }
            }
        }

        public static Coroutine notifyAlarmCoroutine;
        IEnumerator ActivateMapAnimationAndChapterCover(ScriptablePage page)
        {
            yield return new WaitUntil(MapIsOn);

            MapAnimationManager.Singleton.TurnOnMapAnimation(page.title);
            TurnOnTheCover(page);
            yield return new WaitUntil(MapsAnimationFinished);

            SoundManager.Singleton.LoadMusicAudioClip(page);
            MapAnimationManager.Singleton.ResetDefaultSettingsAtTheEndOfMapAnimation();
            View.SavePopUp();

            notifyAlarmCoroutine = StartCoroutine(ActivateClickToContinue());
        }

        void TurnOnTheCover(ScriptablePage page)
        {
            LoadImage imageToLoadInstance = Model.coverOfTheChapter;
            imageToLoadInstance.gameObject.SetActive(true);
            ScriptableImage temporaryImage = imageToLoadInstance.LoadScriptableImage();
            imageToLoadInstance.SetChapterCover(temporaryImage, page);
        }

        bool MapIsOn() { return MapAnimationManager.Singleton != null; }
        bool MapsAnimationFinished() { return MapAnimationManager.mapsAnimationIsFinish; }

        public IEnumerator ActivateClickToContinue(float time = 10f)
        {
            yield return new WaitForSeconds(time);

            if (SkipElementButtonManager.PlayerSkippedVideo()) yield break;

            NotifyTouchManager.Singleton.ActiveNotifyPanel();
            yield return new WaitUntil(SkipElementButtonManager.PlayerSkippedVideo);

            NotifyTouchManager.Singleton.DeactiveNotifyPanel();
            yield break;
        }

        bool IsNotActive()
        {
            return !Model.coverOfTheChapter.GetTitleImageIsActive;
        }

        IEnumerator WaitUntilUIStatsmanagerIsNotNull()
        {
            yield return new WaitUntil(UIStatsManagerIsActived);

            if (PlayerPrefs.GetInt(SettingsMenuView.SHOWGEMS) == 0)
            {
                SettingsMenuView.UPDATEGEMS = false;
                UIStatsManager.Singleton.UpdateGems();
            }
        }

        internal bool UIStatsManagerIsActived()
        {
            return UIStatsManager.Singleton != null;
        }
        IEnumerator ChangeBackgroundColor()
        {
            yield return new WaitForSeconds(2);
            Color tmpbackgroundColor = View.mainBackgroundImage.color;
            tmpbackgroundColor = new Color(188, 135, 135);
            View.mainBackgroundImage.color = tmpbackgroundColor;
        }
        void PageBecameRed()
        {
            float t = 0;
            float duration = 1.5f;
            View.mainBackgroundImage.color = Color.Lerp(View.mainBackgroundImage.color, Color.red, t);

            if (t < 1)
            {
                t += Time.deltaTime / duration;
                View.shadowImage.gameObject.SetActive(false);
            }
        }

        void RemoveButtonClicked(int btnToRemove)
        {
            PlayerManager.Singleton.buttonClicked.RemoveRange(PlayerManager.Singleton.buttonClicked.Count - btnToRemove, btnToRemove);
        }

        int repeatCounter;
        void RepeatSamePage(ScriptablePage page)
        {
            repeatCounter++;

            if (repeatCounter == page.repeatThisPage)
            {
                foreach (ScriptablePage.ChoiceButtonFeatures choice in page.choicesButtons)
                {
                    View.LoadDifferentAfterRepeat(choice);
                }
                repeatCounter = 0;
            }
            else
            {
                foreach (ScriptablePage.ChoiceButtonFeatures choice in page.choicesButtons)
                {
                    View.LoadAButton(choice);
                }
            }
        }

        void LoadBattleView()
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.BattleView;
            app.model.currentView.gameObject.SetActive(true);
        }

        void CheckChapterNumber()
        {
            if (PlayerManager.Singleton.currentPage.chapterSection == ScriptablePage.Section.Titolo)
            {
                PrepareTitleUI();

            }
            else if (PlayerManager.Singleton.currentPage.beforeTitle)
            {
                PrepareTitleUI();
            }
            View.horizontalBtnGroup.gameObject.SetActive(true);
        }

        void PrepareTitleUI()
        {

            switch (PlayerManager.Singleton.currentPage.title)
            {
                case ScriptablePage.Title.AllaSalute:
                    View.txtChapter.text = Localization.Get("capitolo1");
                    // View.txtChapterSubtitle.text = Localization.Get("capitolo1f");
                    break;
                case ScriptablePage.Title.InVinoVeritas:
                    View.txtChapter.text = Localization.Get("capitolo2");
                    // View.txtChapterSubtitle.text = Localization.Get("capitolo2VinoVeritas");
                    break;
                case ScriptablePage.Title.Scappa:
                    View.txtChapter.text = Localization.Get("capitolo2");
                    // View.txtChapterSubtitle.text = Localization.Get("capitolo2Scappa");
                    break;
                case ScriptablePage.Title.Memorie:
                    View.txtChapter.text = Localization.Get("capitolo3");
                    //View.txtChapterSubtitle.text = Localization.Get("capitolo3DuraLex");
                    break;
                case ScriptablePage.Title.DuraLex:
                    View.txtChapter.text = Localization.Get("capitolo3");
                    //View.txtChapterSubtitle.text = Localization.Get("capitolo3Memorie");
                    break;
                case ScriptablePage.Title.Oscurità:
                    View.txtChapter.text = Localization.Get("capitolo4");
                    // View.txtChapterSubtitle.text = Localization.Get("capitolo3Memorie");
                    break;

                default:
                    break;
            }
        }


        #region PopUpController

        void StartPopupManager(params object[] data)
        {
            ScriptablePage page = (ScriptablePage)data[0];
        }

        void ShowItemKeyMessage(params object[] data)
        {
            ScriptableItem tmpItem = (ScriptableItem)data[0];
            if (View.isLock)
            {
                View.panelKeyItemUsed.SetActive(true);
                View.itemUsedToUnlock.text = string.Format("Hai usato {0}", tmpItem.itemNameLocalization);
            }
            else
            {
                View.panelKeyItemUsed.SetActive(true);
                View.itemUsedToUnlock.text = string.Format("Non hai la chiave");
            }

            StartCoroutine(Close());
        }

        //Il tempo che deve metterci il messaggio Hai usato x o non hai x al click del tasto (Per gestire il tempo di cambio
        // pagina guardare ChoiceButton WaiUntillTrue ad ora è 4)
        IEnumerator Close()
        {
            yield return new WaitForSeconds(4);
            View.panelKeyItemUsed.SetActive(false);
        }

        RawImage image;
        public bool animationFinish = false;

        IEnumerator PopupManager(ScriptablePage page)
        {
            View.animator.ResetTrigger("PopUpShowed");

            View.GetPopup.SetActive(true);
            image = View.GetPopup.GetComponentInChildren<RawImage>();
            View.animatorReady = false; //attendo che arriva all'idle
            yield return new WaitUntil(AnimationReady);

            List<Texture2D> newTexture = View.GetPopupImage;
            int index = 0;
            foreach (ScriptablePage.PageBuff pageBuff in page.pageBuffes)
            {
                switch (pageBuff.buffType)
                {
                    case ScriptablePage.BuffType.LifePoint:

                        if (pageBuff.buffLifepoint > 0)
                        {
                            index = newTexture.FindIndex(i => i.name == "CuoreRossastro");
                        }
                        else
                        {
                            index = newTexture.FindIndex(i => i.name == "CuoreGrigio");
                        }
                        break;
                    case ScriptablePage.BuffType.Level:

                        if (pageBuff.buffLevel > 0)
                        {
                            index = newTexture.FindIndex(i => i.name == "CervelloBlue");
                        }
                        else
                        {
                            index = newTexture.FindIndex(i => i.name == "CervelloBianco");
                        }
                        break;
                    case ScriptablePage.BuffType.Inteligence:
                        break;
                    case ScriptablePage.BuffType.Strength:
                        break;
                    case ScriptablePage.BuffType.Constitution:
                        break;
                    case ScriptablePage.BuffType.Dexterity:
                        break;
                    case ScriptablePage.BuffType.Lucky:

                        if (pageBuff.buffLucky > 0)
                        {
                            index = newTexture.FindIndex(i => i.name == "lucky100");
                        }
                        else
                        {
                            index = newTexture.FindIndex(i => i.name == "lucky0");
                        }
                        break;
                    case ScriptablePage.BuffType.Superstition:
                        if (pageBuff.buffSuperstition > 0)
                        {
                            index = newTexture.FindIndex(i => i.name == "superstition100");
                        }
                        else
                        {
                            index = newTexture.FindIndex(i => i.name == "superstition0");
                        }
                        break;
                    case ScriptablePage.BuffType.Courage:
                        if (pageBuff.buffCourage > 0)
                        {
                            index = newTexture.FindIndex(i => i.name == "courage100");
                        }
                        else
                        {
                            index = newTexture.FindIndex(i => i.name == "courage0");
                        }
                        break;
                    default:
                        break;
                }

                image.texture = newTexture[index];
                yield return new WaitUntil(BuffPopupFinish); //attendo che viene visto
                View.imageShowed = false; //la resetto dato che abbiamo un foreach
                //image.GetComponent<Animator>().Play("imgPopup");
                //  CrossFadeIn();
                //  yield return new WaitUntil(ImageFadeIn);
                //  CrossFadeOut();
                //  //CrossFadeOut();
                //  yield return new WaitUntil(ImageFadeOut);
            }
            animationFinish = true;
        }

        public bool AnimationFinish()
        {
            if (animationFinish)
            {
                animationFinish = false;
                return true;
            }
            return false;

        }
        public bool AnimationReady()
        {
            if (View.animatorReady)
            {
                return true;
            }
            return false;
        }
        public bool BuffPopupFinish()
        {
            if (View.imageShowed)
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
