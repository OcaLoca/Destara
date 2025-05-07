using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenorySlot : MonoBehaviour, IDropHandler
{
    public ScriptableItem inventoryFillItem;

    public ScriptableItem item;
    public Image image;

    public void OnDrop(PointerEventData eventData)
    {
        int position = transform.GetSiblingIndex();
    }

    public void Init(ScriptableItem item)
    {
        this.item = item; 
        Color tmpColor = Color.white;
        tmpColor.a = 1f;
        image.color = tmpColor;
        image.sprite = item.icon;
        //image.rectTransform.sizeDelta = new Vector2(item.xDimension * 100, item.yDimension * 100);
    }

    public void SetBusy()
    {
        item = inventoryFillItem;
    }
}
