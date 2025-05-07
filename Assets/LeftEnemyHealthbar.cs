using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;

namespace Game
{
    public class LeftEnemyHealthbar : MonoBehaviour
    {
        /// <summary>
        /// Centrale ID 0 , Centrale e Destro 0 e 1 (2 nemici), Tutti ecc
        /// </summary>

        [SerializeField] Image healtBar;
        public float currentHealth;
        float maxHealt;
        [SerializeField] TMP_Text txtMaxLifePoints;
        [SerializeField] TMP_Text txtLifePoints;
        Unit unit;

        private void Start()
        {
            PrepareLifeBar();
        }

        public void PrepareLifeBar()
        {
            if (PlayerManager.Singleton.currentPage.mobID.Count == 2)
            {
                unit.constitution = PlayerManager.Singleton.currentPage.mobID[0].constitution;
                unit.hp = PlayerManager.Singleton.currentPage.mobID[0].hp;
            }
            else
            {
                unit.constitution = PlayerManager.Singleton.currentPage.mobID[2].constitution;
                unit.hp = PlayerManager.Singleton.currentPage.mobID[2].hp;
            }
            maxHealt = unit.constitution * 10;
            txtMaxLifePoints.text = (unit.constitution * 10).ToString();
            txtLifePoints.text = txtMaxLifePoints.text;
        }

        public void UpdateLeftEnemyLife()
        {
            currentHealth = unit.hp;
            if (currentHealth < 0) { txtLifePoints.text = 0.ToString(); }
            else
            {
                txtLifePoints.text = currentHealth.ToString();
            }
            maxHealt = unit.constitution * 10;
            healtBar.fillAmount = currentHealth / maxHealt;
        }


        public void SetLifeBarHUD(Unit unit)
        {
            this.unit = unit;
            UIEnemiesLifeAnimationFinish = true;
        }

    }
}

