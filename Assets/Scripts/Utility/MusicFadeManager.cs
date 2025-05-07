using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class MusicFadeManager : Model<GameApplication>
    {
        public static IEnumerator MusicFadeIn(AudioSource audioSource, float FastFade, float SlowFade, float ModerateFade, float MaxVolume)
        {
            if (PlayerManager.Singleton.currentPage != null &&
             PlayerManager.Singleton.currentPage.GetFadeInFromZero)
            {
                audioSource.volume = 0;
            }

            float increaseVolume = 0.4f;

            while (audioSource.volume < MaxVolume)
            {
                if (audioSource.volume < 0.04)
                {
                    audioSource.volume += increaseVolume * Time.deltaTime / SlowFade;
                }
                else if ((audioSource.volume < 0.2) && (audioSource.volume > 0.04))
                {
                    audioSource.volume += increaseVolume * Time.deltaTime / ModerateFade;
                }
                else
                {
                    audioSource.volume += increaseVolume * Time.deltaTime / FastFade;
                }

                yield return null;
            }

        }

        public static IEnumerator EffectFadeIn(AudioSource audioSource, float FadeTime,float FastFade,float maxVolume )
        {
            audioSource.volume = 0;
            float increaseVolume = 0.4f;

            while (audioSource.volume < maxVolume)
            {
                if (audioSource.volume < 0.4)
                {
                    audioSource.volume += increaseVolume * Time.deltaTime / FastFade;
                }
                else
                {
                 audioSource.volume += increaseVolume * Time.deltaTime / FadeTime;
                }

                yield return null;
            }

        }


        /// <summary>
        /// 3 riferimenti:
        /// -FadeOut inizio gioco che va a 0
        /// -FadeOut Titoli che porta a 0
        /// -FadeOut per far risaltare neffetti che va a 0.026
        /// <returns></returns>
        /// 

        public static IEnumerator MusicFadeOut(AudioSource audioSource, float lowVolumeFadeVelocity, float highVolumeFadeVelocity, float mediumVolumeFadeVelocity,float minMusicVolume)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > minMusicVolume)
            {
                if (audioSource.volume > 0.3)
                {
                    audioSource.volume -= startVolume * Time.deltaTime / highVolumeFadeVelocity;
                }
                else if ((audioSource.volume > 0.1) && (audioSource.volume < 0.3))
                {
                    audioSource.volume -= startVolume * Time.deltaTime / mediumVolumeFadeVelocity;
                }
                else
                {
                    audioSource.volume -= startVolume * Time.deltaTime / lowVolumeFadeVelocity;
                }

                yield return null;
            }

        }

        public static IEnumerator EffectFadeOut(AudioSource audioSource, float FadeTime,float FastFade )
        {
            float increaseVolume = 0.4f;

            while (audioSource.volume > 0)
            {
                if (audioSource.volume > 0.3)
                {
                    audioSource.volume -= increaseVolume * Time.deltaTime / FastFade;
                }
                else
                {
                audioSource.volume -= increaseVolume * Time.deltaTime / FadeTime;

                }

                yield return null;
            }
            audioSource.clip = null;
        }
    }
}
