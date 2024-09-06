using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Formatters
{
    [AddComponentMenu("")]
    [AddEffectorFeatureMenu("Formatter/Scriptable Object Formatter")]
    public class ScriptableObjectFormatter : FormatterEffectorFeature
    {
        [SerializeField] private int _order;
        [SerializeField] private ScriptableFormatter _formatter;
        private ScriptableFormatter _previousFormatter;

        public override int FormatOrder => _order;

        public ScriptableFormatter Formatter
        {
            get => _formatter;
            set => ChangeFormatter(value);
        }

        private void Start()
        {
            if (_formatter)
            {
                _formatter.OnTextChanged += SetDirty;
                SetDirty();
            }
        }

        private void OnValidate()
        {
            ChangeFormatter(_formatter);
        }

        private void ChangeFormatter(ScriptableFormatter newFormatter)
        {
            if (_formatter == newFormatter)
                return;

            if (_previousFormatter)
                _previousFormatter.OnTextChanged -= SetDirty;

            _previousFormatter = _formatter;
            if (newFormatter)
            {
                _formatter = newFormatter;
                _formatter.OnTextChanged += SetDirty;
            }

            SetDirty();
        }

        public override string FormatText(string text)
        {
            if (!_formatter)
                return text;
            return _formatter.FormatText(text);
        }
    }
}