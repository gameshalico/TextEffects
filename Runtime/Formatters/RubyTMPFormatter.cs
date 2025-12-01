using TextEffects.Core;
using TMPro;
using UnityEngine;

namespace TextEffects.Formatters
{
    [AddComponentMenu("Text Effects/Ruby TMP Formatter")]
    [AddEffectorFeatureMenu("Formatters/Ruby TMP Formatter")]
    [RequireComponent(typeof(TMP_Text))]
    public sealed class RubyTMPFormatter : FormatterEffectorFeature
    {
        [SerializeField] private int _order;
        [SerializeField] [Range(0f, 1f)] private float _rubyScale = 0.5f;
        [SerializeField] private float _rubyVerticalOffset = 1f;
        [SerializeField] private string _rubyPrefixTag = "";
        [SerializeField] private string _rubySuffixTag = "";
        private TMP_Text _textComponent;

        private void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        public override int FormatOrder => _order;

        public float RubyScale
        {
            get => _rubyScale;
            set
            {
                _rubyScale = Mathf.Clamp01(value);
                SetDirty();
            }
        }

        public float RubyVerticalOffset
        {
            get => _rubyVerticalOffset;
            set
            {
                _rubyVerticalOffset = value;
                SetDirty();
            }
        }

        public string RubyPrefixTag
        {
            get => _rubyPrefixTag;
            set
            {
                _rubyPrefixTag = value;
                SetDirty();
            }
        }

        public string RubySuffixTag
        {
            get => _rubySuffixTag;
            set
            {
                _rubySuffixTag = value;
                SetDirty();
            }
        }

        private void OnValidate()
        {
            _rubyScale = Mathf.Clamp01(_rubyScale);
            SetDirty();
        }

        public override string FormatText(string input)
        {
            if (_textComponent == null)
                _textComponent = GetComponent<TMP_Text>();

            return RubyTextHelper.FormatRubyText(input, _rubyScale, _rubyVerticalOffset,
                text => _textComponent.GetPreferredValues(text), _rubyPrefixTag, _rubySuffixTag);
        }
    }
}
