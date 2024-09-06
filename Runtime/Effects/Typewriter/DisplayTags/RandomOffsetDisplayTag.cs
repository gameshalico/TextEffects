using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public class RandomOffsetShowTag : ContainerDisplayTag<RandomOffsetShowTag>
    {
        private float _duration;
        private Vector2[] _offsets;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            var radius = tagInfo.GetFloat("r", 10f);
            _offsets = new Vector2[tagInfo.Length];
            for (var i = 0; i < _offsets.Length; i++) _offsets[i] = Random.insideUnitCircle * radius;
        }


        protected override void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.Quad += Vector3.Lerp(_offsets[animationInfo.CharacterIndex - TagInfo.StartIndex],
                Vector3.zero,
                scriptInfo.ShowProgress(_duration));
        }
    }

    public class RandomOffsetHideTag : ContainerDisplayTag<RandomOffsetHideTag>
    {
        private float _duration;
        private Vector2[] _offsets;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            var radius = tagInfo.GetFloat("r", 50f);
            _offsets = new Vector2[tagInfo.Length];
            for (var i = 0; i < _offsets.Length; i++) _offsets[i] = Random.insideUnitCircle * radius;
        }


        protected override void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.Quad += Vector3.Lerp(Vector3.zero,
                _offsets[animationInfo.CharacterIndex - TagInfo.StartIndex],
                scriptInfo.HideProgress(_duration));
        }
    }
}