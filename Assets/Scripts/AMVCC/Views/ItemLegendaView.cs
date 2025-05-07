using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarworkGC.Localization;

public class ItemLegendaView : MonoBehaviour
{
    public Image iconView;
    public TMP_Text textView;

    public void SetData(Sprite icon,Color color, string text){
        
        if(icon != null){

            this.iconView.sprite = icon;
        }

        if(color == null){
            color = Color.white;
        }
        else{
            this.iconView.color = color;
        }

        if(text != null){
            this.textView.text = Localization.Get(text);
        }

        gameObject.SetActive(true);
    }
}
