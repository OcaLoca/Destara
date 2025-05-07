using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class OpenStoryPanel : MonoBehaviour
    {
        public GameObject StoryPanel;
    
       public void OpenPanel()
        {
            if (StoryPanel != null)
            {
                bool isActive = StoryPanel.activeSelf;
                StoryPanel.SetActive(!isActive);
            }
        }
       
    }
}