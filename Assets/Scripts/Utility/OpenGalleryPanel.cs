using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;
namespace Game
{
    public class OpenGalleryPanel : MonoBehaviour
    {
        public GameObject panel;
        public Button openScrollPanel;
        public Button btnBack;
        

        // Start is called before the first frame update
        void Start()
        {
            openScrollPanel.onClick.RemoveAllListeners();
            openScrollPanel.onClick.AddListener(OnClickOpenScroll);
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);
        }


        void OnClickOpenScroll()
        {
            panel.SetActive(true);
        }
        void OnClickBack()
        {
            panel.SetActive(false);
        }
    }
}
