using TextEffects.Core;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace TextEffects.Editor
{
    [CustomEditor(typeof(TextEffector))]
    public class TextEffectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var textEffector = (TextEffector)target;

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if (GUILayout.Button("Add Feature"))
            {
                var advancedDropdown = new EffectorFeatureAdvancedDropdown(new AdvancedDropdownState());
                advancedDropdown.OnSelectItem += type => { textEffector.gameObject.AddComponent(type); };

                advancedDropdown.Show(new Rect(Event.current.mousePosition, Vector2.zero));
            }
        }
    }
}