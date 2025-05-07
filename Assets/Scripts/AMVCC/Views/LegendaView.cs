using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class LegendaView : MonoBehaviour
{

    public GameObject panel;
    public ItemLegendaView itemPrefab;
    public Transform itemsParent;

    public List<ItemLegendaView> list = new List<ItemLegendaView>();

    private void OnEnable()
    {
        CreateRarityList();
    }

    public void ShowHidePanel(bool active)
    {
        panel.SetActive(active);
    }

    public void CreateItem(Sprite icon, Color color, string text)
    {

        ItemLegendaView item = Instantiate(itemPrefab, itemsParent);
        item.SetData(icon, color, text);


        list.Add(item);
    }

    public void ClearItems()
    {

        foreach (ItemLegendaView item in list)
        {
            Destroy(item.gameObject);
        }

        list.Clear();
    }

    public void CreateRarityList()
    {

        List<Color> colorsList = new List<Color>();

        Color commonColor = ColorDatabase.Singleton.commonColor;
        Color uncommonColor = ColorDatabase.Singleton.uncommonColor;
        Color rareColor = ColorDatabase.Singleton.rareColor;
        Color epicColor = ColorDatabase.Singleton.superRareColor;
        Color legendaryColor = ColorDatabase.Singleton.legendaryColor;

        colorsList.Add(commonColor);
        colorsList.Add(uncommonColor);
        colorsList.Add(rareColor);
        colorsList.Add(epicColor);
        colorsList.Add(legendaryColor);

        List<string> raritiesTxt = new List<string>
        {
            "Common",
            "Uncommon",
            "Rare",
            "Epic",
            "Legendary"
        };

        for (int i = 0; i < colorsList.Count; i++)
        {
            CreateItem(null, colorsList[i], raritiesTxt[i]);
        }
    }

}
