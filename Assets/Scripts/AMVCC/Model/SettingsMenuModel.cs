using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingsMenuModel : Model<GameApplication>
    {
        public bool fullscreen;
        public float volume;
        public TMP_FontAsset font1;
        public TMP_FontAsset defaultFont;
        public RawImage imgShadow;
        public RawImage imgBackground;
    }
}
