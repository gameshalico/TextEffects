using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Core
{
    public partial class TextEffector
    {
        public void AddEffect(ITextAnimationEffect effect)
        {
            InitializeIfNeeded();
            _animationHandler.AddEffect(effect);
            SetDirty();
        }

        public void RemoveEffect(ITextAnimationEffect effect)
        {
            InitializeIfNeeded();
            _animationHandler.RemoveEffect(effect);
            SetDirty();
        }

        private class TextAnimationHandler : ITextAnimationHandler
        {
            private readonly List<ITextAnimationEffect> _effects = new();
            private readonly List<ITextAnimationEffect> _effectsToAdd = new();
            private TagInfo[] _tagInfos;
            private TagInfo[] _prevTagInfos;
            private bool _isReleased;

            public void Release()
            {
                if (_isReleased)
                    return;

                _isReleased = true;

                foreach (var effect in _effects)
                    effect.Release();

                ReleasePrevTagInfos();
            }

            public void Setup(TMP_TextInfo textInfo)
            {
                _isReleased = false;
                _effects.AddRange(_effectsToAdd);
                _effectsToAdd.Clear();

                foreach (var effect in _effects)
                    effect.Setup(textInfo, _tagInfos);
            }

            public void UpdateCharacter(ref TMP_CharacterInfo characterInfo, ref CharacterAnimationInfo animationInfo)
            {
                if (_isReleased)
                    return;

                foreach (var effect in _effects)
                    effect.UpdateCharacter(ref characterInfo, ref animationInfo);
            }

            private void ReleasePrevTagInfos()
            {
                if (_prevTagInfos != null)
                {
                    foreach (var tag in _prevTagInfos)
                        tag.Return();
                    _prevTagInfos = null;
                }
            }

            public void AddEffect(ITextAnimationEffect effect)
            {
                _effectsToAdd.Add(effect);
            }

            public void RemoveEffect(ITextAnimationEffect effect)
            {
                if (!_effects.Contains(effect))
                    return;
                if (!_isReleased)
                    effect.Release();
                _effects.Remove(effect);
            }

            public void SetTags(TagInfo[] tags)
            {
                if (_prevTagInfos != null && !_isReleased)
                    Release();

                _prevTagInfos = _tagInfos;
                _tagInfos = tags;
            }
        }
    }
}