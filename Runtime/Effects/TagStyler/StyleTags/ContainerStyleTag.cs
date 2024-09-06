using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public abstract class ContainerStyleTag<T> : PooledStyleTag<T> where T : ContainerStyleTag<T>, new()
    {
        protected int StartIndex { get; private set; }
        protected int EndIndex { get; private set; }

        public override void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
            OnSetup(textInfo, tags);
        }

        public override void UpdateCharacter(ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo)
        {
            if (animationInfo.CharacterIndex < StartIndex || animationInfo.CharacterIndex >= EndIndex) return;

            UpdateCharacterInTag(ref characterInfo, ref animationInfo);
        }


        public override void Release()
        {
            OnRelease();
            StartIndex = 0;
            EndIndex = 0;
            Return(this as T);
        }

        public override void SetTag(TagInfo tagInfo)
        {
            StartIndex = tagInfo.StartIndex;
            EndIndex = tagInfo.EndIndex;
            OnSetTag(tagInfo);
        }

        protected virtual void OnSetTag(TagInfo tagInfo)
        {
        }

        protected virtual void OnSetup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
        }

        protected virtual void OnRelease()
        {
        }

        protected abstract void UpdateCharacterInTag(ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo);
    }
}