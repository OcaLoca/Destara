using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class RawViewFade : MonoBehaviour
    {
        public RawImage rawImageFade;
        IEnumerator coroutin;

        void Start()
        {
            coroutin = enumerat();
            rawImageFade.canvasRenderer.SetAlpha(1.0f);
            StartCoroutine(coroutin);
        }


        IEnumerator enumerat() 
        {
            rawImageFade.CrossFadeAlpha(0, 4.2f, false);
            yield return new WaitForSeconds(3);
            rawImageFade.gameObject.SetActive(false);
        }
    }
}
