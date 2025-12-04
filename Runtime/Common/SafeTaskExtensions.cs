using System.Runtime.CompilerServices;
#if TEXTEFFECTS_UNITASK_SUPPORT
    using Cysharp.Threading.Tasks;
    using AwaitableType = Cysharp.Threading.Tasks.UniTask;
#else
    using System.Threading.Tasks;
    using AwaitableType = System.Threading.Tasks.ValueTask;
#endif

namespace TextEffects.Common
{
    internal static class SafeTaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForgetSafe(this AwaitableType task)
        {
#if TEXTEFFECTS_UNITASK_SUPPORT
            task.Forget();
#endif
        }
    }
}