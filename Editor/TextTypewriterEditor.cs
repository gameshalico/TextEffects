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
            base.OnInspectorGUI();
            var textTypewriter = (TextTypewriter)target;

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