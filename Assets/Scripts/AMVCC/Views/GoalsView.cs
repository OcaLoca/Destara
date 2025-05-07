using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;
using UnityEngine.UI;
using TMPro;
using StarworkGC.Localization;

namespace Game
{
    public class GoalsView : View<GameApplication>
    {
        public Button btnBack;
        public List<Button> btnTrophyAndMedals = new List<Button>();
        public List<ScriptableGoal> scriptableGoals = new List<ScriptableGoal>();
        int scrollbarSteps;
        [SerializeField] Scrollbar goalViewScrollbar;

        [Header("UI")]
        [SerializeField] GameObject btnContainer;
        public GameObject GetBtnContainer { get => btnContainer; }
        [SerializeField] MuseumButtonManager tmpArtBtnPrefab;
        [SerializeField] EnemyArtContainer tmpContainer;

        List<MuseumButtonManager> buttonInstantiated = new List<MuseumButtonManager>();
        List<EnemyArtContainer> trophyContainer = new List<EnemyArtContainer>();


        public void SetupGoalView()
        {
            scriptableGoals = app.model.GoalsDatabase.scriptableGoals;
            buttonInstantiated.Clear();
            scrollbarSteps = 0;
            float singleStepValue = 1f / scriptableGoals.Count;
            TrophyView trophyView = app.view.TrophyView;

            foreach (ScriptableGoal goal in scriptableGoals)
            {
                MuseumButtonManager btnOpenArt = Instantiate(tmpArtBtnPrefab);
                EnemyArtContainer trophyArtCnt = Instantiate(tmpContainer);
                buttonInstantiated.Add(btnOpenArt);
                scrollbarSteps++;
                trophyArtCnt.SetupTrophy(goal);
                trophyArtCnt.transform.SetParent(trophyView.TrophyLayout.transform, false);
                trophyContainer.Add(trophyArtCnt);
                btnOpenArt.name = goal.GetGoalID;

                if (goal.alreadyShowInMuseum)
                {
                    btnOpenArt.GetComponentInChildren<TMP_Text>().text = goal.GetGoalName;
                }
                else
                {
                    btnOpenArt.GetComponentInChildren<TMP_Text>().text = "???";
                }

                btnOpenArt.transform.SetParent(GetBtnContainer.GetComponent<RectTransform>().transform, false);

                btnOpenArt.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                btnOpenArt.GetComponentInChildren<Button>().onClick.AddListener(OnOpenTrophy);
                btnOpenArt.GetComponentInChildren<Button>().onClick.AddListener(delegate { SetScrollArtPoint(singleStepValue, goal); });

            }
            trophyContainer[0].btnLeft.enabled = false;
            trophyContainer[0].btnLeft.GetComponentInChildren<TMP_Text>().enabled = false;
            trophyContainer[trophyContainer.Count - 1].btnRight.enabled = false;
            trophyContainer[trophyContainer.Count - 1].btnRight.GetComponentInChildren<TMP_Text>().enabled = false;

            buttonInstantiated[buttonInstantiated.Count - 1].GetLily.enabled = false;
            goalViewScrollbar.numberOfSteps = scrollbarSteps;
        }

        public void ReloadTrophyView(string trophyName)
        {
            int cntIndex = trophyContainer.FindIndex(tpyCn => tpyCn.name == trophyName);
            int btnIndex = buttonInstantiated.FindIndex(bt => bt.name == trophyName);
            Debug.Log(trophyContainer.Count);

            trophyContainer[cntIndex].lockedImage.gameObject.SetActive(false);
            buttonInstantiated[cntIndex].GetComponentInChildren<TMP_Text>().text = trophyName;
        }

        void OnEnable()
        {
            app.model.currentView = this;
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(OnClickBack);
        }

        void SetScrollArtPoint(float stepValue, ScriptableGoal goal)
        {
            //attenzione qua lo scarto è calcolato calcolando di avere 15 elementi museum.Count quindi cambiarlo in caso di aumento e diminuzione
            int index = scriptableGoals.FindIndex(u => u.ID == goal.ID);
            goalViewScrollbar.value = (float)stepValue * index;
            if ((index > 6) && (index < 14))
            {
                goalViewScrollbar.value = ((float)stepValue * index) + 0.0300f;
            }
            else if (index >= 14)
            {
                goalViewScrollbar.value = ((float)stepValue * index) + 0.0500f;
            }
        }

        public void SkipGoalScroll()
        {
            scriptableGoals = app.model.GoalsDatabase.scriptableGoals;
            float singleStepValue = 1f / scriptableGoals.Count;
            goalViewScrollbar.value = goalViewScrollbar.value + singleStepValue;
        }
        public void BackGoalScroll()
        {
            scriptableGoals = app.model.GoalsDatabase.scriptableGoals;
            float singleStepValue = 1f / scriptableGoals.Count;
            goalViewScrollbar.value = goalViewScrollbar.value - singleStepValue;
        }

        void OnOpenTrophy()
        {
            Notify(MVCEvents.OPEN_TROPHY_VIEW);
        }

        void OnClickBack()
        {
            Notify(MVCEvents.SWITCH_TO_PREVIOUS_VIEW);
        }

    }
}