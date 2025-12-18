using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChoiceKeyboardController : MonoBehaviour
    {
        public static ChoiceKeyboardController Singleton { get; private set; }
        private List<ChoiceButton> choices = new List<ChoiceButton>();
        private void Awake()
        {
            Singleton = this;
        }

        public void RefreshChoices()
        {
            Debug.Log("Refreshing choices...");
            choices.Clear();
            choices.AddRange(FindObjectsByType<ChoiceButton>(sortMode: FindObjectsSortMode.None));

            choices.Sort((a, b) =>
                b.transform.position.y.CompareTo(a.transform.position.y));

            Debug.Log("Choices found: " + choices.Count);

            for (int i = 0; i < choices.Count; i++)
            {
                choices[i].keyboardIndex = i + 1;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("TASTO 1 PREMUTO");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("SPAZIO PREMUTO");
            }

            for (int i = 0; i < choices.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    choices[i].ClickFromKeyboard();
                    break;
                }
            }
        }
    }
}
