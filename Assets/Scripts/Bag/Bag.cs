using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{

    public class Bag : ScriptableBag
    {
        public Dictionary<ScriptableItem, float> ArchivedItems { get; private set; }

       

      // public void AddItemToBag(ScriptableItem item)
      // {
      //     /*
      //      * CaramellaRara : 2,
      //      * Pokèball : 1
      //      */
      //
      //     if ((ArchivedItems.ContainsKey(item)) && (item.isStackable == true) && (item.starterQuantity < item.maxStackable))   ////setto la quantità presente a quella massima dell'oggetto
      //     {
      //         item.starterQuantity++;
      //         item.starterQuantity = ArchivedItems[item];
      //     }
      //     else if ((ArchivedItems.ContainsKey(item)) && (item.isStackable == false) && (spaceOccupied < maxSpace))
      //     {
      //         item.starterQuantity++;
      //         item.starterQuantity = ArchivedItems[item]; //dovrebbe raccoglierlo ma in un altro punto non è impilabile quindi /faccio/ che occupa spazio
      //         spaceOccupied = spaceOccupied + 1;
      //     }
      //     else if ((!ArchivedItems.ContainsKey(item)) && (item.isStackable == false) && (spaceOccupied < maxSpace))
      //     {
      //         item.starterQuantity = 1;
      //         ArchivedItems[item] = item.starterQuantity;
      //
      //     }
      //     else
      //         Debug.Log("No space");
      // }
    }
}