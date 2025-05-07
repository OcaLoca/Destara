using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;
using StarworkGC.Localization;

namespace Game
{
    public class ClassInfoView : View<GameApplication>
    {
        public Button btnBack;
        public Button btnStatsClass;
        public Button btnStarterEquipmentClass;
        public Button btnStory;
        public GameObject pnlStats;
        public GameObject pnlEquipment;
        public GameObject pnlStory;
        public GameObject orizontalLayout;
        public TMP_Text storyTxt;
        public TMP_Text costitution;
        public TMP_Text strenght;
        public TMP_Text dexterity;
        public TMP_Text inteligence;
        public TMP_Text courage;
        public TMP_Text lucky;
        public TMP_Text superstition;
        public TMP_Text bonus;
        public TMP_Text ability;

        void OnEnable()
        {
            app.model.currentView = this;
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnBackClick);
            btnStatsClass.onClick.RemoveAllListeners();
            btnStatsClass.onClick.AddListener(OnSelectStatsClass);
            btnStory.onClick.RemoveAllListeners();
            btnStory.onClick.AddListener(OnSelectStoryClass);
            btnStarterEquipmentClass.onClick.RemoveAllListeners();
            btnStarterEquipmentClass.onClick.AddListener(OnSelectEquipmentClass);
        }

        void OnBackClick()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

        void OnSelectStatsClass()
        {
            pnlEquipment.SetActive(false);
            pnlStory.SetActive(false);
            pnlStats.SetActive(true);
            btnStatsClass.interactable = false;
            btnStory.interactable = true;
            btnStarterEquipmentClass.interactable = true;
        }
        void OnSelectEquipmentClass()
        {
            pnlEquipment.SetActive(true);
            pnlStory.SetActive(false);
            pnlStats.SetActive(false);
            btnStatsClass.interactable = true;
            btnStory.interactable = true;
            btnStarterEquipmentClass.interactable = false;
        }
        void OnSelectStoryClass()
        {
            pnlEquipment.SetActive(false);
            pnlStory.SetActive(true);
            pnlStats.SetActive(false);
            btnStatsClass.interactable = true;
            btnStory.interactable = false;
            btnStarterEquipmentClass.interactable = true;
        }
    }
}
