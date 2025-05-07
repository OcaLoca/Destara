using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;
using static Game.BattleController;
using StarworkGC.Localization;

namespace Game
{
    public class BattleView : View<GameApplication>
    {
        public Button byTouch;
        string ID;
        [SerializeField] RuntimeAnimatorController[] runtimeAnimatorControllers;
        [SerializeField] GameObject Player;
        [SerializeField] TMP_Text txtPlayerName;
        [SerializeField] TMP_Text txtPlayerLevel;
        [SerializeField] internal BattlePlayerManager battlePlayerManager;
        public BattleEnemyManager battleEnemyManager;
        public BattleController battleController;
        public GameObject pnlButton;

        [Header("UI")]
        [SerializeField] GameObject ItemContainer;
        [SerializeField] GameObject TargetContainer;
        [SerializeField] GameObject PanelFightTransition;
        [SerializeField] GameObject VictoryLosePanel;
        [SerializeField] GameObject VictoryPanel, LosePanel;
        [SerializeField] Button VictoryPanelBtn, LosePanelBtn;
        public GameObject PanelFightTutorial;
        public GameObject PanelYouAreDead;
        public GameObject FirstPanelFightTutorial;

        [SerializeField] Button skipText;
        public GameObject lightUIAnim;
        public GameObject heavyUIAnim;
        public GameObject rangedUIAnim;
        public GameObject specialUIAnim;
        public TMP_Text currenrtWeaponTxt;
        public int animationIndex;

        public GameObject GetItemContainer { get => ItemContainer; }
        public GameObject GetTargetContainer { get => TargetContainer; }

        RuntimeAnimatorController[] GetClassAnimation { get => runtimeAnimatorControllers; }

        BookModel Model { get { return app.model.BookModel; } }

        private void Start()
        {
            txtPlayerName.text = PlayerManager.Singleton.playerName;
        }

        void OnEnable()
        {
            PanelFightTransition.SetActive(true);
            txtPlayerLevel.text = UIUtility.GetLevelForUI(PlayerManager.Singleton.GetPlayerLevel);
            app.view.UITransitionsManager.PlayFightAnimationTransition();
            Model.loadBattleground.SetupGround();
            ID = LoadIDFromFight();

            byTouch.onClick.RemoveAllListeners();
            byTouch.onClick.AddListener(delegate { 
                CloseBattleView();
            }); //due volte o nn carica al primo colpo

            skipText.onClick.RemoveAllListeners();
            skipText.onClick.AddListener(delegate {
                CleanConsole();
            });

            VictoryPanelBtn.onClick.RemoveAllListeners();
            LosePanelBtn.onClick.RemoveAllListeners();
            LosePanelBtn.onClick.AddListener(CloseBattleView);
            VictoryPanelBtn.onClick.AddListener(CloseBattleView);
        }

        void CleanConsole()
        {
            battleController.CleanConsoleText();
            playerSkipText = true;
        }

        string LoadIDFromFight()
        {
            bool isDead = PlayerManager.Singleton.CheckPlayerIsDead;

            if (isDead)
            {
                return PlayerManager.Singleton.currentPage.winOrLoseFightID.loseFightID;
            }
            else
            {
                return PlayerManager.Singleton.currentPage.winOrLoseFightID.winFightID;
            }
        }

        public void LoadIDOnBattleEnd()
        {
            ID = LoadIDFromFight();
        }

        float xpGained;
        public void LoadVictoryLosePanel()
        {
            VictoryLosePanel.gameObject.SetActive(true);

            if (PlayerManager.Singleton.PlayerIsDead())
            {
                VictoryPanel.SetActive(false);
                LosePanel.SetActive(true);
                xpGained = 0;
                VictoryPanelManager.instance.PlayVictoryLosePanelAudio(GameApplication.Singleton.Sounds.LoseSound);
            }
            else
            {
                ScriptablePage pg = PlayerManager.Singleton.currentPage;
                LosePanel.SetActive(false);
                xpGained = pg.battleXPGain;
                VictoryPanel.SetActive(true);
                VictoryPanelManager.instance.DropBattleObjAndGiveXp(pg.battleXPGain, pg.battleDropReward);
                VictoryPanelManager.instance.PlayVictoryLosePanelAudio(GameApplication.Singleton.Sounds.VictorySound);
            }
        }

        public bool VictoryLosePanelIsClosed()
        {
            return !VictoryLosePanel.gameObject.activeSelf;
        }

        public void CloseBattleView()
        {
            StopAllCoroutines();
            UIUtility.ResetCameraAndCanvas();
            VictoryLosePanel.SetActive(false);
            PlayerManager.Singleton.UpdateExperience(xpGained);
            DestroyEnemyObjectOnCloseLosePanel();
            SoundManager.Singleton.LoadMusicAudioClip(PagesMaleDatabase.Singleton.GetPageByID(ID));
            SaveSystem.SavePlayerLastRun(SaveType.Soft, ID); //Salva dopo il fight
            Notify(MVCEvents.OPEN_GAME_VIEW_AT, ID);
            Notify(MVCEvents.LOAD_PAGE, ID);
        }

        public void DestroyEnemyObjectOnCloseLosePanel()
        {
            battleEnemyManager.DestroyAllEnemiesObject();
        }


        public void ShowVictoryPanel()
        {
            Debug.Log("YOU WIN");
            VictoryLosePanel.SetActive(true);
            VictoryPanel.SetActive(true);
        }
        public void ShowLosePanel()
        {
            Debug.Log("YOU LOSE");
            VictoryLosePanel.SetActive(true);
            LosePanel.SetActive(true);
        }

        public void SetName(Unit unit)
        {
            txtPlayerName.text = unit.name;
        }

        public IEnumerator PanelDeathAnimation()
        {
            PanelYouAreDead.SetActive(true);
            yield return new WaitForSeconds(3);
        }

        public void ShowHiddenEnemyShield(Sprite enemyShield)
        {
            battleEnemyManager.enemyShieldSprite.sprite = enemyShield;
            battleEnemyManager.enemyShieldSprite.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_DEFENCE_COLOR);
            battleEnemyManager.enemyShieldSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
        }
    }
}
