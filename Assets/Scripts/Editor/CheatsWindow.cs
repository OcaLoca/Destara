using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Game
{
    public class CheatsWindow : EditorWindow
    {

        VisualElement uiContainer;
        public BookView bookView;

        [MenuItem("Custom/Cheats")]
        public static void ShowWindow()
        {
            CheatsWindow window = GetWindow<CheatsWindow>("Cheats");
            window.Show();
        }

        private void OnEnable()
        {
            SetupBackend();
            SetupFrontend();
        }

        void SetupBackend()
        {

        }

        #region FrontendUIController
        void SetupFrontend()
        {
            if (uiContainer == null)
            {
                uiContainer = rootVisualElement;
            }
            rootVisualElement.Clear();

            VisualTreeAsset windowStructure = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/CheatsWindow/CheatsWindow.uxml");
            windowStructure.CloneTree(uiContainer);

            TextField textFieldPageID = uiContainer.Q<TextField>("txtFieldPageID");
            textFieldPageID.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                OnClickChangeCurrentPage(evt.newValue);
            });

            TextField textFieldItemID = uiContainer.Q<TextField>("txtFieldItemID");
            textFieldItemID.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                OnClickAdditem(evt.newValue);
            });

            //----------slider

            var superstitionSlider = uiContainer.Q<Slider>("sldChangeSuperstition");
            superstitionSlider.value = 50;
            superstitionSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnSuperstitionSliderValueChanged(evt.newValue);
            });

            var courageSlider = uiContainer.Q<Slider>("sldChangeCourage");
            courageSlider.value = 50;
            courageSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnCourageSliderValueChanged(evt.newValue);
            });

            var luckySlider = uiContainer.Q<Slider>("sldChangeLucky");
            luckySlider.value = 50;
            luckySlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnLuckySliderValueChanged(evt.newValue);
            });

            var strenghtSlider = uiContainer.Q<Slider>("sldChangeStrenght");
            strenghtSlider.value = 50;
            strenghtSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnStrenghtSliderValueChanged(evt.newValue);
            });

            var costitutionSlider = uiContainer.Q<Slider>("sldChangeConstitution");
            costitutionSlider.value = 50;
            costitutionSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnConstitutionSliderValueChanged(evt.newValue);
            });

            var dexteritySlider = uiContainer.Q<Slider>("sldChangeDexterity");
            dexteritySlider.value = 50;
            dexteritySlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnDexteritySliderValueChanged(evt.newValue);
            });

            var inteligenceSlider = uiContainer.Q<Slider>("sldChangeInteligence");
            inteligenceSlider.value = 50;
            inteligenceSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnInteligenceSliderValueChanged(evt.newValue);
            });

            var defenceSlider = uiContainer.Q<Slider>("sldChangeDefence");
            defenceSlider.value = 50;
            defenceSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnDefenceSliderValueChanged(evt.newValue);
            });

            var expSlider = uiContainer.Q<Slider>("sldChangeExp");
            expSlider.value = 50;
            expSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnExpSliderValueChanged(evt.newValue);
            });

            var lifeSlider = uiContainer.Q<Slider>("sldChangeLifePoints");
            lifeSlider.value = 50;
            //lifeSlider.highValue = (int)(PlayerManager.Singleton.constitution * 10);
            lifeSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                OnLifeSliderValueChanged(evt.newValue);
            });

            Button btnFirstFight = uiContainer.Q<Button>("btnArrivaStraniero");
            btnFirstFight.SetEnabled(true);
            btnFirstFight.clickable = new Clickable(OnClickFirstFight);
            Button btnTutorialFight = uiContainer.Q<Button>("btnFirstFight");
            btnTutorialFight.SetEnabled(true);
            btnTutorialFight.clickable = new Clickable(OnClickTutorialFight);
            Button btnStartTutorial = uiContainer.Q<Button>("btnInVinoVeritas");
            btnStartTutorial.SetEnabled(true);
            btnStartTutorial.clickable = new Clickable(OnClickStartTutorial);
            Button btnSecondFight = uiContainer.Q<Button>("btnSecondoFight");
            btnSecondFight.SetEnabled(true);
            btnSecondFight.clickable = new Clickable(OnClickSecondFight);
            Button btnSceltaClasse = uiContainer.Q<Button>("btnSceltaClasse");
            btnSceltaClasse.SetEnabled(true);
            btnSceltaClasse.clickable = new Clickable(OnClickClassSelection);
            Button btnUpFloor = uiContainer.Q<Button>("btnCapitoloScappa");
            btnUpFloor.SetEnabled(true);
            btnUpFloor.clickable = new Clickable(OnClickUpperSide);
            Button btnFall = uiContainer.Q<Button>("btnCadi");
            btnFall.SetEnabled(true);
            btnFall.clickable = new Clickable(OnClickFall);
            Button btnMouse = uiContainer.Q<Button>("btnMouse");
            btnMouse.SetEnabled(true);
            btnMouse.clickable = new Clickable(FightUbriaco);
            Button btnAbbot = uiContainer.Q<Button>("btnAbbot");
            btnAbbot.SetEnabled(true);
            btnAbbot.clickable = new Clickable(TakeAbbotClass);
            Button btnBounty = uiContainer.Q<Button>("btnBounty");
            btnBounty.SetEnabled(true);
            btnBounty.clickable = new Clickable(TakeBountyClass);
            Button btnCrone = uiContainer.Q<Button>("btnCrone");
            btnCrone.SetEnabled(true);
            btnCrone.clickable = new Clickable(TakeCroneClass);
            Button btnTrafficker = uiContainer.Q<Button>("btnTrafficker");
            btnTrafficker.SetEnabled(true);
            btnTrafficker.clickable = new Clickable(TakeTraffickerClass);
            Button btnDuraLex = uiContainer.Q<Button>("btnDuraLex");
            btnDuraLex.SetEnabled(true);
            btnDuraLex.clickable = new Clickable(OnClickDuraLex);
            Button btnPlay = uiContainer.Q<Button>("btnPlay");
            btnPlay.SetEnabled(true);
            btnPlay.clickable = new Clickable(OnClickPlay);
            Button btnTalkWithGuard = uiContainer.Q<Button>("btnConvinciGuardiaCelle");
            btnTalkWithGuard.SetEnabled(true);
            btnTalkWithGuard.clickable = new Clickable(OnClickTalkWithGuard);
            Button btnMemorie = uiContainer.Q<Button>("btnMemorie");
            btnMemorie.SetEnabled(true);
            btnMemorie.clickable = new Clickable(VsPantegane);
            Button btnOscurita = uiContainer.Q<Button>("btnOscurita");
            btnOscurita.SetEnabled(true);
            btnOscurita.clickable = new Clickable(OnClickOscurita);
            Button btnPlayCards = uiContainer.Q<Button>("btnPlayCards");
            btnPlayCards.SetEnabled(true);
            btnPlayCards.clickable = new Clickable(OnClickPlayCards);
            Button btnDarkRiddle = uiContainer.Q<Button>("btnDarkRiddle");
            btnDarkRiddle.SetEnabled(true);
            btnDarkRiddle.clickable = new Clickable(OnClickDarkRiddle);
            Button btnFightGufo = uiContainer.Q<Button>("btnFightGufo");
            btnFightGufo.SetEnabled(true);
            btnFightGufo.clickable = new Clickable(OnClickFightGufo);
            Button btnFightCane = uiContainer.Q<Button>("btnFightCane");
            btnFightCane.SetEnabled(true);
            btnFightCane.clickable = new Clickable(OnClickFightCane);
        }

        #endregion

        #region ChangePageByClickCheat

        void OnClickDarkRiddle()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "sconfiggiTrafficantiGuardiaChiede";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "sconfiggiTrafficantiGuardiaChiede")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickFightGufo()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "saltiFortuna";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "saltiFortuna")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickFightCane()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "tiCaliDalBalcone";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "tiCaliDalBalcone")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickPlay()
        {
            GameModel model = GameApplication.Singleton.model;
            PlayerManager.Singleton.pagesRead.Add(model.MainMenu.introChapter.pageID);
            GameApplication.Singleton.view.CharacterCreationView.selectedClass = model.AbbotData;
            GameApplication.Singleton.view.CharacterCreationView.SetClassData();
            OpenGame();
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "arrivaStraniero";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "arrivaStraniero")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickTutorialFight()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "arrivaStraniero";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "arrivaStraniero")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickDuraLex()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "cadiNelVuoto";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "cadiNelVuoto")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickFirstFight()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "vittoriaFightTutorial";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "vittoriaFightTutorial")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void FightUbriaco()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "enigmaUbriaco";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "enigmaUbriaco")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void VsPantegane()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "descrizionePantegane";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "descrizionePantegane")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickOscurita()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "sconfiggiGuardiaAlleata";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "sconfiggiGuardiaAlleata")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickPlayCards()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "combinazioneGiusta";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "combinazioneGiusta")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickFall()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "cadiNelVuotoGuardie";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "cadiNelVuotoGuardie")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickUpperSide()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "inizioPianoSuperiore";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "inizioPianoSuperiore")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickSecondFight()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "fineTutorial";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "fineTutorial")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickClassSelection()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "guardaNelBoccale";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "guardaNelBoccale")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        void OnClickStartTutorial()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "esploraCantina";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "esploraCantina")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
        public void OnClickTalkWithGuard()
        {
            if (!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + "chiamieArrivaGuardia";
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == "chiamieArrivaGuardia")
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }

        string pageID;
        void LoadRightPage(string ciao)
        {
            if(!PlayerManager.Singleton.isMale)
            {
                pageID = LocalizationIDDatabase.FEM_PREFIX + pageID;
            }
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {
                if (scriptableChapter.pageID == pageID)
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }

        }
        void OpenGame()
        {
            GameModel model = GameApplication.Singleton.model;

            if (model.currentView)
            {
                model.currentView.gameObject.SetActive(false);
                model.previousView.Push(GameApplication.Singleton.model.currentView);
            }

            model.currentView = GameApplication.Singleton.view.BookView;
            model.currentView.gameObject.SetActive(true);
        }

        #endregion

        #region ChangePlayerValue
        public void OnSuperstitionSliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.superstition = valueInt;
            Debug.Log("La superstizione è " + PlayerManager.Singleton.superstition);
        }
        public void OnCourageSliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.courage = valueInt;
            Debug.Log("Il coraggio è " + PlayerManager.Singleton.courage);
        }
        public void OnLuckySliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.lucky = valueInt;
            Debug.Log("La tua fortuna è " + PlayerManager.Singleton.lucky);
        }
        public void OnStrenghtSliderValueChanged(float sliderValue)
        {
            int valueInt = (int)sliderValue;
            PlayerManager.Singleton.strength = valueInt;
            Debug.Log("La tua forza è " + PlayerManager.Singleton.strength);
        }
        public void OnDexteritySliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.dexterity = valueInt;
            Debug.Log("La destrezza è " + PlayerManager.Singleton.dexterity);
        }
        public void OnConstitutionSliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.constitution = valueInt;
            Debug.Log("La costituzione è " + PlayerManager.Singleton.constitution);
        }
        public void OnInteligenceSliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.inteligence = valueInt;
            Debug.Log("L'intelligenza è " + PlayerManager.Singleton.inteligence);
        }
        public void OnDefenceSliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.defence = valueInt;
            Debug.Log("La difesa è " + PlayerManager.Singleton.defence);
        }
        public void OnExpSliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.SetPlayerExp(valueInt);
            Debug.Log("EXP è " + PlayerManager.Singleton.GetPlayerExp());
        }
        public void OnLifeSliderValueChanged(float value)
        {
            int valueInt = (int)value;
            PlayerManager.Singleton.lifePoints = valueInt;
            Debug.Log("La vita è " + PlayerManager.Singleton.lifePoints);
        }

        #endregion

        #region Class

        public void TakeAbbotClass()
        {
            GameApplication.Singleton.view.CharacterCreationView.selectedClass = GameApplication.Singleton.model.AbbotData;
            GameApplication.Singleton.view.CharacterCreationView.SetClassData();
        }
        public void TakeCroneClass()
        {
            GameApplication.Singleton.view.CharacterCreationView.selectedClass = GameApplication.Singleton.model.CroneData;
            GameApplication.Singleton.view.CharacterCreationView.SetClassData();
        }
        public void TakeTraffickerClass()
        {
            GameApplication.Singleton.view.CharacterCreationView.selectedClass = GameApplication.Singleton.model.TraffickerData;
            GameApplication.Singleton.view.CharacterCreationView.SetClassData();
        }
        public void TakeBountyClass()
        {
            GameApplication.Singleton.view.CharacterCreationView.selectedClass = GameApplication.Singleton.model.BountyHunterData;
            GameApplication.Singleton.view.CharacterCreationView.SetClassData();
        }

        #endregion

        public void OnClickAdditem(string cheatID)
        {
            //ScriptableItem item = app.model.BookModel.ScriptableItemsDatabase.Singleton.GetItemById(cheatID);
            ScriptableItem item = ScriptableItemsDatabase.Singleton.GetItemById(cheatID);
            if (item)
            {
                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    Debug.Log("Arma " + weapon.attackType);

                    switch (weapon.attackType)
                    {
                        case TypeDatabase.AttackType.Heavy:
                            Debug.Log("Aggiunto arma pesante" + weapon.itemNameLocalization);
                            PlayerManager.Singleton.heavyWeaponList.Add(weapon);
                            break;
                        case TypeDatabase.AttackType.Ranged:
                            Debug.Log("Aggiunto arma distanza" + weapon.itemNameLocalization);
                            PlayerManager.Singleton.rangeWeaponList.Add(weapon);
                            break;
                        case TypeDatabase.AttackType.Light:
                            Debug.Log("Aggiunto arma leggera" + weapon.itemNameLocalization);
                            PlayerManager.Singleton.lightWeaponList.Add(weapon);
                            break;
                        case TypeDatabase.AttackType.Special:
                            Debug.Log("Aggiunto arma speciale" + weapon.itemNameLocalization);
                            PlayerManager.Singleton.specialWeaponList.Add(weapon);
                            break;
                        default:
                            break;
                    }
                    return;
                }
                if (item is Equipment)
                {
                    Equipment equip = (Equipment)item;
                    switch (equip.equipPlaceType.ToString())
                    {
                        case "Head":
                            Debug.Log("Aggiungo head " + equip.itemNameLocalization);
                            PlayerManager.Singleton.lightDefenceList.Add(equip);
                            break;
                        case "Torso":
                            Debug.Log("Aggiungo torso " + equip.itemNameLocalization);
                            PlayerManager.Singleton.balancedDefenceList.Add(equip);
                            break;
                        case "Shield":
                            Debug.Log("Aggiungo shield " + equip.itemNameLocalization);
                            PlayerManager.Singleton.heavyDefenceList.Add(equip);
                            break;
                        case "Accessory1":
                            Debug.Log("Aggiungo accessorio 1 " + equip.itemNameLocalization);
                            PlayerManager.Singleton.talismansList.Add(equip);
                            break;
                        case "Accessory2":
                            Debug.Log("Aggiungo accessorio 2 " + equip.itemNameLocalization);
                            PlayerManager.Singleton.relicsList.Add(equip);
                            break;
                    }
                    return;
                }
                if (item is ScriptableItem)
                {
                    Debug.Log("Aggiungo ITEM : " + item.itemNameLocalization);
                    PlayerManager.Singleton.AddItemToInventory(item, 1);
                    return;
                }
            }
            else
            {
                Debug.Log("L'oggetto inserito non esiste");
            }
        }

        public void OnClickChangeCurrentPage(string cheatID)
        {
            Debug.Log(cheatID);
            //if (PlayerManager.Singleton.isMale)
            //{
            foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
            {

                if (scriptableChapter.pageID == cheatID)
                {
                    GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
                }
            }
        }
    }
}

