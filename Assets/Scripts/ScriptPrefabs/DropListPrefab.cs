using StarworkGC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DropListPrefab : MonoBehaviour
    {

        [SerializeField]
        TMP_Text lblItemName;

        [SerializeField]
        TMP_Text lblItemRarity;
        void Start()
        {

        }

        public void Setup(ScriptableItem itemData)
        {

            int rarityValue = (int)itemData.rarity;
            lblItemRarity.text = RarityCheck(rarityValue);

            if (Localization.Get(itemData.itemNameLocalization) != string.Empty && Localization.Get(itemData.itemNameLocalization) != " ")
            {
                lblItemName.text = Localization.Get(itemData.itemNameLocalization);
            }
            else
            {
                lblItemName.text = itemData.name;
            }

        }

        void Update()
        {

        }

        public static string RarityCheck(int rarityValue)
        {
            switch (rarityValue)
            {
                case 0:
                    return "Common";
                    break;
                case 1:
                    return "Usual";
                    break;
                case 2:
                    return "Inusual";
                    break;
                case 3:
                    return "Rare";
                    break;
                case 4:
                    return "Legendary";
                    break;
            }
            return "error";
        }

    }

}