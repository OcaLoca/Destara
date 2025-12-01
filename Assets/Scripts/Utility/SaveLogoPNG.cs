using UnityEngine;
using System.IO;

public class SaveLogoPNG : MonoBehaviour
{
    // Riferimento alla Camera che inquadra solo il logo
    public Camera targetCamera;

    // Risoluzione desiderata per il logo (es. 512)
    public int resolution = 512;

    // Tasto da premere per salvare lo screenshot
    public KeyCode captureKey = KeyCode.P;

    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            CaptureLogo();
        }
    }

    void CaptureLogo()
    {
        if (targetCamera == null)
        {
            Debug.LogError("Assegna la telecamera target nello script!");
            return;
        }

        // 1. Crea la RenderTexture (la tela temporanea)
        RenderTexture rt = new RenderTexture(resolution, resolution, 24);
        targetCamera.targetTexture = rt;

        // 2. Renderizza il contenuto della Camera sulla RenderTexture
        targetCamera.Render();

        // 3. Leggi i pixel e crea la Texture2D finale
        RenderTexture.active = rt;
        Texture2D texture2D = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        texture2D.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        texture2D.Apply();

        // 4. Ripulisci
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 5. Salva come PNG
        byte[] bytes = texture2D.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, "Logo_512x512_" + System.DateTime.Now.ToString("HHmmss") + ".png");
        File.WriteAllBytes(path, bytes);

        Debug.Log("Logo salvato in: " + path);
    }
}
