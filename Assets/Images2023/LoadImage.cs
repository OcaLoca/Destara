using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;
using TMPro;
using StarworkGC.Localization;

namespace Game
{
    public class LoadImage : MonoBehaviour
    {
        public RawImage imageToShow;
        public List<ScriptableImage> imagesDatabase;
        [SerializeField] TMP_Text txtTitle;
        [SerializeField] TMP_Text txtSubtitle;
        [SerializeField] Image chapterIcon;
        public GameObject panelContainer;

        public bool GetTitleImageIsActive { get { return gameObject.activeSelf; } }
        void Awake()
        {
            foreach (ScriptableImage data in Resources.LoadAll<ScriptableImage>("ScriptableImage"))
            {
                imagesDatabase.Add(data);
            }
        }

        public ScriptableImage LoadScriptableImage()
        {
            foreach (ScriptableImage scriptableImage in imagesDatabase)
            {
                if (scriptableImage.imageID == PlayerManager.Singleton.currentPage.pageID)
                {
                    return scriptableImage;
                }
            }
            return null;
        }

        /// <summary>
        /// Set the cover text and image before the end of the maps video
        /// </summary>
        /// <param name="tmpImage"></param>
        /// <param name="page"></param>
        public void SetChapterCover(ScriptableImage tmpImage, ScriptablePage page)
        {
            panelContainer.gameObject.SetActive(true);
            txtTitle.gameObject.SetActive(true);
            txtSubtitle.gameObject.SetActive(true);
            chapterIcon.gameObject.SetActive(true);
            txtTitle.text = Localization.Get(page.txtTitle);
            txtSubtitle.text = Localization.Get(page.txtSubtitle);
            chapterIcon.sprite = LoadChapterIconOnTitle(page);
            chapterIcon.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_DEFENCE_COLOR);

            if ((tmpImage == null) || (page.pageID == PageIDDatabase.SCELTACLASSEID))
            {
                imageToShow.CrossFadeAlpha(0.0f, 0.0f, false);
            }
            else
            {
                imageToShow.CrossFadeAlpha(5.0f, 0.0f, false);
                imageToShow.texture = tmpImage.imageToRender;
            }
        }

        Sprite LoadChapterIconOnTitle(ScriptablePage page)
        {
            switch (page.pageID)
            {
                case "allaSalute":
                    return IconsDatabase.Singleton.GetSpriteIcon("AllaSaluteIcon");
                case "scappa":
                    return IconsDatabase.Singleton.GetSpriteIcon("ScappaIcon");
                case "inVinoVeritas":
                    return IconsDatabase.Singleton.GetSpriteIcon("InVinoVeritasIcon");
                default:
                    break;
            }
            return null;
        }

        internal void TurnOffChapterCover()
        {
            panelContainer.gameObject.SetActive(false);
            imageToShow.gameObject.SetActive(false);
        }

    }
}

