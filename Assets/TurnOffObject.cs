using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffObject : MonoBehaviour
{
    [SerializeField] GameObject obj;
    public void OnMouseDown()
    {
        obj.SetActive(false);
    }
}
