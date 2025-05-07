using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;

namespace Game
{
    public class BagButtonManager : MonoBehaviour
    {
        [SerializeField] TMP_Text txtButtonPrefab;
        [SerializeField] TMP_Text txtQuantity;
        private string txtButton;
        private int number;
       

        public void Setup(ScriptableItem item, int quantity = 1)
        {
            txtButton = Localization.Get(item.itemNameLocalization);
            number = quantity;
    
            UploadData();
        }

        public void Setup(Unit unit)
        {
            txtButton = unit.name;
            UploadData();
        }

        public void TurnOnSelectedBagButton(ScriptableItem item)
        {
            //selectedButtonPressed.gameObject.SetActive(true);
        }

        public void TurnOffBagButton()
        {
            //selectedButtonPressed.gameObject.SetActive(false);
        }

        void UploadData()
        {
            //lilyTexturePrefab = lilyTexture;
            txtButtonPrefab.text = txtButton;
            txtQuantity.text = string.Format("x" + number.ToString());
        }
    }
}
