using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TextEffects.Common;
using TextEffects.Core;
using TextEffects.Data;
using TMPro;
#if UNITY_EDITOR
using UnityEngine;
#endif
#if TEXTEFFECTS_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;

#else
using System.Threading.Tasks;
#endif

namespace TextEffects.Effects.Typewriter
{
    public class TypewriterEffect : ITextAnimationEffect
    {
        private readonly List<IScriptModifier> _modifiers;
        private readonly List<IScriptListener> _listeners;
        private IDisplayTag[] _displayTags;
        private int _charCount;
        private ScriptInfo _scriptInfo;
        private CancellationTokenSource _playCts;

        public TypewriterEffect(IDisplayTagFactory displayTagFactory)
        {
            DisplayTagFactory = displayTagFactory;

            _modifiers = new List<IScriptModifier>();
            _listeners = new List<IScriptListener>();
        }

        public bool IsPaused { get; private set; }

        public IDisplayTagFactory DisplayTagFactory { get; set; }

        public void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
            foreach (var listener in _listeners) listener.OnSetup(textInfo, tags);

            _charCount = textInfo.characterCount;
            _scriptInfo = new ScriptInfo(_charCount);

            foreach (var modifier in _modifiers) modifier.ModifyScript(tags, _scriptInfo);
            foreach (var listener in _listeners) listener.OnScriptModify(_scriptInfo);

            _displayTags = tags
                .Select(DisplayTagFactory.CreateTag)
                .Where(static tag => tag != null)
                .ToArray();

            foreach (var tag in _displayTags)
                tag.Setup(textInfo, tags);

#if UNITY_EDITOR
            if (!Application.isPlaying)
                PlayScriptLoop(default).ForgetSafe();
#endif
        }

        public void UpdateText(TextAnimationInfo textAnimationInfo)
        {
            foreach (var tag in _displayTags)
                tag.UpdateText(textAnimationInfo, _scriptInfo);
        }

        public void Release()
        {
#if UNITY_EDITOR
            _loopCts?.Cancel();
#endif
            _playCts?.Cancel();

            foreach (var tag in _displayTags)
                tag.Release();
            foreach (var listener in _listeners) listener.OnRelease();
        }

        public void AddModifier(IScriptModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(IScriptModifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        public void AddListener(IScriptListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(IScriptListener listener)
        {
            _listeners.Remove(listener);
        }

        public void ShowAt(int index, bool skipAnimation = false)
        {
            var scriptCharacterInfo = _scriptInfo.ScriptCharacterInfo[index];
            if (scriptCharacterInfo.IsShown)
                return;

            scriptCharacterInfo.Show(skipAnimation);
            _scriptInfo.ScriptCharacterInfo[index] = scriptCharacterInfo;

            foreach (var listener in _listeners) listener.OnCharacterShow(index);
        }

        public void HideAt(int index, bool skipAnimation = false)
        {
            var scriptCharacterInfo = _scriptInfo.ScriptCharacterInfo[index];
            if (scriptCharacterInfo.IsHidden)
                return;

            scriptCharacterInfo.Hide(skipAnimation);
            _scriptInfo.ScriptCharacterInfo[index] = scriptCharacterInfo;

            foreach (var listener in _listeners) listener.OnCharacterHide(index);
        }

        public void ResetAt(int index)
        {
            var scriptCharacterInfo = _scriptInfo.ScriptCharacterInfo[index];
            scriptCharacterInfo.Reset();
            _scriptInfo.ScriptCharacterInfo[index] = scriptCharacterInfo;
        }

        public void ShowAll(bool skipAnimation = false)
        {
            for (var i = 0; i < _charCount; i++)
                ShowAt(i, skipAnimation);
        }

        public void HideAll(bool skipAnimation = false)
        {
            for (var i = 0; i < _charCount; i++)
                HideAt(i, skipAnimation);
        }

        public void ResetAll()
        {
            for (var i = 0; i < _charCount; i++)
                ResetAt(i);
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Stop()
        {
            _playCts?.Cancel();
        }

#if TEXTEFFECTS_UNITASK_SUPPORT
        public async UniTask PlayScriptAsync(CancellationToken cancellationToken = default)
#else
        public async Task PlayScriptAsync(CancellationToken cancellationToken = default)
#endif
        {
            try
            {
                IsPaused = false;
                _playCts?.Cancel();
                _playCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                foreach (var listener in _listeners) listener.OnPlay();

                for (var i = 0; i < _charCount; i++)
                {
                    var scriptCharacterInfo = _scriptInfo.ScriptCharacterInfo[i];
                    if (scriptCharacterInfo.IsShown)
                        continue;
                    if (scriptCharacterInfo.Delay > 0)

                        await SafeTask.Delay(TimeSpan.FromSeconds(scriptCharacterInfo.Delay), _playCts.Token);

                    while (IsPaused)
                        await SafeTask.WaitWhile(() => IsPaused, _playCts.Token);

                    ShowAt(i);
                }

                await SafeTask.Delay(TimeSpan.FromSeconds(_scriptInfo.LastDelay), _playCts.Token);
            }
            catch (OperationCanceledException)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

#if UNITY_EDITOR
        private CancellationTokenSource _loopCts;

#if TEXTEFFECTS_UNITASK_SUPPORT
        private async UniTask PlayScriptLoop(CancellationToken cancellationToken)
#else
        private async Task PlayScriptLoop(CancellationToken cancellationToken)
#endif
        {
            _loopCts?.Cancel();
            _loopCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            await SafeTask.Delay(TimeSpan.FromSeconds(0.01f), _loopCts.Token);
            while (!_loopCts.Token.IsCancellationRequested)
            {
                ResetAll();
                await PlayScriptAsync(_loopCts.Token);
                await SafeTask.Delay(TimeSpan.FromSeconds(0.5f), _loopCts.Token);
                _loopCts.Token.ThrowIfCancellationRequested();
                HideAll();
                await SafeTask.Delay(TimeSpan.FromSeconds(0.5f), _loopCts.Token);
            }
        }
#endif
    }
}