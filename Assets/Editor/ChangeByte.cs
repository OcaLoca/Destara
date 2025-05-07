using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class ChangeByte : EditorWindow
{
    [MenuItem("Tools/Cambia gli Hash degli Asset")]
    public static void ShowWindow()
    {
       GetWindow<ChangeByte>("Cambia Hash");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Cambia Hash per Audio e Immagini"))
        {
            ChangeHashes();
        }
    }

    private static void ChangeHashes()
    {
        // Ottieni tutti i file nel progetto
        string[] filePaths = Directory.GetFiles("Assets", "*", SearchOption.AllDirectories);

        foreach (string filePath in filePaths)
        {
            // Escludere file che non sono audio o immagini
            if (Directory.Exists(filePath) || !IsAudioOrImageFile(filePath))
            {
                continue;
            }

            // Modifica il contenuto per cambiare l'hash
            ModifyFileContent(filePath);

            // Ricarica il file nell'editor di Unity per aggiornare i riferimenti
            AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);
            Debug.Log($"Hash cambiato per il file: {filePath}");
        }
    }

    private static bool IsAudioOrImageFile(string filePath)
    {
        // Verifica se il file è un audio o un'immagine
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".wav" || extension == ".mp3" || extension == ".png" || extension == ".jpg" || extension == ".jpeg";
    }

    private static void ModifyFileContent(string filePath)
    {
        byte[] fileBytes = File.ReadAllBytes(filePath);

        // Aggiungi un byte casuale alla fine del file per cambiare l'hash
        byte[] newFileBytes = new byte[fileBytes.Length + 1];
        System.Array.Copy(fileBytes, newFileBytes, fileBytes.Length);

        // Aggiungi un byte casuale
        newFileBytes[fileBytes.Length] = (byte)Random.Range(0, 256);

        // Scrivi il file modificato di nuovo sul disco
        File.WriteAllBytes(filePath, newFileBytes);
    }
}


