/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class BookmarkManager : MonoBehaviour
    {
        public static BookmarkManager Singleton { get; set; }
        Animator anim;

        [SerializeField] SpriteRenderer iconSpriteRenderer;
        [SerializeField] Button btnOpen;
        [SerializeField] Button btnClose;
        TMP_Text txt;

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            btnClose.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            anim = gameObject.GetComponent<Animator>();
            txt = gameObject.GetComponent<TMP_Text>();
            
            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(CloseBookmark);

            btnOpen.onClick.RemoveAllListeners();
            btnOpen.onClick.AddListener(OpenBookmark);

            btnClose.gameObject.SetActive(false);
            btnOpen.gameObject.SetActive(true);
        }

        public void SetBookmarkIcon()
        {
            if (IconsDatabase.Singleton.GetImageByName() == null) { return; }
            else
            {
                iconSpriteRenderer.sprite = IconsDatabase.Singleton.GetImageByName();
            }
        }
        
        public void OpenBookmark()
        {
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.BookmarkOpened);
            btnClose.gameObject.SetActive(true);
            btnOpen.gameObject.SetActive(false);
            anim.SetTrigger("OpenBookmark");
        }

        public void CloseBookmark()
        {
            UISoundManager.Singleton.PlayAudioClip(GameApplication.Singleton.Sounds.BookmarkOpened);
            btnClose.gameObject.SetActive(false);
            btnOpen.gameObject.SetActive(true);
            anim.SetTrigger("CloseBookmark");
        }

    }

}
