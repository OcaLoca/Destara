using StarworkGC.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Riddle", menuName = "ScriptableObjects/ScriptableRiddle", order = 2)]
    public class ScriptableRiddle : ScriptableObject
    {
        public RiddleType riddleType;
        public string localizedConfirmBtn;

        public string GetLocalizedText()
        {
            return Localization.Get(localizedConfirmBtn);
        }

        public enum RiddleType
        {
            SceltaMultipla,
            InserimentoTestuale,
            InserimentoNumerico,
            Fortuna,
            SerieDiScelteCorrette
        }

        [Serializable]
        public enum MultipleChoiceQuestion
        {
            introQuestion
        }
        public MultipleChoiceQuestion multipleChoiceQuestions;

        [Serializable]
        public struct MultipleChoice
        {
            public string buttonText;
            public string pageID;
            public string otherRealityPageID;
            public bool clickAndDrop;
        }
        public MultipleChoice[] multipleChoice;

        [Serializable]
        public struct WriteTxtAnswer
        {
            public string rightSolution;
            public string pageID;
            public string otherRealityPageID;
        }
        public WriteTxtAnswer writeTxtAnswer; 

        [Serializable]
        public struct WriteAnswer
        {
            //public float [] rightNumberSolution;
            public string [] rightNumberStringSolution;
            public string winPageID;
            public string losePageID;
            public string repeatID;
            public int numberOfAttempt;
            public string highSuperstitionWinPageID;
            public string highSuperstitionLosePageID;
        }
        public WriteAnswer writeSolution;

        [Serializable]
        public struct LuckyButton
        {
            public string btnLuckyText;
            public string pageWinID;
            public string pageLoseID;
            public string anotherCardID;
            public string otherRealityLosePageID;
            public string otherRealityWinPageID;
            public Color32 borderColor;
            public AudioClip audioClip;
        }
        public LuckyButton [] luckyButton;

        [Serializable]
        public struct RightPreviousChoice
        {
            public string btnText;
            public int buttonToClick;
            public string [] rightBtnID;
            public string [] loseBtnID;
            public string [] rightBtnIDEnglish;
            public string [] loseBtnIDEnglish;
            public string winID;
            public string repeatID;
            public string loseID;
            public string finishAttempsID;
            public bool normalButton;
        }
        public RightPreviousChoice [] rightPreviousChoice;

        public int MinNumberOfAttemps;
        public int btnToDelete;

        [HideInInspector]
        public List<string> buttonWinIDList = new List<string>();
        [HideInInspector]
        public List<string> buttonLoseIDList = new List<string>();

    }
}

