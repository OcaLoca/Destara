using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CreateAssetMenu(fileName = "Image", menuName = "ScriptableObjects/ScriptableImage", order = 0)]

    public class ScriptableImage : ScriptableObject
    {
        public Texture imageToRender;
        public ScriptablePage.Section chapterSection;

        public string imageID;
        [Tooltip("Posizione di x")]
        public float xPosition;
        [Tooltip("Posizione di y")]
        public float yPosition;
        [Tooltip("Valore di x")]
        public float xValue;
        [Tooltip("Valore di y")]
        public float yValue;

        [Tooltip("Scala su x")]
        public float changex;
        [Tooltip("Scala su y")]
        public float changey;
        [Tooltip("Scala su z")]
        public float changeDepth;

        public float alpha;

        public bool isAllPage;

        public float GetXPosition()
        {
            return xPosition;
        }
        public float GetYPosition()
        {
            return yPosition;
        }
        public float GetWidht()
        {
            return changey;
        }

        public float GetHeigh()
        {
            return changex;
        }

        public float GetDepth()
        {
            return changeDepth;
        }


        public string GetImageID()
        {
            return imageID;
        }
    }
}