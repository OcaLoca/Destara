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
    public class BookModel : Model<GameApplication>
    {
        public Transform chapterTransform;
        public LoadImage coverOfTheChapter;
        public LongQuestionDatabase longQuestionDatabase;
        public LoadSpriteAsset loadSpriteAsset;
        public DissolveEffect dissolveEffect;
        public SubtitleManager subtitleManager;
        public BattleGroundDatabase loadBattleground;
        public EscapeRoomDatabase loadEscapeRoom;
        public LoadMainCharacter loadMainCharacter;
        public SoundManager soundManager;
        public SoundEffectManager soundEffectManager;
        public MusicFadeManager musicFadeManager;
        public Sprite[] classIconSprites;
        public UISoundManager UISoundManager;
    }
}
