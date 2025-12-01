using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Effects.TagStyler
{
    [AddComponentMenu("Text Effects/Text Tag Styler")]
    [AddEffectorFeatureMenu("Effects/Tag Styler")]
    [DisallowMultipleComponent]
    public class TextTagStyler : TextEffectorFeature
    {
        private TextStyleEffect _textStyleEffect;
        private StyleTagFactoryMap _styleTagFactoryMap;

        protected override void AddFeature(TextEffector textEffector)
        {
            InitializeIfNeeded();
            textEffector.AddEffect(_textStyleEffect);
        }

        protected override void RemoveFeature(TextEffector textEffector)
        {
            textEffector.RemoveEffect(_textStyleEffect);
        }

        public void RegisterStyleTag(string tagName, IStyleTagFactory styleTagFactory)
        {
            InitializeIfNeeded();
            if (_styleTagFactoryMap == null)
            {
                _styleTagFactoryMap = StyleTagFactoryMap.Default.Clone();
                _textStyleEffect.StyleTagFactory = _styleTagFactoryMap;
            }
            _styleTagFactoryMap.RegisterFactory(tagName, styleTagFactory);
            SetDirty();
        }
        public void UnregisterStyleTag(string tagName)
        {
            if (_styleTagFactoryMap == null)
                return;

            _styleTagFactoryMap.UnregisterFactory(tagName);
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