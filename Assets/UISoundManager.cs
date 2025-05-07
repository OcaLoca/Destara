/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.Rendering;

namespace Game
{
    public class UISoundManager : Model<GameApplication>
    {
        public static UISoundManager Singleton { get; set; }
        public AudioSource audioSource;
        public List<AudioClip> audioClipsDatabase = new List<AudioClip>();
        public float maxVolume;
        //
        private AudioClip _audioClip;
        private float _volume;


        private void Awake()
        {
            Singleton = this;
            maxVolume = 1;

            foreach (AudioClip data in Resources.LoadAll<AudioClip>("UIEffect"))
            {
                audioClipsDatabase.Add(data);
            }
        }

        public void PlayAudioClip(AudioClip audioClip, float volume = 0.5f, bool loop = false)
        {
            if(volume > maxVolume) 
                volume = maxVolume;
            _audioClip = audioClip;
            _volume = Mathf.Clamp(volume, 0f, 1.0f);
            audioSource.clip = _audioClip;
            audioSource.loop = loop;
            audioSource.volume = _volume;
            audioSource.Play();
        }

    }

}
