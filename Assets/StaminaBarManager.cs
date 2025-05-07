using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SmartMVC;
using static Game.BattleController;

namespace Game
{

    public class StaminaBarManager : Model<GameApplication>
    {
        public float currentStamina;
        float offset = 0.005f;

        float maxStamina;
        float lastStamina;

        [SerializeField] TMP_Text currentStaminaCount;
        [SerializeField] TMP_Text staminaCost;
        [SerializeField] Image staminaBar;
        //[SerializeField] Image staminaBackgroundBar;

        //[SerializeField] TMP_Text staminaCost;
        //[SerializeField] Image staminaPreviewBar;


        public bool DamageIsCounted;
        private float target;
        private float lastTarget;
        bool UpdatePlayerStaminaAnimFinish = false;
        bool UpdatePlayerStamina = false;
        float tmpStamina;


        public void ShowCurrentWeaponStaminaCost(int weaponCostText)
        {
            //UpdatePlayerStamina = false;
            if(weaponCostText == 0)
            {
                weaponCostText = 20;
            }
            staminaCost.text = weaponCostText.ToString();
        }


        public void UpdateCurrentStaminaBar(float currentStamina)
        {
            if (lastStamina != currentStamina)
            {
                PlayerRecoverStamina = lastStamina < currentStamina;
            }
            else
            {
                return;
            }
            UIPlayerStaminaTextAlreadyUpdate = false;

            if (lastTarget != target) { lastTarget = target; }

            target = currentStamina / maxStamina;
            if (PlayerRecoverStamina)
            {
                StartCoroutine(WhileRecoverUpdateStaminaTxt(currentStamina));
            }
            else
            {
                StartCoroutine(UpdateCurrentStaminaTxt(currentStamina));
            }
            UpdatePlayerStaminaAnimFinish = false;
            UpdatePlayerStamina = true;
            UIPlayerStaminaAlreadyUpdate = false;
        }

        public void SetStaminaOnStartFight(float currentStamina, float currentWeaponStaminaCost)
        {
            maxStamina = PlayerManager.Singleton.dexterity * 10;
            lastStamina = currentStamina;
            SetStaminaTxtOnFightStart(currentStamina.ToString());
            staminaBar.fillAmount = currentStamina / maxStamina;
           // staminaBackgroundBar.fillAmount = currentStamina / maxStamina;
            UpdatePlayerStamina = false;
            ShowCurrentWeaponStaminaCost((int)currentWeaponStaminaCost);
        }

        float currentVelocity = 0;
        private void Update()
        {
            if (UpdatePlayerStamina)
            {
                staminaBar.fillAmount = Mathf.SmoothDamp(staminaBar.fillAmount, target, ref currentVelocity, 100 * Time.fixedDeltaTime);
                if (InRange(staminaBar.fillAmount, target - offset, target + offset))
                {
                   // staminaBackgroundBar.fillAmount = staminaBar.fillAmount;
                    UpdatePlayerStamina = false;
                    UpdatePlayerStaminaAnimFinish = true;
                }
            }
        }

        bool InRange(float myAmount, float minTarget, float maxTarget)
        {
            return ((myAmount - minTarget) * (myAmount - maxTarget)) <= 0;
        }

        public bool PlayerStaminaBarAnimationFinish()
        {
            return UpdatePlayerStaminaAnimFinish;
        }
        public bool PlayerStaminaBarTextFinish()
        {
            return UIPlayerStaminaTextAlreadyUpdate;
        }

        public void SetStaminaTxtOnFightStart(string currentStamina)
        {
            // maxHpCount.text = maxHp.ToString();
            currentStaminaCount.text = currentStamina.ToString();
        }

        IEnumerator UpdateCurrentStaminaTxt(float currentStamina)
        {
            if (currentStamina >= lastStamina)
            {
                UIPlayerLifeTextAlreadyUpdate = true;
                yield return null;
            }

            if (currentStamina <= 0)
            {
                UIPlayerStaminaTextAlreadyUpdate = true;
            }

            int speedAmount = SpeedUpTxtCount(lastStamina - currentStamina);

            do
            {
                lastStamina = lastStamina - speedAmount;
                if (lastStamina >= 0)
                {
                    currentStaminaCount.text = lastStamina.ToString();
                }
                yield return new WaitForEndOfFrame();
            }
            while (!InRange(lastStamina, currentStamina, currentStamina - speedAmount) || lastStamina == 0);

            UIPlayerStaminaTextAlreadyUpdate = true;
            if (lastStamina <= 0) { currentStaminaCount.text = 0.ToString(); }
        }

        IEnumerator WhileRecoverUpdateStaminaTxt(float staminaJustUpdated)
        {
            if (staminaJustUpdated <= lastStamina)
            {
                UIPlayerStaminaTextAlreadyUpdate = true;
                currentStaminaCount.text = staminaJustUpdated.ToString(); // Aggiorna comunque il testo
                yield break; // Esce immediatamente
            }

            if (staminaJustUpdated <= 0)
            {
                UIPlayerStaminaTextAlreadyUpdate = true;
                currentStaminaCount.text = "0";
                yield break; // Esce immediatamente
            }

            int speedAmount = SpeedUpTxtCount(staminaJustUpdated - lastStamina);

            do
            {
                lastStamina = Mathf.Clamp(lastStamina + speedAmount, 0, maxStamina); // Impedisce overshooting

                if (lastStamina >= 0)
                {
                    currentStaminaCount.text = lastStamina.ToString();
                }
                yield return new WaitForEndOfFrame();
            }
            while (!InRange(lastStamina, staminaJustUpdated, staminaJustUpdated + speedAmount));

            UIPlayerStaminaTextAlreadyUpdate = true;

            if (lastStamina <= 0) { 
                currentStaminaCount.text = 0.ToString();
            }
        }

        int SpeedUpTxtCount(float difference)
        {
            switch (difference)
            {
                case > 1000:
                    return 10;
                case > 700:
                    return 8;
                case > 400:
                    return 6;
                case > 200:
                    return 4;
                default:
                    return 1;
            }
        }

    }
}
