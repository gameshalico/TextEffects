

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TextEffects.Common;
using TextEffects.Data;

#if TEXTEFFECTS_UNITASK_SUPPORT
    using AwaitableType = Cysharp.Threading.Tasks.UniTask;
#else
    using AwaitableType = System.Threading.Tasks.ValueTask;
#endif

namespace TextEffects.Effects.Typewriter.ScriptTags
{
    public sealed class EventContext
    {
        private bool _isPaused;
        public event Action<bool> OnPauseChanged;
        public bool IsPaused => _isPaused;
        public TagInfo TagInfo { get; }

        public EventContext(TagInfo tagInfo)
        {
            TagInfo = tagInfo;
        }

        internal void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
            OnPauseChanged?.Invoke(isPaused);
        }
    }

    public sealed class EventScriptTag : PooledItem<EventScriptTag>, IScriptTag
    {
        private Func<EventContext, CancellationToken, AwaitableType> _eventFunc;

        private TagInfo _tagInfo;
        private EventContext _context;
        public void Initialize(Func<EventContext, CancellationToken, AwaitableType> eventFunc, TagInfo tagInfo)
        {
            _eventFunc = eventFunc;
            _tagInfo = tagInfo;
            _context = new EventContext(tagInfo);
        }

        public async AwaitableType ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (_eventFunc != null)
            {
                await _eventFunc(_context, cancellationToken);
            }
        }

        public void Release()
        {
            Return(this);
        }

        public void Pause()
        {
            _context.SetPaused(true);
        }

        public void Resume()
        {
            _context.SetPaused(false);
        }

        public class Factory : IScriptTagFactory
        {
            private readonly Func<EventContext, CancellationToken, UniTask> _eventFunc;

            public Factory(Func<EventContext, CancellationToken, UniTask> eventFunc)
            {
                _eventFunc = eventFunc;
            }

            public IScriptTag CreateTag(TagInfo tagInfo)
            {
                var tag = Rent();
                tag.Initialize(_eventFunc, tagInfo);
                return tag;
            }
        }

    }
}
