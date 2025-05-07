using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class DissolveEffect : MonoBehaviour
    {
        [SerializeField] private Material material;

        public float dissolveAmount;
        public bool isDissolving;
        public float dissolveVelocity;
        public bool isFinished;

        public void Update()
        {
            if (isDissolving)
            {
                DissolveImage();
            }
            else
            {
                RewindDissolve();
            }
        }

        public void DissolveImage()
        {
                dissolveAmount = Mathf.Clamp(1.7f, 0, dissolveAmount + Time.deltaTime * dissolveVelocity);
                material.SetFloat("_DissolveAmount", dissolveAmount);
        }

        public void RewindDissolve()
        {
                dissolveAmount = Mathf.Clamp(1.7f, 0, dissolveAmount - Time.deltaTime * dissolveVelocity);
                material.SetFloat("_DissolveAmount", dissolveAmount);
                isFinished = true;
        }

        public void ResetDissolveAmount()
        {
            if (isFinished)
            {
                dissolveAmount = 0;
            }

        }


    }
}