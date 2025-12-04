using System.Collections.Generic;
using System.Threading;
#if TEXTEFFECTS_UNITASK_SUPPORT
using AwaitableType = Cysharp.Threading.Tasks.UniTask;
#else
using AwaitableType = System.Threading.Tasks.ValueTask;
#endif

namespace TextEffects.Effects.Typewriter
{
    /// <summary>
    /// TypewriterのScriptCharacterの処理中に非同期処理を実行し、完了を待機するタグ
    /// </summary>
    public interface IScriptTag
    {
        AwaitableType ExecuteAsync(CancellationToken cancellationToken = default);
        void Pause();
        void Resume();
        void Release();
    }
}
