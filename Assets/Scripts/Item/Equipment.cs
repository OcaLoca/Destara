using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.TypeDatabase;

namespace Game
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableEquipmentAndWeapon/Equipment", order = 1)]
    public class Equipment : ScriptableItem
    {
        public float defence;
        public int StaminaCost;
        [SerializeField] private float weight;
        public float arcaneDefenceBuffDebuff;
        public EquipmentType equipmentType;
        public ElementalBuff elementalBuff;
        public EquipPlaceType equipPlaceType;
        public DefenseType defenseType;
        public AudioClip hitSound;

        public void SetRealAndFantasyDefence()
        {
            if (PlayerManager.Singleton.superstition > 50) 
            {
                defence += arcaneDefenceBuffDebuff;
            }
        }

        public enum EquipPlaceType
        {
            LightDefence,
            BalancedDefence,
            HeavyDefence,
            Special,
            GemStone,
            Talisman,
            Relic
        }

        public bool IsDefence()
        {
            switch (equipPlaceType)
            {
                case EquipPlaceType.LightDefence:
                case EquipPlaceType.BalancedDefence:
                case EquipPlaceType.HeavyDefence:
                    return true;
                default:
                    return false;
            }
        }

        public float GetDefence()
        {
            return defence;
        }

        public float GetWeight()
        {
            return weight;
        }

        public void SetEquipmentIcon()
        {
            switch (equipPlaceType )
            {
                case EquipPlaceType.LightDefence:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LIGHTSHIELDICON);
                    break;
                case EquipPlaceType.BalancedDefence:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.BALANCEDSHIELDICON);
                    break;
                case EquipPlaceType.HeavyDefence:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.HEAVYSHIELDICON);
                    break;
                case EquipPlaceType.GemStone:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.GEMSTONEICONID);
                    break;
                case EquipPlaceType.Talisman:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.TALISMANICONID);
                    break;
                case EquipPlaceType.Relic:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.RELICICONID);
                    break;
            }
        }

        public Color32 GetEquipmentRarityColor()
        {
            Color32 tmpColor = new Color32();
            switch (rarity)
            {
                case Rarity.Common:
                    tmpColor = ColorDatabase.Singleton.commonColor;
                    break;
                case Rarity.Usual:
                    tmpColor = ColorDatabase.Singleton.uncommonColor;
                    break;
                case Rarity.Rare:
                    tmpColor = ColorDatabase.Singleton.rareColor;
                    break;
                case Rarity.SuperRare:
                    tmpColor = ColorDatabase.Singleton.superRareColor;
                    break;
                case Rarity.Legendary:
                    tmpColor = ColorDatabase.Singleton.legendaryColor;
                    break;
            }
            return tmpColor;
        }

    }
}

