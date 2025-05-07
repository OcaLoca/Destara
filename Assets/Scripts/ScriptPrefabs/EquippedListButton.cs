using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EquippedListButton : MonoBehaviour
    {

        [SerializeField]
        Button btnEquippedList;

        [SerializeField]
        TMP_Text itemEquippedName;
        public void Setup(Weapon equipData)
        {
            itemEquippedName.text = "*" + equipData.name;
        }
        //Setup per Equip
        public void Setup(Equipment equipData)
        {
            itemEquippedName.text = "*" + equipData.name;
        }
    }
}