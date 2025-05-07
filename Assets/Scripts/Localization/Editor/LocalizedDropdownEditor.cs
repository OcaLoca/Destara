using UnityEditor;
using UnityEditor.UI;

namespace StarworkGC.Localization
{
    ///<summary>
    /// Custom inspector for LocalizedDropdown
    ///</summary>
    [CustomEditor(typeof(LocalizedDropdown))]
    public class LocalizedDropdownEditor : DropdownEditor
    {
        SerializedProperty prefix;

        protected override void OnEnable()
        {
            base.OnEnable();
            prefix = serializedObject.FindProperty("prefix");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(prefix);
            serializedObject.ApplyModifiedProperties();
        }
    }
}