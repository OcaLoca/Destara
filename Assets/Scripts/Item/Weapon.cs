using SmartMVC;
using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;
using Random = UnityEngine.Random;

namespace Game
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableEquipmentAndWeapon/Weapon", order = 0)]

    public class Weapon : ScriptableItem
    {
        /// <summary>
        /// Danni che escono nel log in console 
        /// </summary>
        [Tooltip("Danni che si vedono in log")]
        public float damage;
        /// <summary>
        /// Danni dell'arma 
        /// </summary>
        [Tooltip("Danni dell'arma")]
        public float weaponConstDamage;
        /// <summary>
        /// Stamina che consuma 
        /// </summary>
        [Tooltip("Stamina che consuma")]
        public float staminaCost;
        /// <summary>
        /// Su quale stats scala l'arma 
        /// </summary>
        [Tooltip("Stats sul quale scala l'arma")]
        public ScaleType scaleType;
        /// <summary>
        /// Valore numerico che indica la differenza che subisce il valore di danno dell'arma nell'altra superstizione (sia positivo che negativo) 
        /// </summary>
        [Tooltip("Valore numerico che indica la differenza che subisce il valore di danno dell'arma nell'altra superstizione (sia positivo che negativo)")]
        public float arcaneDamageBuffDebuff;

        public TypeDatabase.AttackType attackType; /// weapon di 0 mi dice se è ranged light ecc, tipo 2 se è reale trasformabile(mista) o fantasy
        public ElementalBuff elementalBuff;
        // public Image weaponImage;
        public float criticalDamageChance;
        public float hitChance;
        public AudioClip weaponAttackSound;
        public AudioClip weaponHitSound;

        [Header("RenderFightType"), Tooltip("Serve a settare l'animazione corretta nel fight in base al disegno dell'arma")]
        public RenderType weaponRenderType;
        public enum RenderType
        {
            armaCorta,
            armaLungaUnaMano,
            armaDueMani,
            lancia,
            arco
        }
        public enum ScaleType
        {
            FOR,
            INT,
            DEX
        }

        public string VFXID;
        public string criticalVFXID;

        private void Awake()
        {
            SetWeaponLevel();
        }

        void SetWeaponLevel()
        {
            if (level == 0) level = 1;
        }
        public void SetWeaponIcon()
        {
            switch (attackType)
            {
                case TypeDatabase.AttackType.Light:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.LIGHTWEAPONICONID);
                    break;
                case TypeDatabase.AttackType.Heavy:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.HEAVYWEAPONICONID);
                    break;
                case TypeDatabase.AttackType.Ranged:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.RANGEDWEAPONICONID);
                    break;
                case TypeDatabase.AttackType.Special:
                    icon = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.SPECIALWEAPONICONID);
                    break;
            }
        }
        public Color32 GetWeaponRarityColor()
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

        void DeterminateScaledDamage()
        {
            switch (scaleType)
            {
                case ScaleType.DEX:
                    damage = damage + (PlayerManager.Singleton.dexterity / 100 * damage);
                    break;
                case ScaleType.INT:
                    damage = damage + (PlayerManager.Singleton.inteligence / 100 * damage);
                    break;
                case ScaleType.FOR:
                    damage = damage + (PlayerManager.Singleton.strength / 100 * damage);
                    break;
                default:
                    break;
            }
        }

        void SetRealAndFantasyDamage()
        {
            damage = weaponConstDamage;

            if (PlayerManager.Singleton.superstition > 50)
            {
                damage += arcaneDamageBuffDebuff;   //quando si va in sup alta calcola i danni 
            }
        }

        public float ScaledDamage()
        {
            SetRealAndFantasyDamage();
            DeterminateScaledDamage();

            return damage;
        }

        public float CriticalDamage()
        {
            int result = Random.Range(0, 100);
            if ((result > 0) && (result < criticalDamageChance))
            {
                damage = ScaledDamage() * 2;
                Debug.Log("Critical Attack!");
            }
            else
            { damage = ScaledDamage(); }
            return damage;
        }

        public float ReturnRealDamage()
        {
            int hit = Random.Range(0, 100);

            if (hit <= hitChance)
            {
                return CriticalDamage();
            }
            else
            {
                Debug.Log("Attacco fallito");
                return 0;
            }
        }

        public float CalculateWeaponDamage(Unit target, Unit attacker)
        {
            damage = weaponConstDamage;
            if(string.IsNullOrEmpty(VFXID))
            {
                VFXID = "Slash01.2";
            }

            SetVFX(0, VFXID, target.placeID);
            if (damage == 0)
            {
                zeroDamage = true;
                //effectiveLog = Localization.Get(LocalizationIDDatabase.ZERO_DAMAGE);
                return 1;
            }
            else
            {
                zeroDamage = false;
            }

            if (PlayerManager.Singleton.superstition > 50)
            {
                damage += arcaneDamageBuffDebuff;   //quando si va in sup alta calcola i danni 
            }

            int hit = Random.Range(0, 100);
            if (hit <= hitChance)
            {
                int result = Random.Range(0, 100);
                if (result < criticalDamageChance)
                {
                    damage = damage * 1.5f;
                    if (string.IsNullOrEmpty(criticalVFXID))
                    {
                        criticalVFXID = "Slash01.2Blood";
                    }
                    SetVFX(0, criticalVFXID, target.placeID);
                    effectiveLog = "Critical";
                    damageIsCritical = true;
                    effectiveLog = Localization.Get(LocalizationIDDatabase.CRITICAL_LOG);
                }
                else
                {
                    SetVFX(0, VFXID, target.placeID);
                    damageIsCritical = false;
                    effectiveLog = Localization.Get(LocalizationIDDatabase.EMPTY_HIT_LOG);
                }
                failedToAttack = false;
            }
            else
            {
                failedToAttack = true;
                damageIsCritical = false;
                zeroDamage = true;
                effectiveLog = Localization.Get(LocalizationIDDatabase.MISS_LOG);
                return 0;
            }

            DeterminateScaledDamage();
            damage = TypeDatabase.IsEffectiveOrNot(attackType, target.DefenceType, damage, ref effectiveLog, attacker.isControllable);
            damage = damage - target.defence;

            defenceLog = BattleControllerHelper.GetDefenceResultBattleConsoleLog(damage, true);
            return damage;
        }
    }
}
