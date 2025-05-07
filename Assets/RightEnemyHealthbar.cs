using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;

namespace Game
{
    public class RightEnemyHealthbar : MonoBehaviour
    {
        /// <summary>
        /// Centrale ID 0 , Centrale e Destro 0 e 1 (2 nemici), Tutti ecc
        /// </summary>

        [SerializeField]Image healtBar;
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
            unit.constitution = PlayerManager.Singleton.currentPage.mobID[1].constitution;
            unit.hp = PlayerManager.Singleton.currentPage.mobID[1].hp;
            maxHealt = unit.constitution * 10;
            txtMaxLifePoints.text = (unit.constitution * 10).ToString();
            txtLifePoints.text = txtMaxLifePoints.text;
        }

        public void UpdateRightEnemyLife()
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

