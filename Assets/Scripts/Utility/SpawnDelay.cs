using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Game
{
    public class SpawnDelay : MonoBehaviour
    {
        //ho provato con la courutine ma nn va

        public VisualEffect spawnVisualEffect;
        float currentTime = 2f;

        void Start()
        {
            spawnVisualEffect.pause = true;
        }

        void Update()
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }

            else
            {
                spawnVisualEffect.pause = false;
            }
        }

    }
}