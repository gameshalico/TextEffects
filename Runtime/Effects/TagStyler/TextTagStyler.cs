using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Effects.TagStyler
{
    [RequireComponent(typeof(TextEffector))]
    [ExecuteAlways]
    public class TextTagStyler : MonoBehaviour
    {
        private TextEffector _textEffector;
        private TextStyleEffect _textStyleEffect;

        private void OnEnable()
        {
            InitializeIfNeeded();
            _textEffector.AddEffect(_textStyleEffect);
        }

        private void OnDisable()
        {
            _textEffector.RemoveEffect(_textStyleEffect);
        }

        public void SetStyleTagFactory(IStyleTagFactory styleTagFactory)
        {
            InitializeIfNeeded();
            _textStyleEffect.StyleTagFactory = styleTagFactory;
            _textEffector.SetDirty();
        }

        private void InitializeIfNeeded()
        {
            if (_textStyleEffect != null)
                return;

            _textEffector = GetComponent<TextEffector>();
            _textStyleEffect = new TextStyleEffect(StyleTagFactoryMap.Default);
        }
    }
}