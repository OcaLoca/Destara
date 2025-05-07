using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using static Game.ScriptablePage;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class ScriptableItemsDatabase : MonoBehaviour
    {
        public static ScriptableItemsDatabase Singleton;

        public List<ScriptableAbility> abilityDatabase;
        public List<ScriptableItem> itemsDatabase;
        public List<ScriptableItem> poolAllaSalute;
        public List<int> poolItemsNumberAllaSalute;
        public List<ScriptableItem> poolScappa;
        public List<int> poolItemsNumberScappa;
        public List<ScriptableItem> poolInVinoVeritas;
        public List<int> poolItemsNumberInVinoVeritas;

        void Awake()
        {
            Singleton = this;
        }

        [ContextMenu("Ordina e elimina doppioni dal database di tutti gli oggetti")]
        public void OrderAndRemoveDoubleItems()
        {
            // 1. Ordiniamo la lista per nome
            var orderedList = itemsDatabase.OrderBy(obj => obj.name).ToList();

            // 2. Rimuoviamo i duplicati basati sull'Id
            var distinctList = orderedList
                .GroupBy(obj => obj.ID)       // Raggruppiamo per Id
                .Select(group => group.First()) // Prendiamo il primo di ogni gruppo
                .ToList();

            // 3. Aggiorniamo il "database" rimuovendo i duplicati
            itemsDatabase = distinctList;
        }


        [ContextMenu("Ordina e elimina doppioni dal database abilita")]
        public void OrderAndRemoveDoubleAbilities()
        {
            // 1. Ordiniamo la lista per nome
            var orderedList = abilityDatabase.OrderBy(obj => obj.name).ToList();

            // 2. Rimuoviamo i duplicati basati sull'Id
            var distinctList = orderedList
                .GroupBy(obj => obj.localizedID)       // Raggruppiamo per Id
                .Select(group => group.First()) // Prendiamo il primo di ogni gruppo
                .ToList();

            // 3. Aggiorniamo il "database" rimuovendo i duplicati
            abilityDatabase = distinctList;
        }

        public ScriptableAbility GetAbilityFromDatabaseWithID(string ID)
        {
            foreach(ScriptableAbility ability in abilityDatabase)
            {
                if(ability.localizedID == ID)
                {
                    return ability;
                }
            }
            return null;
        }


        public ScriptableItem GetItemById(string ID)
        {
            if (ID != "" && ID != null)
            {
                foreach (ScriptableItem item in itemsDatabase)
                {
                    if (item)
                    {
                        if (item.ID == ID)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        public void RemoveObjsFromPool(Section section, int randomNumber)
        {
            switch (section)
            {
                case Section.AllaSalute:
                    poolAllaSalute.RemoveAt(randomNumber); 
                    poolItemsNumberAllaSalute.RemoveAt(randomNumber);
                    break;
                case Section.Scappa:
                    poolScappa.RemoveAt(randomNumber); 
                    poolItemsNumberScappa.RemoveAt(randomNumber);
                    break;
                case Section.InVinoVeritas:
                    poolInVinoVeritas.RemoveAt(randomNumber);
                    poolItemsNumberInVinoVeritas.RemoveAt(randomNumber);
                    break;
                case Section.DuraLex:
                    break;
                case Section.Oscurità:
                    break;
                case Section.Memorie:
                    break;
                case Section.Catacombe:
                    break;
                case Section.Monastero:
                    break;
                case Section.Porto:
                    break;
                case Section.Nave:
                    break;
                case Section.Lebbrosario:
                    break;
                default:
                    Debug.Log("Non rientra in nessun pool");
                    break;
            }
        }
        public Section chapterSection;

        public Tuple<List<ScriptableItem>, List<int>> GetObjsFromPool(Section section)
        {
            switch (section)
            {
                case Section.AllaSalute:
                    return Tuple.Create(poolAllaSalute, poolItemsNumberAllaSalute);
                case Section.Scappa:
                    return Tuple.Create(poolScappa, poolItemsNumberScappa);
                case Section.InVinoVeritas:
                    return Tuple.Create(poolInVinoVeritas, poolItemsNumberInVinoVeritas);
                case Section.DuraLex:
                    break;
                case Section.Oscurità:
                    break;
                case Section.Memorie:
                    break;
                case Section.Catacombe:
                    break;
                case Section.Monastero:
                    break;
                case Section.Porto:
                    break;
                case Section.Nave:
                    break;
                case Section.Lebbrosario:
                    break;
                default:
                    Debug.Log("Non rientra in nessun pool");
                    break;
            }
            return null;
        }
    }
}




