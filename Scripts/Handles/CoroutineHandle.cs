using System.Collections;

namespace Lionext.Coroutines.Handles {
    public class CoroutineHandle {
        public bool Running(CoroutineSimple coroutine) {
            if (!coroutine.MoveNextCurrent()) {
                if (coroutine.isEmptyStack) {
                    coroutine.Reset();
                    return false;
                }
                coroutine.PopCurrent();
            } else if (coroutine.Current is IEnumerator current) coroutine.PushCurrent(current);

            return true;
        }
            
        public bool Pause(CoroutineSimple coroutine) => true;

        public bool Stopped(CoroutineSimple coroutine) => false;
    }
}