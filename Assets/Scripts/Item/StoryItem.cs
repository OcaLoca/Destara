using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(fileName = "StoryItem", menuName = "Item/Story", order = 2)]
    public class StoryItem : ScriptableItem
    {
        [SerializeField] string UnlockID;

        private void OnEnable()
        {
            itemType = ItemType.Story;
        }

        //Utilizzare oggetti storia per capire se sblocchi quell codice
        public override string UseStoryItem()
        {
            return UnlockID;
        }

    }
}
