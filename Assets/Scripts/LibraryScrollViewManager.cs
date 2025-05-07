using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LibraryScrollViewManager : MonoBehaviour
{

    ScrollRect scrollRect;
    [SerializeField] RectTransform content;
    [SerializeField] GameObject selectedGameobject;
    [SerializeField] RawImage[] pallini;



    void LateUpdate()
    {
        //Debug.Log(content.anchoredPosition.x);
        if (content.anchoredPosition.x >= 100f && content.anchoredPosition.x <= -100f)
        {
            Debug.Log("tutroial");
        }
        else if (content.anchoredPosition.x >= -670 && content.anchoredPosition.x <= -450f)
        {
            Debug.Log("libro 1");
        }
    }

    void CheckItem()
    {

        Debug.Log(selectedGameobject.name);
    }
   
}
