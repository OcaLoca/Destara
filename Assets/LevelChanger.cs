using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{

    public class LevelChanger : MonoBehaviour
    {
        public static LevelChanger Singleton { get; set; }

        public Animator animator;
        private int levelToLoad;
        float currentTime = 1.5f;

        public GameObject CreditsPanel;
        public GameObject BackgroundImage;
        public GameObject Video;

        // Update is called once per frame
        void Update()
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }

            else
            {
                FadeToNextLevel();
            }

            if (Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name == "Intro")
            {
                LoadGame();
            }
            
        }

        public void FadeToNextLevel()
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void FadeToLevel(int levelIndex)
        {
            levelToLoad = levelIndex;
            animator.SetTrigger("FadeOut");
            
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void LoadGame()
        {
            Debug.Log("click salta video");
            SceneManager.LoadScene("MenuScene");
        }

        public void ActivateCredits()
        {
            BackgroundImage.SetActive(false);
            CreditsPanel.SetActive(true);
        }
       
        public void OnFadeComplete()
        {
            SceneManager.LoadScene(levelToLoad);
        }

        public void CheckScene()
        {
            if (SceneManager.GetActiveScene().name == "MobyBitScene")
            {
                animator.SetBool("Fade", true);
            }
            else
            {
                print("apri video");
                gameObject.SetActive(false);
                Video.SetActive(true);
            }
        }

    }


}