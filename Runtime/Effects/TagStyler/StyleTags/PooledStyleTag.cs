using System.Collections.Generic;
using TextEffects.Common;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public abstract class PooledStyleTag<T> : PooledItem<T>, IStyleTag where T : PooledStyleTag<T>, new()
    {
        protected TagInfo TagInfo { get; private set; }

        public virtual void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
        }

        public virtual void UpdateText(AnimationTextInfo animationInfo)
        {
        }

        public void Release()
        {
            OnRelease();
            Return(this as T);
        }

        private void SetTag(TagInfo tagInfo)
        {
            TagInfo = tagInfo;
            OnSetTag(tagInfo);
        }

        protected virtual void OnSetTag(TagInfo tagInfo)
        {
        }

        protected virtual void OnRelease()
        {
        }

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