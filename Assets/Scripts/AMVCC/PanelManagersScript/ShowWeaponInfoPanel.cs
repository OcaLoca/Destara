/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.TypeDatabase;

namespace Game
{
    public class ShowWeaponInfoPanel : MonoBehaviour
    {
        public static ShowWeaponInfoPanel Singleton { get; set; }

        private void Awake()
        {
            Singleton = this;
        }

        public Image weaponImage;
        public TMP_Text weaponName, weaponDmg, weaponLevel, weaponTypeScale, weaponCri, weaponHitChance, weaponAbility, weaponRarity, weaponType;
        [SerializeField] Button btnClose;

        private void OnEnable()
        {
            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(delegate { 
                UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
                gameObject.SetActive(false); });
        }

        public void ShowWeaponInfo(AttackType attackType, string weaponName, string weaponRarity, string weaponLevel, string weaponDmg,
            string weaponTypeScale, string weaponCri, string weaponHitChance, Sprite weaponImage = null, Color32? rarityColor = null,
            string weaponAbility = "noAbility")
        {
            weaponType.text = Localization.Get(attackType.ToString());
            weaponAbility = Localization.Get(weaponAbility);
            if (weaponImage != null)
            {
                this.weaponImage.sprite = weaponImage;
                this.weaponImage.color = (Color)rarityColor;
            }
            this.weaponName.text = Localization.Get(weaponName);
            this.weaponName.color = (Color)rarityColor;
            this.weaponHitChance.text = weaponHitChance + " %"; 
            this.weaponCri.text = weaponCri + " %";
            this.weaponLevel.text =weaponLevel;
            this.weaponDmg.text = weaponDmg;
            this.weaponTypeScale.text = weaponTypeScale;
            this.weaponAbility.text = weaponAbility;
            this.weaponRarity.text = Localization.Get(weaponRarity);
            this.weaponRarity.color = (Color)rarityColor;
        }
    }
}
