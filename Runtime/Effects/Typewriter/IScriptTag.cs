using System.Collections.Generic;
using System.Threading;
#if TEXTEFFECTS_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
using TextEffects.Data;
#else
using System.Threading.Tasks;
#endif

namespace TextEffects.Effects.Typewriter
{
    /// <summary>
    /// TypewriterのScriptCharacterの処理中に非同期処理を実行し、完了を待機するタグ
    /// </summary>
    public interface IScriptTag
    {
#if TEXTEFFECTS_UNITASK_SUPPORT
        UniTask ExecuteAsync(CancellationToken cancellationToken = default);
#else
        Task ExecuteAsync(CancellationToken cancellationToken = default);
#endif
        void Pause();
        void Resume();
        void Release();
    }
}
