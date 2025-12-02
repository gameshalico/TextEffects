using System.Collections.Generic;
using System.Linq;
using TextEffects.Core;
using TextEffects.Data;

namespace TextEffects.Effects.TagStyler
{
    public sealed class TextStyleEffect : ITextAnimationEffect
    {
        private IStyleTag[] _styleTags;

        public TextStyleEffect(IStyleTagFactory styleTagFactory)
        {
            StyleTagFactory = styleTagFactory;
        }

        public IStyleTagFactory StyleTagFactory { get; set; }

        public void Setup(TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
            if (StyleTagFactory == null)
            {
                return;
            }

            _styleTags = tags
                .Select(StyleTagFactory.CreateTag)
                .Where(static tag => tag != null)
                .ToArray();
        }

        public void UpdateText(AnimationTextInfo animationInfo)
        {
            if (_styleTags == null)
                return;

            foreach (var tag in _styleTags)
                tag.UpdateText(animationInfo);
        }

        public void Release()
        {
            if (_styleTags == null)
                return;
            foreach (var tag in _styleTags)
                tag.Release();

            _styleTags = null;
        }
    }
}