using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public class ShakeStyleTag : ContainerStyleTag<ShakeStyleTag>
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

        protected override void OnUpdateText(AnimationTextInfo animationInfo)
        {
            if (_delay > 0 && Time.unscaledTime - _lastShakeTime > _delay) Shake();
        }

        protected override void UpdateCharacterInTag(ref TMP_CharacterInfo characterInfo,
            ref AnimationCharacterInfo animationInfo)
        {
            animationInfo.Quad += _shakeOffsets[animationInfo.CharacterIndex - TagInfo.StartIndex];
        }
    }
}