using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StarworkGC.Localization;

namespace Game
{
    public enum SaveType
    {
        Hard,
        Soft
    };

    public static class SaveSystem
    {
        public const string LAST_RUN = "LastRun";
        public const string LAST_PAGE_ID = "LastPageID";

        public static void SavePlayerLastRun(SaveType saveType, string page = "")
        {
            string path = SaveSystemUtilities.GetSavePath(LAST_RUN, saveType);
            PlayerData data = new PlayerData();

            if (page != string.Empty)
            {
                data.currentPageID = page;
            }

            try
            {
                string json = JsonUtility.ToJson(data);
                Debug.Log("JSON" + json);
                // Serializzazione in JSON
                File.WriteAllText(path, json);          // Scrittura su file
                Debug.Log($"Player data salvati con successo in {path}");
            }
            catch (IOException e)
            {
                Debug.LogError($"Errore durante il salvataggio del file: {e.Message}");
            }
        }

        public static PlayerData LoadLastRun()
        {
            string path = SaveSystemUtilities.GetSavePath(LAST_RUN, SaveType.Soft);

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log("Save file caricato con successo.");
                    return data;
                }
                catch (IOException e)
                {
                    Debug.LogError($"Errore durante il caricamento del file: {e.Message}");
                    return null;
                }
            }
            else
            {
                Debug.LogWarning($"Save file non trovato in {path}");
                return null;
            }
        }

        public static void WriteSlotInfo()
        {
            string classLevel = string.Format("Lv " + PlayerManager.Singleton.GetPlayerLevel.ToString());
            string chapter = string.Format("{0}",PlayerManager.Singleton.lastChapter);
            //string chapter = PlayerManager.Singleton.currentPage.chapterSection.ToString();
            GameApplication.Singleton.view.ContinueView.SetSavedInfo(PlayerManager.Singleton.playerName, classLevel, chapter, PlayerManager.Singleton.classPlayerTypeToString);
        }
       
    }

}