/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DroppedItem : MonoBehaviour
    {
        [SerializeField] Image sprite;
        [SerializeField] TMP_Text text;
        [SerializeField] TMP_Text number;

        public void SetupItem(ScriptableItem obj, int quantity)
        {
            obj.SetLocalizeTitle();
            text.text = obj.GetLocalizedObjName();

            Color32 myColor = new Color32();

            switch (obj.itemType)
            {
                case ScriptableItem.ItemType.Consumable:
                case ScriptableItem.ItemType.Story:
                case ScriptableItem.ItemType.Collectible:
                    sprite.sprite = IconsDatabase.Singleton.GetSpriteByObjectType(obj.itemType);
                    myColor = ColorDatabase.Singleton.GetRarityColor(obj.rarity);
                    break;
                case ScriptableItem.ItemType.Weapon:
                    Weapon weapon = obj as Weapon;
                    sprite.sprite = IconsDatabase.Singleton.GetWeaponSpriteByAttackType(weapon.attackType);
                    myColor = ColorDatabase.Singleton.GetRarityColor(obj.rarity);
                    break;
                case ScriptableItem.ItemType.Armor:
                    Equipment equip = obj as Equipment;
                    sprite.sprite = IconsDatabase.Singleton.GetArmorSpriteByDefenceType(equip.defenseType);
                    myColor = ColorDatabase.Singleton.GetRarityColor(obj.rarity);
                    break;
                case ScriptableItem.ItemType.Relic:
                case ScriptableItem.ItemType.Talisman:
                case ScriptableItem.ItemType.GemStone:
                    sprite.sprite = IconsDatabase.Singleton.GetSpriteByObjectType(obj.itemType);
                    myColor = ColorDatabase.Singleton.GetRarityColor(obj.rarity);
                    break;
                default:
                    break;
            }
            
            sprite.color = myColor;

            if(obj.GetObjectQuantity > 1)
            {
                quantity = obj.GetObjectQuantity;
            }
          
            number.text = UIUtility.GetNumberForUI(quantity);
        }
    }

}
