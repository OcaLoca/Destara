using Game;
using UnityEngine;

public class SearchAnimationIsFinish : MonoBehaviour
{
    public static bool searchAnimIsFinish = false;
    
    public void SetSearchAnimationIsFinish()
    {
        searchAnimIsFinish = true;
    }

    public static bool GetSearchAnimationIsFinish()
    {
        return searchAnimIsFinish;
    }
}
