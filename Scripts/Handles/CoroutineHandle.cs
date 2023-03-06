using System.Collections;
using System.Collections.Generic;

namespace Lionext.Coroutines.Handles {
    public class CoroutineHandle {
        public bool Running(ref Stack<IEnumerator> stack, ref IEnumerator enumerator) {
            if (!enumerator.MoveNext()) {
                if (stack.Count == 0) return false;
                enumerator = stack.Pop();
            } else if (enumerator.Current is IEnumerator current) {
                stack.Push(enumerator);
                enumerator = current;
            }

            return true;
        }
            
        public bool Pause(ref Stack<IEnumerator> stack, ref IEnumerator enumerator) => true;

        public bool Stopped(ref Stack<IEnumerator> stack, ref IEnumerator enumerator) => false;
    }
}