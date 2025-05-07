using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{


    public class ScriptableGoal : ScriptableObject
    {
        /// <summary>
        ///per creare una nuova medaglia/trofeo tasto destro crea nuovoGoal, una volta creato chiedere ai programmatori di implementare la funzione UnlockTheGoal() Inserire in Model GoalDatabase.
        ///Andare in goal view ed aggiungere il tasto della medaglia/trofeo dando come nome dell'oggetto l'ID dato al goal. 
        /// </summary>

        [SerializeField] string goalName;
        public string ID;
        public bool alreadyShowInMuseum;
        public string description;
        public string PlayerPrefsGoalID;
        public string GetGoalID { get => ID; }
        public string GetGoalName { get => goalName; }
        [SerializeField] float experienceTrophy;
        [SerializeField] bool showmultipleTime;

        public bool GetShowMultipleTimes { get => showmultipleTime; }
        public GoalType goalType;

        public enum GoalType
        {
            trophy,
            medal
        }

        internal string GetLocalizedGoalTitle()
        {
            return Localization.Get(GetGoalName);
        }

        internal string GetLocalizedGoalDescription()
        {
            return Localization.Get(GetGoalName + "Dsc");
        }

        public virtual bool UnlockTheGoal()
        {
            return true;
        }

        public bool NeverUnlockedInThisDevice()
        {
            if (string.IsNullOrEmpty(PlayerPrefsGoalID))
            {
                PlayerPrefsGoalID = goalName.ToUpper();
            }

            if (PlayerPrefs.GetInt(PlayerPrefsGoalID) == 0)
            {
                return true;
            }
            return false;
        }

        public void SetGoalUnlockedInThisDevice()
        {
            if (string.IsNullOrEmpty(PlayerPrefsGoalID))
            {
                PlayerPrefsGoalID = goalName.ToUpper();
            }
            PlayerPrefs.SetInt(PlayerPrefsGoalID, 1);
        }

        public void UnlockInCuriosity()
        {
            //{ GameApplication.Singleton.view.GoalsView.ReloadTrophyView(GetGoalID); }
        }

        public virtual float GetExperienceFromTrophy()
        {
            if (goalType == GoalType.trophy)
            {
                return experienceTrophy;
            }
            else
            {
                return 0;
            }
        }
    }
}

