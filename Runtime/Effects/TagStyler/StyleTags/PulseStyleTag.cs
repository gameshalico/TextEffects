using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public sealed class PulseStyleTag : PooledStyleTag<PulseStyleTag>
    {
        private float _amplitude;
        private float _frequency;
        private float _charStep;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _amplitude = tagInfo.GetFloat("a", 0.2f);
            _frequency = tagInfo.GetFloat("f", 5);
            _charStep = tagInfo.GetFloat("s", 0.5f);
        }

        public override void UpdateText(AnimationTextInfo animationInfo)
        {
            foreach (var item in StyleTagRangeIterator.GetRange(TagInfo, animationInfo))
            {
                var offset = Mathf.Sin(Time.unscaledTime * _frequency + item.AnimationInfo.CharacterIndex * -_charStep) *
                             _amplitude +
                             _amplitude / 2;
                item.AnimationInfo.ApplyMatrixOnCenter(Matrix4x4.Scale(new Vector3(1 + offset, 1 + offset, 1)));
            }
        }
    }
}