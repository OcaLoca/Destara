using System.IO;
using UnityEditor;
using UnityEngine;

public class RenameTool : EditorWindow
{
    private string searchTerm = "OldName";
    private string replaceTerm;
    [MenuItem("Tools/Rename Assets Tool")]
    public static void ShowWindow()
    {
        GetWindow<RenameTool>("Rename Assets");
    }
    private void OnGUI()
    {
        GUILayout.Label("Batch Rename Tool", EditorStyles.boldLabel);
        if (GUILayout.Button("Rename Files and GameObjects"))
        {
            // Genera un nuovo nome per ogni elemento
            GenerateRandomName();
            RenameScripts();
            RenamePrefabs();
            RenameGameObjectsInScenes();
            Debug.Log("Rinominato tutto dove possibile.");
        }
    }
    private void GenerateRandomName()
    {
        // Funzione per generare un nome casuale
        var randomString = Path.GetFileNameWithoutExtension(System.Guid.NewGuid().ToString()).Substring(0, 8);
        replaceTerm = "New" + randomString;  // Prepend con "New" per evitare che il nome sia ambiguo
    }
    private void RenameScripts()
    {
        string[] scriptPaths = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        foreach (string path in scriptPaths)
        {
            if (Path.GetFileName(path).Contains(searchTerm))
            {
                string newPath = path.Replace(searchTerm, replaceTerm);
                File.Move(path, newPath);
            }
        }
        AssetDatabase.Refresh();
    }
    private void RenamePrefabs()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains(searchTerm))
            {
                string newPath = path.Replace(searchTerm, replaceTerm);
                AssetDatabase.RenameAsset(path, Path.GetFileNameWithoutExtension(newPath));
            }
        }
        AssetDatabase.SaveAssets();
    }
    private void RenameGameObjectsInScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
            if (sceneAsset != null)
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scene.path);
                GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    RenameInHierarchy(obj);
                }
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            }
        }
    }
    private void RenameInHierarchy(GameObject obj)
    {
        if (obj.name.Contains(searchTerm))
        {
            obj.name = obj.name.Replace(searchTerm, replaceTerm);
        }
        foreach (Transform child in obj.transform)
        {
            RenameInHierarchy(child.gameObject);
        }
    }
}