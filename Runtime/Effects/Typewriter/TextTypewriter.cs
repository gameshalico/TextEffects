﻿using System;
using System.Collections.Generic;
using System.Threading;
using TextEffects.Common;
using TextEffects.Core;
using TextEffects.Data;
using TextEffects.Effects.Typewriter.Listeners;
using TextEffects.Effects.Typewriter.Modifiers;
using TMPro;
using UnityEngine;
#if TEXTEFFECTS_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#endif

namespace TextEffects.Effects.Typewriter
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(TextEffector))]
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class TextTypewriter : MonoBehaviour
    {
        [SerializeField] private bool _autoPlay;
        [SerializeField] private float _defaultDelay = 0.01f;
        private AutoPlayEffect _autoPlayEffect;
        private DefaultScriptModifier _defaultScriptModifier;
        private Action<TagEventData> _onEventTriggered;
        private TagEventDispatcherScriptListener _tagEventDispatcherScriptListener;
        private TextEffector _textEffector;
        private TypewriterEffect _typewriterEffect;

        public bool AutoPlay
        {
            get => _autoPlay;
            set => _autoPlay = value;
        }

        public float DefaultDelay
        {
            get => _defaultDelay;
            set
            {
                _defaultDelay = value;
                if (_defaultScriptModifier != null)
                {
                    _defaultScriptModifier.DefaultDelay = value;
                    _textEffector.SetDirty();
                }
            }
        }

        private void OnEnable()
        {
            InitializeIfNeeded();
            _textEffector.AddEffect(_typewriterEffect);
            _textEffector.AddEffect(_autoPlayEffect);
        }

        private void OnDisable()
        {
            _textEffector.RemoveEffect(_typewriterEffect);
            _textEffector.RemoveEffect(_autoPlayEffect);
        }

        private void OnValidate()
        {
            if (_defaultScriptModifier != null) DefaultDelay = _defaultDelay;
        }

        public void SetDisplayTagFactory(IDisplayTagFactory displayTagFactory)
        {
            InitializeIfNeeded();
            _typewriterEffect.DisplayTagFactory = displayTagFactory;
        }

        public void ResetScript()
        {
            _typewriterEffect.Stop();
            _typewriterEffect.ResetAll();
        }

        public event Action<TagEventData> OnEventTriggered
        {
            add => _onEventTriggered += value;
            remove => _onEventTriggered -= value;
        }

        public void Play(string text)
        {
            if (!isActiveAndEnabled)
                return;
            _textEffector.TMPText.text = text;
            _textEffector.TMPText.ForceMeshUpdate();
            PlayScript();
        }

        public void ShowAll(bool skipAnimation = false)
        {
            _typewriterEffect.ShowAll(skipAnimation);
        }

        public void HideAll(bool skipAnimation = false)
        {
            _typewriterEffect.HideAll(skipAnimation);
        }

        public void Stop()
        {
            _typewriterEffect.Stop();
        }

        public void Pause()
        {
            _typewriterEffect.Pause();
        }

        public void Resume()
        {
            _typewriterEffect.Resume();
        }

        public void PlayScript()
        {
            if (!isActiveAndEnabled)
                return;
            _typewriterEffect.PlayScriptAsync().ForgetSafe();
        }

        public void AddModifier(IScriptModifier modifier)
        {
            _typewriterEffect.AddModifier(modifier);
        }

        public void RemoveModifier(IScriptModifier modifier)
        {
            _typewriterEffect.RemoveModifier(modifier);
        }

        public void AddListener(IScriptListener listener)
        {
            _typewriterEffect.AddListener(listener);
        }

        public void RemoveListener(IScriptListener listener)
        {
            _typewriterEffect.RemoveListener(listener);
        }

        private void InitializeIfNeeded()
        {
            if (_typewriterEffect != null)
                return;

            _textEffector = GetComponent<TextEffector>();
            _typewriterEffect = new TypewriterEffect(DisplayTagFactoryMap.Default);
            _defaultScriptModifier = new DefaultScriptModifier(_defaultDelay);
            _tagEventDispatcherScriptListener = new TagEventDispatcherScriptListener();
            _tagEventDispatcherScriptListener.OnEventTriggered += _onEventTriggered;
            _autoPlayEffect = new AutoPlayEffect(this);

            _typewriterEffect.AddModifier(_defaultScriptModifier);
            _typewriterEffect.AddModifier(new DelayTagScriptModifier());
            _typewriterEffect.AddListener(_tagEventDispatcherScriptListener);
        }

#if TEXTEFFECTS_UNITASK_SUPPORT
        public async UniTask PlayScriptAsync(CancellationToken cancellationToken = default)
#else
        public async Task PlayScriptAsync(CancellationToken cancellationToken = default)
#endif
        {
            if (!isActiveAndEnabled)
                return;

            await _typewriterEffect.PlayScriptAsync(cancellationToken);
        }

#if TEXTEFFECTS_UNITASK_SUPPORT
        public async UniTask PlayAsync(string text, CancellationToken cancellationToken = default)
#else
        public async Task PlayAsync(string text, CancellationToken cancellationToken = default)
#endif
        {
            if (!isActiveAndEnabled)
                return;
            _textEffector.TMPText.text = text;
            _textEffector.TMPText.ForceMeshUpdate();
            await PlayScriptAsync(cancellationToken);
        }

        private class AutoPlayEffect : ITextAnimationEffect
        {
            private readonly TextTypewriter _owner;

            public AutoPlayEffect(TextTypewriter owner)
            {
                _owner = owner;
            }

            public void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
            {
                if (!_owner._autoPlay)
                    return;

                _owner.PlayScript();
            }

            public void UpdateText(AnimationTextInfo animationInfo)
            {
            }

            public void Release()
            {
            }
        }
    }
}