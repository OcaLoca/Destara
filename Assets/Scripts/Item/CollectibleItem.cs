using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(fileName = "CollectibleItem", menuName = "Item/Collectible", order = 1)]

    public class CollectibleItem : ScriptableItem
    {
        [SerializeField] private byte numberToUnlock;
        public string itemTag;

        private void OnEnable()
        {
            consumableInFight = false;
            itemType = ItemType.Collectible;

            foreach (CollectibleItem item in ScriptableItemsDatabase.Singleton.itemsDatabase.OfType<CollectibleItem>())
            {
                if (item.itemTag == this.itemTag)
                {
                    numberToUnlock++;
                }
            }
        }

        //Utilizzare oggetti collezionabili
        public override bool UnlockAllCollectible()
        {
            int myItems = 0;

            foreach (CollectibleItem item in PlayerManager.Singleton.inventory.Keys.OfType<CollectibleItem>())
            {
                if (item.itemTag == this.itemTag)
                {
                    myItems++;
                }
            }
            if (myItems >= numberToUnlock) { return true; }
            return false;
        }

    }
}



