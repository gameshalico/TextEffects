#if TEXTEFFECTS_UIELEMENTS_SUPPORT
using TextEffects.Core;
using TextEffects.Core.UIElements;
using TextEffects.Effects.TagStyler;
using TextEffects.Effects.Typewriter;
using TextEffects.Effects.Typewriter.Modifiers;
using UnityEngine.UIElements;

namespace TextEffects.UIElements
{
    [UxmlElement]
    public partial class EffectLabel : Label
    {
        private UIElementsTextEffector _effector;
        private TextStyleEffect _styleEffect;
        private TypewriterEffect _typewriterEffect;
        private string _effectText;
        private bool _isAttached;

        [UxmlAttribute]
        public bool EnableStyleEffect { get; set; } = true;

        [UxmlAttribute]
        public bool EnableTypewriter { get; set; } = true;

        [UxmlAttribute]
        public float TypewriterDelay { get; set; } = 0.05f;

        [UxmlAttribute]
        public string EffectText
        {
            get => _effectText;
            set
            {
                _effectText = value;
                if (_isAttached)
                {
                    _effector?.SetText(value);
                }
            }
        }

        public TypewriterEffect Typewriter => _typewriterEffect;
        public TextStyleEffect StyleEffect => _styleEffect;
        public UIElementsTextEffector Effector => _effector;

        public EffectLabel() : base()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public EffectLabel(string text) : base(text)
        {
            _effectText = text;
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            _isAttached = true;
            _effector = new UIElementsTextEffector(this);

            if (EnableStyleEffect)
            {
                _styleEffect = new TextStyleEffect(StyleTagFactoryMap.Default);
                _effector.AddEffect(_styleEffect);
            }

            if (EnableTypewriter)
            {
                _typewriterEffect = new TypewriterEffect(
                    DisplayTagFactoryMap.Default,
                    ScriptTagFactoryMap.Default,
                    keepDisplayOnRefresh: false
                );
                _typewriterEffect.AddModifier(new DefaultDelayScriptModifier(TypewriterDelay));
                _effector.AddEffect(_typewriterEffect);
            }

            if (!string.IsNullOrEmpty(_effectText))
            {
                _effector.SetText(_effectText);
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            _isAttached = false;
            _effector?.Dispose();
            _effector = null;
            _styleEffect = null;
            _typewriterEffect = null;
        }

        public void SetEffectText(string text)
        {
            EffectText = text;
        }

        public void AddEffect(ITextAnimationEffect effect)
        {
            _effector?.AddEffect(effect);
        }

        public void RemoveEffect(ITextAnimationEffect effect)
        {
            _effector?.RemoveEffect(effect);
        }

        public void AddFormatter(ITextFormatter formatter)
        {
            _effector?.AddFormatter(formatter);
        }

        public void RemoveFormatter(ITextFormatter formatter)
        {
            _effector?.RemoveFormatter(formatter);
        }
    }
}
#endif
