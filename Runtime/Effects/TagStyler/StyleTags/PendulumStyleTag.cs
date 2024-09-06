using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public class PendulumStyleTag : ContainerStyleTag<PendulumStyleTag>
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

            var size = animationInfo.Quad.Size;
            animationInfo.ApplyMatrixOnCenter(
                Matrix4x4.Translate(Vector3.up * size.y / 2)
                * Matrix4x4.Rotate(Quaternion.Euler(0, 0, offset))
                * Matrix4x4.Translate(Vector3.down * size.y / 2)
            );
        }
    }
}