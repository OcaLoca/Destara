using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtility : MonoBehaviour
{
    public static void CleanLayout(Transform transform)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public static string GetNumberForUI(int number)
    {
        if(number < 10)
        {
            return string.Format("0{0}", number);
        }
        return number.ToString();
    }

    public static string GetLevelForUI(int number)
    {
        if (number < 10)
        {
            return "Lv." + string.Format("0{0}", number);
        }
        return "Lv."+ number.ToString();
    }

    public static void ResetCameraAndCanvas()
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black;
        Camera.main.Render();
        GL.Clear(true, true, Color.black);
    }
}
