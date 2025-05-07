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
using UnityEngine.Audio;
using SmartMVC;

namespace Game
{
    public class SoundManager : Model<GameApplication>
    {
        public static SoundManager Singleton { get; private set; }
        public AudioSource audioSource;
        public AudioMixer audioMixer;
        public AudioClip menuAudio;
        public List<AudioClip> audioClipsDatabase = new List<AudioClip>();
        public float maxVolume;
        public float starterVolume;

        private void Awake()
        {
            Singleton = this;
        }

        public void ChangeVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void LoadMusicAudioClip(ScriptablePage page = null)
        {
            //se non è silenziata controllo l'audio
            if (!app.view.Settings.GetMusicIsSilenced)
            {
                MusicSoundIsActive();
            }

            if (page == null)
            {
                Play(app.Sounds.MetagameSoundtrack);
                return;
            }

            switch (page.GetSoundtrackSection)
            {
                case ScriptablePage.SoundBackground.NoMusic:
                    break;
                case ScriptablePage.SoundBackground.AllaSaluteSoundtrack:
                    Play(app.Sounds.AllaSaluteSoundtrack);
                    break;
                case ScriptablePage.SoundBackground.ScappaSoundtrack:
                    Play(app.Sounds.ScappaSoundtrack);
                    break;
                case ScriptablePage.SoundBackground.InVinoVeritasSoundtrack:
                    Play(app.Sounds.InVinoVeritasSoundtrack);
                    break;
                case ScriptablePage.SoundBackground.GenericFightSoundtrack:
                    Play(app.Sounds.GenericFightSoundtrack);
                    break;
                case ScriptablePage.SoundBackground.GenericEscapeRoomSoundtrack:
                    break;
                case ScriptablePage.SoundBackground.Death:
                    Play(app.Sounds.DeathSoundtrack);
                    break;
                default:
                    break;
            }
        }

        void MusicSoundIsActive()
        {
            if (!SoundEffectManager.soundEffectIsPlaying || app.view.Settings.GetMusicIsSilenced)
            {
                StartCoroutine(MusicFadeManager.MusicFadeIn(audioSource, 0.6f, 65f, 14f, maxVolume));
            }
            else  ///qua cambio il minimo del volume della musica in base alla canzone di sottofondo 
            {
                switch (PlayerManager.Singleton.currentPage.chapterSection)
                {
                    case ScriptablePage.Section.Domande:
                        MusicFadeOut(0.026f);

                        break;
                    case ScriptablePage.Section.Nulla:
                        MusicFadeOut(0.026f);

                        break;
                    case ScriptablePage.Section.AllaSalute:
                        MusicFadeOut(0.026f);

                        break;
                    case ScriptablePage.Section.Scappa:
                        MusicFadeOut(0.1f);

                        break;
                    case ScriptablePage.Section.InVinoVeritas:
                        MusicFadeOut(0.026f);

                        break;
                    case ScriptablePage.Section.DuraLex:
                        MusicFadeOut(0.026f);

                        break;
                    case ScriptablePage.Section.Oscurità:
                        break;
                    case ScriptablePage.Section.Catacombe:
                        break;
                    case ScriptablePage.Section.Monastero:
                        break;
                    case ScriptablePage.Section.Porto:
                        break;
                    case ScriptablePage.Section.Nave:
                        break;
                    case ScriptablePage.Section.Lebbrosario:
                        break;
                    case ScriptablePage.Section.NonUsare:
                        break;
                    case ScriptablePage.Section.Titolo:
                        break;
                    case ScriptablePage.Section.Fight:
                        break;
                    default:
                        MusicFadeOut(0.026f);
                        break;
                }

            }
        }

        internal void SilenceMusicAudioManager(bool muteAudio)
        {
            audioSource.mute = muteAudio;
        }

        internal void MusicFadeOut(float minMusicVolume = 0, float lowVolumeFadeVelocity = 50f, float highVolumeFadeVelocity = 1f, float mediumVolumeFadeVelocity = 14f)
        {
            StartCoroutine(MusicFadeManager.MusicFadeOut(audioSource, lowVolumeFadeVelocity, highVolumeFadeVelocity, mediumVolumeFadeVelocity, minMusicVolume));
        }

        internal void MusicFadeIn(float maxVolume)
        {
            StartCoroutine(MusicFadeManager.MusicFadeIn(audioSource, 0.6f, 65f, 14f, maxVolume));
        }

        public void Play(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

    }
}
