using TextEffects.Data;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public abstract class ContainerDisplayTag<T> : PooledDisplayTag<T> where T : ContainerDisplayTag<T>, new()
    {
        public sealed override void UpdateText(TextAnimationInfo textAnimationInfo, ScriptInfo scriptInfo)
        {
            OnUpdateText(textAnimationInfo, scriptInfo);
            for (var i = TagInfo.StartIndex; i < TagInfo.EndIndex; i++)
            {
                var characterInfo = textAnimationInfo.TextInfo.characterInfo[i];
                ref var characterAnimationInfo = ref textAnimationInfo.CharacterAnimationInfo[i];
                if (!characterAnimationInfo.IsInitialized)
                    continue;
                ref var scriptCharacterInfo = ref scriptInfo.ScriptCharacterInfo[i];

                UpdateCharacterInTag(ref characterAnimationInfo, ref scriptCharacterInfo);
            }
        }

        protected virtual void OnUpdateText(
            TextAnimationInfo textAnimationInfo,
            ScriptInfo scriptInfo
        )
        {
        }

        protected abstract void UpdateCharacterInTag(
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo
        );
    }
}