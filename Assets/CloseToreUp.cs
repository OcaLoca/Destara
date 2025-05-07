using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CloseToreUp : MonoBehaviour
    {
        // Start is called before the first frame update

        public void ShowBuffes()
        {
            GameApplication.Singleton.view.BookView.GetPopup.gameObject.SetActive(false);
        }

        public void Ready()
        {
            GameApplication.Singleton.view.BookView.animatorReady = true;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
