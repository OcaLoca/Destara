using System;
using UnityEngine;

public class LevelManagerUtilities : MonoBehaviour
{
    public const float BASELEVELTHRESHOLD = 120;

    public static int ConvertExperienceForUI(float experience, int levelPlayer)
    {
        float currentThreshold = GetPlayerCurrentThreshold(levelPlayer);
        float previousThresholds = 0;
        
        for (int i = 0; i < levelPlayer; i++)
        {
            previousThresholds += GetPlayerCurrentThreshold(i); //tutte le soglie fino a questo livello 
        }

        if (currentThreshold == 0)
        {
            throw new ArgumentException("Il valore massimo non può essere 0.");
        }

        float score = CalculateScore(experience, previousThresholds, currentThreshold);
        return (int)Mathf.Clamp(score, 0, 5);
    }

    public static float CalculateScore(float xp, float previousThresholds, float currentThreshold)
    {
        // Calcola l'XP accumulato nel livello attuale
        float xpInCurrentLevel = xp - previousThresholds;
        // Calcola la percentuale di avanzamento
        float progressPercentage = (float)xpInCurrentLevel / currentThreshold * 100f;
        // Converti la percentuale in un punteggio da 0 a 5
        float score = (progressPercentage / 100f) * 5f;
        // Arrotonda il punteggio al numero intero più vicino (opzionale)
        score = Mathf.Round(score);
        return score;
    }

    public static float GetPlayerCurrentThreshold(int playerLevel, int growthRate = 30)
    {
        if (playerLevel <= 1)
        {
            return BASELEVELTHRESHOLD;
        }

        float threshold = BASELEVELTHRESHOLD + growthRate * Mathf.Pow(playerLevel, 2);
        return threshold;
    }

}
