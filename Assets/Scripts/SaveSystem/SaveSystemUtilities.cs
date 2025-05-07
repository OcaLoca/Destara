/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    public static class SaveSystemUtilities
    {
        //returna il percorso del file json in base a classe e tipo di salvataggio
        internal static string GetSavePath(string className, SaveType saveType)
        {
            return Path.Combine(Application.persistentDataPath, $"{className}_{saveType}.json");
        }

        // Metodo per cancellare il file di salvataggio
        public static void EraseSaveFile(SaveType saveType)
        {
            string className = PlayerManager.Singleton.classPlayerTypeToString;
            string path = GetSavePath(className, saveType);

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Cancellato il file di salvataggio: {path}");
            }
            else
            {
                Debug.LogWarning($"File di salvataggio non trovato per la cancellazione: {path}");
            }
        }

        // Metodo per controllare se esiste un salvataggio
        public static bool CheckSave(SaveType saveType, string slotID)
        {
            string path = GetSavePath(slotID, saveType);
            return File.Exists(path);
        }

        public static bool openApp = true;
        // Metodo per verificare se ci sono già salvataggi presenti (inizializzazione)
        public static bool AlreadySave()
        {
            if (CheckSave(SaveType.Soft, SaveSystem.LAST_RUN))
            {
                return true;
            }

            if (CheckSave(SaveType.Soft, "NoClass"))
            {
                //PlayerManager.Singleton.LoadSoftSaveBeforeClass();
                return true;
            }

            //string[] saveID = { "Abbot", "BountyHunter", "Crone", "Trafficker" };

            //foreach (string slotID in saveID)
            //{
            //    if (CheckSave(SaveType.Soft, slotID) || CheckSave(SaveType.Hard, slotID) || CheckSave(SaveType.Hard, slotID))
            //    {
            //        if (openApp)
            //        {
            //            PlayerManager.Singleton.Init(slotID); // Inizializza il player con il salvataggio trovato
            //        }
            //        return true;
            //    }
            //}
            return false;
        }
    }

}
