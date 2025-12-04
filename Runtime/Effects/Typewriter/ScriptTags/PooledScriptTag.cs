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

        public abstract AwaitableType ExecuteAsync(CancellationToken cancellationToken = default);
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