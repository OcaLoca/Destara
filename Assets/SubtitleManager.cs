using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class SubtitleManager : MonoBehaviour
    {
        public TMP_Text subtitle;
        public TMP_Text chapterNumber;

        public void LoadSubtitle(ScriptablePage page)
        {
            if (page.chapterSection == ScriptablePage.Section.Titolo ||
                page.chapterSection == ScriptablePage.Section.Fight)
            {
                subtitle.gameObject.SetActive(false);
            }
            else
            {
                subtitle.gameObject.SetActive(true);

                if (page.chapterSection == ScriptablePage.Section.Death || page.pageBeforeDeath)
                {
                    chapterNumber.gameObject.SetActive(false);
                    subtitle.text = Localization.Get(PageIDDatabase.DEATHPAGESUBTITLEID);
                    return;
                }

                subtitle.text = Localization.Get(page.subtitleID);
                chapterNumber.gameObject.SetActive(true);
                chapterNumber.text = page.chapterNumber;
            }
        }
    }
}

