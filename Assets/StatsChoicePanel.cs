using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SmartMVC;

namespace Game
{
    public class StatsChoicePanel : View<GameApplication>
    {
        public Button btnScrollBack;
        public Button btnScrollForward;

        void OnEnable()
        {
            // SetUI(gameObject.name);

            btnScrollBack.onClick.RemoveAllListeners();
            btnScrollBack.onClick.AddListener(delegate { TurnOnBackPanel(gameObject.name); });

            btnScrollForward.onClick.RemoveAllListeners();
            btnScrollForward.onClick.AddListener(delegate { TurnOnFowardPanel(gameObject.name); });
        }


        public void TurnOnBackPanel(string currentClass)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            switch (currentClass)
            {
                case "BountyHunter":
                    TurnOnSelectedPanel("Abbot");
                    app.view.CharacterCreationView.bountyScrollRect.enabled = false;
                    app.view.CharacterCreationView.abbotScrollRect.enabled = true;
                    SwipableItem.Singleton.ChangeCurrentClassSelectedNumber(0);
                    GameApplication.Singleton.app.Notify(MVCEvents.CHANGE_SELECTED_CLASS, 0);
                    break;
                case "Crone":
                    TurnOnSelectedPanel("BountyHunter");
                    SwipableItem.Singleton.ChangeCurrentClassSelectedNumber(1);

                    GameApplication.Singleton.app.Notify(MVCEvents.CHANGE_SELECTED_CLASS, 1);
                    app.view.CharacterCreationView.bountyScrollRect.enabled = true;
                    app.view.CharacterCreationView.croneScrollRect.enabled = true;
                    break;
                case "Trafficker":
                    TurnOnSelectedPanel("Crone");
                    SwipableItem.Singleton.ChangeCurrentClassSelectedNumber(2);
                    GameApplication.Singleton.app.Notify(MVCEvents.CHANGE_SELECTED_CLASS, 2);
                    app.view.CharacterCreationView.croneScrollRect.enabled = true;
                    app.view.CharacterCreationView.traffickerScrollRect.enabled = false;
                    break;
                default:
                    break;
            }
        }

        public void TurnOnFowardPanel(string currentClass)
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);
            switch (currentClass)
            {
                case "Abbot":
                    GameApplication.Singleton.app.Notify(MVCEvents.CHANGE_SELECTED_CLASS, 1);
                    app.view.CharacterCreationView.bountyScrollRect.enabled = true;
                    app.view.CharacterCreationView.abbotScrollRect.enabled = false;
                    SwipableItem.Singleton.ChangeCurrentClassSelectedNumber(1);
                    TurnOnSelectedPanel("BountyHunter");
                    break;
                case "BountyHunter":
                    GameApplication.Singleton.app.Notify(MVCEvents.CHANGE_SELECTED_CLASS, 2);
                    app.view.CharacterCreationView.bountyScrollRect.enabled = false;
                    app.view.CharacterCreationView.croneScrollRect.enabled = true;
                    SwipableItem.Singleton.ChangeCurrentClassSelectedNumber(2);
                    TurnOnSelectedPanel("Crone");
                    break;
                case "Crone":
                    GameApplication.Singleton.app.Notify(MVCEvents.CHANGE_SELECTED_CLASS, 3);
                    app.view.CharacterCreationView.croneScrollRect.enabled = false;
                    app.view.CharacterCreationView.traffickerScrollRect.enabled = true;
                    SwipableItem.Singleton.ChangeCurrentClassSelectedNumber(3);
                    TurnOnSelectedPanel("Trafficker");
                    break;
                default:
                    break;
            }
        }

        internal void TurnOnSelectedPanel(string panelToTurnOn)
        {
            CharacterCreationView view = app.view.CharacterCreationView;

            foreach (GameObject panel in view.GetClassContainer)
            {
                panel.gameObject.SetActive(false);

                if (panel.gameObject.name == panelToTurnOn)
                {
                    panel.gameObject.SetActive(true);
                }
            }
        }
    }
}
