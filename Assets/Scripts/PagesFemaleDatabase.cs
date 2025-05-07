using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class PagesFemaleDatabase : MonoBehaviour
    {
        public static PagesFemaleDatabase Singleton;

        public List<ScriptablePage> chaptersDatabase;

        void Awake()
        {
            Singleton = this;
        }

        public ScriptablePage GetPageByID(string ID)
        {
            if (chaptersDatabase == null) { return null; }
            foreach (ScriptablePage chapter in chaptersDatabase)
            {
                if (chapter.pageID == ID) { return chapter; }
            }
            return null;
        }
    }
}

