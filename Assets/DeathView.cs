using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class DeathView : View<GameApplication>
    {
        public Button returnMainMenu;
        public Button btnRetryGame;
        // public RawImage deathImage;
        // public TMP_Text deathDescription;

        DeathDatabase Model { get { return app.model.deathDatabase; } }

        // Start is called before the first frame update
        void OnEnable()
        {
            ResetStatsOnDeath();
            SoundManager.Singleton.Play(app.Sounds.MetagameSoundtrack);

            returnMainMenu.onClick.RemoveAllListeners();
            returnMainMenu.onClick.AddListener(OnClickReturn);

            btnRetryGame.onClick.RemoveAllListeners();
            btnRetryGame.onClick.AddListener(OnClickRetry);
        }

        void LoadDeathImage()
        {
            foreach (ScriptableDeath tmpDeath in Model.deathID)
            {
                if (PlayerManager.Singleton.currentPage.pageID == tmpDeath.deathID)
                {
                    //deathDescription.text = tmpDeath.txtDeathDescription;
                    //deathImage.texture = tmpDeath.deathImage;
                }

                else { Debug.Log("Non c'è una morte per la pagina corrente"); }
            }
        }

        void OnClickReturn()
        {
            Notify(MVCEvents.OPEN_MAINMENU_VIEW);
        }

        void OnClickRetry()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.ConfirmDifficultButton);
            PlayerManager.Singleton.selectedClass = null;
            PlayerManager.Singleton.selectedDifficulty = PlayerManager.Singleton.lastDifficultyChoice;
            PlayerManager.Singleton.RefreshPlayerManager();
            Notify(MVCEvents.OPEN_GAME_VIEW);
        }
        public void ResetStatsOnDeath()
        {
            SaveSystemUtilities.EraseSaveFile(SaveType.Soft);
            PlayerPrefs.SetInt(DemoView.RUN_FINISHED, 1);
            PlayerManager.Singleton.ResetOnDeath();
        }
    }
}

