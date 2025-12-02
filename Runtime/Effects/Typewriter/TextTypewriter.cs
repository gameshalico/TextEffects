using System;
using System.Collections.Generic;
using System.Threading;
using TextEffects.Common;
using TextEffects.Core;
using TextEffects.Data;
using TextEffects.Effects.Typewriter.Modifiers;
using UnityEngine;
using TextEffects.Effects.Typewriter.ScriptTags;

#if TEXTEFFECTS_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace TextEffects.Effects.Typewriter
{
    [AddComponentMenu("Text Effects/Text Typewriter")]
    [AddEffectorFeatureMenu("Effects/Typewriter")]
    [DisallowMultipleComponent]
    public sealed class TextTypewriter : TextEffectorFeature
    {
        [SerializeField] private bool _autoPlay;
        [SerializeField] private bool _keepDisplayOnRefresh;
        [SerializeField] private float _defaultDelay = 0.01f;
        private AutoPlayEffect _autoPlayEffect;
        private DefaultDelayScriptModifier _defaultDelayScriptModifier;
#if TEXTEFFECTS_UNITASK_SUPPORT
        private Dictionary<string, Func<EventContext, CancellationToken, UniTask>> _eventTagHandler;
#else
        private Dictionary<string, Func<EventContext, CancellationToken, Task>> _eventTagHandler;
#endif
        private TypewriterEffect _typewriterEffect;
        private ScriptTagFactoryMap _scriptTagFactoryMap;
        private DisplayTagFactoryMap _displayTagFactoryMap;

        public bool IsPaused
        {
            get
            {
                InitializeIfNeeded();
                return _typewriterEffect.IsPaused;
            }
            set
            {
                InitializeIfNeeded();
                if (value)
                {
                    _typewriterEffect.Pause();
                }
                else
                {
                    _typewriterEffect.Resume();
                }
            }
        }

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
                if (_defaultDelayScriptModifier != null)
                {
                    _defaultDelayScriptModifier.DefaultDelay = value;
                    SetDirty();
                }
            }
        }

        public bool KeepDisplayOnRefresh
        {
            get => _keepDisplayOnRefresh;
            set
            {
                _keepDisplayOnRefresh = value;
                if (_typewriterEffect != null)
                {
                    _typewriterEffect.KeepDisplayOnRefresh = value;
                }
            }
        }


        private void OnValidate()
        {
            DefaultDelay = _defaultDelay;
            KeepDisplayOnRefresh = _keepDisplayOnRefresh;
        }

        protected override void AddFeature(TextEffector textEffector)
        {
            InitializeIfNeeded();
            textEffector.AddEffect(_typewriterEffect);
            textEffector.AddEffect(_autoPlayEffect);
        }

        protected override void RemoveFeature(TextEffector textEffector)
        {
            InitializeIfNeeded();
            textEffector.RemoveEffect(_typewriterEffect);
            textEffector.RemoveEffect(_autoPlayEffect);
        }

        public void RegisterDisplayTag(string tagName, IDisplayTagFactory displayTagFactory)
        {
            InitializeIfNeeded();
            if (_displayTagFactoryMap == null)
            {
                _displayTagFactoryMap = DisplayTagFactoryMap.Default.Clone();
                _typewriterEffect.DisplayTagFactory = _displayTagFactoryMap;
            }
            _displayTagFactoryMap.RegisterFactory(tagName, displayTagFactory);
        }
        public void UnregisterDisplayTag(string tagName)
        {
            if (_displayTagFactoryMap == null)
                return;

            _displayTagFactoryMap.UnregisterFactory(tagName);
        }

        public void RegisterScriptTag(string tagName, IScriptTagFactory scriptTagFactory)
        {
            InitializeIfNeeded();
            _scriptTagFactoryMap.RegisterFactory(tagName, scriptTagFactory);
        }

        public void UnregisterScriptTag(string tagName)
        {
            InitializeIfNeeded();
            _scriptTagFactoryMap.UnregisterFactory(tagName);
        }

#if TEXTEFFECTS_UNITASK_SUPPORT
        public void RegisterEventTagHandler(string tagName, Func<EventContext, CancellationToken, UniTask> handler)
#else
        public void RegisterEventTagHandler(string tagName, Func<EventContext, CancellationToken, Task> handler)
#endif
        {
            InitializeIfNeeded();
            _eventTagHandler[tagName] = handler;
        }

        public void ResetScript()
        {
            _typewriterEffect.Stop();
            _typewriterEffect.ResetAll();
        }

        public void Play(string text)
        {
            if (!isActiveAndEnabled)
                return;
            Effector.TMPText.text = text;
            Effector.TMPText.ForceMeshUpdate();
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

#if TEXTEFFECTS_UNITASK_SUPPORT
        private async UniTask InvokeEventTagHandlerAsync(EventContext eventContext, CancellationToken cancellationToken)
#else
        private async Task InvokeEventTagHandlerAsync(EventContext eventContext, CancellationToken cancellationToken)
#endif
        {
            if (_eventTagHandler != null && _eventTagHandler.TryGetValue(eventContext.TagInfo.GetString(""), out var handler))
            {
                await handler(eventContext, cancellationToken);
            }
        }

        private void InitializeIfNeeded()
        {
            if (_typewriterEffect != null)
                return;

            _eventTagHandler = new();

            _scriptTagFactoryMap = ScriptTagFactoryMap.Default.Clone();
            var factory = new EventScriptTag.Factory(InvokeEventTagHandlerAsync);
            _scriptTagFactoryMap.RegisterFactory("evt", factory);
            _scriptTagFactoryMap.RegisterFactory("event", factory);

            _typewriterEffect = new TypewriterEffect(
                DisplayTagFactoryMap.Default,
                _scriptTagFactoryMap,
                _keepDisplayOnRefresh);
            _defaultDelayScriptModifier = new DefaultDelayScriptModifier(_defaultDelay);

            _autoPlayEffect = new AutoPlayEffect(this);

            _typewriterEffect.AddModifier(_defaultDelayScriptModifier);
            _typewriterEffect.AddModifier(new DelayTagScriptModifier());
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
            Effector.TMPText.text = text;
            Effector.TMPText.ForceMeshUpdate();
            await PlayScriptAsync(cancellationToken);
        }

        private class AutoPlayEffect : ITextAnimationEffect
        {
            private readonly TextTypewriter _owner;

            public AutoPlayEffect(TextTypewriter owner)
            {
                _owner = owner;
            }

            public void Setup(TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
            {
                if (!_owner._autoPlay)
                {
                    return;
                }

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