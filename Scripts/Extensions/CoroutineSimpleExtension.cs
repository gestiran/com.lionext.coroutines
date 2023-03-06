namespace Lionext.Coroutines.Extensions {
    public static class CoroutineSimpleExtension {
        public static void Stop(this CoroutineSimple coroutine) => CoroutinesUtility.StopCoroutine(coroutine);
        
        public static void Stop(this CoroutineSimple[] coroutine) => CoroutinesUtility.StopCoroutine(coroutine);
        
        public static void Pause(this CoroutineSimple coroutine) => CoroutinesUtility.Pause(coroutine);

        public static void Pause(this CoroutineSimple[] coroutine) => CoroutinesUtility.Pause(coroutine);
        
        public static void Resume(this CoroutineSimple coroutine) => CoroutinesUtility.Resume(coroutine);

        public static void Resume(this CoroutineSimple[] coroutine) => CoroutinesUtility.Resume(coroutine);
    }
}