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
    public sealed class TypewriterEffect : ITextAnimationEffect
    {
        private readonly List<IScriptModifier> _modifiers;
        private readonly List<IScriptListener> _listeners;
        private IDisplayTag[] _displayTags;
        private (TagInfo Tag, IScriptTag ScriptTag)[] _scriptTags;
        private int _characterCount;
        private ScriptTextInfo _scriptInfo;
        private CancellationTokenSource _playCts;

        public TypewriterEffect(IDisplayTagFactory displayTagFactory, IScriptTagFactory scriptTagFactory, bool keepDisplayOnRefresh)
        {
            DisplayTagFactory = displayTagFactory;
            ScriptTagFactory = scriptTagFactory;
            KeepDisplayOnRefresh = keepDisplayOnRefresh;
            _modifiers = new List<IScriptModifier>();
            _listeners = new List<IScriptListener>();
        }

        public bool IsPaused { get; private set; }

        public IDisplayTagFactory DisplayTagFactory { get; set; }
        public IScriptTagFactory ScriptTagFactory { get; set; }
        public bool KeepDisplayOnRefresh { get; set; }

        public void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
            foreach (var listener in _listeners) listener.OnSetup(textInfo, tags);

            _characterCount = textInfo.characterCount;

#if UNITY_EDITOR
            var isScriptCreate = !KeepDisplayOnRefresh || _scriptInfo == null;
#endif

            if (!KeepDisplayOnRefresh || _scriptInfo == null)
                _scriptInfo = new ScriptTextInfo(_characterCount);
            else
                _scriptInfo.Resize(_characterCount);

            foreach (var modifier in _modifiers) modifier.ModifyScript(tags, _scriptInfo);
            foreach (var listener in _listeners) listener.OnScriptModify(_scriptInfo);

            // ScriptTagの生成と登録
            _scriptTags = tags
                .Select(tagInfo => (Tag: tagInfo, ScriptTag: ScriptTagFactory.CreateTag(tagInfo)))
                .Where(static tag => tag.ScriptTag != null)
                .OrderBy(static tag => tag.Tag.StartIndex)
                .ToArray();

            // DisplayTagの生成と登録
            _displayTags = tags
                .Select(DisplayTagFactory.CreateTag)
                .Where(static tag => tag != null)
                .ToArray();

#if UNITY_EDITOR
            if (!Application.isPlaying && isScriptCreate)
                PlayScriptLoop(default).ForgetSafe();
#endif
        }

        public void UpdateText(AnimationTextInfo animationInfo)
        {
            foreach (var tag in _displayTags)
                tag.UpdateText(animationInfo, _scriptInfo);
        }

        public void Release()
        {
#if UNITY_EDITOR
            if (!KeepDisplayOnRefresh)
                _loopCts?.Cancel();
#endif
            if (!KeepDisplayOnRefresh)
                _playCts?.Cancel();

            foreach (var tag in _displayTags)
                tag.Release();
            foreach (var tagPair in _scriptTags)
                tagPair.ScriptTag.Release();
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
            for (var i = 0; i < _characterCount; i++)
                ShowAt(i, skipAnimation);
        }

        public void HideAll(bool skipAnimation = false)
        {
            for (var i = 0; i < _characterCount; i++)
                HideAt(i, skipAnimation);
        }

        public void ResetAll()
        {
            for (var i = 0; i < _characterCount; i++)
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

                var characterIndex = 0;
                var scriptTagIndex = 0;
                while (!_playCts.Token.IsCancellationRequested)
                {
                    if (characterIndex >= _characterCount)
                        break;

                    var scriptCharacterInfo = _scriptInfo.ScriptCharacterInfo[characterIndex];
                    if (scriptCharacterInfo.IsShown)
                    {
                        characterIndex++;
                        continue;
                    }

                    if (scriptCharacterInfo.Delay > 0)
                        await SafeTask.Delay(TimeSpan.FromSeconds(scriptCharacterInfo.Delay), _playCts.Token);

                    // ScriptTagの実行
                    List<IScriptTag> executingTags = null;
                    while (scriptTagIndex < _scriptTags.Length &&
                        _scriptTags[scriptTagIndex].Tag.StartIndex <= characterIndex)
                    {
                        executingTags ??= new List<IScriptTag>();
                        executingTags.Add(_scriptTags[scriptTagIndex].ScriptTag);
                        scriptTagIndex++;
                    }
                    if (executingTags != null)
                    {
                        await SafeTask.WhenAll(executingTags.Select(tag => tag.ExecuteAsync(_playCts.Token)));
                    }

                    while (IsPaused)
                        await SafeTask.WaitWhile(() => IsPaused, _playCts.Token);

                    _playCts.Token.ThrowIfCancellationRequested();
                    if (characterIndex >= _characterCount) break;

                    ShowAt(characterIndex);

                    characterIndex++;
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