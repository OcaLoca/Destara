using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;
using StarworkGC.Localization;
using System;
using Random = System.Random;

namespace Game
{
    public class DropView : View<GameApplication>
    {

        [SerializeField] Transform dropList;
        [SerializeField] DropListPrefab DropListPrefab;

        public Button btnInventory;
        public Button btnDropSearch;
        public string pageID;
        void OnEnable()
        {

            btnInventory.onClick.RemoveAllListeners();
            btnInventory.onClick.AddListener(OnClickInventory);

            btnDropSearch.onClick.RemoveAllListeners();
            btnDropSearch.onClick.AddListener(OnClickDropSearch);

        }

        void OnClickInventory()
        {
            Notify(MVCEvents.OPEN_INVENTORY_VIEW);
        }

        void OnClickDropSearch()
        {

            pageID = PlayerManager.Singleton.currentPage.pageID;

            //if (PlayerManager.Singleton.isMale)
            //{
                if (PagesMaleDatabase.Singleton.GetPageByID(pageID).containSpecificDrop)
                {
                    int itemDropped = RandomDrop(PagesMaleDatabase.Singleton.GetPageByID(pageID).searchDrop);
                    if (itemDropped != -1)
                    {
                        DropListPrefab dropGeneratedItem = Instantiate(DropListPrefab, dropList);
                        dropGeneratedItem.Setup(PagesMaleDatabase.Singleton.GetPageByID(pageID).searchDrop[itemDropped]);
                        PagesMaleDatabase.Singleton.GetPageByID(pageID).dropped = true;
                    }
                }
                else
                {
                    Debug.Log("Nessun drop disponibile");
                }

            //}

            //else
            //{
            //    if (PagesFemaleDatabase.Singleton.GetPageByID(pageID).containSpecificDrop)
            //    {
            //        int itemDropped = RandomDrop(PagesFemaleDatabase.Singleton.GetPageByID(pageID).searchDrop);
            //        if (itemDropped != -1)
            //        {

            //            DropListPrefab dropGeneratedItem = Instantiate(DropListPrefab, dropList);
            //            dropGeneratedItem.Setup(PagesFemaleDatabase.Singleton.GetPageByID(pageID).searchDrop[itemDropped]);
            //            PagesFemaleDatabase.Singleton.GetPageByID(pageID).dropped = true;
            //        }

            //    }
            //    else
            //    {
            //        Debug.Log("Nessun drop disponibile");
            //    }
            //}
        }

        void OnDisable()
        {
            UIUtility.CleanLayout(dropList);
        }

        // Funzione calcolo drop in base alla rarità, da aggiungere la fortuna
        public static int RandomDrop(List<ScriptableItem> drop)
        {
            int contItem = 0;
            int itemRarityRate = 0;
            int[] itemArray = new int[drop.Count];
            int lucky = PlayerManager.Singleton.lucky;
            int dropMulRate = 0;
            int dropDivRate = 7;

            //Creazione array per rarità item
            foreach (var pageDrop in drop)
            {
                
                switch ((int)pageDrop.rarity)
                {
                    case 0:
                        itemRarityRate = 60;
                        dropMulRate = 1;
                        break;
                    case 1:
                        itemRarityRate = 40;
                        dropMulRate = 2;
                        break;
                    case 2:
                        itemRarityRate = 20;
                        dropMulRate = 3;
                        break;
                    case 3:
                        itemRarityRate = 10;
                        dropMulRate = 4;
                        break;
                    case 4:
                        itemRarityRate = 5;
                        dropMulRate = 5;
                        break;
                }

                if (contItem == 0)
                {
                    itemArray[contItem] = itemRarityRate + (lucky / dropDivRate * dropMulRate);
                }
                else
                {
                    itemArray[contItem] = itemArray[contItem - 1] + itemRarityRate + (lucky / dropDivRate * dropMulRate);
                }

                contItem++;
                
            }

            contItem--;

            //Generazione numero random
            Random randValue = new Random();
            int randomValue = randValue.Next(0, itemArray[contItem]);
            contItem = 0;

            //Selezione e return item droppato
            foreach (var pageDrop in drop)
            {

                if (randomValue < itemArray[contItem])
                {
                    return contItem;
                }

                contItem++;

            }

            return -1;
        }

    }

}