using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

#if TEXTEFFECTS_UNITASK_SUPPORT
    using Cysharp.Threading.Tasks;
    using AwaitableType = Cysharp.Threading.Tasks.UniTask;
#else
    using System.Threading.Tasks;
    using System.Linq;
    using AwaitableType = System.Threading.Tasks.ValueTask;
#endif

#if UNITY_EDITOR
    using UnityEngine;
#endif

namespace TextEffects.Common
{
    public static class SafeTask
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async AwaitableType Delay(TimeSpan timeSpan, CancellationToken cancellationToken)
        {
#if TEXTEFFECTS_UNITASK_SUPPORT
    #if UNITY_EDITOR
            var delayType = Application.isPlaying ? DelayType.DeltaTime : DelayType.Realtime;
            await UniTask.Delay(timeSpan, delayType: delayType, cancellationToken: cancellationToken);
    #else
            // ビルド後は常にゲーム内時間（TimeScale影響あり）
            await UniTask.Delay(timeSpan, cancellationToken: cancellationToken);
    #endif
#else
            await Task.Delay(timeSpan, cancellationToken);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async AwaitableType WaitWhile(Func<bool> predicate, CancellationToken cancellationToken)
        {
#if TEXTEFFECTS_UNITASK_SUPPORT
            await UniTask.WaitWhile(predicate, cancellationToken: cancellationToken);
#else
            while (predicate())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(16, cancellationToken);
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async AwaitableType WhenAll(IEnumerable<AwaitableType> tasks)
        {
#if TEXTEFFECTS_UNITASK_SUPPORT
            await UniTask.WhenAll(tasks);
#else
            await Task.WhenAll(tasks.Select(x => x.AsTask()));
#endif
        }
    }
}