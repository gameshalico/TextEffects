using TextEffects.Data;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public class FadeShowTag : ContainerDisplayTag<FadeShowTag>
    {
        private float _duration;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
        }

        protected override void UpdateCharacterInTag(
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.Color = VertexColor.Lerp(
                animationInfo.Color.WithAlpha(0),
                animationInfo.Color,
                scriptInfo.ShowProgress(_duration)
            );
        }
    }

    public class FadeHideTag : ContainerDisplayTag<FadeHideTag>
    {
        private float _duration;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
        }

        protected override void UpdateCharacterInTag(
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.Color = VertexColor.Lerp(
                animationInfo.Color,
                animationInfo.Color.WithAlpha(0),
                scriptInfo.HideProgress(_duration)
            );
        }
    }
}