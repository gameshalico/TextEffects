using System;
using UnityEngine;

namespace TextEffects.Formatters.ScriptableFormatters
{
    [CreateAssetMenu(fileName = "Text Enclosing Formatter",
        menuName = "Text Effects/Formatters/Text Enclosing Formatter")]
    public class TextEnclosingScriptableFormatter : ScriptableFormatter
    {
        [SerializeField] private int _order;

        [SerializeField] [TextArea] private string _preText;
        [SerializeField] [TextArea] private string _postText;

        public override int FormatOrder => _order;

        private void OnValidate()
        {
            OnTextChanged?.Invoke();
        }

        public override string FormatText(string text)
        {
            return _preText + text + _postText;
        }

        public override event Action OnTextChanged;
    }
}