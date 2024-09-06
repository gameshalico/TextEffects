using TextEffects.Data;
using TMPro;
using UnityEngine;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    public class RainbowStyleTag : ContainerStyleTag<RainbowStyleTag>
    {
        private float _frequency;
        private float _charStep;
        private float _saturation;
        private float _value;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _frequency = tagInfo.GetFloat("f", 0.2f);
            _charStep = tagInfo.GetFloat("s", 0.05f);

            _saturation = tagInfo.GetFloat("sat", 1);
            _value = tagInfo.GetFloat("val", 1);
        }

        protected override void UpdateCharacterInTag(ref TMP_CharacterInfo characterInfo,
            ref AnimationCharacterInfo animationInfo)
        {
            var offset = Mathf.Repeat(Time.unscaledTime * _frequency + animationInfo.CharacterIndex * -_charStep, 1);
            var color = (Color32)Color.HSVToRGB(Mathf.Repeat(offset, 1), _saturation, _value);
            color.a = animationInfo.Color.BottomLeft.a;
            animationInfo.Color = color;
        }
    }
}