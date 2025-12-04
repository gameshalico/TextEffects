using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public sealed class ScaleShowTag : PooledDisplayTag<ScaleShowTag>
    {
        private float _duration;
        private float _initialSize;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _initialSize = tagInfo.GetFloat("a");
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.ApplyMatrixOnCenter(
                    Matrix4x4.Scale(Vector3.one * Mathf.Lerp(_initialSize, 1f, item.ScriptInfo.ShowProgress(_duration))));
            }
        }
    }

    public class ScaleHideTag : PooledDisplayTag<ScaleHideTag>
    {
        private float _duration;
        private float _targetSize;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _targetSize = tagInfo.GetFloat("a");
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.ApplyMatrixOnCenter(
                    Matrix4x4.Scale(Vector3.one * Mathf.Lerp(1, _targetSize, item.ScriptInfo.HideProgress(_duration))));
            }
        }
    }
}