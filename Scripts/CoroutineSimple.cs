using System;
using System.Collections;
using System.Collections.Generic;

namespace Lionext.Coroutines {
    public class CoroutineSimple : IEnumerator {
        public bool isEmptyStack => _stack.Count == 0;
        public bool isComplete => _stack.Count == 0 && _current == null;
        public object Current => _current.Current;
        
        private IEnumerator _current;
        private Func<CoroutineSimple, bool> _updateState;

        private readonly Stack<IEnumerator> _stack;

        public CoroutineSimple(IEnumerator current, Func<CoroutineSimple, bool> updateState) {
            _stack = new Stack<IEnumerator>();
            _current = current;
            _updateState = updateState;
        }

        public void ChangeState(Func<CoroutineSimple, bool> state) => _updateState = state;

        public void PopCurrent() => _current = _stack.Pop();

        public void PushCurrent(IEnumerator next) {
            _stack.Push(_current);
            _current = next;
        }

        public bool MoveNextCurrent() => _current.MoveNext();
        
        public bool MoveNext() => _updateState(this);

        public void Reset() {
            _stack.Clear();
            _current = null;
        }
    }
}