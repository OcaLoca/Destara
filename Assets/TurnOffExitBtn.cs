using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffExitBtn : MonoBehaviour
{
    [SerializeField] GameObject btnExit;

     void OnEnable()
    {
        btnExit.gameObject.SetActive(false);
    }

    public void OnMouseDown()
    {
        btnExit.gameObject.SetActive(true);
    }
}
