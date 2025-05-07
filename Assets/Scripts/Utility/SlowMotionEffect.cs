/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SlowMotionEffect : MonoBehaviour
    {
        public static SlowMotionEffect Instance { get; private set; }
        [SerializeField, Tooltip("Corresponds to the real-time duration in seconds of the slow motion effect.")]
        float slowMotionDuration;
        [SerializeField, Tooltip("The scale at which not real-time in seconds passes during the slow motion effect.")]
        float slowMotionTimeScale;
        Coroutine runningSlowMotionEffect;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void EnableSlowMotion(float slowMotionTimeScale, float slowMotionDuration)
        {
            Time.timeScale = slowMotionTimeScale;
            runningSlowMotionEffect = StartCoroutine(StartSlowMotion(slowMotionDuration));
        }
        IEnumerator StartSlowMotion(float slowMotionDuration)
        {
            yield return new WaitForSecondsRealtime(slowMotionDuration);
            Time.timeScale = 1;
        }

    }

}
