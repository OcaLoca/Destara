/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using StarworkGC.Localization;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class AspectRatioController : MonoBehaviour
    {
        const float ASPECT_RATIO_MAX_LIMIT = 0.6f; //corrispende a 9/15 = 0,6
        float currentAspectRatio;
        [SerializeField] TMP_Text txtAspectRatioToLow;
        [SerializeField] GameObject panelAspectRatioToLow;

        Coroutine myCoroutine;

        private void OnDisable()
        {
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
        }

        private void Start()
        {
            myCoroutine = null;
            currentAspectRatio = (float)Screen.width / Screen.height;

            //if (currentAspectRatio >= ASPECT_RATIO_MAX_LIMIT)
            //{
            //    ShowErrorMessage();
            //    return;
            //}

            myCoroutine = StartCoroutine(LoadGame());
        }

        void ShowErrorMessage()
        {
            panelAspectRatioToLow.SetActive(true);
            string error = string.Format(Localization.Get("aspectRatioBlock"), currentAspectRatio.ToString());
            txtAspectRatioToLow.text = error;

            myCoroutine = StartCoroutine(QuitApplication(4));
        }

        IEnumerator QuitApplication(float duration)
        {
            yield return new WaitForSeconds(duration);
            Application.Quit();
        }

        IEnumerator LoadGame()
        {
            yield return new WaitUntil(LogoIntroAnimationIsFinish.IntroAnimationFinish);
            SceneManager.LoadScene("GameScene");
        }
    }

}
