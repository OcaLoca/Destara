/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ChangedStats : MonoBehaviour
    {
        [SerializeField] Image sprite;
        [SerializeField] Sprite lifeIcon;
        [SerializeField] TMP_Text iconHp, iconAP, iconStamina;
        [SerializeField] TMP_Text buffDebuffAmount, txtStateName;

        public void SetupChangedStats(Sprite sprite, int buffDebuffAmount, string statsName)
        {
            Color32 iconColor = new Color32();
            iconColor = ColorDatabase.Singleton.GetColorByName(statsName);

            this.buffDebuffAmount.text = buffDebuffAmount > 0 ? "+" + buffDebuffAmount.ToString() : buffDebuffAmount.ToString();
            txtStateName.text = Localization.Get(statsName);

            if(statsName == "Life") 
            {
                this.sprite.enabled = false;
                iconHp.gameObject.SetActive(true);
                return;
            }

            this.sprite.enabled = true;
            this.sprite.sprite = sprite;
            this.sprite.color = iconColor;
        }
    }
}

