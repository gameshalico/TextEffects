using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public class RotateShowTag : ContainerDisplayTag<RotateShowTag>
    {
        private float _duration;
        private float _angle;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _angle = tagInfo.GetFloat("a", -90f);
        }

        protected override void UpdateCharacterInTag(
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.ApplyMatrixOnCenter(
                Matrix4x4.Rotate(Quaternion.Euler(0, 0, Mathf.Lerp(_angle, 0, scriptInfo.ShowProgress(_duration))))
            );
        }
    }

    public class RotateHideTag : ContainerDisplayTag<RotateHideTag>
    {
        private float _duration;
        private float _angle;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _angle = tagInfo.GetFloat("a", 90f);
        }

        protected override void UpdateCharacterInTag(
            ref CharacterAnimationInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.ApplyMatrixOnCenter(
                Matrix4x4.Rotate(Quaternion.Euler(0, 0, Mathf.Lerp(0, _angle, scriptInfo.HideProgress(_duration))))
            );
        }
    }
}