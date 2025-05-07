using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartMVC;

namespace Game
{
    public class LongQuestionDatabase : MonoBehaviour
    {
        float playerChoice;
        
        public void IntroQuestionRiddle(string pressedIdFirstQuestion)
        {
            if (PlayerManager.Singleton.currentPage.pageID == QuestionDatabase.FIRSTINTROQUESTION)
            {
                playerChoice = 0;
                Debug.Log("🔄 Nuovo test iniziato, playerChoice resettato a 0.");
            }

            switch (PlayerManager.Singleton.currentPage.pageID)
            {
                case QuestionDatabase.FIRSTINTROQUESTION:
                    if (pressedIdFirstQuestion == "OneAnswer") playerChoice += 8;
                    else if (pressedIdFirstQuestion == "SecondAnswer") playerChoice += 6;
                    else if (pressedIdFirstQuestion == "ThirdAnswer") playerChoice += 4;
                    break;

                case QuestionDatabase.SECONDINTROQUESTION:
                    if (pressedIdFirstQuestion == "FourAnswer") playerChoice += 7;
                    else if (pressedIdFirstQuestion == "FiveAnswer") playerChoice += 5;
                    else if (pressedIdFirstQuestion == "SixAnswer") playerChoice += 3;
                    break;

                case QuestionDatabase.THIRDINTROQUESTION:
                    if (pressedIdFirstQuestion == "SevenAnswer") playerChoice -= 2;
                    else if (pressedIdFirstQuestion == "EightAnswer") playerChoice += 1;
                    else if (pressedIdFirstQuestion == "NineAnswer") playerChoice += 2;
                    else if (pressedIdFirstQuestion == "TenAnswer") playerChoice -= 1;
                    break;

                case QuestionDatabase.FOURTHINTROQUESTION:
                    if (pressedIdFirstQuestion == "ElevenAnswer") playerChoice -= 3;
                    else if (pressedIdFirstQuestion == "TwelveAnswer") playerChoice += 2;
                    else if (pressedIdFirstQuestion == "ThirteenAnswer") playerChoice -= 1;
                    else if (pressedIdFirstQuestion == "FourteenAnswer") playerChoice += 3;
                    break;

                case QuestionDatabase.FIFTHINTROQUESTION:
                    if (pressedIdFirstQuestion == "FifteenAnswer") playerChoice -= 2;
                    else if (pressedIdFirstQuestion == "SixteenAnswer") playerChoice += 1;
                    else if (pressedIdFirstQuestion == "SeventeenAnswer") playerChoice -= 1;
                    else if (pressedIdFirstQuestion == "EighteenAnswer") playerChoice += 2;
                    break;

                case QuestionDatabase.SIXTHINTROQUESTION:
                    if (pressedIdFirstQuestion == "NineteenAnswer") playerChoice -= 2;
                    else if (pressedIdFirstQuestion == "TwentyAnswer") playerChoice += 1;
                    else if (pressedIdFirstQuestion == "TwentyoneAnswer") playerChoice -= 1;
                    else if (pressedIdFirstQuestion == "TwentytwoAnswer") playerChoice += 2;

                    // Salviamo il punteggio finale
                    //PlayerManager.Singleton.playerChoice = playerChoice;
                    Debug.Log($"🏹 Punteggio finale: {playerChoice}");

                    if (playerChoice < 4)
                        PlayerManager.answerClassSolution = LocalizationIDDatabase.BOUNTYADVICE;
                    else if (playerChoice >= 4 && playerChoice < 10)
                        PlayerManager.answerClassSolution = LocalizationIDDatabase.TRAFFICKERADVICE;
                    else if (playerChoice >= 10 && playerChoice < 16)
                        PlayerManager.answerClassSolution = LocalizationIDDatabase.ABBOTADVICE;
                    else
                        PlayerManager.answerClassSolution = LocalizationIDDatabase.CRONEADVICE;

                    Debug.Log($"🎭 Classe assegnata: {PlayerManager.answerClassSolution}");
                    break;
            }

            Debug.Log($"🟡 Scelta: {pressedIdFirstQuestion} → playerChoice attuale: {playerChoice}");
        }
    }

}
