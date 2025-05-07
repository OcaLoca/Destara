using UnityEngine;
namespace Game
{
    public class HideObjFromScript : MonoBehaviour
    {
        public void HideObj()
        {
            gameObject.SetActive(false);
        }
    }
}

