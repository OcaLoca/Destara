using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class ImgPopupManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public void PopupShowed()
        {
            GameApplication.Singleton.view.BookView.imageShowed = true;
        }
    }
}

