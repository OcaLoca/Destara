using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using AV;

namespace Game
{
    public class GameModel : Model<GameApplication>
    {
        public Abbot AbbotData;
        public Trafficker TraffickerData;
        public Crone CroneData;
        public BountyHunter BountyHunterData;
        public ScriptableObjectsDatabase ScriptableObjectsDatabase;
        public ScriptableItemsDatabase scriptableItemsDatabase;
        public DeathDatabase deathDatabase;
        public HealthBarManager playerHealthbarManager;
        public LeftEnemyHealthbar LeftEnemyHealthbar;
        public RightEnemyHealthbar RightEnemyHealthbar;
        public CentralEnemyHealthbar CentralEnemyHealthbar;
        public BuffManager BuffManager;
        public GoalsDatabase GoalsDatabase;
        public SaveNotificationManager SaveNotificationManager;
        public StaminaBarManager SingletonStaminaBarManager;

        [SerializeField]
        internal SoundsTable sounds;

        public SettingsMenuModel Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = GetComponent<SettingsMenuModel>();
                }
                return settings;
            }
        }
        SettingsMenuModel settings;

        public NewGameModel NewGame
        {
            get
            {
                if (newGame == null)
                {
                    newGame = GetComponent<NewGameModel>();
                }
                return newGame;
            }
        }
        NewGameModel newGame;

        public BookModel BookModel
        {
            get
            {
                if (book == null)
                {
                    book = GetComponent<BookModel>();
                }
                return book;
            }
        }
        BookModel book;

        public MainMenuModel MainMenu
        {
            get
            {
                if (mainMenu == null)
                {
                    mainMenu = GetComponent<MainMenuModel>();
                }
                return mainMenu;
            }
        }
        MainMenuModel mainMenu;



        /// <summary>
        /// lo stack è come una pila 
        /// </summary>
        public Stack<View<GameApplication>> previousView;
        public View<GameApplication> currentView;
    }
}