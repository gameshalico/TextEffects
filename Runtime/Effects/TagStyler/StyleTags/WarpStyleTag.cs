using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public class WarpStyleTag : ContainerStyleTag<WarpStyleTag>
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
        }

        protected override void UpdateCharacterInTag(ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo)
        {
            if (_delay > 0 && Time.unscaledTime - _lastWarpTime > _delay)
            {
                Warp();
                _lastWarpTime = Time.unscaledTime;
            }

            animationInfo.Quad += _offsets[animationInfo.CharacterIndex - StartIndex];
        }
    }
}