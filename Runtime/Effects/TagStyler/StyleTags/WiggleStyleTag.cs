using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public sealed class WiggleStyleTag : PooledStyleTag<WiggleStyleTag>
    {
        private float _amplitude;
        private float _frequency;
        private float _charStep;

        private Vector2[] _directions;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _amplitude = tagInfo.GetFloat("a", 5);
            _frequency = tagInfo.GetFloat("f", 5);
            _charStep = tagInfo.GetFloat("s", 0.5f);

            _directions = new Vector2[tagInfo.Length];
            for (var i = 0; i < _directions.Length; i++) _directions[i] = Random.insideUnitCircle.normalized;
        }

        public override void UpdateText(AnimationTextInfo animationInfo)
        {
            foreach (var item in StyleTagRangeIterator.GetRange(TagInfo, animationInfo))
            {
                var offset = Mathf.Sin(Time.unscaledTime * _frequency + item.AnimationInfo.CharacterIndex * _charStep) *
                             _amplitude;
                item.AnimationInfo.Quad += _directions[item.AnimationInfo.CharacterIndex - TagInfo.StartIndex] * offset;
            }
        }
    }
}