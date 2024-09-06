using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public abstract class ContainerStyleTag<T> : PooledStyleTag<T> where T : ContainerStyleTag<T>, new()
    {
        public override void UpdateText(TextAnimationInfo textAnimationInfo)
        {
            OnUpdateText(textAnimationInfo);
            for (var i = TagInfo.StartIndex; i < TagInfo.EndIndex; i++)
            {
                ref var characterInfo = ref textAnimationInfo.TextInfo.characterInfo[i];
                ref var characterAnimationInfo = ref textAnimationInfo.CharacterAnimationInfo[i];
                if (!characterAnimationInfo.IsInitialized)
                    continue;

                UpdateCharacterInTag(ref characterInfo, ref characterAnimationInfo);
            }
        }

        protected virtual void OnUpdateText(
            TextAnimationInfo textAnimationInfo)
        {
        }

        protected virtual void UpdateCharacterInTag(
            ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo)
        {
        }
    }
}