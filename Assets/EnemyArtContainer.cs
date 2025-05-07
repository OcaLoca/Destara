
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
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EnemyArtContainer : MonoBehaviour
    {
        private Texture art;
        private string storyArt;
        private string nameArt;
        private TMP_Text [] txtDescription;
        private Texture [] texture;
        private Button btnBackScroll;
        private Button btnFoward;

        [SerializeField] TMP_Text [] nameArtPrefab;
        [SerializeField] TMP_Text storyPrefab;
        [SerializeField] RawImage imagePrefab;
        [SerializeField] Button btnBack;
        [SerializeField] Button btnInfo;
        [SerializeField] Button btnCloseInfo;
        public Button btnLeft;
        public Button btnRight;
        [SerializeField] GameObject infoPanel;
        public RawImage lockedImage;
        public bool isUnlocked;
        public bool isAlreadyShowed;



        public void Setup(MuseumScriptableObject museumArt)
        {
            gameObject.name = museumArt.GetItemID;
            nameArt = museumArt.GetItemName;
            storyArt = museumArt.GetItemDescription;
            //art = museumArt.GetImage.texture;
            //txtDescription = scriptableArtContent.GetStory;
            // texture = scriptableArtContent.GetImage;
            isUnlocked = museumArt.isUnlocked;
            isAlreadyShowed = museumArt.alreadyShow;
            UploadData();
        }

        public void SetupTrophy(ScriptableGoal goal)
        {
            gameObject.name = goal.GetGoalID;
            nameArt = goal.GetGoalName;
            storyArt = goal.description;
            isAlreadyShowed = goal.alreadyShowInMuseum;
            UploadData();
        }

        void UploadData() 
        {
            if (isAlreadyShowed) { lockedImage.gameObject.SetActive(false); } else { lockedImage.gameObject.SetActive(true); }
            nameArtPrefab[0].text = nameArt;
            nameArtPrefab[1].text = nameArt;
            storyPrefab.text = storyArt;
            imagePrefab.texture = art;
        }

        void Refresh()
        {



        }

        private void OnEnable()
        {
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnBackClick);
            btnInfo.onClick.RemoveAllListeners();
            btnInfo.onClick.AddListener(OnInfoClicked);
            btnLeft.onClick.RemoveAllListeners();
            btnLeft.onClick.AddListener(OnClickLeft);
            btnRight.onClick.RemoveAllListeners();
            btnRight.onClick.AddListener(OnClickRight);
            btnCloseInfo.onClick.RemoveAllListeners();
            btnCloseInfo.onClick.AddListener(delegate { infoPanel.SetActive(false);btnCloseInfo.gameObject.SetActive(false); });
        }

        void OnBackClick()
        {
                if (GameApplication.Singleton.model.previousView.Count < 1) { return; }
                if (GameApplication.Singleton.model.currentView)
                {
                 GameApplication.Singleton.model.currentView.gameObject.SetActive(false);
                }
                GameApplication.Singleton.model.currentView = GameApplication.Singleton.model.previousView.Pop();
                GameApplication.Singleton.model.currentView.gameObject.SetActive(true); 
        }

        void OnClickLeft()
        {
            GameApplication.Singleton.view.GoalsView.BackGoalScroll();
        }
        void OnClickRight()
        {
            GameApplication.Singleton.view.GoalsView.SkipGoalScroll();
        }

        void OnInfoClicked()
        {
            infoPanel.SetActive(true);
            btnCloseInfo.gameObject.SetActive(true);
        }

    }
}
