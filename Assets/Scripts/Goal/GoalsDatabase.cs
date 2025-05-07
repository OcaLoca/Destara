using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace Game
{
    public class GoalsDatabase : MonoBehaviour
    {
        public List<ScriptableGoal> scriptableGoals = new List<ScriptableGoal>();
        public List<ScriptableGoal> currentGameScriptableGoals = new List<ScriptableGoal>();

        public static GoalsDatabase Singleton { get; private set; }

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            OnStartNewGameResetTrophyDatabase();
        }

        public void OnStartNewGameResetTrophyDatabase()
        {
            currentGameScriptableGoals = scriptableGoals;
        }
        public void GoalsUnlocked()
        {
            int count = currentGameScriptableGoals.Count;
            if (count == 0) { Debug.LogWarning("Attenzione 0"); }
            for (int i = 0; i < count; i++)
            {
                if (currentGameScriptableGoals[i].UnlockTheGoal())
                {
                    if (!currentGameScriptableGoals[i].GetShowMultipleTimes)
                    {
                        if (currentGameScriptableGoals[i].NeverUnlockedInThisDevice())
                        {
                            ShowUnlockedGoal(currentGameScriptableGoals[i]);

                            //int index = scriptableGoals.IndexOf(currentGameScriptableGoals[i]);
                            //scriptableGoals.RemoveAt(index);
                            break;
                        }
                    }
                    else
                    {
                        ShowUnlockedGoal(currentGameScriptableGoals[i]);
                        //int newIndex = currentGameScriptableGoals.IndexOf(currentGameScriptableGoals[i]);
                        //currentGameScriptableGoals.RemoveAt(newIndex); //rimuovo solo nella lista corrente
                        break;
                    }
                }
            }
        }

        public void ShowUnlockedGoal(ScriptableGoal unlockGoal)
        {
            unlockGoal.SetGoalUnlockedInThisDevice();
            GoalNotificationManager.Singleton.Show(unlockGoal.GetLocalizedGoalTitle(), unlockGoal.GetLocalizedGoalDescription(), unlockGoal.goalType);
        }


    }
}
