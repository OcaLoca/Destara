using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MuseumButtonManager : MonoBehaviour
    {
        [SerializeField] Image Image;
        [SerializeField] Button button;
        [SerializeField] Image lily;

        public Image GetLily { get => lily; }

        private void Awake()
        {
            button = this.GetComponent<Button>();
        }
       
        public void SetData(Sprite img)
        {
            Image.sprite = img;
        }

    }
}
