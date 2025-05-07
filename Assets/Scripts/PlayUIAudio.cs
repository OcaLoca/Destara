/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace Game
{
    public class PlayUIAudio : MonoBehaviour
    {
        [SerializeField] UISOUND FXToPlay;
        public enum UISOUND
        {
            GenericBackSound,
            GenericClickSound
        }

        public void PlayAudio()
        {
            if(FXToPlay == UISOUND.GenericClickSound)
            {
                UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericMenuButton);
                return;
            }
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.GenericBackButton);
        }

    }

}
