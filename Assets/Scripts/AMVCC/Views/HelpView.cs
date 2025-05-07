using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;

namespace Game
{
    public class HelpView : View<GameApplication>
    {
        [SerializeField] Button btnIcon;
        [SerializeField] Button btnGameMechanics;
        [SerializeField] Button btnRarity;
        [SerializeField] Button btnDefenceType;
        [SerializeField] Button btnCloseGameMechanicsPanel;
        [SerializeField] Button btnWebsite;
        [SerializeField] Button btnContactUs;
        [SerializeField] Button btnBack;
        [SerializeField] Button btnCloseRarityPanel;
        [SerializeField] Button btnCloseDefenceTypePanel;
        [SerializeField] GameObject GameMechanicsPanel;
        [SerializeField] GameObject IconsPanel;
        [SerializeField] GameObject RarityPanel;
        [SerializeField] GameObject DefenceTypePanel;
        [SerializeField] ScrollRect IconPanelsScrollRect;

        void OnEnable()
        {
            btnIcon.onClick.RemoveAllListeners();
            btnIcon.onClick.AddListener(OnClickIcon);

            btnGameMechanics.onClick.RemoveAllListeners();
            btnGameMechanics.onClick.AddListener(OnClickGameMechanics);

            btnCloseGameMechanicsPanel.onClick.RemoveAllListeners();
            btnCloseGameMechanicsPanel.onClick.AddListener(delegate { GameMechanicsPanel.gameObject.SetActive(false); });

            btnDefenceType.onClick.RemoveAllListeners();
            btnDefenceType.onClick.AddListener(OnClickDefenceType);

            btnCloseDefenceTypePanel.onClick.RemoveAllListeners();
            btnCloseDefenceTypePanel.onClick.AddListener(delegate { DefenceTypePanel.gameObject.SetActive(false); });

            btnRarity.onClick.RemoveAllListeners();
            btnRarity.onClick.AddListener(OnClickRarity);

            btnCloseRarityPanel.onClick.RemoveAllListeners();
            btnCloseRarityPanel.onClick.AddListener(delegate { RarityPanel.gameObject.SetActive(false); });

            btnWebsite.onClick.RemoveAllListeners();
            btnWebsite.onClick.AddListener(OnClickWebsite);

            btnContactUs.onClick.RemoveAllListeners();
            btnContactUs.onClick.AddListener(OnClickContactUs);

            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);
        }

        void OnClickIcon()
        {
            IconsPanel.SetActive(true);
            IconPanelsScrollRect.verticalNormalizedPosition = 1f;
        }

        const string AEQUILIBER_WEBSITE = "https://www.mobybit.it/portfolio/equiliber/";

        void OnClickWebsite()
        {
            Application.OpenURL(AEQUILIBER_WEBSITE);
        }

        const string AEQUILIBER_CONTACT_US = "https://www.mobybit.it/contatti/";

        void OnClickContactUs()
        {
            Application.OpenURL(AEQUILIBER_CONTACT_US);
        }
        void OnClickBack()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

        void OnClickGameMechanics()
        {
            GameMechanicsPanel.gameObject.SetActive(true);
        }
        void OnClickRarity()
        {
            RarityPanel.gameObject.SetActive(true);
        }
        void OnClickDefenceType()
        {
            DefenceTypePanel.gameObject.SetActive(true);
        }
    }
}
