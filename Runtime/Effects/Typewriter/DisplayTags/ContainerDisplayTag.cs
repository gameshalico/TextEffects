using TextEffects.Data;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public abstract class ContainerDisplayTag<T> : PooledDisplayTag<T> where T : ContainerDisplayTag<T>, new()
    {
        public sealed override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            OnUpdateText(animationInfo, scriptInfo);
            for (var i = TagInfo.StartIndex; i < TagInfo.EndIndex; i++)
            {
                ref var characterAnimationInfo = ref animationInfo.AnimationCharacterInfo[i];
                if (!characterAnimationInfo.IsInitialized)
                    continue;
                ref var scriptCharacterInfo = ref scriptInfo.ScriptCharacterInfo[i];

                UpdateCharacterInTag(ref characterAnimationInfo, ref scriptCharacterInfo);
            }
        }

        protected virtual void OnUpdateText(
            AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo
        )
        {
        }

        protected virtual void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo, ref ScriptCharacterInfo scriptInfo
        )
        {
        }
    }
}