/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ShowAccessoryInfoPanel : MonoBehaviour
    {
        public static ShowAccessoryInfoPanel Singleton { get; set; }
        [SerializeField] Button btnClose, btnChangeToBio;

        private void Awake()
        {
            Singleton = this;
        }

        public Image equipmentImage;
        public TMP_Text equipName, equipRarity, equipLevel, equipDsc, equipType;

        private void Start()
        {
            btnChangeToBio.onClick.RemoveAllListeners();
            btnChangeToBio.onClick.AddListener(ShowBioOrInfo);
        }

        private void OnEnable()
        {
            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(delegate {
                UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
                gameObject.SetActive(false); });
        }


        bool showBio = true;
        string txtBio;
        string txtInfo;

        void ShowBioOrInfo()
        {
            equipDsc.text = string.Empty;
           
            if (showBio)
            {
                equipDsc.text = txtBio;
                showBio = false;
            }
            else
            {
                equipDsc.text = txtInfo;
                showBio = true;
            }

            Canvas.ForceUpdateCanvases();
        }
      
        public void ShowTalismanInfo(string equipName, string equipType, string equipRarity, string equipLevel, string equipDsc, string equipBio,
             Sprite equipmentImage = null, Color32? rarityColor = null)
        {
            if (equipmentImage != null)
            {
                this.equipmentImage.sprite = equipmentImage;
                this.equipmentImage.color = (Color)rarityColor;
            }
            txtBio = equipBio;
            txtInfo = equipDsc;
            this.equipType.text = equipType; 
            this.equipName.text = Localization.Get(equipName);
            this.equipName.color = (Color)rarityColor;
            this.equipLevel.text = equipLevel;
            this.equipDsc.text = equipDsc;
            this.equipRarity.text = Localization.Get(equipRarity); 
        }
    }
}
