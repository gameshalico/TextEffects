using System.Collections.Generic;
using System.Linq;
using TextEffects.Core;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler
{
    public class TextStyleEffect : ITextAnimationEffect
    {
        private IStyleTag[] _styleTags;

        public TextStyleEffect(IStyleTagFactory styleTagFactory)
        {
            StyleTagFactory = styleTagFactory;
        }

        public IStyleTagFactory StyleTagFactory { get; set; }

        public void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
            if (StyleTagFactory == null)
                return;

            _styleTags = tags
                .Select(StyleTagFactory.CreateTag)
                .Where(static tag => tag != null)
                .ToArray();

            foreach (var tag in _styleTags)
                tag.Setup(textInfo, tags);
        }

        public void UpdateText(TextAnimationInfo textAnimationInfo)
        {
            if (_styleTags == null)
                return;

            foreach (var tag in _styleTags)
                tag.UpdateText(textAnimationInfo);
        }

        public void Release()
        {
            if (_styleTags == null)
                return;
            foreach (var tag in _styleTags)
                tag.Release();

            _styleTags = null;
        }

        public void UpdateText(TMP_TextInfo textInfo, CharacterAnimationInfo[] animationInfo)
        {
        }
    }
}