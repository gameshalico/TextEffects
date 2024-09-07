using TextEffects.Effects.Typewriter;
using UnityEditor;
using UnityEngine;

namespace TextEffects.Editor
{
    [CustomEditor(typeof(TextTypewriter))]
    public class TextTypewriterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var textTypewriter = (TextTypewriter)target;

            var autoPlay = serializedObject.FindProperty("_autoPlay");
            var defaultDelay = serializedObject.FindProperty("_defaultDelay");

            EditorGUILayout.PropertyField(autoPlay);
            EditorGUILayout.PropertyField(defaultDelay);

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Play"))
                    textTypewriter.PlayScript();
                if (GUILayout.Button("Reset"))
                    textTypewriter.ResetScript();
            }
        }
    }
}