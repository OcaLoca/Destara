using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class Event : ScriptableObject
{
    [TextArea]
    public string finalText;
    public string text;

    private void OnEnable()
    {
       text = Localization.Get(finalText);
    }

}

