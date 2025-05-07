/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class UIStatsManager : MonoBehaviour
    {
        public static UIStatsManager Singleton { get; set; }
        Coroutine fillSuperstitionIcon, fillLuckyIcon, fillCourageIcon;

        [SerializeField] Image courageIcon, superstitionIcon, luckyIcon;
        Animator anim;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnEnable()
        {
            anim = gameObject.GetComponent<Animator>();
        }

        private void OnDisable()
        {
            // StopAllCoroutines();
        }

        public void ScrollDownChain()
        {
            anim.SetTrigger("ScrollDownChain");
        }

        public void ScrollUpChain()
        {
            anim.SetTrigger("ScrollUpChain");
        }

        float superstitionToFloat = 0;
        float courageToFloat = 0;
        float luckyToFloat = 0;

        float superstitionFillAmount = 0;
        float luckyFillAmount = 0;
        float courageFillAmount = 0;

        public void UpdateGems()
        {
            if (gameObject.activeSelf && PlayerPrefs.GetInt(SettingsMenuView.SHOWGEMS) == 0)
            {
                UpdateGemsFillAmount();
                fillLuckyIcon = StartCoroutine(SmoothFillIcon(luckyIcon, luckyFillAmount));
                fillSuperstitionIcon = StartCoroutine(SmoothFillIcon(superstitionIcon, superstitionFillAmount));
                fillCourageIcon = StartCoroutine(SmoothFillIcon(courageIcon, courageFillAmount));
            }
        }

        internal void UpdateGemsFillAmount()
        {
            UpdateCourageIconFillAmount();
            UpdateLuckyIconFillAmount();
            UpdateSuperstitionIconFillAmount();
        }

        public void UpdateSuperstitionIconFillAmount()
        {
            superstitionToFloat = (float)PlayerManager.Singleton.superstition / 100f;
            superstitionFillAmount = superstitionToFloat;
        }
        public void UpdateCourageIconFillAmount()
        {
            courageToFloat = (float)PlayerManager.Singleton.courage / 100f;
            courageFillAmount = courageToFloat;
        }
        public void UpdateLuckyIconFillAmount()
        {
            luckyToFloat = (float)PlayerManager.Singleton.lucky / 100f;
            luckyFillAmount = luckyToFloat;
        }

        float offset = 0.01f; // Permette di uscire dal ciclo una volta vicino al target

        private IEnumerator SmoothFillIcon(Image icon, float targetFill)
        {
            while (!InRange(icon.fillAmount, targetFill - offset, targetFill + offset))
            {
                float direction = icon.fillAmount < targetFill ? 1 : -1;

                float result = direction  * Time.deltaTime;

                icon.fillAmount = Mathf.Clamp(icon.fillAmount + result, 0f, 1f);

                Debug.Log($"Icon {icon.name} - FillAmount attuale: {icon.fillAmount}");

                yield return null; // Aspetta un frame
            }

            icon.fillAmount = targetFill;
        }



        bool InRange(float myAmount, float minTarget, float maxTarget)
        {
            return (myAmount - minTarget) * (myAmount - maxTarget) <= 0;
        }

        public void SetStarterFillAmount(float superstition, float courage, float lucky)
        {
            luckyIcon.fillAmount = lucky;
            superstitionIcon.fillAmount = superstition;
            courageIcon.fillAmount = courage;
        }
    }

}
