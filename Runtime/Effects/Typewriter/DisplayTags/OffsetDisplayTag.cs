using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public class OffsetShowTag : ContainerDisplayTag<OffsetShowTag>
    {
        private float _duration;
        private Vector3 _offset;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _offset = new Vector3(tagInfo.GetFloat("x"), tagInfo.GetFloat("y"));
        }

        protected override void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.Quad += Vector3.Lerp(_offset, Vector3.zero, scriptInfo.ShowProgress(_duration));
        }
    }

    public class OffsetHideTag : ContainerDisplayTag<OffsetHideTag>
    {
        private float _duration;
        private Vector3 _offset;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _offset = new Vector3(tagInfo.GetFloat("x"), tagInfo.GetFloat("y"));
        }

        protected override void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.Quad += Vector3.Lerp(Vector3.zero, _offset, scriptInfo.HideProgress(_duration));
        }
    }
}