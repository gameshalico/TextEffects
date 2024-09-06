using System.Runtime.CompilerServices;
#if TEXTEFFECTS_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;

#else
using System.Threading.Tasks;
#endif

namespace TextEffects.Common
{
    internal static class SafeTaskExtensions
    {
#if TEXTEFFECTS_UNITASK_SUPPORT
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForgetSafe(this UniTask task)
        {
            task.Forget();
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void ForgetSafe(this Task task)
        {
            try {
                await task;
            } catch {
                // Ignore exceptions
            }
        }
#endif
    }
}