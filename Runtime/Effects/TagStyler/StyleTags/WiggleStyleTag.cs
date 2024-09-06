using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public class WiggleStyleTag : ContainerStyleTag<WiggleStyleTag>
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

        protected override void UpdateCharacterInTag(ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo)
        {
            var offset = Mathf.Sin(Time.unscaledTime * _frequency + animationInfo.CharacterIndex * _charStep) *
                         _amplitude;
            animationInfo.Quad += _directions[animationInfo.CharacterIndex - TagInfo.StartIndex] * offset;
        }
    }
}