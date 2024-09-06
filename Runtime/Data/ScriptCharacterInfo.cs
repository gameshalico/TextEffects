using System;
using UnityEngine;

namespace TextEffects.Data
{
    public struct ScriptCharacterInfo : IEquatable<ScriptCharacterInfo>
    {
        private float ShowElapsedTime => IsShown ? Time.unscaledTime - _showStartTime : 0;
        private float HideElapsedTime => IsHidden ? Time.unscaledTime - _hideStartTime : 0;
        public int CharacterIndex { get; init; }
        public float Delay { get; set; }
        public bool IsShown => _showStartTime >= 0;
        public bool IsHidden => _hideStartTime >= 0;

        private float _showStartTime;
        private float _hideStartTime;
        private bool _showImmediately;
        private bool _hideImmediately;

        public float ShowProgress(float duration)
        {
            if (_showImmediately)
                return 1;
            if (duration == 0)
                return IsShown ? 1 : 0;

            return Mathf.Clamp01(ShowElapsedTime / duration);
        }

        public float HideProgress(float duration)
        {
            if (_hideImmediately)
                return 1;
            if (duration == 0)
                return IsHidden ? 1 : 0;

            return Mathf.Clamp01(HideElapsedTime / duration);
        }

        public void Show(bool skipAnimation = false)
        {
            if (_showStartTime >= 0)
                return;

            _showImmediately = skipAnimation;
            _showStartTime = Time.unscaledTime;
        }

        public void Hide(bool skipAnimation = false)
        {
            if (_hideStartTime >= 0)
                return;

            _hideImmediately = skipAnimation;
            _hideStartTime = Time.unscaledTime;
        }

        public void Reset()
        {
            _showStartTime = -1;
            _hideStartTime = -1;
        }

        public bool Equals(ScriptCharacterInfo other)
        {
            return _showStartTime.Equals(other._showStartTime) && _hideStartTime.Equals(other._hideStartTime) &&
                   Delay.Equals(other.Delay);
        }

        public override bool Equals(object obj)
        {
            return obj is ScriptCharacterInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_showStartTime, _hideStartTime, Delay);
        }
    }
}