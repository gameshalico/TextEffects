using System.Text;
using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Formatters
{
    [AddComponentMenu("Text Effects/Enclosing Formatter")]
    [AddEffectorFeatureMenu("Formatters/Enclosing Formatter")]
    public class TextEnclosingFormatter : FormatterEffectorFeature
    {
        [SerializeField] private int _priority;
        [TextArea] [SerializeField] private string _preText;
        [TextArea] [SerializeField] private string _postText;

        public string PreText
        {
            get => _preText;
            set
            {
                _preText = value;
                SetDirty();
            }
        }

        public string PostText
        {
            get => _postText;
            set
            {
                _postText = value;
                SetDirty();
            }
        }

        public override int FormatOrder => _priority;

        private void OnValidate()
        {
            SetDirty();
        }

        public override string FormatText(string text)
        {
            var sb = new StringBuilder();

            sb.Append(_preText);
            sb.Append(text);
            sb.Append(_postText);

            return sb.ToString();
        }
    }
}