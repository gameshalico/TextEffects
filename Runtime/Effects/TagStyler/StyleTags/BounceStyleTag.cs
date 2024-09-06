using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public class BounceStyleTag : ContainerStyleTag<BounceStyleTag>
    {
        private float _amplitude;
        private float _frequency;
        private float _charStep;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _amplitude = tagInfo.GetFloat("a", 10);
            _frequency = tagInfo.GetFloat("f", 0.5f);
            _charStep = tagInfo.GetFloat("s", 0.025f);
        }

        private static float EaseOutBounce(float t)
        {
            if (t < 1 / 2.75f) return 7.5625f * t * t;

            if (t < 2 / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return 7.5625f * t * t + 0.75f;
            }

            if (t < 2.5 / 2.75)
            {
                t -= 2.25f / 2.75f;
                return 7.5625f * t * t + 0.9375f;
            }

            t -= 2.625f / 2.75f;
            return 7.5625f * t * t + 0.984375f;
        }

        private static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        private float CalcOffset(float t)
        {
            // 0~60 : delay
            if (t < 0.6f) return 0;
            // 60-70 : move up
            if (t < 0.7f) return Remap(t, 0.6f, 0.7f, 0, 1) * _amplitude;
            // 70-100 : bounce
            return EaseOutBounce(Remap(t, 0.7f, 1, 0, 1)) * -_amplitude + _amplitude;
        }

        protected override void UpdateCharacterInTag(ref TMP_CharacterInfo characterInfo,
            ref CharacterAnimationInfo animationInfo)
        {
            var t = Mathf.Repeat(Time.unscaledTime * _frequency + animationInfo.CharacterIndex * -_charStep, 1f);
            var offset = CalcOffset(t);
            animationInfo.Quad += new Vector3(0, offset, 0);
        }
    }
}