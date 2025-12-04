using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public sealed class RotateShowTag : PooledDisplayTag<RotateShowTag>
    {
        private float _duration;
        private float _angle;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _angle = tagInfo.GetFloat("a", -90f);
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.ApplyMatrixOnCenter(
                    Matrix4x4.Rotate(Quaternion.Euler(0, 0, Mathf.Lerp(_angle, 0, item.ScriptInfo.ShowProgress(_duration))))
                );
            }
        }
    }

    public class RotateHideTag : PooledDisplayTag<RotateHideTag>
    {
        private float _duration;
        private float _angle;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _angle = tagInfo.GetFloat("a", 90f);
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.ApplyMatrixOnCenter(
                    Matrix4x4.Rotate(Quaternion.Euler(0, 0, Mathf.Lerp(0, _angle, item.ScriptInfo.HideProgress(_duration))))
                );
            }
        }
    }
}