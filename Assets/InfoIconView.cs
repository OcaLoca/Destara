/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InfoIconView : MonoBehaviour
    {
        [SerializeField] Button btnBack;
        [SerializeField] Button btnPreviewIcon;
        [SerializeField] Button btnNextIcon;


        private void OnEnable()
        {
            btnBack.onClick.RemoveAllListeners();
            btnPreviewIcon.onClick.RemoveAllListeners();
            btnNextIcon.onClick.RemoveAllListeners();

            btnBack.onClick.AddListener(OnBackClick);
            btnNextIcon.onClick.AddListener(OnBackClick);
            btnPreviewIcon.onClick.AddListener(OnBackClick);
        }

        void OnBackClick()
        {
            gameObject.SetActive(false);
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
        }




    }

}
