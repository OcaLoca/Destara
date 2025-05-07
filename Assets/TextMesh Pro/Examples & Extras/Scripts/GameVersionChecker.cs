/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    
    public class GameVersionChecker : MonoBehaviour
    {
        [Header("Store URLs")]
        public string googlePlayUrl = "https://play.google.com/store/apps/details?id=com.example.yourapp"; // Replace with your app's Play Store URL
        public string appleStoreUrl = "https://apps.apple.com/us/app/your-app-name/id123456789"; // Replace with your app's App Store URL

        [Header("UI Elements")]
        public GameObject updatePrompt; // Reference to a UI panel or object to prompt the update
        public TMP_Text updateText; // Optional: Text to show messages

        private string currentVersion;

        void Start()
        {
            // Get the current version of the app
            currentVersion = Application.version;

            if (Application.platform == RuntimePlatform.Android)
            {
                StartCoroutine(CheckVersionOnGooglePlay());
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                StartCoroutine(CheckVersionOnAppleStore());
            }
        }

        IEnumerator CheckVersionOnGooglePlay()
        {
            UnityWebRequest request = UnityWebRequest.Get(googlePlayUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string pageContent = request.downloadHandler.text;

                // Extract version from Play Store page
                string versionTag = "Current Version";
                int index = pageContent.IndexOf(versionTag);
                if (index > 0)
                {
                    int startIndex = pageContent.IndexOf(">", index) + 1;
                    int endIndex = pageContent.IndexOf("<", startIndex);
                    string storeVersion = pageContent.Substring(startIndex, endIndex - startIndex).Trim();

                    CompareVersions(storeVersion);
                }
            }
            else
            {
                Debug.LogError("Failed to fetch Google Play version: " + request.error);
            }
        }

        IEnumerator CheckVersionOnAppleStore()
        {
            UnityWebRequest request = UnityWebRequest.Get(appleStoreUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string pageContent = request.downloadHandler.text;

                // Extract version from App Store page
                string versionTag = "\"version\":\"";
                int index = pageContent.IndexOf(versionTag);
                if (index > 0)
                {
                    int startIndex = index + versionTag.Length;
                    int endIndex = pageContent.IndexOf("\"", startIndex);
                    string storeVersion = pageContent.Substring(startIndex, endIndex - startIndex).Trim();

                    CompareVersions(storeVersion);
                }
            }
            else
            {
                Debug.LogError("Failed to fetch Apple Store version: " + request.error);
            }
        }

        void CompareVersions(string storeVersion)
        {
            if (string.Compare(currentVersion, storeVersion) < 0) // Compare versions
            {
                // Show update prompt
                updatePrompt.SetActive(true);
                if (updateText != null)
                {
                    updateText.text = "A new version is available. Please update to continue.";
                }
            }
            else
            {
                Debug.Log("App is up-to-date");
            }
        }

        public void OpenStore()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Application.OpenURL(googlePlayUrl);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.OpenURL(appleStoreUrl);
            }
        }

    }

}
