

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TextEffects.Common;
using TextEffects.Data;
using TextEffects.Effects.Typewriter.DisplayTags;

namespace TextEffects.Effects.Typewriter.ScriptTags
{
    public sealed class EventScriptTag : PooledItem<EventScriptTag>, IScriptTag
    {
#if TEXTEFFECTS_UNITASK_SUPPORT
        private Func<TagInfo, CancellationToken, UniTask> _eventFunc;
#else
        private Func<TagInfo, CancellationToken, Task> _eventFunc;
#endif

        private TagInfo _tagInfo;
        public void Initialize(Func<TagInfo, CancellationToken, UniTask> eventFunc, TagInfo tagInfo)
        {
            _eventFunc = eventFunc;
            _tagInfo = tagInfo;
        }

#if TEXTEFFECTS_UNITASK_SUPPORT
        public async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
#else
        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
#endif
        {
            if (_eventFunc != null)
            {
                await _eventFunc(_tagInfo, cancellationToken);
            }
        }

        public void Release()
        {
            Return(this);
        }

        public class Factory : IScriptTagFactory
        {
            private readonly Func<TagInfo, CancellationToken, UniTask> _eventFunc;

            public Factory(Func<TagInfo, CancellationToken, UniTask> eventFunc)
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
