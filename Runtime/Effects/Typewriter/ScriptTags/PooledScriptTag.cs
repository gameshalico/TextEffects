using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TextEffects.Common;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter.ScriptTags
{
    public abstract class PooledScriptTag<T> : PooledItem<T>, IScriptTag where T : PooledScriptTag<T>, new()
    {
        protected TagInfo TagInfo { get; private set; }

        public void Release()
        {
            OnRelease();
            Return(this as T);
        }

        private void SetTag(TagInfo tagInfo)
        {
            TagInfo = tagInfo;
            OnSetTag(tagInfo);
        }

        protected virtual void OnSetTag(TagInfo tagInfo)
        {
        }

        protected virtual void OnRelease()
        {
        }

#if TEXTEFFECTS_UNITASK_SUPPORT
        public abstract UniTask ExecuteAsync(CancellationToken cancellationToken = default);
#else
        public abstract Task ExecuteAsync(CancellationToken cancellationToken = default);
#endif
        public abstract void Pause();
        public abstract void Resume();

        public sealed class Factory : IScriptTagFactory
        {
            public IScriptTag CreateTag(TagInfo tagInfo)
            {
                var tag = Rent();
                tag.SetTag(tagInfo);
                return tag;
            }
        }
    }
}