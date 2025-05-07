using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;

namespace Game
{
    public class HideImage : View <GameApplication>
    {
       [SerializeField] RawImage imgLag;

        private void Start()
        {
            if (PlayerManager.Singleton.currentPage.pageID == "intro")
            {
                if(imgLag == null) return;
                imgLag.gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            if (PlayerManager.Singleton.currentPage.pageID == "intro")
            {
                imgLag = gameObject.GetComponent<RawImage>();
                imgLag.CrossFadeAlpha(0, 0.1f, false);
                imgLag.gameObject.SetActive(false);
            }
        }
    }
}
