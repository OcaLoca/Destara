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
    public class ShowEquipmentInfoPanel : MonoBehaviour
    {
        public static ShowEquipmentInfoPanel Singleton { get; set; }
        [SerializeField] Button btnClose, btnWear;

        private void Awake()
        {
            Singleton = this;
        }

        public Image equipmentImage;
        public TMP_Text equipName, equipRarity, equipLevel, equipDef, equipWeight, equipAbility, defenseType;
        public DefenseType tmpDefenseType;

        private void OnEnable()
        {
            tmpDefenseType = DefenseType.Null;
            btnWear.onClick.RemoveAllListeners();
            btnWear.onClick.AddListener(OnClickWear);

            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(delegate
            {
                UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
                gameObject.SetActive(false);
            });
        }

        public void ShowEquipmentInfo(DefenseType defenseType, string equipName, string equipRarity, string equipLevel, string equipDef,
             string equipWeight, Sprite equipmentImage = null, Color32? rarityColor = null, string equipAbility = "noAbility")
        {
            if (defenseType != PlayerManager.Singleton.initialEquipmentDefenseType)
            {
                tmpDefenseType = defenseType;
                btnWear.gameObject.SetActive(true);
            }
            else
            {
                tmpDefenseType = DefenseType.Null;
                btnWear.gameObject.SetActive(false);
            }

            equipAbility = Localization.Get(equipAbility);
            if (equipmentImage != null)
            {
                this.equipmentImage.sprite = equipmentImage;
                this.equipmentImage.color = (Color)rarityColor;
            }
            this.equipName.text = Localization.Get(equipName);
            this.equipName.color = (Color)rarityColor;
            this.defenseType.text = Localization.Get(defenseType.ToString());
            this.equipWeight.text = equipWeight;
            this.equipDef.text = equipDef;
            this.equipLevel.text = equipLevel;
            this.equipRarity.text = Localization.Get(equipRarity);
            this.equipRarity.color = (Color)rarityColor;
            this.equipAbility.text = equipAbility;
        }

        void OnClickWear()
        {
            PlayerManager.Singleton.initialEquipmentDefenseType = tmpDefenseType;
            PlayerManager.Singleton.playerEquipment.equippedClassDefaultDefence = PlayerManager.Singleton.GetDefaultClassDefense();
            GameApplication.Singleton.view.EquipmentView.SetDefaultDefence();
            gameObject.SetActive(false);
        }
    }
}
