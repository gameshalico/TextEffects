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
                ref var characterAnimationInfo = ref animationInfo.AnimationCharacterInfo[i];
                if (!characterAnimationInfo.IsInitialized)
                    continue;

                UpdateCharacterInTag(ref characterAnimationInfo);
            }
        }

        protected virtual void OnUpdateText(
            AnimationTextInfo animationInfo)
        {
        }

        protected virtual void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo)
        {
        }
    }
}