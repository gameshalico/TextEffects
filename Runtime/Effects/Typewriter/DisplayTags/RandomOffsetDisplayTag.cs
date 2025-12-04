using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public sealed class RandomOffsetShowTag : PooledDisplayTag<RandomOffsetShowTag>
    {
        private float _duration;
        private Vector2[] _offsets;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            var radius = tagInfo.GetFloat("a", 10f);
            _offsets = new Vector2[tagInfo.Length];
            for (var i = 0; i < _offsets.Length; i++) _offsets[i] = Random.insideUnitCircle * radius;
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.Quad += Vector3.Lerp(_offsets[item.AnimationInfo.CharacterIndex - TagInfo.StartIndex],
                    Vector3.zero,
                    item.ScriptInfo.ShowProgress(_duration));
            }
        }
    }

    public class RandomOffsetHideTag : PooledDisplayTag<RandomOffsetHideTag>
    {
        private float _duration;
        private Vector2[] _offsets;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            var radius = tagInfo.GetFloat("a", 50f);
            _offsets = new Vector2[tagInfo.Length];
            for (var i = 0; i < _offsets.Length; i++) _offsets[i] = Random.insideUnitCircle * radius;
        }

        public override void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo)
        {
            foreach (var item in DisplayTagRangeIterator.GetRange(TagInfo, animationInfo, scriptInfo))
            {
                item.AnimationInfo.Quad += Vector3.Lerp(Vector3.zero,
                    _offsets[item.AnimationInfo.CharacterIndex - TagInfo.StartIndex],
                    item.ScriptInfo.HideProgress(_duration));
            }
        }
    }
}