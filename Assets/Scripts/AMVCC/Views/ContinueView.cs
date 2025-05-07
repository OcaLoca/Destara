using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class ContinueView : View<GameApplication>
    {
        public Button btnBack, btnContinue;
        [SerializeField] TMP_Text emptyStringBounty;
        [SerializeField] TMP_Text emptyStringCrone;
        [SerializeField] TMP_Text emptyStringAbbot;
        [SerializeField] TMP_Text emptyStringSmuggler;
        public TMP_Text[] abbotSaveInfo = new TMP_Text[3];
        public Image[] abbotPopUP = new Image[2];
        public TMP_Text[] bountySaveInfo = new TMP_Text[3];
        public Image[] bountyPopUP = new Image[2];
        public TMP_Text[] croneSaveInfo = new TMP_Text[3];
        public Image[] cronePopUP = new Image[2];
        public TMP_Text[] traffickerSaveInfo = new TMP_Text[3];
        public Image[] smugglerPopUP = new Image[2];

        public Button[] btnLoadGame = new Button[4];

        public static bool StartGameFromContinue = false;

        void OnEnable()
        {
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);
            btnContinue.onClick.RemoveAllListeners();
            btnContinue.onClick.AddListener(OnClickContinue);
            btnLoadGame[0].onClick.RemoveAllListeners();
            
            btnLoadGame[1].onClick.RemoveAllListeners();
            btnLoadGame[2].onClick.RemoveAllListeners();
            btnLoadGame[3].onClick.RemoveAllListeners();
        }

        void OnClickBack()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericBackButton);
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

        public void SetSavedInfo(string txtPlayerName, string txtPlayerLevel, string txtChapter, string classToCheck)
        {
            if (classToCheck == "Abbot")
            {
                abbotSaveInfo[0].text = txtPlayerName;
                abbotSaveInfo[1].text = txtPlayerLevel;
                abbotSaveInfo[2].text = txtChapter;
                emptyStringAbbot.gameObject.SetActive(false);
                foreach (TMP_Text tMP_Text in abbotSaveInfo) { tMP_Text.gameObject.SetActive(true); }
                foreach (Image img in abbotPopUP) { img.gameObject.SetActive(true); }

            }
            else if (classToCheck == "BountyHunter")
            {
                bountySaveInfo[0].text = txtPlayerName;
                bountySaveInfo[1].text = txtPlayerLevel;
                bountySaveInfo[2].text = txtChapter;
                emptyStringBounty.gameObject.SetActive(false);
                foreach (TMP_Text tMP_Text in bountySaveInfo) { tMP_Text.gameObject.SetActive(true); }
                foreach (Image img in bountyPopUP) { img.gameObject.SetActive(true); }
            }
            else if (classToCheck == "Crone")
            {
                croneSaveInfo[0].text = txtPlayerName;
                croneSaveInfo[1].text = txtPlayerLevel;
                croneSaveInfo[2].text = txtChapter;
                emptyStringCrone.gameObject.SetActive(false);

                foreach (TMP_Text tMP_Text in croneSaveInfo) { tMP_Text.gameObject.SetActive(true); }
                foreach (Image img in cronePopUP) { img.gameObject.SetActive(true); }

            }
            else if (classToCheck == "Trafficker")
            {
                emptyStringSmuggler.gameObject.SetActive(false);
                traffickerSaveInfo[0].text = txtPlayerName;
                foreach (TMP_Text tMP_Text in abbotSaveInfo) { tMP_Text.gameObject.SetActive(true); }
                foreach (Image img in smugglerPopUP) { img.gameObject.SetActive(true); }
            }
        }

        void OnClickContinue()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.ConfirmDifficultButton);
            string currentPageID = string.Empty;
            PlayerManager.Singleton.LoadLastRunSave(SaveType.Soft, out currentPageID);
            
            StartGameFromContinue = true;
            SettingsMenuView.UPDATEGEMS = true;

            app.model.BookModel.soundManager.LoadMusicAudioClip(PagesMaleDatabase.Singleton.GetPageByID(currentPageID));
            Notify(MVCEvents.OPEN_GAME_VIEW_CONTINUE);
            Notify(MVCEvents.LOAD_PAGE, currentPageID);
        }
    }
}