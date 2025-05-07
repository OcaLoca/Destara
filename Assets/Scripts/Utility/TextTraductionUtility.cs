/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using Crosstales.Common.Util;
using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class TextTraductionUtility
    {
        /// <summary>
        /// Restituisce il testo localizzato in versione maschile o femminile
        /// </summary>
        /// <param name="textToGet">testo da localizzare</param>
        /// <returns></returns>
        public static string GetCorrectSexText(string textToGet, string firstParameter = null)
        {
            if (firstParameter == null)
            {
                return PlayerManager.Singleton.isMale ? Localization.Get(textToGet) : Localization.Get(LocalizationIDDatabase.FEM_PREFIX + textToGet);
            }
            return PlayerManager.Singleton.isMale ? string.Format(Localization.Get(textToGet), firstParameter) : string.Format(Localization.Get(LocalizationIDDatabase.FEM_PREFIX + textToGet), firstParameter);
        }

        public static string AddFemalePrefixToPageID(string pageID)
        {
            return PlayerManager.Singleton.isMale ? pageID : LocalizationIDDatabase.FEM_PREFIX + pageID;
        }

        public static string RemoveFemalePrefixToPageID(string pageID)
        {
            if(pageID.Contains(LocalizationIDDatabase.FEM_PREFIX))
            {
                return pageID.Substring(pageID.IndexOf(LocalizationIDDatabase.FEM_PREFIX));
            }
            return pageID;
        }

    }

}
