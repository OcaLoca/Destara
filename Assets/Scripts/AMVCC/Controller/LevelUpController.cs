/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = System.Random;

namespace Game
{
    public class LevelUpController : MonoBehaviour
    {
        public static LevelUpController LevelUpSingleton { get; set; }

        private void Awake()
        {
            LevelUpSingleton = this;
        }

        ushort dexterityProgress;
        ushort constitutionProgress;
        ushort intelligenceProgress;
        ushort strenghtProgress;

        [SerializeField] TMP_Text txtConstitution;
        [SerializeField] TMP_Text txtDexterity;
        [SerializeField] TMP_Text txtIntelligence;
        [SerializeField] TMP_Text txtStrenght;
        [SerializeField] TMP_Text txtPlayerName;
        [SerializeField] TMP_Text txtLevel;
        [SerializeField] TMP_Text txtClass;
        [SerializeField] Image imgClass;

        public void UpdateStatsOnPlayerProgression(int level, ScriptableClass selectedClass)
        {
            Random random = new Random();
            switch (selectedClass)
            {
                case Abbot:
                    constitutionProgress = SetPoints(level, level * 2, random);
                    strenghtProgress = SetPoints(level, level + 1, random);
                    intelligenceProgress = (ushort)(2 + level * 2);
                    dexterityProgress = (ushort)(2 + level * 2);
                    imgClass.sprite = GameApplication.Singleton.model.BookModel.classIconSprites[0];
                    break;
                case BountyHunter:
                    constitutionProgress = SetPoints(level, level + 3, random);
                    strenghtProgress = SetPoints(level, level + 2, random);
                    intelligenceProgress = SetPoints(level - 1, level, random);
                    dexterityProgress = SetPoints(level - 1, level, random);
                    imgClass.sprite = GameApplication.Singleton.model.BookModel.classIconSprites[1];
                    break;
                case Crone:
                    constitutionProgress = SetPoints(level - 1, level, random);
                    strenghtProgress = SetPoints(level - 1, level, random);
                    intelligenceProgress = SetPoints(level, level + 2, random);
                    dexterityProgress = SetPoints(level, level + 2, random);
                    imgClass.sprite = GameApplication.Singleton.model.BookModel.classIconSprites[2];
                    break;
                case Trafficker:
                    constitutionProgress = (ushort)(2 + level * 2);
                    strenghtProgress = (ushort)(2 + level * 2);
                    intelligenceProgress = (ushort)(2 + level * 2);
                    dexterityProgress = (ushort)(2 + level * 2);
                    imgClass.sprite = GameApplication.Singleton.model.BookModel.classIconSprites[3];
                    break;
            }

            PlayerManager.Singleton.UpdateCostitution(constitutionProgress, constitutionProgress, true);
            PlayerManager.Singleton.UpdateLifePoints(constitutionProgress * 10, constitutionProgress * 10);
            PlayerManager.Singleton.UpdateStrenght(strenghtProgress, strenghtProgress, true);
            PlayerManager.Singleton.UpdateIntelligence(intelligenceProgress, intelligenceProgress, true);
            PlayerManager.Singleton.UpdateDexterity(dexterityProgress, dexterityProgress, true);

            InstantiateText(constitutionProgress.ToString(), dexterityProgress.ToString(), intelligenceProgress.ToString(), strenghtProgress.ToString(), PlayerManager.Singleton.playerName, GetTextNewLevel(level), selectedClass.GetClassName);
        }

        private string GetTextNewLevel(int level)
        {
            level = level + 1; //rimetto il livello tolto quando l'ho passato
            if(level < 10)
            {
                return "0" + level.ToString();
            }
            return level.ToString();
        }
        private ushort SetPoints(int min, int max, Random random)
        {
            return (ushort)random.Next(min, max);
        }

        internal void InstantiateText(string txtConstitution, string txtDexterity, string txtIntelligence, string txtStrenght, string txtPlayerName, string txtLevel, string txtClass)
        {
            this.txtConstitution.text = "+" + txtConstitution;
            this.txtDexterity.text = "+" + txtDexterity;
            this.txtIntelligence.text = "+" + txtIntelligence;
            this.txtStrenght.text = "+" + txtStrenght;
            this.txtPlayerName.text = txtPlayerName;
            this.txtLevel.text = txtLevel;
            this.txtClass.text = txtClass;
        }
    }
}
