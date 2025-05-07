/* ----------------------------------------------
 * 
 * 				MobyBit
 * 
 * Creation Date: 01/09/2020 00:10:30
 * 
 * Copyright � MobyBit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class SoundEffectManager : Model<GameApplication>
    {
        public AudioSource audioSource;
        public List<AudioClip> audioClipsDatabase = new List<AudioClip>();
        public float maxVolume;
        public static SoundEffectManager Singleton { get; set; }

        private AudioClip _audioClip;
        private float _volume;

        private void Awake()
        {
            Singleton = this;
            maxVolume = 1;
            audioSource.Play();

            foreach (AudioClip data in Resources.LoadAll<AudioClip>("AudioEffects"))
            {
                audioClipsDatabase.Add(data);
            }
        }

        Coroutine playEffectAudio;
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public static bool soundEffectIsPlaying = false;
        public void LoadSoundEffectDuringReadingPhase(ScriptablePage page)
        {
            if (page == null) { return; }

            if (page.HaveEffectAudio)
            {
                LoadNewEffect(page);
                return;
            }

            FadeOutCurrentEffect();
        }

        private void FadeOutCurrentEffect()
        {
            if (audioSource.isPlaying) // Assicura che l'audio sia in esecuzione prima del fade out
            {
                playEffectAudio = StartCoroutine(MusicFadeManager.EffectFadeOut(audioSource, 10f, 1f));
                soundEffectIsPlaying = false;
            }
        }

        void LoadNewEffect(ScriptablePage page)
        {
            if (!app.view.Settings.GetEffectIsSilenced)
            {
                playEffectAudio = StartCoroutine(MusicFadeManager.EffectFadeIn(audioSource, 3f, 1f, maxVolume));
            }

            foreach (AudioClip audio in audioClipsDatabase)
            {
                if (page.GetEffectAudioID == audio.name)
                {
                    soundEffectIsPlaying = true;
                    PlayAudioClip(audio, page.effectAudioVolume, page.effectWithLoop, page.effectStartDelay);
                    return;
                }
            }
        }

        public void PlayAudioClip(AudioClip audioClip, float volume = 1, bool loop = false, float delay = 0)
        {
            playEffectAudio = StartCoroutine(PlayThisAudioClip(audioClip, volume, loop, delay));
        }

        IEnumerator PlayThisAudioClip(AudioClip audioClip, float volume = 1, bool loop = false, float delay = 0)
        {
            _audioClip = audioClip;
            _volume = Mathf.Clamp(volume, 0f, 1.0f);
            audioSource.clip = _audioClip;
            audioSource.loop = loop;
            audioSource.volume = _volume;

            yield return new WaitForSeconds(delay);
            audioSource.Play();
        }

        public void PlayAudio(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
