using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;
using static Game.BattleController;

namespace Game
{

    public class HealthBarManager : Model<GameApplication>
    {
        public float currentHealth;
        float offset = 0.005f;

        float maxHealth;
        float lastHealth;

        [SerializeField] TMP_Text currentHpCount;
        [SerializeField] Image lifeBar;

        [SerializeField] Color32 lowColorLife;
        [SerializeField] Color32 normalColorLife;
        [SerializeField] Color32 criticalColorLife;

        public bool DamageIsCounted;
        private float target;
        private float lastTarget;
        bool UpdatePlayerLifeAnimFinish = false;
        bool UpdatePlayerLife = false;
        float tmpLife;

        public void UpdateCurrentLife(float currentHealth)
        {
            if (lastHealth != currentHealth)
            {
                PlayerRecoverLife = lastHealth < currentHealth;
            }
            else
            {
                return;
            }

            UIPlayerLifeTextAlreadyUpdate = false;

            if (lastTarget != target) { lastTarget = target; }

            target = currentHealth / maxHealth;
            if (PlayerRecoverLife)
            {
                StartCoroutine(RecoverUpdateText(currentHealth));
            }
            else
            {
                StartCoroutine(UpdateCurrentHpTxt(currentHealth));
            }
            UpdatePlayerLife = true;
            UpdatePlayerLifeAnimFinish = false;
            UIPlayerLifeAlreadyUpdate = false;
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

        public void SetLifeOnStartFight(float currentHealth)
        {
            maxHealth = PlayerManager.Singleton.constitution * 10;
            lastHealth = currentHealth;
            SetFirstTimePlayerHpTxt(currentHealth);
            lifeBar.fillAmount = currentHealth / maxHealth;
            ChangeLifeBarColor();
            //UpdatePlayerLife = false;
        }

        float currentVelocity = 0;
        private void Update()
        {
            if (UpdatePlayerLife)
            {
                if (PlayerManager.Singleton.PlayerIsDead())
                {
                    target = 0;
                }
                Debug.LogFormat("IL TARGET ERA {0} ora è {1}", lastTarget, target);

                lifeBar.fillAmount = Mathf.SmoothDamp(lifeBar.fillAmount, target, ref currentVelocity, 100 * Time.fixedDeltaTime);
                ChangeLifeBarColor();
                if (InRange(lifeBar.fillAmount, target - offset, target + offset))
                {
                    UpdatePlayerLife = false;
                    UpdatePlayerLifeAnimFinish = true;
                }
            }
        }

        bool InRange(float myAmount, float minTarget, float maxTarget)
        {
            return ((myAmount - minTarget) * (myAmount - maxTarget)) <= 0;
        }

        public bool PlayerLifeBarFinish()
        {
            return UpdatePlayerLifeAnimFinish;
        }
        public bool PlayerLifeTextBarFinish()
        {
            return UIPlayerLifeTextAlreadyUpdate;
        }

        public void SetFirstTimePlayerHpTxt(float currentHealth)
        {
            // maxHpCount.text = maxHp.ToString();
            currentHpCount.text = currentHealth.ToString();
        }

        IEnumerator UpdateCurrentHpTxt(float currentHealth)
        {
            if (currentHealth >= lastHealth)
            {
                UIPlayerLifeTextAlreadyUpdate = true;
                yield return null;
            }

            if (currentHealth <= 0)
            {
                UIPlayerLifeTextAlreadyUpdate = true;
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

            UIPlayerLifeTextAlreadyUpdate = true;
            if (lastHealth <= 0) { currentHpCount.text = 0.ToString(); }
        }

        IEnumerator RecoverUpdateText(float healthJustUpdated)
        {
            if (healthJustUpdated <= lastHealth)
            {
                UIPlayerLifeTextAlreadyUpdate = true;
                currentHpCount.text = healthJustUpdated.ToString(); // Aggiorna comunque il testo
                yield break; // Esce immediatamente
            }

            if (healthJustUpdated <= 0)
            {
                UIPlayerLifeTextAlreadyUpdate = true;
                currentHpCount.text = "0";
                yield break;
            }

            int speedAmount = SpeedUpTxtCount(healthJustUpdated - lastHealth);

            do
            {
                lastHealth = Mathf.Clamp(lastHealth + speedAmount, 0, maxHealth); // Impedisce overshooting
                
                if (lastHealth >= 0)
                {
                    currentHpCount.text = lastHealth.ToString();
                }
                yield return new WaitForEndOfFrame();
            }
            while (!InRange(lastHealth, healthJustUpdated, healthJustUpdated + speedAmount));

            UIPlayerLifeTextAlreadyUpdate = true;
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
