using System.Collections;

namespace Lionext.Coroutines.Extensions {
    public static class CoroutineSimpleExtension {
        public static void Stop(this CoroutineSimple coroutine) => CoroutinesUtility.StopRoutine(coroutine);
        
        public static void Stop(this CoroutineSimple[] coroutine) => CoroutinesUtility.StopRoutine(coroutine);
        
        public static void Pause(this CoroutineSimple coroutine) => CoroutinesUtility.Pause(coroutine);

        public static void Pause(this CoroutineSimple[] coroutine) => CoroutinesUtility.Pause(coroutine);
        
        public static void Resume(this CoroutineSimple coroutine) => CoroutinesUtility.Resume(coroutine);

        public static void Resume(this CoroutineSimple[] coroutine) => CoroutinesUtility.Resume(coroutine);

        public static CoroutineSimple StartAsCoroutine<T>(this T enumerator) where T : IEnumerator => CoroutinesUtility.StartRoutine(enumerator);
        
        public static CoroutineSimple[] StartAsCoroutine<T>(this T[] enumerator) where T : IEnumerator => CoroutinesUtility.StartRoutine(enumerator);
    }
}