using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewFade : MonoBehaviour
    {
        public Image imageFade;
        IEnumerator courutine;

        void Start()
        {
            courutine = enumerator();
            imageFade.canvasRenderer.SetAlpha(1.0f);
            StartCoroutine(courutine);
        }


        IEnumerator enumerator() 
        {
            imageFade.CrossFadeAlpha(0, 2, false);
            yield return new WaitForSeconds(2);
            imageFade.gameObject.SetActive(false);
        }
    }
}
