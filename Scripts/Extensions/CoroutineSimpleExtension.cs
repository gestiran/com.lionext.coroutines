using System.Collections;

namespace Lionext.Coroutines.Extensions {
    public static class CoroutineSimpleExtension {
        public static void Stop(this CoroutineSimple[] routines) => CoroutinesUtility.StopRoutine(routines);

        public static void Pause(this CoroutineSimple[] routines) => CoroutinesUtility.Pause(routines);

        public static void Resume(this CoroutineSimple[] routines) => CoroutinesUtility.Resume(routines);

        public static CoroutineSimple StartAsRoutine<T>(this T enumerator) where T : IEnumerator => CoroutinesUtility.StartRoutine(enumerator);
        
        public static CoroutineSimple[] StartAsRoutine<T>(this T[] enumerator) where T : IEnumerator => CoroutinesUtility.StartRoutine(enumerator);
    }
}