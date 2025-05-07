using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game
{
    public class HideObjOnButtonPress : MonoBehaviour
    {
        public Button btn;

        private void OnEnable()
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(HideObject);
            Debug.LogWarning("Ricordarsi di rimuovere il tasto prima della build ufficiale dato che causa un bug che chiude la scena e apre la pagina non ancora caricata");
        }
        public void HideObject()
        {
            MapAnimationManager.mapsAnimationIsFinish = true;
            this.gameObject.SetActive(false);
        }
    }
}

