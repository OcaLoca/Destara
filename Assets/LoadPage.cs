using Game;
using UnityEngine;

public class LoadPage : MonoBehaviour
{
    public void OnInputValueChanged(string pageID)
    {
        Debug.Log(pageID);

        foreach (ScriptablePage scriptableChapter in PagesMaleDatabase.Singleton.chaptersDatabase)
        {
            if (scriptableChapter.pageID == pageID)
            {
                GameApplication.Singleton.view.BookView.ShowPage(scriptableChapter);
            }
        }
    }
}
