using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public class SwingStyleTag : ContainerStyleTag<SwingStyleTag>
    {
        private float _amplitude;
        private float _frequency;
        private float _charStep;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _amplitude = tagInfo.GetFloat("a", 5);
            _frequency = tagInfo.GetFloat("f", 5);
            _charStep = tagInfo.GetFloat("s", 0.5f);
        }

        protected override void UpdateCharacterInTag(ref TMP_CharacterInfo characterInfo,
            ref AnimationCharacterInfo animationInfo)
        {
            var offset = Mathf.Sin(Time.unscaledTime * _frequency + animationInfo.CharacterIndex * _charStep) *
                         _amplitude;
            animationInfo.ApplyMatrixOnCenter(Matrix4x4.Rotate(Quaternion.Euler(0, 0, offset)));
        }
    }
}