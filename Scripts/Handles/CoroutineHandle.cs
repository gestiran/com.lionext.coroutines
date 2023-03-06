using System.Collections;

namespace Lionext.Coroutines.Handles {
    public class CoroutineHandle {
        public bool Running(CoroutineSimple coroutine) {
            if (!coroutine.MoveNext()) {
                if (coroutine.isEmptyStack) {
                    coroutine.ChangeEnumerator(null);
                    return false;
                }
                coroutine.ChangeEnumerator(coroutine.Pop());
            } else if (coroutine.Current is IEnumerator current) {
                coroutine.Push();
                coroutine.ChangeEnumerator(current);
            }

            return true;
        }
            
        public bool Pause(CoroutineSimple coroutine) => true;

        public bool Stopped(CoroutineSimple coroutine) => false;
    }
}