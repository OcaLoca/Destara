using SmartMVC;
using StarworkGC.Localization;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Game
{
    public class NewGameController : Controller<GameApplication>
    {
        NewGameView View { get { return app.view.NewGame; } }

        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.CHANGE_LAST_SELECTED_DIFFICULTY, ChangeSelectedDifficulty);
            AddEventListenerToApp(MVCEvents.CONFIRM_DIFFICULTY_SELECTION, OnClickContinue);
            AddEventListenerToApp(MVCEvents.OPEN_GAME_VIEW, OpenGame);
            AddEventListenerToApp(MVCEvents.OPEN_GAME_VIEW_CONTINUE, OpenGameFromContinue);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.CHANGE_LAST_SELECTED_DIFFICULTY, ChangeSelectedDifficulty);
            RemoveEventListenerFromApp(MVCEvents.CONFIRM_DIFFICULTY_SELECTION, OnClickContinue);
            RemoveEventListenerFromApp(MVCEvents.OPEN_GAME_VIEW, OpenGame);
        }

        void ChangeSelectedDifficulty(params object[] data)
        {
            //View.GetSkullContainer.gameObject.SetActive(true);
            PlayerManager.Difficulty difficulty = (PlayerManager.Difficulty)data[0];
            GameApplication.Singleton.model.NewGame.selectedDifficulty = difficulty;
            PlayerManager.Singleton.selectedDifficulty = difficulty;
            PlayerManager.Singleton.lastDifficultyChoice = difficulty; //Serve per quando fa riprova

            switch (difficulty)
            {
                case PlayerManager.Difficulty.Coward:
                    //PlayerManager.COWARD_DESCRIPTION
                    GameApplication.Singleton.view.NewGame.lblDifficultyDescription.text = Localization.Get("demo_cowardDsc");
                    View.GetBlackSkulls[0].enabled = true;
                    View.GetBlackSkulls[1].enabled = false;
                    View.GetBlackSkulls[2].enabled = false;
                    GameApplication.Singleton.view.NewGame.btnConfirmSelection.interactable = true;
                    GameApplication.Singleton.view.NewGame.btnConfirmSelection.GetComponent<Animator>().enabled = true;
                    break;
                case PlayerManager.Difficulty.Fearless:
                    //PlayerManager.FEARLESS_DESCRIPTION
                    GameApplication.Singleton.view.NewGame.lblDifficultyDescription.text = Localization.Get("DifficultPayBlock");
                    View.GetBlackSkulls[0].enabled = true;
                    View.GetBlackSkulls[1].enabled = true;
                    View.GetBlackSkulls[2].enabled = false;
                   
                    GameApplication.Singleton.view.NewGame.btnConfirmSelection.interactable = false;
                    GameApplication.Singleton.view.NewGame.btnConfirmSelection.GetComponent<Animator>().enabled = false;
                    break;
                case PlayerManager.Difficulty.Insane:
                    //PlayerManager.INSANE_DESCRIPTIO
                    GameApplication.Singleton.view.NewGame.lblDifficultyDescription.text = Localization.Get("DifficultPayBlock");
                    View.GetBlackSkulls[0].enabled = true;
                    View.GetBlackSkulls[1].enabled = true;
                    View.GetBlackSkulls[2].enabled = true;
                   
                    GameApplication.Singleton.view.NewGame.btnConfirmSelection.interactable = false;
                    GameApplication.Singleton.view.NewGame.btnConfirmSelection.GetComponent<Animator>().enabled = false;
                    break;
                default:
                    break;
            }
        }

        void OpenGame(params object[] data)
        {
            PlayerManager.Singleton.pagesRead.Add(app.model.MainMenu.introChapter.pageID);

            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.BookView;
            app.model.currentView.gameObject.SetActive(true);
        }

        void OpenGameFromContinue(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.BookView;
            app.model.currentView.gameObject.SetActive(true);
        }

        void OpenOverwritePopUp(params object[] data)
        {
            PlayerManager.Singleton.selectedDifficulty = GameApplication.Singleton.model.NewGame.selectedDifficulty;
            //View.GetSkullContainer.gameObject.SetActive(false);
            app.view.NewGame.pnlOverwrite.gameObject.SetActive(true);
        }

        void OnClickContinue(params object[] data)
        {
            app.view.NewGame.OnClickContinue();
        }

    }
}