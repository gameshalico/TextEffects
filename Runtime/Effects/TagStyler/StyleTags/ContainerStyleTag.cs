using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public abstract class ContainerStyleTag<T> : PooledStyleTag<T> where T : ContainerStyleTag<T>, new()
    {
        public override void UpdateText(AnimationTextInfo animationInfo)
        {
            OnUpdateText(animationInfo);
            for (var i = TagInfo.StartIndex; i < TagInfo.EndIndex; i++)
            {
                ref var characterInfo = ref animationInfo.TextInfo.characterInfo[i];
                ref var characterAnimationInfo = ref animationInfo.AnimationCharacterInfo[i];
                if (!characterAnimationInfo.IsInitialized)
                    continue;

                UpdateCharacterInTag(ref characterInfo, ref characterAnimationInfo);
            }
        }

        protected virtual void OnUpdateText(
            AnimationTextInfo animationInfo)
        {
        }

        protected virtual void UpdateCharacterInTag(
            ref TMP_CharacterInfo characterInfo,
            ref AnimationCharacterInfo animationInfo)
        {
        }
    }
}