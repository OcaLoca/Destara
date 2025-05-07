/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using Crosstales.BWF;
using Crosstales.BWF.Model.Enum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    
    public class WordFilter : MonoBehaviour
    {
        [SerializeField] TMP_InputField TMP_InputField;
        [SerializeField] ManagerMask Mask;
        [SerializeField] string[] Source;

        public void Filter()
        {
            TMP_InputField.text = BWFManager.Instance.ReplaceAll(TMP_InputField.text, Mask, Source);
        }
       

    }

}
