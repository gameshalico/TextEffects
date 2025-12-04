using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public sealed class WarpStyleTag : PooledStyleTag<WarpStyleTag>
    {
        private float _delay;
        private float _amplitude;
        private Quad[] _offsets;
        private float _lastWarpTime;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _delay = tagInfo.GetFloat("d", 0.05f);
            _amplitude = tagInfo.GetFloat("a", 2);

            _offsets = new Quad[tagInfo.Length];
            Warp();
        }

        private void Warp()
        {
            for (var i = 0; i < _offsets.Length; i++)
                _offsets[i] = new Quad(Random.insideUnitCircle * _amplitude, Random.insideUnitCircle * _amplitude,
                    Random.insideUnitCircle * _amplitude, Random.insideUnitCircle * _amplitude);
            _lastWarpTime = Time.unscaledTime;
        }

        public override void UpdateText(AnimationTextInfo animationInfo)
        {
            if (_delay > 0 && Time.unscaledTime - _lastWarpTime > _delay) Warp();

            foreach (var item in StyleTagRangeIterator.GetRange(TagInfo, animationInfo))
            {
                item.AnimationInfo.Quad += _offsets[item.AnimationInfo.CharacterIndex - TagInfo.StartIndex];
            }
        }
    }
}