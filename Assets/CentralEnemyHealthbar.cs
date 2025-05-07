using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game.BattleController;

namespace Game
{
    public class CentralEnemyHealthbar : MonoBehaviour
    {

        /// <summary>
        /// Centrale ID 0 , Centrale e Destro 0 e 1 (2 nemici), Tutti ecc
        /// </summary>

        Unit unit;
        public float currentHealth;
        public float lastHealth;
        float maxHealth;
        [SerializeField] TMP_Text currentHpCount;
        [SerializeField] Image lifeBar;

        [SerializeField] Color32 lowColorLife;
        [SerializeField] Color32 normalColorLife;
        [SerializeField] Color32 criticalColorLife;

        public bool DamageIsCounted;
        private float target;
        bool UpdateCentralEnemyLifeAnimFinish = false;
        bool UpdateCentralEnemyLife = false;

        public void UpdateCentralEnemyCurrentLife(float currentHealth)
        {
            target = currentHealth / maxHealth;
            StartCoroutine(UpdateCurrentHpTxt(currentHealth));
            UpdateCentralEnemyLife = true;
            UIEnemiesLifeAnimationFinish = false;
            UIEnemiesLifeTextCountUpdateFinish = false;
        }

        void ChangeLifeBarColor()
        {
            switch (lifeBar.fillAmount)
            {
                case < 0.3f:
                    lifeBar.color = criticalColorLife;
                    break;
                case < 0.5f:
                    lifeBar.color = lowColorLife;
                    break;
                default:
                    lifeBar.color = normalColorLife;
                    break;
            }
        }

        public void SetCentralEnemyLifeOnStartFight(Unit enemy)
        {
            maxHealth = enemy.constitution * 10;
            lastHealth = maxHealth;
            currentHealth = enemy.hp;
            SetFirstTimeLifeTxt(currentHealth);
            lifeBar.fillAmount = currentHealth / maxHealth;
            ChangeLifeBarColor();
        }

        void SetFirstTimeLifeTxt(float currentHealth)
        {
            if (currentHealth < 0) { currentHpCount.text = 0.ToString(); return; }
            currentHpCount.text = currentHealth.ToString();
        }

        float currentVelocity = 0;
        private void Update()
        {
            if (UpdateCentralEnemyLife)
            {
                lifeBar.fillAmount = Mathf.SmoothDamp(lifeBar.fillAmount, target, ref currentVelocity, 100 * Time.fixedDeltaTime);
                ChangeLifeBarColor();

                if (InRange(lifeBar.fillAmount, target - 0.1f, target + 0.02f))
                {
                    UpdateCentralEnemyLife = false;
                    UIEnemiesLifeAnimationFinish = true;
                }
            }
        }

        bool InRange(float myAmount, float minTarget, float maxTarget)
        {
            return ((myAmount - minTarget) * (myAmount - maxTarget)) <= 0;
        }

        IEnumerator UpdateCurrentHpTxt(float currentHealth)
        {
            if (currentHealth >= lastHealth)
            {
                UIEnemiesLifeTextCountUpdateFinish = true;
                yield return null;
            }

            if (currentHealth <= 0)
            {
                UIEnemiesLifeTextCountUpdateFinish = true;
                yield return null;
            }

            int speedAmount = SpeedUpTxtCount(lastHealth - currentHealth);
            do
            {
                lastHealth = lastHealth - speedAmount;

                if (lastHealth >= 0)
                {
                    currentHpCount.text = lastHealth.ToString();
                }
                yield return new WaitForEndOfFrame();
            }
            while (!InRange(lastHealth, currentHealth, currentHealth - speedAmount) || lastHealth == 0);

            UIEnemiesLifeTextCountUpdateFinish = true;
            if (lastHealth <= 0) { currentHpCount.text = 0.ToString(); }
        }

        int SpeedUpTxtCount(float difference)
        {
            switch (difference)
            {
                case > 1000:
                    return 5;
                case > 700:
                    return 4;
                case > 400:
                    return 3;
                case > 200:
                    return 2;
                default:
                    return 1;
            }

        }
    }
}


