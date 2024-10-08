﻿using TextEffects.Data;
using UnityEngine;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    public class ScaleShowTag : ContainerDisplayTag<ScaleShowTag>
    {
        private float _duration;
        private float _initialSize;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _initialSize = tagInfo.GetFloat("a");
        }

        protected override void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.ApplyMatrixOnCenter(
                Matrix4x4.Scale(Vector3.one * Mathf.Lerp(_initialSize, 1f, scriptInfo.ShowProgress(_duration))));
        }
    }

    public class ScaleHideTag : ContainerDisplayTag<ScaleHideTag>
    {
        private float _duration;
        private float _targetSize;

        protected override void OnSetTag(TagInfo tagInfo)
        {
            _duration = tagInfo.GetFloat("d", 0.1f);
            _targetSize = tagInfo.GetFloat("a");
        }

        protected override void UpdateCharacterInTag(
            ref AnimationCharacterInfo animationInfo,
            ref ScriptCharacterInfo scriptInfo)
        {
            animationInfo.ApplyMatrixOnCenter(
                Matrix4x4.Scale(Vector3.one * Mathf.Lerp(1, _targetSize, scriptInfo.HideProgress(_duration))));
        }
    }
}