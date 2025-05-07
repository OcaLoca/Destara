using SmartMVC;
using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.TypeDatabase;

namespace Game
{
    public class CharacterCreationView : View<GameApplication>
    {
        [Header("UIObject")]
        [SerializeField] Scrollbar verticalBountyScrollbar;
        public ScrollRect bountyScrollRect;
        public ScrollRect abbotScrollRect;
        public ScrollRect traffickerScrollRect;
        public ScrollRect croneScrollRect;
        public GameObject pnlOverwrite;
        public GameObject panelSelectName;
        public GameObject alarmInvalidName;
        [SerializeField] GameObject[] classContainer;
        public GameObject[] GetClassContainer { get => classContainer; }
        public GameObject PanelTutorial;
        public GameObject PanelLegend;

        [Header("Button&Txt")]
        public TMP_InputField insertName;
        public TMP_Text insertNameTxt;
        public Button[] btnSelectClass;
        public Button btnConfirm;
        public Button btnCloseNamePanel;
        // [SerializeField] Button[] btnInfo;
        public Button btnYesOverwrite;
        public Button btnNoOverwrite;
        public Button[] btnOpenLegendPanel;

        [SerializeField]
        ScriptableClass[] allClass;
        public ScriptableClass selectedClass;
        [SerializeField]
        public Dictionary<Button, ScriptableClass> classesButtons = new Dictionary<Button, ScriptableClass>();
        public bool isActive = true;
        public int scrollSwitch;

        [Header("UIForClass")]
        public Sprite[] classLogo;
        public Sprite[] classLogoPressed;

        [Header("References")]
        public BattlePlayerManager BattlePlayerManager;

        [Header("Sounds")]
        [SerializeField] AudioClip confirmClass;
        [SerializeField] AudioClip confirmName;
        [SerializeField] AudioClip backAndFoward;
        [SerializeField] AudioClip overwriteConfirm;
        [SerializeField] AudioClip backPanel;

        Dictionary<string, string> classRefName = new Dictionary<string, string>()
        {
            {"Abate","Abbot"},
            {"Cacciatore","Bounty Hunter"},
            {"Megera","Crone"},
            {"Trafficante","Trafficker"}
        };

        void Awake()
        {
            panelSelectName.gameObject.SetActive(false);
            insertNameTxt.gameObject.SetActive(false);
            btnCloseNamePanel.gameObject.SetActive(false);
            isActive = true;
            for (int i = 0; i < allClass.Length; i++)
            {
                classesButtons.Add(btnSelectClass[i], allClass[i]);
            }
        }

        void OnEnable()
        {
            if (PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKPLAYERACTIVETUTORIAL) == 1 ||
                PlayerPrefs.GetInt(LocalizationIDDatabase.CHECKACTIVETUTORIALONFIRSTLAUNCH) == 1)
            {
                PanelTutorial.SetActive(true);
            }
            app.model.currentView = this;

            foreach (Button btn in btnSelectClass)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(SetClassData);
                btn.onClick.AddListener(SetRandomName);
                btn.onClick.AddListener(WriteName);
            }

            foreach (Button btn in btnOpenLegendPanel)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OpenLegendPanel);
            }

            InitializeClassesValues();

            PlayerManager.Singleton.CleanEquipment();

            //Reset scroll vertical position
            abbotScrollRect.verticalNormalizedPosition = 1f;
            bountyScrollRect.verticalNormalizedPosition = 1f;
            croneScrollRect.verticalNormalizedPosition = 1f;
            traffickerScrollRect.verticalNormalizedPosition = 1f;

            //Panel overwrite save
            pnlOverwrite.gameObject.SetActive(false);
            btnYesOverwrite.onClick.RemoveAllListeners();
            btnYesOverwrite.onClick.AddListener(ClickYesOverwrite);
            btnNoOverwrite.onClick.RemoveAllListeners();
            btnNoOverwrite.onClick.AddListener(ClickNoOverwrite);
        }


        [Header("BountyHunterValues")]
        [SerializeField] TMP_Text bhConstitution;
        [SerializeField] TMP_Text bhDexterity;
        [SerializeField] TMP_Text bhIntelligence;
        [SerializeField] TMP_Text bhStrenght;
        //[SerializeField] TMP_Text bhDefenceType;
        //[SerializeField] TMP_Text bhDefenceValue;
        [SerializeField] TMP_Text bhSuperstition;
        [SerializeField] TMP_Text bhCourage;
        [SerializeField] TMP_Text bhLucky;
        [SerializeField] TMP_Text bhAbilityPoints;
        [SerializeField] TMP_Text bhLifePoints;
        [SerializeField] TMP_Text bhStamina;

        [Header("CroneValues")]
        [SerializeField] TMP_Text crConstitution;
        [SerializeField] TMP_Text crDexterity;
        [SerializeField] TMP_Text crIntelligence;
        [SerializeField] TMP_Text crStrenght;
        //[SerializeField] TMP_Text crDefenceType;
        //[SerializeField] TMP_Text crDefenceValue;
        [SerializeField] TMP_Text crSuperstition;
        [SerializeField] TMP_Text crCourage;
        [SerializeField] TMP_Text crLucky;
        [SerializeField] TMP_Text crAbilityPoints;
        [SerializeField] TMP_Text crLifePoints;
        [SerializeField] TMP_Text crStamina;

        public void InitializeClassesValues()
        {
            BountyHunter bh = app.model.BountyHunterData;
            bhConstitution.text = bh.constitution.ToString();
            bhDexterity.text = bh.dexterity.ToString();
            bhIntelligence.text = bh.inteligence.ToString();
            bhStrenght.text = bh.strength.ToString();
            //bhDefenceType.text = Localization.Get(PlayerManager.Singleton.GetDefenceType(bh.equippedHeavyDefence.GetWeight() + bh.equippedLightDefence.GetWeight() + bh.equippedBalancedDefence.GetWeight()).ToString());
            //bhDefenceValue.text = (bh.equippedHeavyDefence.defence + bh.equippedLightDefence.defence + bh.equippedBalancedDefence.defence).ToString();
            bhSuperstition.text = bh.superstition.ToString();
            bhCourage.text = bh.courage.ToString();
            bhLucky.text = bh.luck.ToString();
            bhAbilityPoints.text = (bh.inteligence).ToString();
            bhStamina.text = (bh.dexterity * 10).ToString();
            bhLifePoints.text = (bh.constitution * 10).ToString();

            Crone cr = app.model.CroneData;
            crConstitution.text = cr.constitution.ToString();
            crDexterity.text = cr.dexterity.ToString();
            crIntelligence.text = cr.inteligence.ToString();
            crStrenght.text = cr.strength.ToString();
            //crDefenceType.text = Localization.Get(PlayerManager.Singleton.GetDefenceType(cr.equippedHeavyDefence.GetWeight() + cr.equippedLightDefence.GetWeight() + cr.equippedBalancedDefence.GetWeight()).ToString());
            //crDefenceValue.text = (cr.equippedHeavyDefence.defence + cr.equippedLightDefence.defence + cr.equippedBalancedDefence.defence).ToString();
            crSuperstition.text = cr.superstition.ToString();
            crCourage.text = cr.courage.ToString();
            crLucky.text = cr.luck.ToString();
            crAbilityPoints.text = (cr.inteligence).ToString();
            crStamina.text = (cr.dexterity * 10).ToString();
            crLifePoints.text = (cr.constitution * 10).ToString();
        }

        void OnDisable()
        {
            btnCloseNamePanel.onClick.RemoveAllListeners();
            btnConfirm.onClick.RemoveAllListeners();

            StopAllCoroutines();
        }

        public static int choiceIndexClass;
        public void ChangeSelectedClass(int index)
        {
            selectedClass = allClass[index];
            choiceIndexClass = index;
        }

        public void SetRandomName()
        {
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.GenericMenuButton);

            string[] randomFemaleName = new string[] { "Elize", "Ardarya", "Momoa", "Ursa" };
            string[] randomMaleName = new string[] { "Gauthak", "Argon", "Lobot", "Lok" };
            string[] randomNames = new string[0];

            switch (choiceIndexClass)
            {
                case 0:
                    randomNames = randomMaleName;
                    break;
                case 1:
                    randomNames = randomMaleName;
                    break;
                case 2:
                    randomNames = randomFemaleName;
                    break;
                case 3:
                    randomNames = randomFemaleName;
                    break;
            }

            btnConfirm.interactable = true;
            btnConfirm.GetComponent<Animator>().enabled = true;
            int random = Random.Range(0, randomNames.Length - 1);
            insertName.text = randomNames[random];
        }

        public void OnClickInfo()
        {
            StartCoroutine(ScrollDown(5f));
        }

        public IEnumerator ScrollDown(float FadeTime)
        {
            float startPosition = verticalBountyScrollbar.value;

            while (verticalBountyScrollbar.value > 0.60f)
            {
                verticalBountyScrollbar.value -= startPosition * Time.deltaTime / FadeTime;

                yield return null;
            }
        }
        public IEnumerator ScrollUp(float FadeTime)
        {
            float startPosition = verticalBountyScrollbar.value;

            while (verticalBountyScrollbar.value < 1f)
            {
                verticalBountyScrollbar.value += startPosition * Time.deltaTime / FadeTime;

                yield return null;
            }
        }

        public void SetClassData()
        {
            PlayerManager.Singleton.selectedClass = selectedClass;
            SetStarterWeapon();
            SetArmor();
            OnChoiceClassSetBookValue();
            OnChoiceClassSetFightValue();
            SetAbility();
            PlayerManager.Singleton.UpdateEquipStats();
        }

        void OpenLegendPanel()
        {
            PanelLegend.SetActive(true);
        }

        public void SetStarterWeapon()
        {
            //Equipaggio ma non aggiungo in elenco per non duplicare
            PlayerManager.Singleton.playerWeapon.lightWeapon = selectedClass.lightWeapon;
            PlayerManager.Singleton.playerWeapon.heavyWeapon = selectedClass.heavyWeapon;
            PlayerManager.Singleton.playerWeapon.rangeWeapon = selectedClass.rangeWeapon;
            PlayerManager.Singleton.playerWeapon.specialWeapon = selectedClass.specialWeapon;
        }


        void SetArmor()
        {
            PlayerManager.Singleton.playerEquipment.equippedClassDefaultDefence = selectedClass.equippedClassDefence;
            PlayerManager.Singleton.playerEquipment.equippedLightDefence = selectedClass.equippedLightDefence;
            PlayerManager.Singleton.playerEquipment.equippedBalancedDefence = selectedClass.equippedBalancedDefence;
            PlayerManager.Singleton.playerEquipment.equippedHeavyDefence = selectedClass.equippedHeavyDefence;
            PlayerManager.Singleton.playerEquipment.equippedGemstone = selectedClass.equippedGemStone;
            PlayerManager.Singleton.playerEquipment.equippedTalisman = selectedClass.equippedTalisman;
            PlayerManager.Singleton.playerEquipment.equippedRelic = selectedClass.equippedRelic;
        }

        public void SetAbility()
        {
            PlayerManager.Singleton.playerAbility = selectedClass.abilities;
            PlayerManager.Singleton.highPlayerAbility = selectedClass.highAbilities;
            PlayerManager.Singleton.finalAbility = selectedClass.finalAbility;
            PlayerManager.Singleton.highFinalAbility = selectedClass.highFinalAbility;
        }

        public void OnChoiceClassSetBookValue()
        {
            PlayerManager.Singleton.superstition = selectedClass.superstition;
            PlayerManager.Singleton.courage = selectedClass.courage;
            PlayerManager.Singleton.lucky = selectedClass.luck;
            PlayerManager.Singleton.maxTorchCount = selectedClass.GetMaxTorchCount;
            PlayerManager.Singleton.maxPaperCount = selectedClass.GetMaxPaperCount;
            PlayerManager.Singleton.classPlayerTypeToString = selectedClass.GetSavedName;
        }
        public void OnChoiceClassSetFightValue()
        {
            PlayerManager.Singleton.constitution = selectedClass.constitution;
            PlayerManager.Singleton.strength = selectedClass.strength;
            PlayerManager.Singleton.dexterity = selectedClass.dexterity;
            PlayerManager.Singleton.inteligence = selectedClass.inteligence;
            PlayerManager.Singleton.lifePoints = selectedClass.constitution * 10;
            PlayerManager.Singleton.SetPlayerAbilityPoints((int)(selectedClass.inteligence));
            PlayerManager.Singleton.SetPlayermanagerStamina((int)(selectedClass.dexterity * 10));
        }

        void WriteName()
        {
            panelSelectName.gameObject.SetActive(true);
            insertNameTxt.gameObject.SetActive(true);
            btnCloseNamePanel.onClick.RemoveAllListeners();
            btnCloseNamePanel.gameObject.SetActive(true);
            btnCloseNamePanel.onClick.AddListener(delegate { CloseNamePanel(); });
            btnConfirm.onClick.RemoveAllListeners();

            AvoidOverwriteSaveQuestionOnDemo();
            insertName.onValueChanged.AddListener(delegate { HideButton(); });
        }

        void AvoidOverwriteSaveQuestionOnDemo()
        {
            btnConfirm.onClick.AddListener(delegate { ConfirmName(); });
        }

        void AskOverwriteLastSave()
        {
            if (SaveSystemUtilities.CheckSave(SaveType.Hard, classRefName[PlayerManager.Singleton.selectedClass.GetClassName]))
            {
                Debug.Log("Esiste già il save");
                panelSelectName.gameObject.SetActive(false);
                pnlOverwrite.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Non esistono salvataggi");
                btnConfirm.onClick.AddListener(delegate { ConfirmName(); });
            }
        }

        void HideButton()
        {
            int letterCount = insertName.text.Length;
            if (letterCount < 9 && letterCount > 0)
            {
                btnConfirm.interactable = true;
                btnConfirm.GetComponent<Animator>().enabled = true;
                alarmInvalidName.SetActive(false);
            }
            else
            {
                btnConfirm.interactable = false;
                btnConfirm.GetComponent<Animator>().enabled = false;
                alarmInvalidName.SetActive(true);
            }
        }
        void CloseNamePanel()
        {
            panelSelectName.gameObject.SetActive(false);
            insertNameTxt.gameObject.SetActive(false);
            UISoundManager.Singleton.PlayAudioClip(backPanel);
        }

        Coroutine loadWeaponsInEquipmentUI;
        void ConfirmName()
        {
            panelSelectName.gameObject.SetActive(false);
            PlayerManager.Singleton.playerName = insertName.text;
            BookView view = app.view.BookView;
            byte classIndex = SetPlayerClassAndName();

            //disattivo gemme
            PlayerPrefs.SetInt(SettingsMenuView.SHOWGEMS, 0);
            UISoundManager.Singleton.PlayAudioClip(app.Sounds.ConfirmDifficultButton);

            view.classButtonIndex = classIndex;
            BattlePlayerManager.SetClassBattleUI(classIndex);
            PlayerManager.Singleton.SetPlayerLvl(1);
            PlayerManager.Singleton.UpdateDeathPlayerManagerCurrentState();
            loadWeaponsInEquipmentUI = StartCoroutine(LoadWeaponAndObjectInTheUI());

            GameApplication.Singleton.model.SaveNotificationManager.ShowSavePopUp();
            SaveSystem.SavePlayerLastRun(SaveType.Soft, "inizioTutorial");
            Debug.LogFormat("Sto salvando dopo la scelta classe!");
            //SaveSystem.SavePlayer(SaveType.Hard);
            
            string pageID = "inizioTutorial";
            LoadRightPage(pageID);
        }

        void LoadRightPage(string pageID)
        {
            if (app.model.currentView)
            {
                app.model.currentView.gameObject.SetActive(false);
                app.model.previousView.Push(app.model.currentView);
            }
            app.model.currentView = app.view.BookView;
            app.model.currentView.gameObject.SetActive(true);
            GameApplication.Singleton.view.BookView.LoadPage(pageID);
            UIStatsManager.Singleton.UpdateGems();
        }

        IEnumerator LoadWeaponAndObjectInTheUI()
        {
            yield return new WaitUntil(GiveStarterPackClassObject);
            ActivateUILoadingSystem();
        }

        void ActivateUILoadingSystem()
        {
            GameApplication.Singleton.view.EquipmentView.LoadEquippedLabels();
        }

        public bool GiveStarterPackClassObject()
        {
            foreach (ScriptableItem item in PlayerManager.Singleton.selectedClass.itemInventory)
            {
                PlayerManager.Singleton.AddItemDrop(item, item.GetObjectQuantity);
            }
            return true;
            
        }

        byte SetPlayerClassAndName()
        {
            byte classIndex = 0;

            if (PlayerManager.Singleton.selectedClass is Abbot)
            {
                PlayerManager.Singleton.className = GameApplication.Singleton.model.AbbotData.GetClassName;
                PlayerManager.Singleton.isMale = true;
                PlayerManager.Singleton.initialEquipmentDefenseType = DefenseType.Avarage;
            }
            else if (PlayerManager.Singleton.selectedClass is BountyHunter)
            {
                PlayerManager.Singleton.className = GameApplication.Singleton.model.BountyHunterData.GetClassName;
                PlayerManager.Singleton.isMale = true;
                PlayerManager.Singleton.initialEquipmentDefenseType = DefenseType.Heavy;
                classIndex = 1;
            }
            else if (PlayerManager.Singleton.selectedClass is Crone)
            {
                PlayerManager.Singleton.className = GameApplication.Singleton.model.CroneData.GetClassName;
                PlayerManager.Singleton.isMale = false;
                PlayerManager.Singleton.initialEquipmentDefenseType = DefenseType.Light;
                classIndex = 2;
            }
            else
            {
                PlayerManager.Singleton.className = GameApplication.Singleton.model.TraffickerData.GetClassName;
                PlayerManager.Singleton.isMale = false;
                PlayerManager.Singleton.initialEquipmentDefenseType = DefenseType.Avarage;
                classIndex = 3;
            }

            return classIndex;
        }


        void ClickNoOverwrite()
        {
            Debug.Log("Non sovrascrivo");
            pnlOverwrite.gameObject.SetActive(false);
            UISoundManager.Singleton.PlayAudioClip(overwriteConfirm);
        }

        void ClickYesOverwrite()
        {
            Debug.Log("Sovrascrivo");
            pnlOverwrite.gameObject.SetActive(false);
            panelSelectName.gameObject.SetActive(true);
            btnConfirm.onClick.RemoveAllListeners();
            btnConfirm.onClick.AddListener(delegate { ConfirmName(); });
            UISoundManager.Singleton.PlayAudioClip(overwriteConfirm);
        }
    }
}


