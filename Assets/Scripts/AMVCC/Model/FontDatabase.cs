using TMPro;
using UnityEngine;

public class FontDatabase : MonoBehaviour
{
    public static FontDatabase Singleton { get; private set; }

    [SerializeField] private TMP_FontAsset[] fonts, buttonFonts;

    private void Awake()
    {
        Singleton = this;
    }

    public TMP_FontAsset GetFontByIndex(int index)
    {
        if (fonts == null || fonts.Length == 0)
        {
            Debug.LogWarning("L'array di font è vuoto o non impostato.");
            return null;
        }

        // Clamping dell'indice per evitare errori di out-of-bounds
        int clampedIndex = Mathf.Clamp(index, 0, fonts.Length - 1);
        return fonts[clampedIndex];
    }

    public TMP_FontAsset GetButtonFontByIndex(int index)
    {
        if (buttonFonts == null || buttonFonts.Length == 0)
        {
            Debug.LogWarning("L'array di font del tasto è vuoto o non impostato.");
            return null;
        }

        // Clamping dell'indice per evitare errori di out-of-bounds
        int clampedIndex = Mathf.Clamp(index, 0, buttonFonts.Length - 1);
        return buttonFonts[clampedIndex];
    }
}
