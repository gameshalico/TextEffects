using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Effects.TagStyler
{
    [AddComponentMenu("Text Effects/Text Tag Styler")]
    [AddEffectorFeatureMenu("Effects/Tag Styler")]
    public class TextTagStyler : TextEffectorFeature
    {
        private TextStyleEffect _textStyleEffect;

        protected override void AddFeature(TextEffector textEffector)
        {
            InitializeIfNeeded();
            textEffector.AddEffect(_textStyleEffect);
        }

        protected override void RemoveFeature(TextEffector textEffector)
        {
            textEffector.RemoveEffect(_textStyleEffect);
        }

        public void SetStyleTagFactory(IStyleTagFactory styleTagFactory)
        {
            InitializeIfNeeded();
            _textStyleEffect.StyleTagFactory = styleTagFactory;
            SetDirty();
        }

        private void InitializeIfNeeded()
        {
            if (_textStyleEffect != null)
                return;

            _textStyleEffect = new TextStyleEffect(StyleTagFactoryMap.Default);
        }
    }
}