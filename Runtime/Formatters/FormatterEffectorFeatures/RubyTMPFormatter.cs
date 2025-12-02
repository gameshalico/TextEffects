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
        [SerializeField] private bool _fixedLineHeight = false;
        [SerializeField] [Range(1f, 3f)] private float _lineHeightMultiplier = 1.5f;
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

        public bool FixedLineHeight
        {
            get => _fixedLineHeight;
            set
            {
                _fixedLineHeight = value;
                SetDirty();
            }
        }

        public float LineHeightMultiplier
        {
            get => _lineHeightMultiplier;
            set
            {
                _lineHeightMultiplier = Mathf.Max(1f, value);
                SetDirty();
            }
        }

        private void OnValidate()
        {
            _rubyScale = Mathf.Clamp01(_rubyScale);
            _lineHeightMultiplier = Mathf.Max(1f, _lineHeightMultiplier);
            SetDirty();
        }

        public override string FormatText(string input)
        {
            if (_textComponent == null)
                _textComponent = GetComponent<TMP_Text>();

            var formatted = RubyTextHelper.FormatRubyText(input, _rubyScale, _rubyVerticalOffset,
                text => _textComponent.GetPreferredValues(text), _rubyPrefixTag, _rubySuffixTag);

            // 固定行の高さを適用
            if (_fixedLineHeight)
            {
                formatted = $"<line-height={_lineHeightMultiplier * 100}%>{formatted}</line-height>";
            }

            return formatted;
        }
    }
}
