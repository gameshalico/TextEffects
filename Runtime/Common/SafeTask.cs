using System;
using System.Runtime.CompilerServices;
using System.Threading;
#if TEXTEFFECTS_UNITASK_SUPPORT
#if UNITY_EDITOR
using UnityEngine;
using System.Threading.Tasks;
#endif

using Cysharp.Threading.Tasks;

#else
using System.Threading.Tasks;
#endif


namespace TextEffects.Common
{
    public static class SafeTask
    {
#if TEXTEFFECTS_UNITASK_SUPPORT
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask Delay(TimeSpan timeSpan, CancellationToken cancellationToken)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                await Task.Delay(timeSpan, cancellationToken);
                return;
            }
#endif
            await UniTask.Delay(timeSpan, true, cancellationToken: cancellationToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask WaitWhile(Func<bool> predicate, CancellationToken cancellationToken)
        {
            await UniTask.WaitWhile(predicate, cancellationToken: cancellationToken);
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            await Task.Delay(timeSpan, cancellationToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task WaitWhile(Func<bool> predicate, CancellationToken cancellationToken)
        {
            while (predicate())
            {
                await Task.Yield();
            }
        }
#endif
    }
}