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
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class GameView : View<GameApplication>
    {
        public ContinueView ContinueView
        {
            get
            {
                continueMenu = Find(continueMenu);
                return continueMenu;
            }
        }
        ContinueView continueMenu;

        public MainMenuView MainMenu
        {
            get
            {
                mainMenu = Find(mainMenu);
                return mainMenu;
            }
        }
        MainMenuView mainMenu;
        

        public CuriosityView CuriosityView
        {
            get
            {
                curiosityView = Find(curiosityView);
                return curiosityView;
            }

        }
        CuriosityView curiosityView;
        public SettingsMenuView Settings
        {
            get
            {
                settings = Find(settings);
                return settings;
            }
        }
        SettingsMenuView settings;

        public NewGameView NewGame
        {
            get
            {
                newGame = Find(newGame);
                return newGame;
            }
        }
        NewGameView newGame;

        public BookView BookView
        {
            get
            {
                bookView = Find(bookView);
                return bookView;
            }
        }
        BookView bookView;

        public CharacterCreationView CharacterCreationView
        {
            get
            {
                characterCreationView = Find(characterCreationView);
                return characterCreationView;
            }
        }
        CharacterCreationView characterCreationView;

        //---------------- View Secondarie---------------------

        public TeamView TeamView
        {
            get
            {
                teamView = Find(teamView);
                return teamView;
            }
        }
        TeamView teamView;

        public GoalsView GoalsView
        {
            get
            {
                goalsView = Find(goalsView);
                return goalsView;
            }
        }
        GoalsView goalsView;

        public NewInventoryView InventoryView
        {
            get
            {
                inventoryView = Find(inventoryView);
                return inventoryView;
            }
        }
        NewInventoryView inventoryView;

        public StatsView StatsView
        {
            get
            {
                statsView = Find(statsView);
                return statsView;
            }
        }
        StatsView statsView;

        public StatsChoicePanel StatsChoicePanel
        {
            get
            {
                statsChoicePanel = Find(statsChoicePanel);
                return statsChoicePanel;
            }
        }
        StatsChoicePanel statsChoicePanel;

        DemoView demoView;
        public DemoView DemoView
        {
            get
            {
                demoView = Find(demoView);
                return demoView;
            }
        }

        public DropView DropView
        {
            get
            {
                dropView = Find(dropView);
                return dropView;
            }
        }
        DropView dropView;

        public EquipView EquipView
        {
            get
            {
                equipView = Find(equipView);
                return equipView;
            }
        }
        EquipView equipView;

        public EquipmentView EquipmentView
        {
            get
            {
                equipmentView = Find(equipmentView);
                return equipmentView;
            }
        }
        EquipmentView equipmentView;

        public LibraryView LibraryView
        {
            get
            {
                libraryView = Find(libraryView);
                return libraryView;
            }
        }
        LibraryView libraryView;


        public TutorialView TutorialView
        {
            get
            {
                tutorialView = Find(tutorialView);
                return tutorialView;
            }
        }
        TutorialView tutorialView;

        public LanguageView LanguageView
        {
            get
            {
                languageView = Find(languageView);
                return languageView;
            }
        }
        LanguageView languageView;

        public FirstView FirstView
        {
            get
            {
                firstView = Find(firstView);
                return firstView;
            }
        }
        FirstView firstView;

        public BattleView BattleView
        {
            get
            {
                battleView = Find(battleView);
                return battleView;
            }
        }
        BattleView battleView ;

        public EscapeRoomView EscapeRoomView
        {
            get
            {
                escapeRoomView = Find(escapeRoomView);
                return escapeRoomView;
            }
        }
        EscapeRoomView escapeRoomView;

        public DeathView DeathView
        {
            get
            {
                deathView = Find(deathView);
                return deathView;
            }
        }
        DeathView deathView;

        public WeAreView WeAreView
        {
            get
            {
                weAreView = Find(weAreView);
                return weAreView;
            }
        }
        WeAreView weAreView;

        public MobyBitView MobyBitView
        {
            get
            {
                mobyBitView = Find(mobyBitView);
                return mobyBitView;
            }
        }
        MobyBitView mobyBitView;

        public GameSettingsView GameSettingsView
        {
            get
            {
                gameSettingsView = Find(gameSettingsView);
                return gameSettingsView;
            }
        }
        GameSettingsView gameSettingsView;

        public HelpView HelpView
        {
            get
            {
                helpView = Find(helpView);
                return helpView;
            }
        }
        HelpView helpView;

        //-------View per richiamare immagini e contorni

        public LoadImage LoadImage
        {
            get
            {
                loadImage = Find(loadImage);
                return loadImage;
            }
        }
        LoadImage loadImage;


        public TrophyView TrophyView
        {
            get
            {
                trophyView = Find(trophyView);
                return trophyView;
            }
        }
        TrophyView trophyView;

        public UITransitionsManager UITransitionsManager
        {
            get
            {
                uITransitions = Find(uITransitions);
                return uITransitions;
            }
        }
        UITransitionsManager uITransitions;
    }
}
