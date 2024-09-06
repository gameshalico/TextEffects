using System.Collections.Generic;
using TextEffects.Common;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public abstract class PooledDisplayTag<T> : PooledItem<T>, IDisplayTag where T : PooledDisplayTag<T>, new()
    {
        protected TagInfo TagInfo { get; private set; }

        public virtual void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
        }

        public virtual void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
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