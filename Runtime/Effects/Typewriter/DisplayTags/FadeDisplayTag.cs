using TextEffects.Data;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public sealed class FadeShowTag : PooledDisplayTag<FadeShowTag>
    {
        private float _duration;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.Color = VertexColor.Lerp(
                    item.AnimationInfo.Color.WithAlpha(0),
                    item.AnimationInfo.Color,
                    item.ScriptInfo.ShowProgress(_duration)
                );
            }
        }
    }

    public class FadeHideTag : PooledDisplayTag<FadeHideTag>
    {
        private float _duration;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.Color = VertexColor.Lerp(
                    item.AnimationInfo.Color,
                    item.AnimationInfo.Color.WithAlpha(0),
                    item.ScriptInfo.HideProgress(_duration)
                );
            }
        }
    }
}