using UnityEngine;

public class DropObjPanelIsActive : MonoBehaviour
{
    internal static bool dropObjPanelIsActive = false;
    public void SetDropObjPanelActive()
    {
        dropObjPanelIsActive = true;
    }

    public static bool GetDropPanelIsActive()
    {
        return dropObjPanelIsActive;
    }

    public void TurnOffDropPanel()
    { 
        gameObject.SetActive(false);    
    }

}
