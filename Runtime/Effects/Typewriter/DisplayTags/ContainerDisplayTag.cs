using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public abstract class ContainerDisplayTag<T> : PooledDisplayTag<T> where T : ContainerDisplayTag<T>, new()
    {
        protected int StartIndex { get; private set; }
        protected int EndIndex { get; private set; }

        public sealed override void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
            OnSetup(textInfo, tags);
        }

        public sealed override void UpdateCharacter(
            ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            if (animationInfo.CharacterIndex < StartIndex || animationInfo.CharacterIndex >= EndIndex) return;

            UpdateCharacterInTag(ref characterInfo, ref animationInfo, ref scriptInfo);
        }

        public sealed override void Release()
        {
            OnRelease();
            StartIndex = 0;
            EndIndex = 0;
            Return(this as T);
        }

        public sealed override void SetTag(TagInfo tagInfo)
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

        protected abstract void UpdateCharacterInTag(
            ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo
        );
    }
}