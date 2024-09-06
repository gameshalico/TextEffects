using System.Collections.Generic;
using TextEffects.Common;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public abstract class PooledStyleTag<T> : PooledItem<T>, IStyleTag where T : PooledStyleTag<T>, new()
    {
        public abstract void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags);

        public abstract void UpdateCharacter(
            ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo
        );

        public abstract void Release();
        public abstract void SetTag(TagInfo tagInfo);

        public class Factory : IStyleTagFactory
        {
            public IStyleTag CreateTag(TagInfo tagInfo)
            {
                var tag = Rent();
                tag.SetTag(tagInfo);
                return tag;
            }
        }
    }
}