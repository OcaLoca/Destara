using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class GoalNotificationManager : MonoBehaviour
    {
        [SerializeField] Sprite trophyImg;
        //[SerializeField] Sprite medalImg;
        [SerializeField] GameObject panelToShow;
        [SerializeField] Image imgToShow;
        [SerializeField] TMP_Text description, title;

        public static GoalNotificationManager Singleton { get; private set; }

        private void Awake()
        {
            Singleton = this;
        }

        public void Show(string title, string newDescription, ScriptableGoal.GoalType type = ScriptableGoal.GoalType.medal)
        {
            panelToShow.SetActive(true);
            this.title.text = title;
            description.text = newDescription;
            if (type == ScriptableGoal.GoalType.trophy)
            {
                imgToShow.sprite = trophyImg;
            }
            //else
            //{
            //    imgToShow.sprite = medalImg;
            //}
        }

        public void TurnOffGoal()
        {
           panelToShow.SetActive(false);
        }
    }
}
