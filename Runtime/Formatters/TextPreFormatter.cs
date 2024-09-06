using System.Text;
using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Formatters
{
    [AddComponentMenu("")]
    [AddEffectorFeatureMenu("Formatter/Pre Formatter")]
    [RequireComponent(typeof(TextEffector))]
    [ExecuteAlways]
    public class TextPreFormatter : MonoBehaviour, ITextFormatter
    {
        [SerializeField] private TextFormatterBase _textFormatter;

        [TextArea] [SerializeField] private string _preText;
        [TextArea] [SerializeField] private string _postText;

        private TextEffector _textEffector;

        public string PreText
        {
            get => _preText;
            set
            {
                _preText = value;
                OnTextChanged();
            }
        }

        public string PostText
        {
            get => _postText;
            set
            {
                _postText = value;
                OnTextChanged();
            }
        }

        private void Start()
        {
            if (_textFormatter)
            {
                _textFormatter.OnTextChanged += OnTextChanged;
                OnTextChanged();
            }
        }

        private void OnEnable()
        {
            _textEffector = GetComponent<TextEffector>();
            _textEffector.AddFormatter(this);
        }

        private void OnDisable()
        {
            _textEffector.RemoveFormatter(this);
        }

        private void OnValidate()
        {
            OnTextChanged();
        }

        public string FormatText(string text)
        {
            var sb = new StringBuilder();

            sb.Append(_preText);
            sb.Append(text);
            sb.Append(_postText);

            if (_textFormatter == null)
                return sb.ToString();

            return _textFormatter.FormatText(sb.ToString());
        }

        private void OnTextChanged()
        {
            if (isActiveAndEnabled)
                _textEffector.SetDirty();
        }
    }
}