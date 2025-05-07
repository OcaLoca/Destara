/* ---------------------------------------------- 
 * Copyright ? Mobybit
 * ----------------------------------------------
 */

using Game;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystemManager : MonoBehaviour
{
    public GameObject[] TutorialPanelsDatabase;
    public GameObject[] TutorialPanelsDatabaseSecondPart;
    public GameObject TutorialPanelsDatabaseThirdPart, SuperstitionButtonTutorial, CourageButtonTutorial, LuckyButtonTutorial, BeforeFightTutorial, preparationGhost, preparationViandante;
    public Button btn;
    int panelIndexNumber = 0;
    int firstDataLenght;
    int panelIndexNumberSecondTutorial = 0;
    int secondDataLenght;

    private void OnEnable()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnButtonClicked);
    }

    internal void ShowFirstPartOfTutorial()
    {
        TurnOffAllPanels();
        firstDataLenght = TutorialPanelsDatabase.Length - 1;
        TutorialPanelsDatabase[0].SetActive(true);
        gameObject.SetActive(true);
    }

    internal void ShowTorchTutorial()
    {
        TurnOffAllPanels();
        TutorialPanelsDatabaseThirdPart.SetActive(true);
        gameObject.SetActive(true);
    }

    internal void ShowSuperstitionButtonTutorial()
    {
        TurnOffAllPanels();
        SuperstitionButtonTutorial.SetActive(true);
        gameObject.SetActive(true);
    }

    internal void ShowLuckyButtonTutorial()
    {
        TurnOffAllPanels();
        LuckyButtonTutorial.SetActive(true);
        gameObject.SetActive(true);
    }

    internal void ShowBeforeFightTutorial()
    {
        TurnOffAllPanels();
        preparationTutorialShowed = false;
        BeforeFightTutorial.SetActive(true);
        gameObject.SetActive(true);
    }
    internal void ShowCourageButtonTutorial()
    {
        TurnOffAllPanels();
        CourageButtonTutorial.SetActive(true);
        gameObject.SetActive(true);
    }

    public static bool FirstTutorialPartNotShowed = true;
    public static bool SecondTutorialPartNotShowed = true;
    public static bool InventoryTutorialPartNotShowed = true;
    public static bool AnalyzeTutorialPartNotShowed = true;
    public static bool TorchTutorialPartNotShowed = true;
    public static bool BeforeFightTutorialPartNotShowed = true;
    public static bool FightTutorialPartNotShowed = true;
    public static bool preparationTutorialShowed = false;
    public static bool bringPlayerToChangeDefense = false;
    internal void ShowPreparationTutorial()
    {
        bringPlayerToChangeDefense = true;
        preparationTutorialShowed = true;
        TurnOffAllPanels();

        if (PlayerManager.Singleton.currentPage.pageID == PageIDDatabase.BEFOREFIRSTFIGHT)
        {
            preparationViandante.SetActive(true);
        }
        if (PlayerManager.Singleton.currentPage.pageID == PageIDDatabase.BEFOREFIRSTFIGHTHIGH)
        {
            preparationGhost.SetActive(true);
        }
        gameObject.SetActive(true);
    }


    internal void SetTutorialFinish()
    {
        PlayerPrefs.SetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL, 0);
    }

    internal void ShowSecondPartOfTutorial()
    {
        TurnOffAllPanels();
        secondDataLenght = TutorialPanelsDatabaseSecondPart.Length - 1;
        TutorialPanelsDatabaseSecondPart[0].SetActive(true);
        gameObject.SetActive(true);
    }

    public void RefreshDataForTheTutorial()
    {
        TutorialPanelsDatabase[0].SetActive(true);
        StatsView.notShowed = true; //Serve per settare che il tutorial nelle stats non è ancora stato visto
        BattleController.AlreadyShowTutorialOneTime = false;
        SecondTutorialPartNotShowed = true;
        FirstTutorialPartNotShowed = true;
        InventoryTutorialPartNotShowed = true;
        AnalyzeTutorialPartNotShowed = true;
        TorchTutorialPartNotShowed = true;
        BeforeFightTutorialPartNotShowed = true;
        FightTutorialPartNotShowed = true;
    }

    public void OnButtonClicked()
    {
        TurnOffAllPanels();

        if (PlayerManager.Singleton.selectedClass != null)
        {
            string currentPageID = PlayerManager.Singleton.currentPage.pageID;
            switch (currentPageID)
            {
                case PageIDDatabase.TUTORIALFIRSTPAGE:
                    OpenNextSecondTutorialPanel();
                    break;
                case PageIDDatabase.TUTORIALLUCKYID:
                    LuckyButtonTutorial.SetActive(false);
                    gameObject.SetActive(false);
                    break;
                case PageIDDatabase.TUTORIALCOURAGEID:
                    CourageButtonTutorial.SetActive(false);
                    gameObject.SetActive(false);
                    break;
                case PageIDDatabase.ANALIZZAMENESTRELLOID:
                    SuperstitionButtonTutorial.SetActive(false);
                    gameObject.SetActive(false);
                    break;
                case PageIDDatabase.BEFOREFIRSTFIGHT:
                case PageIDDatabase.BEFOREFIRSTFIGHTHIGH:
                    BeforeFightTutorial.SetActive(false);
                    gameObject.SetActive(false);
                    if (preparationTutorialShowed)
                    {
                        InventoryTutorialPartNotShowed = false;
                        PreparationTutorialOpenCharacterMenu();
                        bringPlayerToChangeDefense = true;
                        preparationGhost.SetActive(false);
                        preparationViandante.SetActive(false);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        ShowPreparationTutorial();
                    }
                    break;
                case PageIDDatabase.TORCHPAGEID:
                    TutorialPanelsDatabaseThirdPart.SetActive(false);
                    gameObject.SetActive(false);
                    break;
            }
        }
        else
        {
            OpenNextFirstTutorialPanel();
        }
    }

    void PreparationTutorialOpenCharacterMenu()
    {
        GameApplication.Singleton.view.StatsView.OnClickEquipButton(true);
        GameApplication.Singleton.view.EquipmentView.ShowPreparationBeforeFight();
    }

    void TurnOffAllPanels()
    {
        TurnOffFirstTutorialPanels();
        foreach (var panel in TutorialPanelsDatabaseSecondPart)
        {
            panel.gameObject.SetActive(false);
        }
    }

    void TurnOffFirstTutorialPanels()
    {
        foreach (var panel in TutorialPanelsDatabase)
        {
            panel.gameObject.SetActive(false);
        }
    }

    void OpenNextFirstTutorialPanel()
    {
        panelIndexNumber++;
        if (panelIndexNumber > firstDataLenght)
        {
            panelIndexNumber = 0;
            gameObject.SetActive(false);
            AttentionPanel.Singleton.ActiveAttentionPanel("tutorialPreferencePanel");
            return;
        }
        TutorialPanelsDatabase[panelIndexNumber].SetActive(true);
    }

    void OpenNextSecondTutorialPanel()
    {
        panelIndexNumberSecondTutorial++;
        if (panelIndexNumberSecondTutorial > secondDataLenght)
        {
            panelIndexNumberSecondTutorial = 0;
            GameApplication.Singleton.view.BookView.OpenCharacterMenu(true);
            gameObject.SetActive(false);
            return;
        }
        TutorialPanelsDatabaseSecondPart[panelIndexNumberSecondTutorial].SetActive(true);
    }
}
