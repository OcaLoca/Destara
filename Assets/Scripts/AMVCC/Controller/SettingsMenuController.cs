using SmartMVC;
using TMPro;
using UnityEngine;

namespace Game
{
    public class SettingsMenuController : Controller<GameApplication>
    {
        void OnEnable()
        {
            AddEventListenerToApp(MVCEvents.SAVE_SETTINGS, OnSaveSettings);
            AddEventListenerToApp(MVCEvents.CHANGE_FONT, OnClickNewFont);
            AddEventListenerToApp(MVCEvents.NEW_FONT_SIZE, OnClickNewSize);
            AddEventListenerToApp(MVCEvents.NEW_BUTTON_FONT, OnClickNewButtonFont);
            AddEventListenerToApp(MVCEvents.NEW_FONT_BUTTON_SIZE, OnClickNewButtonSize);
        }

        void OnDisable()
        {
            RemoveEventListenerFromApp(MVCEvents.SAVE_SETTINGS, OnSaveSettings);
            RemoveEventListenerFromApp(MVCEvents.CHANGE_FONT, OnClickNewFont);
            RemoveEventListenerFromApp(MVCEvents.NEW_FONT_SIZE, OnClickNewSize);
            RemoveEventListenerFromApp(MVCEvents.NEW_BUTTON_FONT, OnClickNewButtonFont);
            RemoveEventListenerFromApp(MVCEvents.NEW_FONT_BUTTON_SIZE, OnClickNewButtonSize);
        }

        #region ChangeFont

        void OnClickNewFont(params object[] data)
        {
            var selectedFont = (int)data[0];

            TMP_FontAsset newFont = FontDatabase.Singleton.GetFontByIndex(selectedFont);
            GameView view = app.view;
            view.BookView.textPrefab.font = newFont;
            view.Settings.txtPreview.font = newFont;
            view.Settings.txtBackgroundPreview.font = newFont;
            view.BookView.btnOptionPrefab.SetFont(newFont);

            PlayerPrefs.SetInt(SettingsMenuView.SELECTEDFONT, selectedFont);
        }

        void OnClickNewButtonFont(params object[] data)
        {
            var selectedFont = (int)data[0];
            TMP_FontAsset newFont = FontDatabase.Singleton.GetButtonFontByIndex(selectedFont);
            GameView view = app.view;

            view.BookView.btnOptionPrefab.SetFont(newFont);
            view.Settings.txtButtonPreviewText.font = newFont;
            PlayerPrefs.SetInt(SettingsMenuView.SELECTEDBUTTONFONT, selectedFont);
        }

        #endregion

        void OnSaveSettings(params object[] data)
        {
            float volume = app.model.Settings.volume;
            bool fullscreen = app.model.Settings.fullscreen;
        }

        #region ChangeSize

        void OnClickNewSize(params object[] data)
        {
            float scrollSize = (float)data[0];
            float fontSize = scrollSize * 100;

            GameView view = app.view;
            view.BookView.textPrefab.fontSize = fontSize;
            view.Settings.txtPreview.fontSize = fontSize;

            PlayerPrefs.SetFloat(SettingsMenuView.FONTSIZESCROLLVALUE, scrollSize);
        }

        public const float BUTTONMINFONTSIZE = 45;
        public const float BUTTONMAXFONTSIZE = 65;
        void OnClickNewButtonSize(params object[] data)
        {
            float scrollSize = (float)data[0];
            float fontSize = scrollSize * 100;

            //if (fontSize < BUTTONMINFONTSIZE) { fontSize = BUTTONMINFONTSIZE; }
            //if (fontSize > BUTTONMAXFONTSIZE) { fontSize = BUTTONMAXFONTSIZE; }

            GameView view = app.view;
            view.Settings.txtButtonPreviewText.fontSize = fontSize;
            view.BookView.btnOptionPrefab.SetNewFontSize(fontSize);

            PlayerPrefs.SetFloat(SettingsMenuView.FONTBUTTONSIZESCROLLVALUE, scrollSize);
        }

        #endregion
    }
}