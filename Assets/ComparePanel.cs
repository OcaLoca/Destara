using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class ComparePanel : MonoBehaviour
    {
        [Header("RectTransform")]
        [SerializeField] RectTransform panelsContainer;

        [Header("Texts")]
        [SerializeField] TMP_Text itemName;
        [SerializeField] TMP_Text itemLvlValue;
        [SerializeField] TMP_Text itemRarityValue;
        [SerializeField] TMP_Text itemDMGLabel;
        [SerializeField] TMP_Text itemDMGValue;
        [SerializeField] TMP_Text itemCTRLabel;
        [SerializeField] TMP_Text itemCRTValue;
        [SerializeField] TMP_Text itemHITValue;
        [SerializeField] TMP_Text itemSTValue;
        [SerializeField] TMP_Text itemInfo;

        [Header("Container")]
        [SerializeField] GameObject containerDmg, containerCrt, containerHit, containerStamina, containerInfo;

        [Header("Images")]

        [SerializeField] Image lvl_comparing_Img;
        //[SerializeField] Image rarity_comparing_Img;
        [SerializeField] Image damageCompareImg;
        [SerializeField] Image stamina_comparing_Img;
        [SerializeField] Image ctr_comparing_Img;
        [SerializeField] Image hit_comparing_Img;

        [Header("Sprites")]
        [SerializeField] Sprite buffImg, debuffImg;

        public void Setup()
        {
            itemName.text = "N.D.";
            itemLvlValue.text = "N.D.";
            itemDMGValue.text = "N.D.";
            itemCRTValue.text = "N.D.";
            itemHITValue.text = "N.D.";
            itemSTValue.text = "N.D.";
        }
        public void SetItem(Weapon item)
        {
            itemName.text = item.GetLocalizedObjName();
            itemLvlValue.text = item.level.ToString();
            itemDMGValue.text = "" + item.weaponConstDamage;
            itemCRTValue.text = item.criticalDamageChance.ToString();
            itemHITValue.text = item.hitChance.ToString();
            itemSTValue.text = item.staminaCost.ToString();

            itemDMGLabel.text = "DMG";
            itemCTRLabel.text = "CTR";

        }
        public void SetItem(Weapon item, Weapon compare_item)
        {
            itemName.text = item.GetLocalizedObjName();
            itemName.color = GetRarity(item.rarity);
            itemRarityValue.text = compare_item.rarity.ToString();
            itemRarityValue.color = GetRarity(item.rarity);
            itemLvlValue.text = item.level.ToString();
            itemSTValue.text = item.staminaCost.ToString();
            itemDMGValue.text = "" + item.weaponConstDamage;
            itemCRTValue.text = item.criticalDamageChance.ToString();
            itemHITValue.text = item.hitChance.ToString();

            itemDMGLabel.text = "DMG";
            itemCTRLabel.text = "CTR";

            Compare(item, compare_item);
        }
        public void SetItem(Equipment item, Equipment compare_item)
        {
            itemName.text = item.GetLocalizedObjName();
            itemName.color = GetRarity(item.rarity);
            itemRarityValue.text = compare_item.rarity.ToString();
            itemRarityValue.color = GetRarity(item.rarity);
            itemLvlValue.text = item.level.ToString();
            itemDMGValue.text = "" + item.defence;
            itemCRTValue.text = item.arcaneDefenceBuffDebuff.ToString();
            itemSTValue.text = item.StaminaCost.ToString();
            itemHITValue.text = "N.D.";

            itemDMGLabel.text = "DEF";
            itemCTRLabel.text = "A-DEf";

            Compare(item, compare_item);
        }

        public void SetAccessoryItem(Equipment item, Equipment compare_item)
        {
            itemName.text = item.GetLocalizedObjName();
            itemName.color = GetRarity(item.rarity);
            itemRarityValue.text = compare_item.rarity.ToString();
            itemRarityValue.color = GetRarity(item.rarity);
            itemLvlValue.text = item.level.ToString();

            Compare(item, compare_item, true);
        }

        public void SetItem()
        {
            itemName.text = "NOT EQUIPPED";
            itemLvlValue.text = "N.D.";
            itemDMGValue.text = "N.D.";
            itemCRTValue.text = "N.D.";
            itemHITValue.text = "N.D.";
            itemSTValue.text = "N.D.";
            itemDMGLabel.text = "DMG";
            itemCTRLabel.text = "CTR";
        }

        public Color32 GetRarity(ScriptableItem.Rarity rarity)
        {
            return ColorDatabase.Singleton.GetRarityColor(rarity);
        }

        public void Compare(Weapon item, Weapon other_item)
        {
            Color32 invisible = new Color32(0, 0, 0, 0);
            Color32 defaultColour = lvl_comparing_Img.color;

            invisible.a = 0;
            itemRarityValue.text = item.GetLocalizedRarity(item.rarity);
            itemRarityValue.color = GetRarity(item.rarity);

            containerDmg.gameObject.SetActive(true);
            containerStamina.gameObject.SetActive(true);
            containerCrt.gameObject.SetActive(true);
            containerHit.gameObject.SetActive(true);
            itemInfo.text = string.Empty;

            if (item.level > other_item.level)
            {
                lvl_comparing_Img.color = defaultColour;
                lvl_comparing_Img.sprite = buffImg;
            }
            else if (item.level < other_item.level)
            {
                lvl_comparing_Img.color = defaultColour;
                lvl_comparing_Img.sprite = debuffImg;
            }
            else
                lvl_comparing_Img.color = invisible;

            if (item.weaponConstDamage > other_item.weaponConstDamage)
            {
                damageCompareImg.sprite = buffImg;
            }
            else if (item.weaponConstDamage < other_item.weaponConstDamage)
            {
                damageCompareImg.sprite = debuffImg;
            }
            else
                damageCompareImg = null;

            if (item.staminaCost > other_item.staminaCost)
            {
                stamina_comparing_Img.gameObject.SetActive(true);
                stamina_comparing_Img.sprite = debuffImg;
            }
            else if (item.staminaCost < other_item.staminaCost)
            {
                stamina_comparing_Img.gameObject.SetActive(true);
                stamina_comparing_Img.sprite = buffImg;
            }
            else
            {
                stamina_comparing_Img.gameObject.SetActive(false);
            }

            if (item.criticalDamageChance >= other_item.criticalDamageChance)
            {
                ctr_comparing_Img.sprite = buffImg;
            }
            else
            {
                ctr_comparing_Img.sprite = debuffImg;
            }

            if (item.hitChance >= other_item.hitChance)
            {
                hit_comparing_Img.sprite = buffImg;
            }
            else
            {
                hit_comparing_Img.sprite = debuffImg;
            }

            ForzaDimensioni();
        }

        public void Compare(Equipment item, Equipment other_item, bool isAccessory = false)
        {
            itemRarityValue.text = item.GetLocalizedRarity(item.rarity);
            itemRarityValue.color = GetRarity(item.rarity);
            containerCrt.gameObject.SetActive(false);
            containerHit.gameObject.SetActive(false);

            if (item.level > other_item.level)
            {
                lvl_comparing_Img.enabled = true;
                lvl_comparing_Img.sprite = buffImg;
            }
            else if (item.level < other_item.level)
            {
                lvl_comparing_Img.enabled = true;
                lvl_comparing_Img.sprite = debuffImg;
            }
            else
            {
                lvl_comparing_Img.enabled = false;
            }


            if (isAccessory)
            {
                containerDmg.gameObject.SetActive(false); //contiene difesa un caso di difesa 
                containerStamina.gameObject.SetActive(false);
                itemInfo.text = item.GetLocalizedInfo();
                ForzaDimensioni();
                return;

            }
            else
            {
                containerDmg.gameObject.SetActive(true);
                containerStamina.gameObject.SetActive(true);
                itemInfo.text = string.Empty;
            }


            if (item.StaminaCost > other_item.StaminaCost)
            {
                stamina_comparing_Img.gameObject.SetActive(true);
                stamina_comparing_Img.sprite = debuffImg;
            }
            else if (item.StaminaCost < other_item.StaminaCost)
            {
                stamina_comparing_Img.gameObject.SetActive(true);
                stamina_comparing_Img.sprite = buffImg;
            }
            else
            {
                stamina_comparing_Img.gameObject.SetActive(false);
            }

            if (item.defence > other_item.defence)
            {
                damageCompareImg.sprite = buffImg;
            }
            else if (item.defence < other_item.defence)
            {
                damageCompareImg.sprite = debuffImg;
            }
            else
                damageCompareImg = null;

            ForzaDimensioni();
        }

        void ForzaDimensioni()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(panelsContainer);
        }

    }

}