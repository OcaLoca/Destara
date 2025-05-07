using SmartMVC;
using UnityEngine;

namespace Game
{
    public class MainMenuController : Controller<GameApplication>
    {
        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.MAIN_START_BUTTON_CLICKED, OnContinueClicked);
            AddEventListenerToApp(MVCEvents.OPEN_NEW_GAME_VIEW, OnNewGameClicked);
            AddEventListenerToApp(MVCEvents.OPEN_CURIOSITY_VIEW, OnCuriosityButtonClicked);
            AddEventListenerToApp(MVCEvents.OPEN_CHARACTER_CREATION_VIEW, OnCharacterCreationView);
            AddEventListenerToApp(MVCEvents.SWITCH_TO_PREVIOUS_VIEW, OnBackButtonClicked);
            AddEventListenerToApp(MVCEvents.OPEN_SETTING, OnOpenSettingsButtonClicked);
            AddEventListenerToApp(MVCEvents.OPEN_WEBSITE, OpenWebsite);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.MAIN_START_BUTTON_CLICKED, OnContinueClicked);
            RemoveEventListenerFromApp(MVCEvents.OPEN_NEW_GAME_VIEW, OnNewGameClicked);
            RemoveEventListenerFromApp(MVCEvents.OPEN_CURIOSITY_VIEW, OnCuriosityButtonClicked);
            RemoveEventListenerFromApp(MVCEvents.OPEN_CHARACTER_CREATION_VIEW, OnCharacterCreationView);
            RemoveEventListenerFromApp(MVCEvents.SWITCH_TO_PREVIOUS_VIEW, OnBackButtonClicked);
            RemoveEventListenerFromApp(MVCEvents.OPEN_SETTING, OnOpenSettingsButtonClicked);
            RemoveEventListenerFromApp(MVCEvents.OPEN_WEBSITE, OpenWebsite);
        }

        void OnContinueClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.ContinueView;
            app.model.currentView.gameObject.SetActive(true);
        }

        void OnNewGameClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.NewGame;
            app.model.currentView.gameObject.SetActive(true);
        }

        void OnCuriosityButtonClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                //spengo il model view
                app.model.currentView.gameObject.SetActive(false);
                //salvo nello stack per quando uso back
                app.model.previousView.Push(app.model.currentView);
            }

            //setto la view al model e attivo
            app.model.currentView = app.view.CuriosityView;
            app.model.currentView.gameObject.SetActive(true);
        }

        void OnCharacterCreationView(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.CharacterCreationView;
            app.model.currentView.gameObject.SetActive(true);
        }
        void OnOpenSettingsButtonClicked(params object[] data)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }

            app.model.currentView = app.view.Settings;
            app.model.currentView.gameObject.SetActive(true);

        }

        void OpenWebsite(params object[] data)
        {
            string URL = "https://www.mobybit.it/";
            Application.OpenURL(URL);
        }

        void OnBackButtonClicked(params object[] data)
        {
            if (app.model.previousView.Count < 1) { return; }

            //spengo la view attuale
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
            }

            //View<GameApplication> temp = app.model.currentView; //salvo la view attuale in una variabile temporanea
            app.model.currentView = app.model.previousView.Pop(); //la view attuale diventa quella precedente
            app.model.currentView.gameObject.SetActive(true); //attivo la nuova view attuale
            //app.model.previousView = temp; //salvo la vecchia view attuale come nuova view precedente
        }
    }
}