using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public sealed class ShakeStyleTag : PooledStyleTag<ShakeStyleTag>
    {
        private float _delay;
        private float _amplitude;
        private float _lastShakeTime;
        private Vector2[] _shakeOffsets;


        protected override void OnSetTag(TagInfo tagInfo)
        {
            _delay = tagInfo.GetFloat("d", 0.05f);
            _amplitude = tagInfo.GetFloat("a", 2);

            _shakeOffsets = new Vector2[tagInfo.Length];
            Shake();
        }

        private void Shake()
        {
            for (var i = 0; i < _shakeOffsets.Length; i++)
                _shakeOffsets[i] = Random.insideUnitCircle * _amplitude;
            _lastShakeTime = Time.unscaledTime;
        }

        public override void UpdateText(AnimationTextInfo animationInfo)
        {
            if (_delay > 0 && Time.unscaledTime - _lastShakeTime > _delay) Shake();

            foreach (var item in StyleTagRangeIterator.GetRange(TagInfo, animationInfo))
            {
                item.AnimationInfo.Quad += _shakeOffsets[item.AnimationInfo.CharacterIndex - TagInfo.StartIndex];
            }
        }
    }
}