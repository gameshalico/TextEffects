using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public sealed class OffsetShowTag : PooledDisplayTag<OffsetShowTag>
    {
        private float _duration;
        private Vector3 _offset;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _offset = new Vector3(tagInfo.GetFloat("x"), tagInfo.GetFloat("y"));
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.Quad += Vector3.Lerp(_offset, Vector3.zero, item.ScriptInfo.ShowProgress(_duration));
            }
        }
    }

    public class OffsetHideTag : PooledDisplayTag<OffsetHideTag>
    {
        private float _duration;
        private Vector3 _offset;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _offset = new Vector3(tagInfo.GetFloat("x"), tagInfo.GetFloat("y"));
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.Quad += Vector3.Lerp(Vector3.zero, _offset, item.ScriptInfo.HideProgress(_duration));
            }
        }
    }
}