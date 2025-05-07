using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LoadMainCharacter : MonoBehaviour
    {
        public RawImage mainCharacter;
        public RawImage secondCharacter;

        public List<ScriptableImage> imagesDatabase;

        void Awake()
        {
            foreach (ScriptableImage data in Resources.LoadAll<ScriptableImage>("MainFightCharacter"))
            {
                imagesDatabase.Add(data);
            }
        }

        public ScriptableImage LoadMainCharacterTexture()
        {
            foreach (ScriptableImage scriptableImage in imagesDatabase)
            {
                if (PlayerManager.Singleton.selectedClass.name == scriptableImage.imageID)
                {
                    return scriptableImage;
                }

            }
            return null;
        }


        public void LoadImageTexture()
        {
            ScriptableImage temporaryImage = LoadMainCharacterTexture();

            if ((temporaryImage == null) || (PlayerManager.Singleton.currentPage.pageID == "sceltaClasse"))
            {
                Debug.LogError("NoImage");
                mainCharacter.CrossFadeAlpha(0.0f, 0.0f, false);
            }

            else
            {
                mainCharacter.CrossFadeAlpha(5.0f, 0.0f, false);
                mainCharacter.texture = temporaryImage.imageToRender;
                mainCharacter.rectTransform.localScale = new Vector3(temporaryImage.GetHeigh(), temporaryImage.GetWidht(), temporaryImage.GetDepth());
                if (temporaryImage.isAllPage) { return; }
                //imageToShow.gameObject.transform.position = new Vector3(temporaryImage.GetXPosition(), temporaryImage.GetYPosition(), temporaryImage.GetZPosition());
            }
        }
    }
}
