using System;
using System.Collections.Generic;
using UnityEngine;


namespace AV
{
    [System.Serializable, CreateAssetMenu(fileName = "Colors Table", menuName = "Game/Colors Table")]
    public class ColorsTable : ScriptableObject
    {
        [Serializable]
        struct NamedColor
        {
            public string name;
            public Color color;
        }

        [SerializeField]
        NamedColor[] colors;

        public Color GetColorByName(string colorName)
        {
            foreach (NamedColor namedColor in colors)
            {
                if (namedColor.name.Equals(colorName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return namedColor.color;
                }
            }

            Debug.LogError(string.Format("Color {0} not found!", colorName));
            return Color.white;
        }
    }

}
