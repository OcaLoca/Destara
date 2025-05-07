using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColorFromColorDatabase : MonoBehaviour
{
    [SerializeField] bool takeRarityColor;
    enum ColorData
    {
        DefaultIcon,
        Courage,
        Lucky,
        Superstition,
        hp,
        level,
        damageColor,
        genericWeaponColor,
        genericDefenceColor,
        genericAccesoriesColor,
        genericAbilityColor,
        White
    }
    [SerializeField] ColorData colorData;

    public ScriptableItem.Rarity rarity;

    private void Start()
    {
        Image image = gameObject.GetComponent<Image>();
        if(takeRarityColor)
        {
            image.color = ColorDatabase.Singleton.GetRarityColor(rarity);
            return;
        }
        image.color = ColorDatabase.Singleton.GetColorByName(colorData.ToString());
    }
}
