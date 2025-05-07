/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */
using StarworkGC.Localization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Game.PlayerManager;

namespace Game
{
    public class LocalizeTextByDevice : MonoBehaviour
    {
        const string ENGLISH = "English";
        public TMP_Text txtToTranslate;
        private Dictionary<string, string> traductions = new Dictionary<string, string>
    {
        { LocalizationIDDatabase.ITALIAN, "Lingua" },
        { LocalizationIDDatabase.ENGLISH, "Language" },
        { LocalizationIDDatabase.FRENCH, "Langue" },
        { LocalizationIDDatabase.SPANISH, "Idioma" },
        { LocalizationIDDatabase.GERMAN, "Sprache" },
        { LocalizationIDDatabase.PORTOGUESE, "Idioma" },
        {  LocalizationIDDatabase.JAPANESE, "言語を選択" }
    };

        private void OnEnable()
        {
            if (Singleton.selectedLanguage != SelectedLanguage.None)
            {
                txtToTranslate.text = Localization.Get("SelectLanguage");
                return;
            }

            string deviceLanguage = Application.systemLanguage.ToString();
            SetLocalLanguage(deviceLanguage);
        }

        private void SetLocalLanguage(string lingua_del_dispositivo)
        {
            if (traductions.ContainsKey(lingua_del_dispositivo))
            {
                txtToTranslate.text = traductions[lingua_del_dispositivo];
            }
            else
            {
                txtToTranslate.text = traductions[ENGLISH];
            }
        }
    }
}
