using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;

namespace Game
{
    public class GameSettingsView : View<GameApplication>
    {
        [SerializeField] Button btnOption;
        [SerializeField] Button btnHelp;
        [SerializeField] Button btnExit;
        [SerializeField] Button btnBack;

        void OnEnable()
        {
            btnOption.onClick.RemoveAllListeners();
            btnOption.onClick.AddListener(OnClickoption);

            btnHelp.onClick.RemoveAllListeners();
            btnHelp.onClick.AddListener(OnClickHelp);

            btnExit.onClick.RemoveAllListeners();
            btnExit.onClick.AddListener(OnClickExit);

            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnReturnInGameSession);
        }

        void OnClickoption()
        {
            SettingsMenuView.TurnOnLanguageButton = false;
            Notify(MVCEvents.OPEN_SETTING);
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

        void OnClickExit()
        {
            SaveSystemUtilities.openApp = false;
            BookView.gameOpenedFromSettingsInGame = false; // refresh
            SoundManager.Singleton.LoadMusicAudioClip();
            SoundEffectManager.Singleton.audioSource.Stop();
            //Svuoto inventario
            PlayerManager.Singleton.CleanInventory();
            Notify(MVCEvents.OPEN_MAINMENU_VIEW);
        }
        void OnReturnInGameSession()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
            Notify(MVCEvents.LOAD_PAGE, PlayerManager.Singleton.currentPage.pageID);
        }

    }
}
