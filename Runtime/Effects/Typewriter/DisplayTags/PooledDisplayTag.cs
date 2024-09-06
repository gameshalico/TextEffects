using System.Collections.Generic;
using TextEffects.Common;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public abstract class PooledDisplayTag<T> : PooledItem<T>, IDisplayTag where T : PooledDisplayTag<T>, new()
    {
        public virtual void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
        }

        public virtual void UpdateCharacter(
            ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo
        )
        {
        }

        public virtual void Release()
        {
        }

        public virtual void SetTag(TagInfo tagInfo)
        {
        }

        public class Factory : IDisplayTagFactory
        {
            public IDisplayTag CreateTag(TagInfo tagInfo)
            {
                var tag = Rent();
                tag.SetTag(tagInfo);
                return tag;
            }
        }
    }
}