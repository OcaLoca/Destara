/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    public class HideObjAndOpenGameAt : MonoBehaviour
    {
        [SerializeField] string ID;

        public void HideObject()
        {
            OpenGameAt(ID);
            this.gameObject.SetActive(false);
        }
        void OpenGameAt(params object[] data)
        {
            string pageID = (string)data[0];

            PlayerManager.Singleton.pagesRead.Add(pageID);

            if (GameApplication.Singleton.model.currentView)
            {
                GameApplication.Singleton.model.currentView.gameObject.SetActive(false);
                GameApplication.Singleton.model.previousView.Push(GameApplication.Singleton.model.currentView);
            }

            GameApplication.Singleton.model.currentView = GameApplication.Singleton.view.BookView;
            GameApplication.Singleton.model.currentView.gameObject.SetActive(true);
        }

    }

}
