using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlobalLightManager : MonoBehaviour
    {
       // public GameObject globalLight;
        public float dissolveVelocity;
        [SerializeField] float globalLightAmount;

        void OnEnable()
        {
            DeactivateGlobalLight();
        }

        public void ActivateGlobalLight()
        {
            //globalLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 1;
            gameObject.SetActive(false);
        }
        public void DeactivateGlobalLight()
        {
           // globalLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0.35f;
        }

        public void OnFinishAnimationEnableSearchButton()
        {
            GameApplication.Singleton.view.BookView.PlayerCanUseTorch();
        }


    }
}
