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
    public class VictoryPanelManager : MonoBehaviour
    {
        public static VictoryPanelManager instance;

        private void Awake()
        {
            instance = this;
        }

        [SerializeField] TMP_Text xpGained;
        [SerializeField] GameObject dropObjectRecord;
        [SerializeField] TMP_Text objectName;
        [SerializeField] TMP_Text objectLevel;
        [SerializeField] Image objSpriteIcon;


        internal void PlayVictoryLosePanelAudio(AudioClip audioClip)
        {
            UISoundManager.Singleton.PlayAudioClip(audioClip);
        }

        public void DropBattleObjAndGiveXp(float xpGained = 0, ScriptableItem obj = null)
        {
            this.xpGained.text = "+" + xpGained.ToString() + " xp";
            this.xpGained.color = ColorDatabase.Singleton.levelColor;

            if (obj)
            {
                switch (obj)
                {
                    case Weapon:
                        Weapon weapon = (Weapon)obj;
                        weapon.SetWeaponIcon();
                        break;
                    case Equipment:
                        Equipment equipment = (Equipment)obj;
                        equipment.SetEquipmentIcon();
                        break;
                    default:
                        break;
                }

                dropObjectRecord.SetActive(true);
                objectName.text = obj.GetLocalizedObjName();
                objectLevel.text = "lvl " + obj.level.ToString();
                objSpriteIcon.color = ColorDatabase.Singleton.GetRarityColor(obj.rarity);
                objSpriteIcon.sprite = obj.icon;

                PlayerManager.Singleton.AddItemDrop(obj);
            }
            else
            {
                dropObjectRecord.SetActive(false);
            }
        }
    }

}
