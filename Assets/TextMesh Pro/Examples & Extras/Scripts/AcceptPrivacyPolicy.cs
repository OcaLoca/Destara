/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class AcceptPrivacyPolicy : MonoBehaviour
    {
        [SerializeField] GameObject PanelPrivacy;

        private void Start()
        {
            if (PlayerPrefs.GetInt(LocalizationIDDatabase.PRIVACYPOLICY) == 0)
            {
                PanelPrivacy.SetActive(true);
                PlayerPrefs.SetInt(LocalizationIDDatabase.PRIVACYPOLICY, 1);
            }
        }
        public void Accept()
        {
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.ConfirmDifficultButton);
            PanelPrivacy.SetActive(false);
        }
    }

}
