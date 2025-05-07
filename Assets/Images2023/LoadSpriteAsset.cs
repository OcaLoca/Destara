using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LoadSpriteAsset : MonoBehaviour
    {
        public List<TMP_SpriteAsset> spriteDatabase;
        public TextMeshProUGUI textMeshPro;

        void Awake()
        {
            foreach (TMP_SpriteAsset data in Resources.LoadAll<TMP_SpriteAsset>("PersonalSprite"))
            {
                spriteDatabase.Add(data);
            }

            textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        }

        public TMP_SpriteAsset SearchSprite()
        {
            foreach (TMP_SpriteAsset personalSprite in spriteDatabase)
            {
                if (personalSprite.name == PlayerManager.Singleton.currentPage.chapterSection.ToString())
                {
                    return personalSprite;
                }
            }
            return null;
        }

        public void LoadCurrentChapterSpriteAtlas()
        {
            if(SearchSprite() == null) { return; }
            TMP_SpriteAsset temporarySprite = SearchSprite();
            textMeshPro.spriteAsset = temporarySprite;
        }
    }
}
